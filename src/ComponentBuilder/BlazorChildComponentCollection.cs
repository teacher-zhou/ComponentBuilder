using System.Collections;

namespace ComponentBuilder;

/// <summary>
/// Represents a collection containing components.
/// </summary>
public class BlazorComponentCollection : ICollection<IBlazorComponent>
{
    /// <summary>
    /// Initialize a new instance of the <see cref="BlazorComponentCollection"/> class.
    /// </summary>
    internal BlazorComponentCollection() => _components = [];

    private List<IBlazorComponent> _components;

    /// <summary>
    /// Gets the number of elements contained in the component.
    /// </summary>
    public int Count => _components.Count;

    bool ICollection<IBlazorComponent>.IsReadOnly => false;

    /// <summary>
    /// Gets or sets the component at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <returns>Specifies the component at the index.</returns>
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
    ///  Adds the component to the end of the collection.
    /// </summary>
    /// <param name="component">The component to add to the end of the collection.</param>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> is null.</exception>
    public void Add(IBlazorComponent component)
    {
        if (component is null)
        {
            throw new ArgumentNullException(nameof(component));
        }
        _components.Add(component);
    }
    /// <summary>
    /// Removes all components from the collection.
    /// </summary>
    public void Clear() => _components.Clear();

    /// <summary>
    /// Determines whether the component is in the collection.
    /// </summary>
    /// <param name="component">Components to locate in the collection.</param>
    /// <returns>If a component is found in the collection, return <c>true</c>; Otherwise, return <c>false</c>.</returns>
    public bool Contains(IBlazorComponent component) => _components.Contains(component);
    /// <summary>
    /// Removes the first occurrence of a specific component from the collection.
    /// </summary>
    /// <param name="component">Component to remove.</param>
    /// <returns>Return <c>true</c> if successfully removed, otherwise <c>false</c>. Also return <c>false</c> if the component is not found in the collection.</returns>
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
    /// Inserts a component into the list at the given index. Increase the size of the list by 1. If necessary, double the capacity of the list before inserting a new component.
    /// </summary>
    /// <param name="index">The index to insert.</param>
    /// <param name="component">The component to insert.</param>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> is null.</exception>
    /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> less than 0。</exception>
    public void Index(int index, IBlazorComponent component)
    {
        if ( index < 0 )
        {
            throw new IndexOutOfRangeException($"{nameof(index)} must be larger than zero");
        }
        ArgumentNullException.ThrowIfNull(component);
        _components.Insert(index, component);
    }
    /// <inheritdoc/>
    public void CopyTo(IBlazorComponent[] array, int arrayIndex) => _components.CopyTo(array, arrayIndex);
    /// <inheritdoc/>
    public IEnumerator<IBlazorComponent> GetEnumerator() => _components.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
