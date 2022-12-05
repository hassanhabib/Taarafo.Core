// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Taarafo.Core.Models.GroupPosts;

namespace Taarafo.Core.Models.Groups
{
	public class Group
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public GroupStatus Status { get; set; }
		public DateTimeOffset CreatedDate { get; set; }
		public DateTimeOffset UpdatedDate { get; set; }

		[JsonIgnore]
		public IEnumerable<GroupPost> GroupPosts { get; set; }
	}
}
