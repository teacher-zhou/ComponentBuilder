namespace ComponentBuilder.Automation.Builder;

/// <summary>
/// Default <see cref="ICssClassBuilder"/> implementation.
/// </summary>
internal class DefaultCssClassBuilder : ICssClassBuilder
{

    private readonly IList<string> _classes;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultCssClassBuilder"/> class.
    /// </summary>
    public DefaultCssClassBuilder() => _classes = new List<string>();

    /// <summary>
    /// Gets CSS class list.
    /// </summary>
    public IEnumerable<string> CssList => _classes;

    /// <inheritdoc/>
    public ICssClassBuilder Append(string? value)
    {
        if (!string.IsNullOrEmpty(value) && !Contains(value))
        {
            _classes.Add(value);
        }
        return this;
    }

    /// <summary>
    /// Clear CSS list.
    /// </summary>
    public void Clear() => _classes.Clear();

    /// <inheritdoc/>
    public ICssClassBuilder Insert(int index, string? value)
    {
        if (!string.IsNullOrEmpty(value) && !_classes.Contains(value))
        {
            _classes.Insert(index, value);
        }
        return this;
    }

    /// <inheritdoc/>
    public bool Contains(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        return _classes.Contains(value);
    }

    /// <inheritdoc/>
    public bool IsEmpty() => !_classes.Any();

    /// <inheritdoc/>
    public ICssClassBuilder Remove(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return this;
        }
        _classes.Remove(value);
        return this;
    }


    /// <inheritdoc/>
    public override string ToString()
    {
        var result = string.Join(" ", _classes.Distinct());
        //this.Clear();
        return result;
    }
}
