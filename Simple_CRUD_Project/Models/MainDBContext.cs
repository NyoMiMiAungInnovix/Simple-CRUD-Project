using Microsoft.EntityFrameworkCore;

namespace Simple_CRUD_Project.Models
{
    public class MainDBContext : DbContext
    {
        public IConfiguration Configuration { get; set; }

        public MainDBContext()
        {
            var builder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }

        public virtual DbSet<TblComment> TblComments { get; set; }

        public virtual DbSet<TblPost> TblPosts { get; set; }

        public virtual DbSet<TblUser> TblUsers { get; set; }

        public virtual DbSet<TblFiledetail> TblFiledetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblComment>(entity =>
            {
                entity.ToTable("Tbl_comment");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Comment).HasColumnName("comment");
                entity.Property(e => e.CommentUserId).HasColumnName("comment_user_id");
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
                entity.Property(e => e.PostId).HasColumnName("post_id");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");
            });

            modelBuilder.Entity<TblPost>(entity =>
            {
                entity.ToTable("Tbl_post");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");
                entity.Property(e => e.ImageUrl)
                    .IsUnicode(false)
                    .HasColumnName("image_url");
                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
                entity.Property(e => e.PostContent).HasColumnName("post_content");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");
                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.ToTable("Tbl_user");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.ContactNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("contact_no");
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");
                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.Password).IsUnicode(false);
                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");
                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<TblFiledetail>(entity =>
            {
                entity.ToTable("Tbl_filedetails");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.FileData).HasColumnName("file_data");
                entity.Property(e => e.FileName)
                    .HasMaxLength(300)
                    .HasColumnName("file_name");
                entity.Property(e => e.FileSize).HasColumnName("file_size");
                entity.Property(e => e.FileType).HasColumnName("file_type");
                entity.Property(e => e.UploadedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("uploaded_at");
            });

        }
    }
}
