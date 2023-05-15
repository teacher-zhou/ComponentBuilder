//using Microsoft.AspNetCore.Components.Routing;
//using Microsoft.Extensions.DependencyInjection;
//using System.Diagnostics;

//namespace ComponentBuilder.Automation.Interceptors;

///// <summary>
///// An interceptor to support nav link of component witch implemented from <see cref="IHasNavLink"/>.
///// </summary>
//internal class NavLinkComponentInterceptor : ComponentInterceptorBase
//{
//    bool _isActive;

//    public override int Order => 5000;
//    NavigationManager NavigationManager { get; set; }
//    /// <inheritdoc/>
//    public override void InterceptOnInitialized(IBlazorComponent component)
//    {
//        NavigationManager=component.ServiceProvider.GetRequiredService<NavigationManager>();
//        if ( component is IHasNavLink navLink )
//        {
//            NavigationManager.LocationChanged += (sender, args) => NotifyLocationChanged(navLink, sender, args);
//        }
//    }

//    public override void InterceptOnResolvingAttributes(IBlazorComponent component, IDictionary<string, object> attributes)
//    {
//        if ( component is not IHasNavLink navLink )
//        {
//            return;
//        }
//        _isActive = ShouldMatch(navLink, NavigationManager.Uri);
//        navLink.IsActive = _isActive;
//    }

//    //public override void InterceptOnBuildContent(IBlazorComponent component, RenderTreeBuilder builder, int sequence)
//    //{
//    //    if ( component is IHasNavLink navLink )
//    //    {
//    //        builder.CreateComponent<NavLink>(sequence, navLink.ChildContent, new
//    //        {
//    //            navLink.Match,
//    //            ActiveClass = navLink.ActiveCssClass,
//    //            navLink.AdditionalAttributes
//    //        });
//    //    }
//    //}

//    //public override void InterceptOnParameterSet(IBlazorComponent component)
//    //{
//    //    if ( component is not IHasNavLink navLink )
//    //    {
//    //        return;
//    //    }
//    //    _isActive = ShouldMatch(navLink, NavigationManager.Uri);
//    //    navLink.IsActive = _isActive;
//    //    //navLink.NotifyStateChanged();
//    //    // UpdateCssClass(navLink);
//    //}

//    /// <inheritdoc/>
//    public override void InterceptOnDispose(IBlazorComponent component)
//    {
//        if ( component is IHasNavLink navLink )
//        {
//            NavigationManager.LocationChanged -= (sender, args) => NotifyLocationChanged(navLink, sender, args);
//        }
//    }

//    //void UpdateCssClass(IHasNavLink navLink)
//    //{
//    //    if ( _isActive )
//    //    {
//    //        navLink.AdditionalAttributes!.TryAddOrConcat("class", $"{navLink.ActiveCssClass} ", false);
//    //    }
//    //}

//    /// <summary>
//    /// Occurs when location of navigation is changed.
//    /// </summary>
//    /// <param name="navLink"></param>
//    /// <param name="sender"></param>
//    /// <param name="args"></param>
//    private void NotifyLocationChanged(IHasNavLink navLink, object? sender, LocationChangedEventArgs args)
//    {
//        navLink.CssClassBuilder.Clear();
//        // We could just re-render always, but for component component we know the
//        // only relevant state change is to the _isActive property.
//        var shouldBeActiveNow = ShouldMatch(navLink, args.Location);

//        //Debug.WriteLine($"【{nameof(NotifyLocationChanged)}】New Location：{args.Location}");
//        //Debug.WriteLine($"{navLink.GetType().Name} | {navLink.AdditionalAttributes["href"]}");
//        //Debug.WriteLine($"{nameof(shouldBeActiveNow)}: {shouldBeActiveNow}");

