namespace StarTrek.Tests;



public static class PasswordChecker
{
    public static PasswordStrength Check(string password)
    {
        return PasswordChecker2(password).Verdict;
    }

    public static (PasswordStrength Verdict, string[] Reasons) PasswordChecker2(string password,
        PasswordRequirements requirements = PasswordRequirements.Standard)
    {
        List<string> reasons = [];
        var passwordLength = requirements == PasswordRequirements.Admin ? 10 : 7;
        if (password.Length <= passwordLength) reasons.Add("To short, hon!");

        if (DoesNotHave(password, char.IsDigit)) reasons.Add("N0 numb3rzzz!");
        if (DoesNotHave(password, char.IsLetter)) reasons.Add("Missing 4lph4num3ric bro0!");
        
        if (requirements == PasswordRequirements.Admin)
        {
            if (DoesNotHave(password, c => c == '!')) reasons.Add("Missing special character");
            if (password.Last() != '!') reasons.Add("Missing special character");
        }
        
        
        var passwordStrength = reasons.Any() ? PasswordStrength.Weak : PasswordStrength.Strong;
        return (passwordStrength, [..reasons]);
    }

    public enum PasswordStrength
    {
        Weak,
        Strong
    }
    
    public enum PasswordRequirements
    {
        Admin,
        Standard
    }

    private static bool DoesNotHave(string password, Func<char, bool> isDigit)
        => password.Any(isDigit) is false;
}