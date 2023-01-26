// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Taarafo.Core.Models.Groups;
using Taarafo.Core.Models.Posts;

namespace Taarafo.Core.Models.GroupPosts
{
    public class GroupPost
    {
        public Guid GroupId { get; set; }
        public Group Group { get; set; }

        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}
