namespace ComponentBuilder.FluentClass;

/// <summary>
/// 构建丝滑 CSS 类的抽象实现。
/// <para>
/// 该类支持为流畅CSS类构建自定义规则，例如：
/// <code language="html">
/// &lt;Component class="Margin.Is3.OnTop.FromXL.Is4">...&lt;/Component>
/// class : mt-xl-3 m-4
/// </code>
/// </para>
/// </summary>
/// <typeparam name="TKey">主键类型。</typeparam>
/// <typeparam name="TRule">规则类型。</typeparam>
public abstract class FluentClassProvider<TKey, TRule> : IFluentClassProvider where TKey : notnull
{
    private readonly List<string> _classList;
    private readonly IDictionary<TKey, List<TRule>> _rules;
    /// <summary>
    /// 初始化 <see cref="FluentClassProvider{TKey,TRule}"/> 类的新实例。
    /// </summary>
    protected FluentClassProvider()
    {
        _classList = new List<string>();
        _rules = new Dictionary<TKey, List<TRule>>();
    }

    /// <summary>
    /// 获取存储的规则。
    /// </summary>
    protected IEnumerable<KeyValuePair<TKey, List<TRule>>> Rules => _rules;

    /// <summary>
    /// 获取一个布尔值，表示规则已经设置。
    /// </summary>
    protected bool IsDirty { get; private set; }

    /// <summary>
    /// 获取当前的键。
    /// </summary>
    protected TKey CurrentKey { get; private set; }

    /// <inheritdoc/>
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
    /// 代表规则是通过流畅的调用来设定的。
    /// </summary>
    protected void Dirty() => IsDirty = true;

    /// <summary>
    /// 为 <see cref="CurrentKey"/> 添加一个新规则。同一个键可以有多个规则。
    /// </summary>
    /// <param name="rule">要添加的规则。</param>
    /// <param name="ignoreIfDuplicate">当在同一键中有相同的规则时，忽略添加规则。</param>
    /// <exception cref="ArgumentNullException"><paramref name="rule"/> 是 <c>null</c>。</exception>
    /// <exception cref="InvalidOperationException"><see cref="CurrentKey"/> 是 null 并执行 <see cref="ChangeKey(TKey)"/> 至少一次。</exception>
    protected virtual void AddRule(TRule rule, bool ignoreIfDuplicate = true)
    {
        if ( rule is null )
        {
            throw new ArgumentNullException(nameof(rule));
        }

        if ( CurrentKey is null )
        {
            throw new InvalidOperationException($"请确定 {nameof(ChangeKey)} 至少执行了一次");
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
    /// 更改当前键。如果在规则中没有找到指定的键，将添加新的键规则。
    /// </summary>
    /// <param name="key">改变的键。</param>
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
    /// 为每个规则格式化一个CSS类字符串。
    /// </summary>
    /// <param name="key">规则的键。</param>
    /// <param name="rule">每个规则的值。</param>
    /// <returns>格式化后的 CSS 类的字符串。</returns>
    protected abstract string? Format(TKey key, TRule rule);

    /// <summary>
    /// 格式化不带任何规则的CSS类字符串。
    /// </summary>
    /// <param name="key">规则的键。</param>
    /// <returns>格式化后的 CSS 类的字符串。</returns>
    protected abstract string? Format(TKey key);

    /// <summary>
    /// 用指定键和值的规则格式化CSS类字符串。
    /// </summary>
    /// <param name="key">规则的键。</param>
    /// <param name="rules">规则集合。</param>
    /// <returns>格式化后的 CSS 类的字符串。</returns>
    protected virtual string? Format(TKey key, IEnumerable<TRule> rules) => string.Join(" ", rules.Select(m => Format(key, m)));
}
