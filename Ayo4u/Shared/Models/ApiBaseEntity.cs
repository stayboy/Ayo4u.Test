﻿namespace Ayo4u.Web.Shared.Models;

public abstract class ApiBaseEntity<T>
{
    public T Id { get; set; } = default!;

    public DateTime Created { get; set; }

    public bool IsDeleted { get; set; } = false;

    public string? CreatedByUserFullName { get; set; }

    public string? CreatedByUserEmail { get; set; }
}
