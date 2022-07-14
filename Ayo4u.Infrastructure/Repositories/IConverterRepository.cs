namespace Ayo4u.Infrastructure.Repositories;

public interface IConverterRepository
{
    Task<ServiceUnitConverter> GetAsync(int id);

    Task<ServiceUnitConverter> GetAsync(string inputType, string outputType);

    Task<EntityResult<ServiceUnitConverter>> AddUpdateConverter(DataUnitConverterUpdate converter);

    Task<IEnumerable<ServiceUnitConverter>> BrowseAsync(UnitConverterSearchParameters parameters);

    Task<EntityResult<ServiceUnitConverter>> SetStatus(int id, BlockStatus status, DataUnitConverterUpdate? converter = null);
}
