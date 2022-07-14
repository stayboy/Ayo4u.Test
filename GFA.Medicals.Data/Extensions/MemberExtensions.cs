namespace GFA.Medicals.Data.Extensions
{
    internal static class MemberExtensions
    {
        public static Member ToMember(this DataMemberUpdate value, GFAUser? user = null, Member? updateModel = null)
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

            if (value.Profile != null)
            {
                updateModel.MemberType = value.Profile.MemberType;
                updateModel.FirstName = value.Profile.FirstName;    
                updateModel.LastName = value.Profile.LastName;
                updateModel.DateOfBirth= value.Profile.DateOfBirth;
                updateModel.Gender = value.Profile.Gender;
                updateModel.DateRegistered = value.Profile.DateRegistered;
                updateModel.GFAUniqueNo = value.Profile.GFAUniqueNo;
            }

            if (value.Upload != null)
            {
                updateModel.PicturePath = value.Upload.ToFileRecord(user, updateModel.PicturePath);

                updateModel.PictureUrl = updateModel.PicturePath.PathUri;
            }

            if (value.Clubs != null)
            {
                var registeredClubs = updateModel.ClubRegisterations ?? new List<MemberClub>();

                var q = from c in value.Clubs
                        join m in registeredClubs on c.EntityId equals m.Id into mgroup
                        from a in mgroup.DefaultIfEmpty()
                        select c.ToMemberClub(user, a);

                updateModel.ClubRegisterations = q.ToArray();
            }

            if (value.Medicals != null)
            {
                var medicals = updateModel.ClubRegisterations ?? new List<MemberClub>();

                updateModel.ClubRegisterations = medicals.Select(x =>
                {
                    var q = from c in value.Medicals
                            join m in x.Medicals on c.EntityId equals m.Id into mgroup
                            from a in mgroup.DefaultIfEmpty()
                            select c.ToMedical(user, a);

                    x.Medicals = q.ToList();

                    return x;
                }).ToArray();
            }

            return updateModel;
        }

        public static IEnumerable<ServiceMember> ToServiceMembers(this IEnumerable<Member> values) =>
            values.Select(x => x.ToServiceMember());

        public static ServiceMember ToServiceMember(this Member value)
        {
            IEnumerable<ServiceMedical>? medicalNotes = null;
            if (value.ClubRegisterations?.SelectMany(x => x.Medicals) is IEnumerable<Medical> medicals)
            {
                // medicalNotes = medicals.to;
            }

            var result = new ServiceMember()
            {
                FirstName = value.FirstName,
                LastName = value.LastName,
                DateOfBirth = value.DateOfBirth,
                Gender = value.Gender,
                PicturePath = value.PicturePath?.ToServiceFileRecord(),
                MemberType = value.MemberType,
                PicturePathId = value.PicturePathId,
                PictureUrl = value.PictureUrl,
                Remarks = value.Remarks,
                ClubsPlayed = value.Clubs?.ToServiceClubs(),
                ReporterMedicalNotes = value.ReporterMedicalNotes?.ToServiceMedicals(),
                PlayerMedicalNotes = medicalNotes,
                ClubRegisterations = value.ClubRegisterations?.ToServiceMemberClubs()
            };

            return value.ToBaseEntity(result);
        }
    }
}
