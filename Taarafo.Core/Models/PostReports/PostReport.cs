// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Taarafo.Core.Models.Profiles;

namespace Taarafo.Core.Models.Posts
{
    public class PostReport
    {
        public Guid Id { get; set; }
        public string Details { get; set; }

        public Guid PostId { get; set; }
        public Post Post { get; set; }

        public Guid ReporterId { get; set; }
        public Profile Profile { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}