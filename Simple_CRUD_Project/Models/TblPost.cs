using System;
using System.Collections.Generic;

namespace Simple_CRUD_Project.Models;

public partial class TblPost
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public string? PostContent { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public string? ImageUrl { get; set; }
}
