using Shouldly;
using Test.Shared;

namespace Core.Test;

[TestClass]
public sealed class EnumExtensionTestTests
{
    [TestMethod]
    public void VerifyInvalidValueThrows_SupportsAllEnumUnderlyingTypes()
    {
        VerifyExtensionIsInvoked<ByteEnum>();
        VerifyExtensionIsInvoked<SByteEnum>();
        VerifyExtensionIsInvoked<ShortEnum>();
        VerifyExtensionIsInvoked<UShortEnum>();
        VerifyExtensionIsInvoked<IntEnum>();
        VerifyExtensionIsInvoked<UIntEnum>();
        VerifyExtensionIsInvoked<LongEnum>();
        VerifyExtensionIsInvoked<ULongEnum>();
    }

    private static void VerifyExtensionIsInvoked<TEnum>() where TEnum : struct, Enum
    {
        bool invoked = false;
        EnumExtensionTest.VerifyInvalidValueThrows<TEnum>(value =>
        {
            Enum.IsDefined(value).ShouldBeFalse();
            invoked = true;
            throw new ArgumentOutOfRangeException(nameof(value));
        });
        invoked.ShouldBeTrue();
    }

    private enum ByteEnum : byte { Known }
    private enum SByteEnum : sbyte { Known }
    private enum ShortEnum : short { Known }
    private enum UShortEnum : ushort { Known }
    private enum IntEnum : int { Known }
    private enum UIntEnum : uint { Known }
    private enum LongEnum : long { Known }
    private enum ULongEnum : ulong { Known }
}
