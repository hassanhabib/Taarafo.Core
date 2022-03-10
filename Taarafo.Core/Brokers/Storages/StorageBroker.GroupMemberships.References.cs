// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Taarafo.Core.Models.GroupMemberships;
using Taarafo.Core.Models.Profiles;

namespace Taarafo.Core.Brokers.Storages
{
	public partial class StorageBroker
	{
		private static void AddGroupMembershipReferences(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<GroupMembership>()
				.HasKey(membership => new { membership.GroupId, membership.ProfileId });

			modelBuilder.Entity<GroupMembership>()
				.HasOne(groupMembership => groupMembership.Profile)
				.WithMany(profile => profile.GroupMemberships)
				.HasForeignKey(groupMembership => groupMembership.ProfileId);

			modelBuilder.Entity<GroupMembership>()
				.HasOne(groupMembership => groupMembership.Group)
				.WithMany(group => group.GroupMemberships)
				.HasForeignKey(groupMembership => groupMembership.GroupId);
		}
	}
}


