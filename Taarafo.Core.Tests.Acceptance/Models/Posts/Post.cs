using System;

namespace Taarafo.Core.Tests.Acceptance.Models.Posts
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid Author { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
