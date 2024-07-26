using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Simple_CRUD_Project.Common.Entities;

namespace Simple_CRUD_Project.Models
{
    public class MainDBContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public MainDBContext()
        {

        }

        public MainDBContext(DbContextOptions<MainDBContext> options, IConfiguration configuration)
    : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<TblComment> TblComments { get; set; }

        public virtual DbSet<TblPost> TblPosts { get; set; }

        public virtual DbSet<TblUser> TblUsers { get; set; }

        public virtual DbSet<TblFiledetail> TblFiledetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
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

            base.OnModelCreating(modelBuilder);

            // Apply configurations for all entities inheriting from BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).Property<DateTime?>("CreatedAt")
                        .HasDefaultValueSql("GETUTCDATE()")
                        .ValueGeneratedOnAdd(); // Optional, depending on how you want to generate the default value

                    modelBuilder.Entity(entityType.ClrType).Property<DateTime?>("UpdatedAt")
                        .HasDefaultValueSql("GETUTCDATE()")
                        .ValueGeneratedOnAddOrUpdate(); // Optional, depending on how you want to generate the default value
                }
            }

        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var now = DateTime.UtcNow;
                //((BaseEntity)entry.Entity).UpdatedAt = now;

                if (entry.State == EntityState.Added)
                {
                    ((BaseEntity)entry.Entity).CreatedAt = now;
                }

                else if (entry.State == EntityState.Modified)
                {
                    ((BaseEntity)entry.Entity).UpdatedAt = now;
                }
            }
        }
    }
}
