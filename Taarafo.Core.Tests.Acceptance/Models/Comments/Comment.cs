// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;

namespace Taarafo.Core.Tests.Acceptance.Models.Comments
{
	public class Comment
	{
		public Guid Id { get; set; }
		public string Content { get; set; }
		public DateTimeOffset CreatedDate { get; set; }
		public DateTimeOffset UpdatedDate { get; set; }
		public Guid PostId { get; set; }
	}
}
