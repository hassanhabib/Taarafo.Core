// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Taarafo.Core.Models.Groups;

namespace Taarafo.Core.Brokers.Storages
{
	public partial class StorageBroker
	{
		public DbSet<Group> Groups { get; set; }	
	}
}
