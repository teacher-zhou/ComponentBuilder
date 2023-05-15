using System.Collections;

namespace ComponentBuilder.Automation;

/// <summary>
/// Represents a collectio that contains component.
/// </summary>
public class BlazorComponentCollection : ICollection<IBlazorComponent>
{
    private readonly List<IBlazorComponent> _components = new();
    /// <summary>
    /// Initializes a new instance of <see cref="BlazorComponentCollection"/> class.
    /// </summary>
    public BlazorComponentCollection() { }

    /// <summary>
    /// Initializes a new instance of <see cref="BlazorComponentCollection"/> class that contains components copied from the specified components and has sufficient capacity to accommodate the number of elements copied.
    /// </summary>
    /// <param name="components">The collection whose components are copied to the new list.</param>
    /// <exception cref="ArgumentNullException"><paramref name="components"/> is null.</exception>
    public BlazorComponentCollection(IEnumerable<IBlazorComponent> components)
    {
        if (components is null)
        {
            throw new ArgumentNullException(nameof(components));
        }

        _components.AddRange(components);
    }
    /// <summary>
    /// Gets the number of elements contained int the components.
    /// </summary>
    public int Count => _components.Count;

    /// <summary>
    /// The collection is not read-only.
    /// </summary>
    bool ICollection<IBlazorComponent>.IsReadOnly => false;

    /// <summary>
    /// Gets or sets the component at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <returns>The component at the specified index.</returns>
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
    ///  Adds a component to the end of the colletion.
    /// </summary>
    /// <param name="component">The component to be added to the end of the collection.</param>
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
    /// Determines whether a component is in the collection.
    /// </summary>
    /// <param name="component">The component to locate in the</param>
    /// <returns><c>true</c> if item is found in the <see cref="BlazorComponentCollection"/>; otherwise, <c>false</c>.</returns>
    public bool Contains(IBlazorComponent component) => _components.Contains(component);
    /// <summary>
    /// Removes the first occurrence of a specific component from the <see cref="BlazorComponentCollection"/>.
    /// </summary>
    /// <param name="component"> The component to remove from the <see cref="BlazorComponentCollection"/>.</param>
    /// <returns><c>true</c> if item is successfully removed; otherwise, <c>false</c>. This method also returns <c>false</c> if item was not found in the <see cref="BlazorComponentCollection"/>.</returns>
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
    /// Inserts an element into this list at a given index. The size of the list
    /// is increased by one. If required, the capacity of the list is doubled
    /// before inserting the new element.
    /// </summary>
    /// <param name="index">The index insert.</param>
    /// <param name="component">The component to insert.</param>
    /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> less than zero.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> is null.</exception>
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
    ///  Copies the entire <see cref="BlazorComponentCollection"/> to a compatible one-dimensional array, starting at the specified index of the target array.
    /// </summary>
    /// <param name="array"> The one-dimensional System.Array that is the destination of the elements copied
    /// from <see cref="BlazorComponentCollection"/>. The System.Array must have zero-based indexing.
    /// </param>
    /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
    public void CopyTo(IBlazorComponent[] array, int arrayIndex) => _components.CopyTo(array, arrayIndex);
    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>An <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.</returns>
    public IEnumerator<IBlazorComponent> GetEnumerator() => _components.GetEnumerator();
    /// <summary>
    ///  Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/> that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
