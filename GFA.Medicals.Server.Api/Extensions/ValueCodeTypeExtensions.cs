
namespace GFA.Medicals.Server.Api.Extensions;

internal static class ValueCodeTypeExtensions
{
    public static IEnumerable<ApiValueCodeType> ToApiValueCodeTypes(this IEnumerable<ServiceValueCodeType> codes) =>
        codes.Select(x => x.ToApiValueCodeType());

    public static ApiValueCodeType ToApiValueCodeType(this ServiceValueCodeType code)
    {

        ApiValueCodeType result = new()
        {
            Code = code.ValueCode,
            Description = code.Description,
            CodeType = code.CodeType,
            Name = code.ValueName
        };

        return result.ToApiBaseEntity(code);
    }

    public static DataValueCodeTypeUpdate ToDataValueCodeTypeUpdate(this ApiValueCodeTypeChange value, DateTime now, ServiceGFAUser user) =>
        new(value.Id, value.Created ?? now)
        {
            GFAUser = user,
            Code = value.Code,
            CodeType = value.CodeType,
            Description = value.Description,
            Name = value.Name
        };

    public static ValueCodeTypeSearchParameters ToValueCodeTypeSearchParameters(this ApiValueCodeTypeSearchParameters parameters) =>
        new()
        {
            Ids = parameters.ids,
            SearchText = parameters.search,
            IsDeleted  = parameters.deleted,
            Top = parameters.top,
            Codes = parameters.childcodes,
            CodeTypes = parameters.parentcodes
        };
}
