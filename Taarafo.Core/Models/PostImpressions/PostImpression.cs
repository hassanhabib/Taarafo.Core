// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Taarafo.Core.Models.Posts;
using Taarafo.Core.Models.Profiles;

namespace Taarafo.Core.Models.PostImpressions
{
    public class PostImpression
    {
        public Guid PostId { get; set; }
        public Post Post { get; set; }

        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }

        public PostImpressionType Impression { get; set; }
    }
}
