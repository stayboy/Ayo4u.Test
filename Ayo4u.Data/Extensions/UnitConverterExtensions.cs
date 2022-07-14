namespace Ayo4u.Data.Extensions
{
    internal static class UnitConverterExtensions
    {
        public static IEnumerable<ServiceUnitConverter> ToServiceUnitConverters(this IEnumerable<UnitConverter> values) =>
            values.Select(x => x.ToServiceUnitConverter());

        public static ServiceUnitConverter ToServiceUnitConverter(this UnitConverter value)
        {
            var result = new ServiceUnitConverter()
            {
                 InUnitType = value.InUnitType,
                 OutUnitType = value.OutUnitType,
                 Multiplier = value.Multiplier,
                 Formula = value.Formula
            };

            if (value.RequestLogs != null)
            {
                result.RequestLogs = value.RequestLogs.ToServiceRequestActions();
            }

            return value.ToBaseEntity(result);
        }

        public static UnitConverter ToUnitConverter(this DataUnitConverterUpdate value, AyoUser? user, UnitConverter? updateModel = null)
        {
            updateModel ??= new()
            {
                Id = value.Id,
                CreatedAt = value.CreatedAt,
                CreatedByUserId = user?.Id
            };

            if (user != null)
            {
                updateModel.ModifiedByUserId = user?.Id;
            }

            updateModel.InUnitType = value.InUnitType;
            updateModel.OutUnitType = value.OutUnitType;
            updateModel.Multiplier = value.Multiplier;
            updateModel.Formula = value.Formula;

            return updateModel;
        }
    }
}
