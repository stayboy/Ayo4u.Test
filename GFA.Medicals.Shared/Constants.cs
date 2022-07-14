namespace GFA.Medicals.Shared.Constants;


public static class CodeTypes
{
    public const string CLUB_TEAMS = "teams";
    public const string BODY_INJURIES = "body_injuries";
    // public const string INJURY_PERIODS = "injury_periods";
    public const string RECOVERY_STATES = "recovery_states";
    public const string CLUB_EXIT_MODES = "club_exit_modes";
    public const string INJURY_STATUS = "injury_status";
}

public static class ShortCodes
{
    public static Dictionary<string, string> Genders = new() {
        { "M", "Male" },
        { "F", "Female" }
    };

    public static Dictionary<string, string> MemberTypes = new() {
        { "CO", "Coach" },
        { "PL", "Player" },
        { "MD", "Medical Doctor" }
    };

    public static Dictionary<int, string> YearPeriods =>
        Enumerable.Range(1, 20).Select(x => DateTime.Today.Year - (x - 1)).ToDictionary(x => x, x => x.ToString());

    public static Dictionary<string, string> HistoryPeriods = new() {
        { "current", "Current" },
        { "month_ago", "A Month Ago" },
        { "month_3_ago", "3 Months Ago" },
        { "month_6_ago", "6 Months Ago" },
        { "year_ago", "A Year Ago" }
    };

    public static Dictionary<string, string> ClubDivisions = new()
    {
        { "lower_division", "Lower Division" },
        { "upper_division", "Upper Division" },
        { "premiur_league", "Premier League" }
    };
}
