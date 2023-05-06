namespace GithubFeatured.Infra.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
    }
}
