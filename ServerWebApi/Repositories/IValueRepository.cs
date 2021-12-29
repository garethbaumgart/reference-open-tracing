namespace ServerWebApi.Repositories
{
    public interface IValueRepository
    {
        Task<IEnumerable<string>> GetValues();
    }
}
