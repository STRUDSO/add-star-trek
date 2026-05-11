namespace StarTrek.Tests;

public class PasswordCheckerTests
{
    private const string StrongNoneAdminPassword = "abcd1234";
    private const string BreaksLengthRule = "abcd123";
    private const string BreaksDigitRule = "abcdabcd";
    private const string BreaksCharacterRule = "12345678";
    private const string StrongAdminPassword = "123456789a!";

    [Theory]
    [InlineData(PasswordStrength.Weak, BreaksDigitRule)]
    [InlineData(PasswordStrength.Strong, StrongNoneAdminPassword)]
    [InlineData(PasswordStrength.Weak, BreaksLengthRule)]
    [InlineData(PasswordStrength.Weak, "")]
    [InlineData(PasswordStrength.Weak, BreaksCharacterRule)]
    public void CheckWeakPassword_MissingDigit(PasswordStrength expected, string input)
    {
        Assert.Equal(expected, PasswordChecker(input));
    }
    
    /*
     *      
       £ Some clients want to be able to pass a Boolean “Admin” flag
       
       to the API and, if true, the password must also…
       £ Be > 10 characters long.
       £ Contain a special character.
       £ Have a special character or digit as the last character.
     */

    [Fact]
    public void PasswordChecker2_Simple_Weak()
    {
        var passwordChecker2 = PasswordChecker2("");
        
        Assert.Equal(PasswordStrength.Weak, passwordChecker2.Verdict);
    }
    
    [Theory]
    [InlineData(BreaksLengthRule, "To short, hon!")]
    [InlineData(BreaksDigitRule, "N0 numb3rzzz!")]
    [InlineData(BreaksCharacterRule, "Missing 4lph4num3ric bro0!")]
    public void PasswordChecker2_BreaksLengthRule_Weak(string password, string expectedReason)
    {
        var passwordChecker2 = PasswordChecker2(password);
        
        Assert.Equal([expectedReason], passwordChecker2.Reasons);
    }
    
    [Fact]
    public void PasswordChecker2_Strong_Strong()
    {
        var passwordChecker2 = PasswordChecker2(StrongNoneAdminPassword);
        
        Assert.Equal(PasswordStrength.Strong, passwordChecker2.Verdict);
    }

    [Fact]
    public void PasswordChecker2_Strong_Admin_Strong()
    {
        var passwordChecker2 = PasswordChecker2(StrongAdminPassword, PasswordRequirements.Admin);
        
        Assert.Equal(PasswordStrength.Strong, passwordChecker2.Verdict);
    }
    
    [Fact]
    public void PasswordChecker2_Strong_Admin_Weak()
    {
        var passwordChecker2 = PasswordChecker2("12345678a!", PasswordRequirements.Admin);
        
        Assert.Equal(PasswordStrength.Weak, passwordChecker2.Verdict);
    }
    
    



    private PasswordStrength PasswordChecker(string password)
    {
        return PasswordChecker2(password).Verdict;
    }

    private (PasswordStrength Verdict, string[] Reasons) PasswordChecker2(string password, PasswordRequirements requirements = PasswordRequirements.Standard)
    {
        List<string> reasons = [];
        if (requirements == PasswordRequirements.Admin)
        {
            if (password.Length <= 10) reasons.Add("To short, hon!");
        }
        else
        {
            if (password.Length <= 7) reasons.Add("To short, hon!");
        }

        if(DoesNotHave(password, char.IsDigit)) reasons.Add("N0 numb3rzzz!");
        if(DoesNotHave(password, char.IsLetter)) reasons.Add("Missing 4lph4num3ric bro0!");
        var passwordStrength = reasons.Any() ? PasswordStrength.Weak : PasswordStrength.Strong;
        return (passwordStrength, [..reasons]);
    }


    public enum PasswordStrength
    {
        Weak, Strong
    }

    private static bool DoesNotHave(string password, Func<char, bool> isDigit) 
        => password.Any(isDigit) is false;
}

public enum PasswordRequirements
{
    Admin,
    Standard
}