// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Taarafo.Core.Models.Posts;

namespace Taarafo.Core.Models.Pictures
{
    public class Picture
    {
		public Guid Id { get; set; }
		public string Description { get; set; }
		public DateTime UploadedDate { get; set; }

		public Guid PostId { get; set; }
		public Post Post { get; set; }
	}
}