namespace GFA.Medicals.Web.Shared.Models
{
    public class ApiValueCodeType : ApiBaseEntity<int>
    {
        public string CodeType { get; set; } = default!;

        public string Code { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string? Description { get; set; }
    }
}
