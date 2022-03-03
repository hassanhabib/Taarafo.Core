// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taarafo.Core.Models.GroupMemberships;
using Taarafo.Core.Models.Groups;

namespace Taarafo.Core.Brokers.Storages
{
	public partial interface IStorageBroker
	{
        DbSet<GroupMembership> GroupMemberships { get; set; }

        ValueTask<Group> InsertGroupAsync(Group group);
        void onModelCreating(ModelBuilder modelBuilder);
        IQueryable<Group> SelectAllGroups();
		ValueTask<Group> SelectGroupByIdAsync(Guid groupId);
	}
}
