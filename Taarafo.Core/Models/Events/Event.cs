// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;

namespace Taarafo.Core.Models.Events
{
	public class Event
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTimeOffset Date { get; set; }
		public string Location { get; set; }
		public string Image { get; set; }
		public Guid CreatedBy { get; set; }
		public DateTimeOffset CreatedDate { get; set; }
		public Guid UpdatedBy { get; set; }
		public DateTimeOffset UpdatedDate { get; set; }
	}
}