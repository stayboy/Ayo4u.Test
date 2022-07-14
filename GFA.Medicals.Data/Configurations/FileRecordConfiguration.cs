using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFA.Medicals.Data.Configurations;

internal class FileRecordConfiguration : GFAIdModelConfiguration<int, FileRecord>
{
    public FileRecordConfiguration() : base("FileBlobs")
    {
    }

    public override void Configure(EntityTypeBuilder<FileRecord> builder)
    {
        base.Configure(builder);

        builder.HasMany(x => x.PictureMembers)
            .WithOne(x => x.PicturePath)
            .HasForeignKey(x => x.PicturePathId);

        builder.HasMany(x => x.FileMedicals)
            .WithMany(x => x.Files)
            .UsingEntity(x => x.ToTable("MedicalFiles"));
    }
}
