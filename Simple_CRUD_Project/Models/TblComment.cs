using Simple_CRUD_Project.Common.Entities;
using System;
using System.Collections.Generic;

namespace Simple_CRUD_Project.Models;

public partial class TblComment : BaseEntity
{
    public Guid Id { get; set; }

    public Guid? PostId { get; set; }

    public string? Comment { get; set; }

    public bool? IsDeleted { get; set; }

    public Guid? CommentUserId { get; set; }
}
