namespace Ayo4u.Infrastructure.Repositories;

public interface IRequestActionRepository
{
    Task<ServiceRequestAction?> GetAsync(int id);

    Task<EntityResult<ServiceRequestAction>> AddUpdateRequestAction(DataRequestActionUpdate request);

    Task<IEnumerable<ServiceRequestAction>> BrowseAsync(RequestActionSearchParameters parameters);

    Task<EnumerableEntityResult<ServiceRequestAction>> DeleteLogs(int[] ids, BlockStatus status);
     
}
