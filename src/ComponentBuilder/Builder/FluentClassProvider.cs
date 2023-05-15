namespace ComponentBuilder;

/// <summary>
/// An abstract class to build fluent class.
/// <para>
/// This class supports to build customization rules for fluent CSS class, for examples:
/// <code language="html">
/// &lt;Component class="Margin.Is3.OnTop.FromXL.Is4">...&lt;/Component>
/// class : mt-xl-3 m-4
/// </code>
/// </para>
/// </summary>
/// <typeparam name="TKey">The type of key.</typeparam>
/// <typeparam name="TRule">The type of rule.</typeparam>
public abstract class FluentClassProvider<TKey, TRule> : IFluentClassProvider where TKey : notnull
{
    private readonly List<string> _classList;
    private readonly IDictionary<TKey, List<TRule>> _rules;
    /// <summary>
    /// Initializes a new instance of the <see cref="FluentClassProvider{TKey,TRule}"/> class.
    /// </summary>
    protected FluentClassProvider()
    {
        _classList = new List<string>();
        _rules = new Dictionary<TKey, List<TRule>>();
    }

    /// <summary>
    /// Gets the rules stored.
    /// </summary>
    protected IEnumerable<KeyValuePair<TKey, List<TRule>>> Rules => _rules;

    /// <summary>
    /// Gets a bool value indicated rules has set.
    /// </summary>
    protected bool IsDirty { get; private set; }

    /// <summary>
    /// Gets the current key.
    /// </summary>
    protected TKey CurrentKey { get; private set; }

    /// <summary>
    /// Create CSS class string within specified key and rules.
    /// </summary>
    /// <returns><inheritdoc/></returns>
    public virtual IEnumerable<string> Create()
    {
        if ( IsDirty )
        {
            foreach ( var rule in Rules )
            {
                if ( !rule.Value.Any() )
                {
                    var css= Format(rule.Key);
                    if ( css.IsNotNullOrEmpty() )
                    {
                        _classList.Add(css!.TrimEnd());
                    }
                }

                rule.Value.ForEach(value =>
                {
                    var classString = Format(rule.Key, value);
                    if ( classString.IsNotNullOrEmpty() )
                    {
                        _classList.Add(classString!.TrimEnd());
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
    /// Add a new rule for <see cref="CurrentKey"/>. A same key can have multiple rules.
    /// </summary>
    /// <param name="rule">The rule to be add.</param>
    /// <param name="ignoreIfDuplicate">Ignore to add rule when has same rule in same key.</param>
    /// <exception cref="ArgumentNullException"><paramref name="rule"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException"><see cref="CurrentKey"/> is null and call <see cref="ChangeKey(TKey)"/> at least once.</exception>
    protected virtual void AddRule(TRule rule, bool ignoreIfDuplicate = true)
    {
        if ( rule is null )
        {
            throw new ArgumentNullException(nameof(rule));
        }

        if ( CurrentKey is null )
        {
            throw new InvalidOperationException($"Make sure {nameof(ChangeKey)} is called at least once");
        }


        if ( _rules.TryGetValue(CurrentKey, out var list) )
        {
            if ( !list.Contains(rule) || !ignoreIfDuplicate )
            {
                list.Add(rule);
            }
        }
        else
        {
            _rules.Add(CurrentKey, new() { rule });
        }
        Dirty();
    }


    /// <summary>
    /// Changes current key. A new rules of key will be added if specified key in rules does not found.
    /// </summary>
    /// <param name="key">The key to change.</param>
    protected virtual void ChangeKey(TKey key)
    {
        CurrentKey = key;
        if ( !_rules.ContainsKey(key) )
        {
            _rules.Add(key, new());
            Dirty();
        }
    }

    /// <summary>
    /// Format a CSS class string for each rule.
    /// </summary>
    /// <param name="key">The key of rule.</param>
    /// <param name="rule">The value for each rule.</param>
    /// <returns>A string representing a format CSS class.</returns>
    protected abstract string? Format(TKey key, TRule rule);

    /// <summary>
    /// Format a CSS class string without any rules.
    /// </summary>
    /// <param name="key">The key of rule.</param>
    /// <returns>A string representing a format CSS class.</returns>
    protected abstract string? Format(TKey key);

    /// <summary>
    /// Format a CSS class string with rule by specify key and value.
    /// </summary>
    /// <param name="key">The key of rule.</param>
    /// <param name="rules">The rules value.</param>
    /// <returns>A string representing a format CSS class.</returns>
    protected virtual string? Format(TKey key, IEnumerable<TRule> rules) => string.Join(" ", rules.Select(m => Format(key, m)));
}
