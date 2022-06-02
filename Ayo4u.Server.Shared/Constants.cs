using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayo4u.Server.Shared.Constants;

public enum BlockStatus : short
{
    Blocked = 1,
    Deleted = 3,
    Clone = 5,
    Activate = 7
}

public enum Formulae
{
    Celcius_To_Fahrenheit = 1,
    Fahrenheit_To_Celcius = 3
}

public static class CodeTypes
{
    public const string CLUB_TEAMS = "teams";
    public const string INJURIES = "body_injuries";
}