using Simple_CRUD_Project.Common.Entities;

namespace Simple_CRUD_Project.Models;

public partial class TblUser : BaseEntity
{
    public Guid Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? ContactNo { get; set; }

    public bool? IsActive { get; set; }
}
