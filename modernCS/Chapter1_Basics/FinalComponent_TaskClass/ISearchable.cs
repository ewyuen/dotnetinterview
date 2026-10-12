public interface ISearchable<T>
{
    IEnumerable<T> Search(string query);
}