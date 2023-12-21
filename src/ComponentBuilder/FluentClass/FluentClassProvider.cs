namespace ComponentBuilder.FluentClass;

/// <summary>
/// Build an abstract implementation of the fluent CSS class.
/// <para>
/// Example:
/// <code language="html">
/// &lt;Component class="Margin.Is3.OnTop.FromXL.Is4">...&lt;/Component>
/// class : mt-xl-3 m-4
/// </code>
/// </para>
/// </summary>
/// <typeparam name="TKey">The key type.</typeparam>
/// <typeparam name="TRule">The rule type.</typeparam>
public abstract class FluentClassProvider<TKey, TRule> : IFluentClassProvider where TKey : notnull
{
    private readonly List<string> _classList = [];
    private readonly IDictionary<TKey, List<TRule>> _rules = new Dictionary<TKey, List<TRule>>();
    /// <summary>
    /// 初始化 <see cref="FluentClassProvider{TKey,TRule}"/> 类的新实例。
    /// </summary>
    protected FluentClassProvider()
    {
    }

    /// <summary>
    /// Gets all rules.
    /// </summary>
    protected IEnumerable<KeyValuePair<TKey, List<TRule>>> Rules => _rules;

    /// <summary>
    /// Gets a Boolean value indicating that the rule has been set.
    /// </summary>
    protected bool IsDirty { get; private set; }

    /// <summary>
    /// Gets the current key.
    /// </summary>
    protected TKey? CurrentKey { get; private set; }

    /// <inheritdoc/>
    public virtual IEnumerable<string> Create()
    {
        if (IsDirty)
        {
            foreach (var rule in Rules)
            {
                if (!rule.Value.Any())
                {
                    var css = Format(rule.Key);
                    if (css.IsNotNullOrEmpty())
                    {
                        _classList.Add(css!.TrimEnd());
                    }
                }

                rule.Value.ForEach(value =>
                {
                    var classString = Format(rule.Key, value);
                    if (classString.IsNotNullOrEmpty())
                    {
                        _classList.Add(classString!.TrimEnd());
                    }
                });
            }
        }
        return _classList;
    }

    /// <summary>
    /// Representative rules are set through fluid calls.
    /// </summary>
    protected void Dirty() => IsDirty = true;

    /// <summary>
    /// Add a new rule for <see cref="CurrentKey"/>. The same key can have multiple rules.
    /// </summary>
    /// <param name="rule">A new rule to add.</param>
    /// <param name="ignoreIfDuplicate">Omit adding rules when there are the same rules in the same key.</param>
    /// <exception cref="ArgumentNullException"><paramref name="rule"/> is <c>null</c>。</exception>
    /// <exception cref="InvalidOperationException"><see cref="CurrentKey"/> is null but <see cref="ChangeKey(TKey)"/> execute at least once.</exception>
    protected virtual void AddRule(TRule rule, bool ignoreIfDuplicate = true)
    {
        if (rule is null)
        {
            throw new ArgumentNullException(nameof(rule));
        }

        if (CurrentKey is null)
        {
            throw new InvalidOperationException($"请确定 {nameof(ChangeKey)} 至少执行了一次");
        }


        if (_rules.TryGetValue(CurrentKey, out var list))
        {
            if (!list.Contains(rule) || !ignoreIfDuplicate)
            {
                list.Add(rule);
            }
        }
        else
        {
            _rules.Add(CurrentKey, [rule]);
        }
        Dirty();
    }


    /// <summary>
    /// Change the current key. If the specified key is not found in the rule, a new key rule is added.
    /// </summary>
    /// <param name="key">Key to change.</param>
    protected virtual void ChangeKey(TKey key)
    {
        CurrentKey = key;
        if (!_rules.ContainsKey(key))
        {
            _rules.Add(key, []);
            Dirty();
        }
    }

    /// <summary>
    /// Format a CSS class string for each rule.
    /// </summary>
    /// <param name="key">The key of rule.</param>
    /// <param name="rule">The rule to format.</param>
    /// <returns>Formatted string of the CSS class.</returns>
    protected abstract string? Format(TKey key, TRule rule);

    /// <summary>
    /// Format a string of CSS classes without any rules.
    /// </summary>
    /// <param name="key">The key of rule.</param>
    /// <returns>Formatted string of the CSS class.</returns>
    protected abstract string? Format(TKey key);

    /// <summary>
    /// Format CSS class strings with rules specifying keys and values.
    /// </summary>
    /// <param name="key">The key of rule.</param>
    /// <param name="rules">The rules to format.</param>
    /// <returns>Formatted string of the CSS class.</returns>
    protected virtual string? Format(TKey key, IEnumerable<TRule> rules) => string.Join(" ", rules.Select(m => Format(key, m)));
}
