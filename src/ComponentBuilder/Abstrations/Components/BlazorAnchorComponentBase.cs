using System.Diagnostics;

using Microsoft.AspNetCore.Components.Routing;

namespace ComponentBuilder;

/// <summary>
/// Represents a base class for navigation compnent.
/// </summary>
/// 
[Obsolete("The class will be deleted in next version. For anchor component just drived from BlazorAbstractComponentBase and implement from IHasNavLink interface")]
public abstract class BlazorAnchorComponentBase : BlazorComponentBase, IHasChildContent, IDisposable
{
    private string? _hrefAbsolute;
    private bool _isActive;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Inject] protected NavigationManager NavigationManger { get; set; } = default!;
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the behavior to match link.
    /// </summary>
    [Parameter] public NavLinkMatch Match { get; set; } = NavLinkMatch.All;

    /// <summary>
    /// Gets a value indicating the url is matched.
    /// <para>
    /// You can use this value to build CSS class when url is matched.
    /// </para>
    /// </summary>
    protected bool IsActive => _isActive;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnInitialized()
    {
        // We'll consider re-rendering on each location change
        base.OnInitialized();
        NavigationManger.LocationChanged += OnLocationChanged;
    }

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        // Update computed state
        var href = (string?)null;
        if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("href", out var obj))
        {
            href = Convert.ToString(obj);
        }

        _hrefAbsolute = href == null ? null : NavigationManger.ToAbsoluteUri(href).AbsoluteUri;
        _isActive = ShouldMatch(NavigationManger.Uri);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void Dispose()
    {
        // To avoid leaking memory, it's important to detach any event handlers in Dispose()
        NavigationManger.LocationChanged -= OnLocationChanged;
    }

    /// <summary>
    /// Ons the location changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The args.</param>
    private void OnLocationChanged(object? sender, LocationChangedEventArgs args)
    {
        // We could just re-render always, but for this component we know the
        // only relevant state change is to the _isActive property.
        var shouldBeActiveNow = ShouldMatch(args.Location);
        if (shouldBeActiveNow != _isActive)
        {
            _isActive = shouldBeActiveNow;
            CssClassBuilder.Dispose();
            StateHasChanged();
        }
    }

    /// <summary>
    /// Shoulds the match.
    /// </summary>
    /// <param name="currentUriAbsolute">The current uri absolute.</param>
    /// <returns>A bool.</returns>
    private bool ShouldMatch(string currentUriAbsolute)
    {
        if (_hrefAbsolute == null)
        {
            return false;
        }

        if (EqualsHrefExactlyOrIfTrailingSlashAdded(currentUriAbsolute))
        {
            return true;
        }

        if (Match == NavLinkMatch.Prefix
            && IsStrictlyPrefixWithSeparator(currentUriAbsolute, _hrefAbsolute))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Equals the href exactly or if trailing slash added.
    /// </summary>
    /// <param name="currentUriAbsolute">The current uri absolute.</param>
    /// <returns>A bool.</returns>
    private bool EqualsHrefExactlyOrIfTrailingSlashAdded(string currentUriAbsolute)
    {
        Debug.Assert(_hrefAbsolute != null);

        if (string.Equals(currentUriAbsolute, _hrefAbsolute, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (currentUriAbsolute.Length == _hrefAbsolute.Length - 1)
        {
            // Special case: highlight links to http://host/path/ even if you're
            // at http://host/path (with no trailing slash)
            //
            // This is because the router accepts an absolute URI value of "same
            // as base URI but without trailing slash" as equivalent to "base URI",
            // which in turn is because it's common for servers to return the same page
            // for http://host/vdir as they do for host://host/vdir/ as it's no
            // good to display a blank page in that case.
            if (_hrefAbsolute[_hrefAbsolute.Length - 1] == '/'
                && _hrefAbsolute.StartsWith(currentUriAbsolute, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Are the strictly prefix with separator.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="prefix">The prefix.</param>
    /// <returns>A bool.</returns>
    private static bool IsStrictlyPrefixWithSeparator(string value, string prefix)
    {
        var prefixLength = prefix.Length;
        if (value.Length > prefixLength)
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
