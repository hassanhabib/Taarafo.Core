// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Taarafo.Core.Models.Posts;

namespace Taarafo.Core.Models.Comments
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; }

        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}
