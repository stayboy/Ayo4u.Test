using Ayo4u.Infrastructure.Models;
using Ayo4u.Infrastructure.Queries;

namespace Ayo4u.Server.Api.Extensions;

internal static class ConverterExtensions
{
    public static IEnumerable<ApiUnitConverter> ToApiUnitConverters(this IEnumerable<ServiceUnitConverter> converters) =>
        converters.Select(x => x.ToApiUnitConverter());

    public static ApiUnitConverter ToApiUnitConverter(this ServiceUnitConverter converter)
    {
        ApiValueTypeRecord<int>? apiFormulae = null;
        if (converter.Formula != null)
        {
            apiFormulae = ApiValueTypeRecord<Formulae>.GetIntApiValueTypeRecord(converter.Formula.Value);
        }

        var result = new ApiUnitConverter()
        {
            Multiplier = converter.Multiplier,
            InUnitType = converter.InUnitType,
            OutUnitType = converter.OutUnitType,
            Formula = apiFormulae
        };

        return result.ToApiBaseEntity(converter);
    }

    public static DataUnitConverterUpdate ToDataUnitConverterUpdate(this ApiUnitConverterChange change, DateTime now, ServiceAyoUser? user = null)
    {
        Formulae? formula = null;
        if (change.Formula > 0)
        {
            formula = ApiValueTypeRecord<Formulae>.GetEnumApiValueTypeRecordFromInt(change.Formula.Value)?.Id;
        }

        return new(change.Id, change.Created ?? now)
        {
            InUnitType = change.InUnitType,
            OutUnitType = change.OutUnitType,
            Multiplier = change.Multiplier,
            Formula = formula,
            AyoUser = user
        };
    }

    public static UnitConverterSearchParameters ToConversionSearchParameters(this ApiConverterSearchParameters parameters)
    {
        return new()
        {
            Ids = parameters.ids,
            OutUnitType = parameters.outUnit,
            InUnitType = parameters.inUnit,
            IsDeleted = parameters.deleted,
            IncludeTopLogs = parameters.loadTopLogs,
            SearchText = parameters.search
        };
    }
}
