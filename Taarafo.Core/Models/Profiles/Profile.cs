// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Taarafo.Core.Models.PostImpressions;

namespace Taarafo.Core.Models.Profiles
{
	public class Profile
	{
		public Guid Id { get; set; }
		public string Username { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Avatar { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
		public DateTimeOffset UpdatedDate { get; set; }
		public IEnumerable<PostImpression> PostImpressions { get; set; }
	}
}
