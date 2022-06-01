namespace Ayo4u.Web.Shared.Models;

public record ApiValueTypeRecord<T>(T Id, string? Name) where T: struct
{
    public string? Code { get; set; }

    public static ApiValueTypeRecord<T> GetEnumApiValueTypeRecord(T value)
    {
        return new(value, value.ToString());
    }

    public static ApiValueTypeRecord<int> GetIntApiValueTypeRecord(T value)
    {
        return new(Convert.ToInt32(value), value.ToString());
    }

    public static ApiValueTypeRecord<T> GetEnumApiValueTypeRecordFromInt(int value)
    {
        var type = typeof(T);

        var cType = (T)Enum.ToObject(type, value);

        return new(cType, Enum.GetName(type, value));
    }

}