//        //navLink.IsActive = shouldBeActiveNow;
//        if ( shouldBeActiveNow != _isActive )
//        {
//            navLink.IsActive = _isActive = shouldBeActiveNow;
//            //UpdateCssClass(navLink);
//            navLink.NotifyStateChanged();
//        }
//    }

//    /// <summary>
//    /// Shoulds the match.
//    /// </summary>
//    /// <param name="navLink"></param>
//    /// <param name="currentUriAbsolute">The current uri absolute.</param>
//    /// <returns>A bool.</returns>
//    private bool ShouldMatch(IHasNavLink navLink, string currentUriAbsolute)
//    {

//        string? href = null;
//        if ( navLink.AdditionalAttributes.TryGetValue("href", out var value) )
//        {
//            href = Convert.ToString(value);
//        }

//        if ( href is null )
//        {
//            return false;
//        }

//        href = NavigationManager.ToAbsoluteUri(href!).AbsoluteUri;

//        if ( EqualsHrefExactlyOrIfTrailingSlashAdded(currentUriAbsolute) )
//        {
//            return true;
//        }

//        if ( navLink.Match == NavLinkMatch.Prefix
//            && IsStrictlyPrefixWithSeparator(currentUriAbsolute, href) )
//        {
//            return true;
//        }

//        return false;


//        bool EqualsHrefExactlyOrIfTrailingSlashAdded(string currentUriAbsolute)
//        {
//            Debug.Assert(href != null);

//            if ( string.Equals(currentUriAbsolute, href, StringComparison.OrdinalIgnoreCase) )
//            {
//                return true;
//            }

//            if ( currentUriAbsolute.Length == href.Length - 1 )
//            {
//                // Special case: highlight links to http://host/path/ even if you're
//                // at http://host/path (with no trailing slash)
//                //
//                // This is because the router accepts an absolute URI value of "same
//                // as base URI but without trailing slash" as equivalent to "base URI",
//                // which in turn is because it's common for servers to return the same page
//                // for http://host/vdir as they do for host://host/vdir/ as it's no
//                // good to display a blank page in that case.
//                if ( href[href.Length - 1] == '/'
//                    && href.StartsWith(currentUriAbsolute, StringComparison.OrdinalIgnoreCase) )
//                {
//                    return true;
//                }
//            }

//            return false;
//        }

//        bool IsStrictlyPrefixWithSeparator(string value, string prefix)
//        {
//            var prefixLength = prefix.Length;
//            if ( value.Length > prefixLength )
//            {
//                return value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
//                    && (
//                        // Only match when there's a separator character either at the end of the
//                        // prefix or right after it.
//                        // Example: "/abc" is treated as a prefix of "/abc/def" but not "/abcdef"
//                        // Example: "/abc/" is treated as a prefix of "/abc/def" but not "/abcdef"
//                        prefixLength == 0
//                        || !char.IsLetterOrDigit(prefix[prefixLength - 1])
//                        || !char.IsLetterOrDigit(value[prefixLength])
//                    );
//            }
//            else
//            {
//                return false;
//            }
//        }
//    }

//    //public override void InterceptOnAfterRender(IBlazorComponent component, in bool firstRender)
//    //{
//    //    if (component is IHasNavLink navLink && navLink.AdditionalAttributes.TryGetValue("class", out var className))
//    //    {
//    //        var cssClass = Convert.ToString(className);
//    //        navLink.IsActive = (!string.IsNullOrEmpty(cssClass) && cssClass.Split(' ').Any(m => navLink.ActiveCssClass == m));

//    //        Console.WriteLine($"{nameof(navLink.IsActive)}:{navLink.IsActive}");
//    //        Debug.WriteLine($"{nameof(navLink.IsActive)}:{navLink.IsActive}");
//    //    }
//    //}

//    //public override void InterceptOnUpdatingAttributes(IBlazorComponent component, IDictionary<string, object?> attributes)
//    //{
//    //}

//    //public override void InterceptOnParameterSet(IBlazorComponent component)
//    //{

//    //}
//}
