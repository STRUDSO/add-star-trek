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
        var isAdmin = requirements == PasswordRequirements.Admin;

        var reasons = Reasons(password, isAdmin);

        var passwordStrength = reasons.Any() ? PasswordStrength.Weak : PasswordStrength.Strong;
        return (passwordStrength, [..reasons]);
    }

    private static List<string> Reasons(string password, bool isAdmin)
    {
        if (isAdmin)
        {
            var reasons = StandReasons(password, 10);
            reasons.AddRange(Admin(password));
            return reasons;
        }
        else
        {
            return StandReasons(password, 7);
        }
    }

    private static List<string> Admin(string password)
    {
        var reasons = new List<string>();
        List<char> specialCharacters = ['!', '*'];

        if (DoesNotHave(password, c => c == '!')) reasons.Add("Missing special character");
        if (!specialCharacters.Contains(password.Last()) && !char.IsDigit(password.Last())) reasons.Add("Missing special character");
        return reasons;
    }

    private static List<string> StandReasons(string password, int passwordLength)
    {
        var reasons = new List<string>();
        if (password.Length <= passwordLength) reasons.Add("To short, hon!");

        if (DoesNotHave(password, char.IsDigit)) reasons.Add("N0 numb3rzzz!");
        if (DoesNotHave(password, char.IsLetter)) reasons.Add("Missing 4lph4num3ric bro0!");
        return reasons;
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