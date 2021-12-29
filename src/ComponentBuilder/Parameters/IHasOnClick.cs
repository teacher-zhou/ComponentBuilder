namespace ComponentBuilder.Parameters
{
    /// <summary>
    /// Provides click event for component.
    /// </summary>
    public interface IHasOnClick
    {
        /// <summary>
        /// Performed a callback when clicking component.
        /// </summary>
        EventCallback<MouseEventArgs> OnClick { get; set; }
    }
}
