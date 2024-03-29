﻿using Ayo4u.Server.Shared.Constants;

namespace Ayo4u.Infrastructure.Models;

public record DataUnitConverterUpdate (int Id, DateTime CreatedAt)
{
    public string InUnitType { get; set; } = default!;

    public string OutUnitType { get; set; } = default!;

    public float Multiplier { get; set; }

    public Formulae? Formula { get; set; }

    public ServiceAyoUser? AyoUser { get; set; }
}
