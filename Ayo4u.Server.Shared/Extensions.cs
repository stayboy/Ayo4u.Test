using System.Reflection;

namespace Server.Shared;

internal static class Extensions
{
    public static HashSet<KeyValuePair<string, string>>? GetStaticPropertyFields(Type param, string name, string value)
    {
        var constants = new HashSet<KeyValuePair<string, string>>();

        if (!string.IsNullOrWhiteSpace(name))
        {
            var sField = param.GetField(name, BindingFlags.Public | BindingFlags.Static);

            if (sField != null)
            {
                return new HashSet<KeyValuePair<string, string>> { new KeyValuePair<string, string>(sField.Name, (string)sField.GetValue(null)!) };
            }

            return default;
        }

        var fields = param.GetFields(BindingFlags.Public | BindingFlags.Static);

        if (!string.IsNullOrWhiteSpace(value)) fields = fields.Where(q => string.Equals((string)q.GetValue(null)!, value)).ToArray();

        foreach (var constant in fields)
        {
            if (constant.IsLiteral && !constant.IsInitOnly)
                constants.Add(new(constant.Name.Replace('_', ' '), (string)constant.GetValue(null)!));
        }

        return constants;
    }

}
