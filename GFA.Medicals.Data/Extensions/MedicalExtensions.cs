namespace GFA.Medicals.Data.Extensions;

internal static class MedicalExtensions
{
    public static Medical ToMedical(this DataMedicalUpdate value, GFAUser? user, Medical? updateModel)
    {
        updateModel ??= new()
        {
            Id = value.EntityId,
            CreatedAt = value.CreatedAt,
            CreatedByUserId = user?.Id
        };

        if (user != null)
        {
            updateModel.ModifiedByUserId = user?.Id;
        }

        if (value.History != null)
        {
            updateModel.InjuryType = value.History.InjuryType;
            updateModel.PlayerClubId = value.History.PlayerClubId;
            updateModel.PeriodType = value.History.PeriodType;
            updateModel.PeriodYear = value.History.PeriodYear;
            updateModel.ReportedById = value.History.ReportedById;
            updateModel.DateStarted = value.History.DateStarted;
            updateModel.ExpectedRecoveryDate = value.History.ExpectedRecoveryDate;
            updateModel.DateRecovered = value.History.DateRecovered;
            updateModel.Remarks = value.History.Remarks;
            updateModel.Height = value.History.Height;
            updateModel.Weight = value.History.Weight;
        }

        if (value.Uploads != null)
        {
            IEnumerable<FileRecord> files = updateModel.Files ?? new List<FileRecord>();

            var q = from c in value.Uploads
                    join m in files on c.EntityId equals m.Id into mgroup
                    from a in mgroup.DefaultIfEmpty()
                    select c.ToFileRecord(user, a);

            updateModel.Files = q.ToList();
        }

        return updateModel;
    }

    public static IEnumerable<ServiceMedical> ToServiceMedicals(this IEnumerable<Medical> values) =>
        values.Select(x => x.ToServiceMedical());

    public static ServiceMedical ToServiceMedical(this Medical value)
    {
        var result = new ServiceMedical()
        {
            InjuryType = value.InjuryType,
            PlayerClubId = value.PlayerClubId,
            PeriodType = value.PeriodType,
            PeriodYear = value.PeriodYear,
            MedicalOfficerId = value.ReportedById,
            DateStarted = value.DateStarted,
            ExpectedRecoveryDate = value.ExpectedRecoveryDate,
            DateRecovered = value.DateRecovered,
            Remarks = value.Remarks,
            Height = value.Height,
            Weight = value.Weight,

            Files = value.Files?.ToServiceFileRecords(),
            PlayerClub = value.PlayerClub?.ToServiceMemberClub(),
            MedicalOfficer = value.ReportedBy?.ToServiceMember()
        };

        return value.ToBaseEntity(result);
    }
}
