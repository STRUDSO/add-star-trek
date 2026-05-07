namespace StarTrek.Tests;

public class PasswordCheckerTests
{
    /*
     * Use Test-Driven Development to develop a simple, in-process, easy-
       to-use password-strength-checking API. The caller shouldn’t have to
       have knowledge of what checks are being done to the password
       string. A simple Boolean return value of true (strong enough) or false
       (too weak) is wanted.
       In order to be an acceptably strong password, a string must…
       £ Have a length greater than 7 characters.
       £ Contain at least one alphabetic character.
       £ Contain at least one digit.
       When you are done building it, please answer the following:
       Did you have a prior test fail when a subsequent test was made to
       pass? If so, what did you do in response to that issue?
     */
    [Theory]
    [InlineData(PasswordStrength.Weak, "abcdabcd")]
    [InlineData(PasswordStrength.Strong, "abcd1234")]
    [InlineData(PasswordStrength.Weak, "abcd123")]
    [InlineData(PasswordStrength.Weak, "")]
    [InlineData(PasswordStrength.Weak, "12345678")]
    public void CheckWeakPassword_MissingDigit(PasswordStrength expected, string input)
    {
        Assert.Equal(expected, PasswordChecker(input));
    }


    public enum PasswordStrength
    {
        Weak, Strong
    }

    private PasswordStrength PasswordChecker(string password)
    {
        if(password.Length <= 7) return PasswordStrength.Weak;
        if(DoesNotHave(password, char.IsDigit)) return PasswordStrength.Weak;
        if(DoesNotHave(password, char.IsLetter)) return PasswordStrength.Weak;
        
        return PasswordStrength.Strong;
    }

    private static bool DoesNotHave(string password, Func<char, bool> isDigit) 
        => password.Any(isDigit) is false;
}