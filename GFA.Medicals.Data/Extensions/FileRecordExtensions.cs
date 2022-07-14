
namespace GFA.Medicals.Data.Extensions;

internal static class FileRecordExtensions
{
    public static IEnumerable<ServiceFileRecord> ToServiceFileRecords(this IEnumerable<FileRecord> files) =>
        files.Select(x => x.ToServiceFileRecord());

    public static ServiceFileRecord ToServiceFileRecord(this FileRecord file)
    {
        var result = new ServiceFileRecord()
        {
            Id = file.Id,
            FileTitle = file.Title,
            DateUploaded = file.Uploaded,
            ContentType = file.ContentType,
            Remarks = file.Remarks,
            PathUri = file.PathUri
        };

        return file.ToBaseEntity(result);
    }

    public static FileRecord ToFileRecord(this DataFileRecordUpdate file, GFAUser? user = null, FileRecord? updateModel = null)
    {
        updateModel ??= new()
        {
            Id = file.EntityId,
            CreatedAt = file.CreatedAt,
            CreatedByUserId = user?.Id
        };

        updateModel.ModifiedByUserId = user?.Id;

        if (!string.IsNullOrWhiteSpace(file.PathUri))
        {
            updateModel.PathUri = file.PathUri;
        }

        if (file.Record != null)
        { 
            updateModel.Title = file.Record.Title;
            updateModel.ContentType = file.Record.ContentType;
            updateModel.Remarks = file.Record.Remarks;
        }

        return updateModel;
    }
}
