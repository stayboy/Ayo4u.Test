namespace Ayo4u.Server.Api.Extensions;

internal static class FormulaExtensions
{
    public static float ToCelcius(this float input)
    {
        return (input - 32) * 5/9;
    }

    public static float ToFahrenheit(this float input)
    {
        return (input * 9/5) + 32;
    }
}
