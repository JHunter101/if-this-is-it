using Shouldly;

namespace Test.Shared;

public static class EnumExtensionTest
{
    public static void VerifyCanProcessAllKnown<TEnum>(Action<TEnum> action)
        where TEnum : struct, Enum
    {
        foreach (TEnum value in Enum.GetValues<TEnum>())
        {
            Should.NotThrow(() => action(value));
        }
    }

    public static void VerifyInvalidValueThrows<TEnum>(Action<TEnum> extension)
        where TEnum : struct, Enum
    {
        var invalidValue = (TEnum)Enum.ToObject(typeof(TEnum), sbyte.MaxValue);
        Enum.IsDefined(invalidValue).ShouldBeFalse();
        Should.Throw<ArgumentOutOfRangeException>(() => extension(invalidValue));
    }
}
