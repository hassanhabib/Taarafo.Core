using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taarafo.Core.Models.Posts
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid Author { get; set; }
    }
}
