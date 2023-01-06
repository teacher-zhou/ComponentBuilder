using Microsoft.AspNetCore.Components.Routing;
using System.Diagnostics;

namespace ComponentBuilder.Interceptors;

/// <summary>
/// An interceptor to support nav link of component witch implemented from <see cref="IHasNavLink"/>.
/// </summary>
internal class NavLinkComponentInterceptor : ComponentInterceptorBase
{
    bool _isActive;
    string? _hrefAbsolute;

    /// <inheritdoc/>
    public override void InterceptOnInitialized(IBlazorComponent component)
    {
        if(component is IHasNavLink navLink )
        {
            navLink.NavigationManager.LocationChanged += async (sender, args) => await NotifyLocationChanged(navLink, sender, args);
        }
    }

    /// <inheritdoc/>
    public override void InterceptOnDispose(IBlazorComponent component)
    {
        if ( component is IHasNavLink navLink )
        {
            navLink.NavigationManager.LocationChanged -= async (sender, args) => await NotifyLocationChanged(navLink, sender, args);
        }
    }

    /// <inheritdoc/>
    public override void InterceptOnResolvedAttributes(IBlazorComponent component, IDictionary<string, object> attributes)
    {
        if ( component is not IHasNavLink navLink )
        {
            return;
        }
        string? href = null;
        if ( attributes.TryGetValue("href", out var value) )
        {
            href = Convert.ToString(value);
        }

        _hrefAbsolute = href is null ? null : navLink.NavigationManager.ToAbsoluteUri(href!).AbsoluteUri;
        _isActive = ShouldMatch(navLink, navLink.NavigationManager.Uri);


        navLink.IsActive = _isActive;
    }

    /// <summary>
    /// Occurs when location of navigation is changed.
    /// </summary>
    /// <param name="component"></param>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private async Task NotifyLocationChanged(IHasNavLink component, object? sender, LocationChangedEventArgs args)
    {
        // We could just re-render always, but for component component we know the
        // only relevant state change is to the _isActive property.
        var shouldBeActiveNow = ShouldMatch(component,args.Location);
        if ( shouldBeActiveNow != _isActive )
        {
            _isActive = shouldBeActiveNow;
            component.CssClassBuilder.Clear();
            await component.NotifyStateChanged();
        }
    }

    /// <summary>
    /// Shoulds the match.
    /// </summary>
    /// <param name="component"></param>
    /// <param name="currentUriAbsolute">The current uri absolute.</param>
    /// <returns>A bool.</returns>
    private bool ShouldMatch(IHasNavLink component, string currentUriAbsolute)
    {
        if ( _hrefAbsolute == null )
        {
            return false;
        }

        if ( EqualsHrefExactlyOrIfTrailingSlashAdded(currentUriAbsolute) )
        {
            return true;
        }

        if ( component.Match == NavLinkMatch.Prefix
            && IsStrictlyPrefixWithSeparator(currentUriAbsolute, _hrefAbsolute) )
        {
            return true;
        }

        return false;


        bool EqualsHrefExactlyOrIfTrailingSlashAdded(string currentUriAbsolute)
        {
            Debug.Assert(_hrefAbsolute != null);

            if ( string.Equals(currentUriAbsolute, _hrefAbsolute, StringComparison.OrdinalIgnoreCase) )
            {
                return true;
            }

            if ( currentUriAbsolute.Length == _hrefAbsolute.Length - 1 )
            {
                // Special case: highlight links to http://host/path/ even if you're
                // at http://host/path (with no trailing slash)
                //
                // This is because the router accepts an absolute URI value of "same
                // as base URI but without trailing slash" as equivalent to "base URI",
                // which in turn is because it's common for servers to return the same page
                // for http://host/vdir as they do for host://host/vdir/ as it's no
                // good to display a blank page in that case.
                if ( _hrefAbsolute[^1] == '/'
                    && _hrefAbsolute.StartsWith(currentUriAbsolute, StringComparison.OrdinalIgnoreCase) )
                {
                    return true;
                }
            }

            return false;
        }

        bool IsStrictlyPrefixWithSeparator(string value, string prefix)
        {
            var prefixLength = prefix.Length;
            if ( value.Length > prefixLength )
            {
                return value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                    && (
                        // Only match when there's a separator character either at the end of the
                        // prefix or right after it.
                        // Example: "/abc" is treated as a prefix of "/abc/def" but not "/abcdef"
                        // Example: "/abc/" is treated as a prefix of "/abc/def" but not "/abcdef"
                        prefixLength == 0
                        || !char.IsLetterOrDigit(prefix[prefixLength - 1])
                        || !char.IsLetterOrDigit(value[prefixLength])
                    );
            }
            else
            {
                return false;
            }
        }
    }
}
