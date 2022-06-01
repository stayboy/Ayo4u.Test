using Ayo4u.Infrastructure.Models;
using Ayo4u.Infrastructure.Queries;

namespace Ayo4u.Server.Api.Extensions
{
    internal static class RequestLogExtensions
    {
        public static async Task<ApiRequestChange?> ToApiUnitRequestChange(this ApiUnitConversionInput input, DateTime now, Func<int, Task<ApiUnitConverter>> handler)
        {
            var convert = await handler.Invoke(input.ConvertUnitId);

            if (convert == null) return default;

            return new(0, now)
            {
                IsSuccess = true,
                ConversionId = convert.Id,
                InputValue = input.InValue,
                OutputValue = convert.Multiplier * input.InValue,
                InUnitType = convert.InUnitType,
                OutUnitType = convert.OutUnitType
            };
        }

        public static async Task<ApiRequestChange> ToApiRequestChange(this ApiConversionInput input, DateTime now, 
            Func<string, string, Task<ApiUnitConverter>> handler)
        {
            var convert = await handler.Invoke(input.InputType, input.OutputType);

            float? outValue = null;
            bool issuccess = false;

            if (convert != null)
            {
                issuccess = true;

                if (convert.Formula?.Id > 0)
                {
                    outValue = ApiValueTypeRecord<Formulae>.GetEnumApiValueTypeRecordFromInt(convert.Formula.Id)?.Id switch
                    {
                        Formulae.Fahrenheit_To_Celcius => input.InValue.ToCelcius(),
                        Formulae.Celcius_To_Fahrenheit => input.InValue.ToFahrenheit(),
                        _ => throw new ArgumentException("Invalid Formula")
                    };
                }
                else
                {
                    outValue = convert.Multiplier * input.InValue;
                }
            }

            return new(0, now)
            {
                IsSuccess = issuccess,
                ConversionId = convert?.Id,
                InputValue = input.InValue,
                OutputValue = outValue,
                InUnitType = input.InputType,
                OutUnitType = input.OutputType
            };
        }

        public static IEnumerable<ApiRequestAction> ToApiRequestActions(this IEnumerable<ServiceRequestAction> requests) =>
            requests.Select(x => x.ToApiRequestAction());

        public static ApiRequestAction ToApiRequestAction(this ServiceRequestAction request)
        {
            var result = new ApiRequestAction()
            {
                ConversionId = request.ConversionId,
                InputValue = request.InputValue,
                OutputValue = request.OutputValue,
                InUnitType = request.InUnitType,
                OutUnitType = request.OutUnitType,
                IsSuccess = request.IsSuccess,
                Remarks = request.Remarks,
                RequestAyoUser = request.RequestAyoUser.ToApiAyoUser()
            };

            return result.ToApiBaseEntity(request);
        }

        public static RequestActionSearchParameters ToRequestActionSearchParameters(this ApiRequestLogSearchParameters parameters)
        {
            return new()
            {
                Ids = parameters.ids,
                EndCreatedAt = parameters.toCreatedAt,
                StartCreatedAt = parameters.FromCreatedAt,
                InUnitType = parameters.inUnit,
                OutUnitType = parameters.outUnit,
                IsDeleted = parameters.deleted,
                RequestAyoUserId = parameters.userId,
                RequesterName = parameters.userName
            };
        }

        public static DataRequestActionUpdate ToDataRequestActionChange(this ApiRequestChange change, DateTime now, ApiAyoUser? user = null)
        {
            return new(change.Id, change.Created ?? now, change.IsSuccess)
            {
                AyoUser = user?.ToServiceAyoUser(),
                RequestAyoUserId = user?.Id,
                ConversionId = change.ConversionId,
                InputValue = change.InputValue,
                OutputValue = change.OutputValue,
                InputType = change.InUnitType,
                OutputType = change.OutUnitType
            };
        }
    }
}
