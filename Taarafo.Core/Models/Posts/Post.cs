// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Taarafo.Core.Models.Comments;
using Taarafo.Core.Models.GroupPosts;
using Taarafo.Core.Models.PostImpressions;

namespace Taarafo.Core.Models.Posts
{
	public class Post
	{
		public Guid Id { get; set; }
		public string Content { get; set; }
		public string Author { get; set; }
		public DateTimeOffset CreatedDate { get; set; }
		public DateTimeOffset UpdatedDate { get; set; }

		[JsonIgnore]
		public IEnumerable<GroupPost> GroupPosts { get; set; }
		public IEnumerable<Comment> Comments { get; set; }
		public IEnumerable<PostImpression> PostImpressions { get; set; }
	}
}
