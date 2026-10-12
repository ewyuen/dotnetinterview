public class Library<T> : ICollection<T>, ISearchable<T>
{
    private List<T> _items = new List<T>();

    public int Count => _items.Count;

    public bool IsReadOnly => false;

    public void Add(T item)
    {
        _items.Add(item);
    }

    public void Clear()
    {
        _items.Clear();
    }

    public bool Contains(T item)
    {
        return _items.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _items.CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    public bool Remove(T item)
    {
        return _items.Remove(item);
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    public IEnumerable<T> Search(string query)
    {
        // For simplicity, assume T is Book and search by title or author
        if (typeof(T) == typeof(Book))
        {
            return _items.Where(item =>
            {
                var book = item as Book;
                return book != null && (book.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                                        book.Author.Contains(query, StringComparison.OrdinalIgnoreCase));
            });
        }
        return Enumerable.Empty<T>();
    }

    public void GetAll()
    {
        foreach (var item in _items)
        {
            if (item is Book book)
            {
                book.DisplayInfo();
            }
            else if (item is Vehicle vehicle)
            {
                vehicle.DisplayInfo();
            }
        }
    }
}