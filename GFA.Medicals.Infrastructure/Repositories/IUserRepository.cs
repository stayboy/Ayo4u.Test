namespace GFA.Medicals.Infrastructure.Repositories;

public interface IUserRepository : IGUIDRepository<ServiceGFAUser, GFAUserSearchParameters, DataGFAUserUpdate>
{
    Task<ServiceGFAUser?> GetAsync(string email);
}
