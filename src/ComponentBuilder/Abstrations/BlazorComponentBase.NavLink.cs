using Microsoft.AspNetCore.Components.Routing;
using System.Diagnostics;

namespace ComponentBuilder;

/// <summary>
/// The partial of <see cref="BlazorComponentBase"/> class.
/// </summary>
partial class BlazorComponentBase
{

    #region NavLink

    /// <summary>
    /// Gets a bool value indicates current nav link is actived.
    /// </summary>
    protected bool IsNavLinkActived { get; private set; }
    string? _hrefAbsolute;

    /// <summary>
    /// Occurs when location of navigation is changed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnNavLinkLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        // We could just re-render always, but for this component we know the
        // only relevant state change is to the _isActive property.
        var shouldBeActiveNow = ShouldNavLinkMatch(e.Location);
        if ( shouldBeActiveNow != IsNavLinkActived )
        {
            IsNavLinkActived = shouldBeActiveNow;
            CssClassBuilder.Dispose();
            StateHasChanged();
        }
    }

    /// <summary>
    /// Shoulds the match.
    /// </summary>
    /// <param name="currentUriAbsolute">The current uri absolute.</param>
    /// <returns>A bool.</returns>
    private bool ShouldNavLinkMatch(string currentUriAbsolute)
    {
        if ( this is not IHasNavLink navLink )
        {
            return false;
        }

        if ( _hrefAbsolute == null )
        {
            return false;
        }

        if ( EqualsHrefExactlyOrIfTrailingSlashAdded(currentUriAbsolute) )
        {
            return true;
        }

        if ( navLink.Match == NavLinkMatch.Prefix
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
    #endregion
}

