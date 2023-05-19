using System.Collections;

namespace ComponentBuilder;

/// <summary>
/// 表示包含组件的集合。
/// </summary>
public class BlazorComponentCollection : ICollection<IBlazorComponent>
{
    private readonly List<IBlazorComponent> _components = new();
    /// <summary>
    /// 初始化 <see cref="BlazorComponentCollection"/> 类的新实例。
    /// </summary>
    public BlazorComponentCollection() { }

    /// <summary>
    /// 获取组件中包含的元素数。
    /// </summary>
    public int Count => _components.Count;

    /// <summary>
    /// 集合不是只读的。
    /// </summary>
    bool ICollection<IBlazorComponent>.IsReadOnly => false;

    /// <summary>
    /// 获取或设置指定索引处的组件。
    /// </summary>
    /// <param name="index">要获取或设置的元素的从零开始的索引。</param>
    /// <returns>指定索引处的组件。</returns>
    public IBlazorComponent this[int index]
    {
        get
        {
            if (index >= Count)
            {
                throw new IndexOutOfRangeException($"{nameof(index)} is equal to or greater than {nameof(Count)}");
            }
            return _components[index];
        }

        set
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException($"{nameof(index)} is less than zero");
            }
            _components[index] = value;
        }
    }

    /// <summary>
    ///  将组件添加到集合的末尾。
    /// </summary>
    /// <param name="component">要添加到集合末尾的组件。</param>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> 是 null.</exception>
    public void Add(IBlazorComponent component)
    {
        if (component is null)
        {
            throw new ArgumentNullException(nameof(component));
        }
        _components.Add(component);
    }
    /// <summary>
    /// 从集合中删除所有组件。
    /// </summary>
    public void Clear() => _components.Clear();

    /// <summary>
    /// 确定组件是否在集合中。
    /// </summary>
    /// <param name="component">要在集合中定位的组件。</param>
    /// <returns>如果在集合中找到组件，则返回 <c>true</c>；否则返回 <c>false</c> 。</returns>
    public bool Contains(IBlazorComponent component) => _components.Contains(component);
    /// <summary>
    /// 从集合中删除特定组件的第一个出现项。
    /// </summary>
    /// <param name="component">要移除的组件。</param>
    /// <returns>如果成功移除，则返回 <c>true</c>，否则返回 <c>false</c>。如果组件在集合中没有找到，也返回 <c>false</c>。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> is null.</exception>
    public bool Remove(IBlazorComponent component)
    {
        if ( component is null )
        {
            throw new ArgumentNullException(nameof(component));
        }

        return _components.Remove(component);
    }
    /// <summary>
    /// 将一个组件插入到给定索引处的列表中。列表的大小增加1。如果需要，则在插入新组件之前将列表的容量加倍。
    /// </summary>
    /// <param name="index">要插入的索引。</param>
    /// <param name="component">要插入的组件。</param>
    /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> 小于 0。</exception>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> 是 null.</exception>
    public void Index(int index, IBlazorComponent component)
    {
        if ( index < 0 )
        {
            throw new IndexOutOfRangeException($"{nameof(index)} must be larger than zero");
        }
        if ( component is null )
        {
            throw new ArgumentNullException(nameof(component));
        }
        _components.Insert(index, component);
    }
    /// <summary>
    ///  从目标数组的指定索引处开始，将整个集合复制到兼容的一维数组。
    /// </summary>
    /// <param name="array">作为从集合复制的元素的目标的一维数组。数组必须具有从零开始的索引。
    /// </param>
    /// <param name="arrayIndex">数组中开始复制的从零开始的索引。</param>
    public void CopyTo(IBlazorComponent[] array, int arrayIndex) => _components.CopyTo(array, arrayIndex);
    /// <summary>
    /// 返回遍历集合的枚举数。
    /// </summary>
    /// <returns>可用于遍历集合的<see cref="IEnumerator{T}"/>。</returns>
    public IEnumerator<IBlazorComponent> GetEnumerator() => _components.GetEnumerator();
    /// <summary>
    ///  返回遍历集合的枚举数。
    /// </summary>
    /// <returns>可用于遍历集合的<see cref="IEnumerator"/>。</returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
