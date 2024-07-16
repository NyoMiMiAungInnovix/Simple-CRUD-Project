using System;
using System.Collections.Generic;

namespace Simple_CRUD_Project.Models;

public partial class TblFiledetail
{
    public Guid Id { get; set; }

    public string? FileName { get; set; }

    public byte[] FileData { get; set; } = null!;

    public FileType FileType { get; set; }

    public int? FileSize { get; set; }

    public DateTime? UploadedAt { get; set; }
}
