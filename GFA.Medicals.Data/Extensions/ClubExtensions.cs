namespace GFA.Medicals.Data.Extensions;

internal static class ClubExtensions
{
    public static MemberClub ToMemberClub(this DataMemberClubUpdate club, GFAUser? user, MemberClub? updateModel)
    {
        updateModel ??= new()
        {
            Id = club.EntityId,
            CreatedAt = club.CreatedAt,
            CreatedByUserId = user?.Id
        };

        if (user != null)
        {
            updateModel.ModifiedByUserId = user?.Id;
        }

        updateModel.ClubId = club.ClubId;
        updateModel.DateJoined = club.DateJoined;
        updateModel.DateExited = club.DateExited;
        updateModel.ExitStatus = club.ModeOfExit;
        updateModel.Remarks = club.Remarks;
        updateModel.SigningCoachId = club.SigningCoachId;
        updateModel.SellingCoachId = club.SellingCoachId;
        updateModel.UniqueNo = club.UniqueNo;

        return updateModel;
    }

    public static Club ToClub(this DataClubUpdate club, GFAUser? user, Club? updateModel)
    {
        updateModel ??= new()
        {
            Id = club.EntityId,
            CreatedAt = club.CreatedAt,
            CreatedByUserId = user?.Id
        };

        if (user != null)
        {
            updateModel.ModifiedByUserId = user?.Id;
        }

        updateModel.CountryId = club.CountryId;
        updateModel.Name = club.Name;
        updateModel.Initials = club.Initials;
        updateModel.Division = club.Division;
        updateModel.Effective = club.Effective;

        return updateModel;
    }

    public static IEnumerable<ServiceClub> ToServiceClubs(this IEnumerable<Club> clubs) =>
        clubs.Select(x => x.ToServiceClub());

    public static ServiceClub ToServiceClub(this Club club)
    {
        var result = new ServiceClub()
        {
            ClubName = club.Name,
            ClubDivision = club.Division,
            CountryId = club.CountryId,
            Initials = club.Initials,
            Effective = club.Effective,
            ClubCountry = club.ClubCountry?.ToServiceValueCodeType(),
            Players = club.Players?.ToServiceMembers()
        };

        return club.ToBaseEntity(result);
    }

    public static IEnumerable<ServiceMemberClub> ToServiceMemberClubs(this IEnumerable<MemberClub> clubs) =>
        clubs.Select(x => x.ToServiceMemberClub());

    public static ServiceMemberClub ToServiceMemberClub(this MemberClub club)
    {
        var result = new ServiceMemberClub()
        {
             PlayerId = club.PlayerId,
             ClubId = club.ClubId,

             DateJoined = club.DateJoined,
             DateExited = club.DateExited,
             ModeOfExit = club.ExitStatus,
             UniqueNo = club.UniqueNo,

             Remarks = club.Remarks,
             SigningCoachId = club.SigningCoachId,
             SellingCoachId = club.SellingCoachId,

             Club = club.Club?.ToServiceClub(),
             Player = club.Player?.ToServiceMember(),
             SigningCoach = club.SigningCoach?.ToServiceMember(),
             SellingCoach = club.SellingCoach?.ToServiceMember(),
             Medicals = club.Medicals?.ToServiceMedicals()
        };

        return club.ToBaseEntity(result);
    }
}
