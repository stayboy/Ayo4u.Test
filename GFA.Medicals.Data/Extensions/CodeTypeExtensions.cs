namespace GFA.Medicals.Data.Extensions;

internal static class CodeTypeExtensions
{
    public static ValueCodeType ToValueCodeType(this DataValueCodeTypeUpdate value, GFAUser? user = null, ValueCodeType? updateModel = null)
    {
        updateModel ??= new()
        {
            Id = value.EntityId,
            CreatedAt = value.CreatedAt,
            CreatedByUserId = user?.Id
        };

        if (user != null)
        {
            updateModel.ModifiedByUserId = user?.Id;
        }

        updateModel.Code = value.Code;
        updateModel.CodeType = value.CodeType;
        updateModel.Name = value.Name;
        updateModel.Description = value.Description;

        return updateModel;
    }

    public static IEnumerable<ServiceValueCodeType> ToServiceValueTypeCodes(this IEnumerable<ValueCodeType> values) =>
        values.Select(x => x.ToServiceValueCodeType());

    public static ServiceValueCodeType ToServiceValueCodeType(this ValueCodeType value)
    {
        var result = new ServiceValueCodeType()
        {
            CodeType = value.CodeType,
            ValueCode = value.Code,
            ValueName = value.Name,
            Description = value.Description
        };

        return value.ToBaseEntity(result);
    }
}
