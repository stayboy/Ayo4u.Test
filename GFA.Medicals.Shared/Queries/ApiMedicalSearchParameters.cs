namespace GFA.Medicals.Web.Shared.Queries
{
    public class ApiMedicalSearchParameters : ApiSearchParameters<int>
    {
        public int? injury { get; set; }

        public int? medofficer { get; set; }

        public int? injuryyear { get; set; }

        public string? injuryperiod { get; set; }

        public int? club { get; set; }

        public int? player { get; set; }

        public bool? recovered { get; set; }

        public int? injurystatus { get; set; }

        public bool? showplayer { get; set; }
    }
}
