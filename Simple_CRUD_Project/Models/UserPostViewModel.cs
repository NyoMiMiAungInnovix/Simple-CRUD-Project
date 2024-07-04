namespace Simple_CRUD_Project.Models
{
    public class UserPostViewModel
    {
        public Guid UserId { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? ContactNo { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? IsActive { get; set; }
        public List<UserPost> UserPosts { get; set; }
    }

    public class UserPost
    {
        public Guid PostId { get; set; }

        public string? PostContent { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? ImageUrl { get; set; }
        public List<PostComment> PostComments { get; set; }
    }
    public class PostComment
    {
        public Guid CommentId { get; set; }
        public string? Comment { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CommentUser { get; set; }
    }
}
