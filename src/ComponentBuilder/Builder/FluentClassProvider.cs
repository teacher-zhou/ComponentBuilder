namespace ComponentBuilder;

/// <summary>
/// An abstract class to build fluent class.
/// </summary>
public abstract class FluentClassProvider<TKey,TValue> : IFluentClassProvider where TKey:notnull
{
    List<string> _classList;
    /// <summary>
    /// Initializes a new instance of the <see cref="FluentClassProvider{TKey,TValue}"/> class.
    /// </summary>
    protected FluentClassProvider()
    {
        _classList = new List<string>();
        _rules = new Dictionary<TKey,List<TValue>>();
    }

    private IDictionary<TKey,List<TValue>> _rules;
    /// <summary>
    /// Gets the rules stored.
    /// </summary>
    protected IEnumerable<KeyValuePair<TKey,List<TValue>>> Rules => _rules;
    /// <summary>
    /// Gets a bool value indicated rules had set.
    /// </summary>
    protected bool IsDirty { get; private set; }

    /// <summary>
    /// Gets the key.
    /// </summary>
    protected TKey Key { get; private set; }

    /// <inheritdoc/>
    public IEnumerable<string> Create()
    {
        if ( IsDirty )
        {
            //_classList.Add(string.Join(" ", Rules.Select(m => Format(m.Key, m.Value))));

            foreach ( var rule in Rules )
            {
                if ( !rule.Value.Any() )
                {
                    var css= Format(rule.Key);
                    if ( css.IsNotNullOrEmpty() )
                    {
                        _classList.Add(css!);
                    }
                }

                rule.Value.ForEach(value =>
                {
                    var classString = Format(rule.Key, value);
                    if ( classString.IsNotNullOrEmpty() )
                    {
                        _classList.Add(classString!);
                    }
                });
            }
        }
        return _classList;
    }

    /// <summary>
    /// Represent rules had been set by fluent calling.
    /// </summary>
    protected void Dirty() => IsDirty = true;

    /// <summary>
    /// Sets the value for specified key.
    /// </summary>
    /// <param name="value">The value to set.</param>
    /// <exception cref="InvalidOperationException"><see cref="Key"/> is null and call <see cref="SetKey(TKey)"/> at least once.</exception>
    protected virtual void SetValue(TValue value)
    {
        if(Key is null )
        {
            throw new InvalidOperationException("Make sure SetKey is called at least once");
        }

        if(_rules.TryGetValue(Key,out var list) )
        {
            list.Add(value);
        }
        else
        {
            _rules.Add(Key, new() { value });
        }
        Dirty();
    }


    /// <summary>
    /// Sets the key of rule.
    /// </summary>
    /// <param name="key">The key to set.</param>
    protected virtual void SetKey(TKey key)
    {
        Key = key;
        if ( !_rules.ContainsKey(key) )
        {
            _rules.Add(key, new());
            Dirty();
        }
    }

    /// <summary>
    /// Format a CSS class string with rule by specify key and value.
    /// </summary>
    /// <param name="key">The key of rule.</param>
    /// <param name="value">The value for each rule.</param>
    /// <returns>A string representing a format CSS class.</returns>
    protected abstract string? Format(TKey key,TValue value);

    protected abstract string? Format(TKey key);

    /// <summary>
    /// Format a CSS class string with rule by specify key and value.
    /// </summary>
    /// <param name="key">The key of rule.</param>
    /// <param name="values">The value rules.</param>
    /// <returns>A string representing a format CSS class.</returns>
    protected virtual string? Format(TKey key, IEnumerable<TValue> values) => string.Join(" ", values.Select(m => Format(key, m)));
}
