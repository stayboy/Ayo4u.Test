namespace Ayo4u.Data.Extensions;

internal static class RequestActionExtensions
{
    public static IEnumerable<ServiceRequestAction> ToServiceRequestActions(this IEnumerable<RequestAction> values) =>
        values.Select(x => x.ToServiceRequestAction());

    public static ServiceRequestAction ToServiceRequestAction(this RequestAction value)
    {
        var result = new ServiceRequestAction()
        {
            RequestAyoUser = value.RequestAyoUser!.ToServiceAyoUser(),

            ConversionId = value.ConversionId,

            InUnitType = value.InputType,
            OutUnitType = value.OutputType,

            IsSuccess = value.IsSuccess,

            InputValue = value.InputValue,
            OutputValue = value.OutputValue,

            Remarks = value.LogMessage                
        };

        return value.ToBaseEntity(result);
    }

    public static RequestAction ToRequestAction(this DataRequestActionUpdate value, AyoUser? user = null, RequestAction? updateModel = null)
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

        updateModel.RequestAyoUserId = value.RequestAyoUserId;
        updateModel.ConversionId = value.ConversionId;

        updateModel.InputValue = value.InputValue;
        updateModel.OutputValue = value.OutputValue;

        updateModel.InputType = value.InputType;
        updateModel.OutputType = value.OutputType;

        updateModel.IsSuccess = value.IsSuccess;
        updateModel.LogMessage = value.Remarks;

        return updateModel;
    }
}
