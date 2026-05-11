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

    [Theory]
    [InlineData(BreaksLengthRule, "To short, hon!")]
    [InlineData(BreaksDigitRule, "N0 numb3rzzz!")]
    [InlineData(BreaksCharacterRule, "Missing 4lph4num3ric bro0!")]
    public void PasswordChecker2_BreaksLengthRule_Weak(string password, string expectedReason)
    {
        var passwordChecker2 = PasswordChecker.PasswordChecker2(password);

        Assert.Equal([expectedReason], passwordChecker2.Reasons);
    }

    [Theory]
    [InlineData(StrongNoneAdminPassword, PasswordChecker.PasswordStrength.Strong)]
    [InlineData("", PasswordChecker.PasswordStrength.Weak)]
    public void PasswordChecker2_Strong_Strong(string password, PasswordChecker.PasswordStrength expected)
    {
        var passwordChecker2 = PasswordChecker.PasswordChecker2(password);

        Assert.Equal(expected, passwordChecker2.Verdict);
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
    
    [Theory]
    [InlineData("12345678a!",  PasswordChecker.PasswordStrength.Weak, "To short, hon!")]
    [InlineData("12345a!",     PasswordChecker.PasswordStrength.Weak, "To short, hon!")]
    [InlineData("123456789a0", PasswordChecker.PasswordStrength.Weak, "Missing special character")]
    [InlineData("123456789!a", PasswordChecker.PasswordStrength.Weak, "Missing special character in the end, br0!")]
    public void PasswordChecker2_AdminVersion_Strong(string password, PasswordChecker.PasswordStrength expected, string expectedReason)
    {
        var passwordChecker2 = PasswordChecker.PasswordChecker2(password, PasswordChecker.PasswordRequirements.Admin);

        Assert.Equal(expected, passwordChecker2.Verdict);
        Assert.Equal([expectedReason], passwordChecker2.Reasons);
    }    
    
    [Fact]
    public void PasswordChecker2_AdminVersion_MultipleFailures_Strong()
    {
        var passwordChecker2 = PasswordChecker.PasswordChecker2("123456789aa", PasswordChecker.PasswordRequirements.Admin);

        Assert.Equal(PasswordChecker.PasswordStrength.Weak, passwordChecker2.Verdict);
        Assert.Equal(["Missing special character", "Missing special character in the end, br0!"], passwordChecker2.Reasons);
    }
}

