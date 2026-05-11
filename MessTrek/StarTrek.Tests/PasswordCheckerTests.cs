using PasswordChecker = StarTrek.Tests.PasswordChecker;

namespace StarTrek.Tests;

public class PasswordCheckerTests
{
    private const string StrongNoneAdminPassword = "abcd1234";
    private const string BreaksLengthRule = "abcd123";
    private const string BreaksDigitRule = "abcdabcd";
    private const string BreaksCharacterRule = "12345678";

    [Theory]
    [InlineData(PasswordChecker.PasswordStrength.Weak, BreaksDigitRule)]
    [InlineData(PasswordChecker.PasswordStrength.Strong, StrongNoneAdminPassword)]
    [InlineData(PasswordChecker.PasswordStrength.Weak, BreaksLengthRule)]
    [InlineData(PasswordChecker.PasswordStrength.Weak, "")]
    [InlineData(PasswordChecker.PasswordStrength.Weak, BreaksCharacterRule)]
    public void CheckWeakPassword_MissingDigit(PasswordChecker.PasswordStrength expected, string input)
    {
        Assert.Equal(expected, PasswordChecker.Check(input));
    }

    [Fact]
    public void PasswordChecker2_Simple_Weak()
    {
        var passwordChecker2 = PasswordChecker.PasswordChecker2("");

        Assert.Equal(PasswordChecker.PasswordStrength.Weak, passwordChecker2.Verdict);
    }

    [Theory]
    [InlineData(BreaksLengthRule, "To short, hon!")]
    [InlineData(BreaksDigitRule, "N0 numb3rzzz!")]
    [InlineData(BreaksCharacterRule, "Missing 4lph4num3ric bro0!")]
    public void PasswordChecker2_BreaksLengthRule_Weak(string password, string expectedReason)
    {
        var passwordChecker2 = PasswordChecker.PasswordChecker2(password);

        Assert.Equal([expectedReason], passwordChecker2.Reasons);
    }

    [Fact]
    public void PasswordChecker2_Strong_Strong()
    {
        var passwordChecker2 = PasswordChecker.PasswordChecker2(StrongNoneAdminPassword);

        Assert.Equal(PasswordChecker.PasswordStrength.Strong, passwordChecker2.Verdict);
    }

    [Theory]
    [InlineData('!')]
    [InlineData('*')]
    [InlineData('0')]
    [InlineData('1')]
    public void PasswordChecker2_Strong_Admin_Strong(char specialChar)
    {
        var passwordChecker2 = PasswordChecker.PasswordChecker2("12345678!a" + specialChar, PasswordChecker.PasswordRequirements.Admin);

        Assert.Equal(PasswordChecker.PasswordStrength.Strong, passwordChecker2.Verdict);
    }

    [Fact]
    public void PasswordChecker2_AdminTooShort_Weak()
    {
        var passwordChecker2 = PasswordChecker.PasswordChecker2("12345678a!", PasswordChecker.PasswordRequirements.Admin);

        Assert.Equal(PasswordChecker.PasswordStrength.Weak, passwordChecker2.Verdict);
    }    
    
    [Fact]
    public void PasswordChecker2_NoSpecialChars_Weak()
    {
        var passwordChecker2 = PasswordChecker.PasswordChecker2("123456789a0", PasswordChecker.PasswordRequirements.Admin);

        Assert.Equal(PasswordChecker.PasswordStrength.Weak, passwordChecker2.Verdict);
    }
    
    [Fact]
    public void PasswordChecker2_AdminWithoutSpecialChar_Weak()
    {
        var passwordChecker2 = PasswordChecker.PasswordChecker2("123456789aa", PasswordChecker.PasswordRequirements.Admin);

        Assert.Equal(PasswordChecker.PasswordStrength.Weak, passwordChecker2.Verdict);
    }
    
    [Fact]
    public void PasswordChecker2_AdminNotEndingWithSpecialCharacter_Weak()
    {
        var passwordChecker2 = PasswordChecker.PasswordChecker2("123456789!a", PasswordChecker.PasswordRequirements.Admin);

        Assert.Equal(PasswordChecker.PasswordStrength.Weak, passwordChecker2.Verdict);
    }
    
}

