namespace Ayo4u.Web.Shared.Models;

public record ApiConversionInput (float InValue, string InputType, string OutputType);

public record ApiUnitConversionInput (float InValue, int ConvertUnitId);
