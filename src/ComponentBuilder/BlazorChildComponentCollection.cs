using System.Collections;

namespace ComponentBuilder;

/// <summary>
/// Represents a collectio that contains component.
/// </summary>
/// <typeparam name="TComponent">The type of component.</typeparam>
public class BlazorComponentCollection<TComponent> : ICollection<TComponent> where TComponent : ComponentBase
{
    private readonly List<TComponent> _components = new();
    /// <summary>
    /// Initializes a new instance of <see cref="BlazorComponentCollection{TComponent}"/> class.
    /// </summary>
    public BlazorComponentCollection() { }

    /// <summary>
    /// Initializes a new instance of <see cref="BlazorComponentCollection{TComponent}"/> class that contains components copied from the specified components and has sufficient capacity to accommodate the number of elements copied.
    /// </summary>
    /// <param name="components">The collection whose components are copied to the new list.</param>
    /// <exception cref="ArgumentNullException"><paramref name="components"/> is null.</exception>
    public BlazorComponentCollection(IEnumerable<TComponent> components)
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
    bool ICollection<TComponent>.IsReadOnly => false;

    /// <summary>
    /// Gets or sets the component at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <returns>The component at the specified index.</returns>
    public TComponent this[int index]
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
    /// <exception cref="InvalidOperationException">A same component is already in component collection.</exception>
    public void Add(TComponent component)
    {
        if (component is null)
        {
            throw new ArgumentNullException(nameof(component));
        }

        if (Contains(component))
        {
            throw new InvalidOperationException("A same component is already in collection");
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
    /// <returns><c>true</c> if item is found in the <see cref="BlazorComponentCollection{TComponent}"/>; otherwise, <c>false</c>.</returns>
    public bool Contains(TComponent component) => _components.Contains(component);
    /// <summary>
    ///  Copies the entire <see cref="BlazorComponentCollection{TComponent}"/> to a compatible one-dimensional array, starting at the specified index of the target array.
    /// </summary>
    /// <param name="array"> The one-dimensional System.Array that is the destination of the elements copied
    /// from <see cref="BlazorComponentCollection{TComponent}"/>. The System.Array must have zero-based indexing.
    /// </param>
    /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
    public void CopyTo(TComponent[] array, int arrayIndex) => _components.CopyTo(array, arrayIndex);
    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>An <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.</returns>
    public IEnumerator<TComponent> GetEnumerator() => _components.GetEnumerator();
    /// <summary>
    /// Removes the first occurrence of a specific component from the <see cref="BlazorComponentCollection{TComponent}"/>.
    /// </summary>
    /// <param name="component"> The component to remove from the <see cref="BlazorComponentCollection{TComponent}"/>.</param>
    /// <returns><c>true</c> if item is successfully removed; otherwise, <c>false</c>. This method also returns <c>false</c> if item was not found in the <see cref="BlazorComponentCollection{TComponent}"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> is null.</exception>
    public bool Remove(TComponent component)
    {
        if (component is null)
        {
            throw new ArgumentNullException(nameof(component));
        }

        return _components.Remove(component);
    }
    /// <summary>
    ///  Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/> that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
