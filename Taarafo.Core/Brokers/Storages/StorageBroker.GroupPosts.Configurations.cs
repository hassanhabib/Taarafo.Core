// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Taarafo.Core.Models.GroupPosts;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void AddGroupPostConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupPost>()
                .HasKey(groupPost =>
                    new { groupPost.GroupId, groupPost.PostId });

            modelBuilder.Entity<GroupPost>()
                .HasOne(groupPost => groupPost.Group)
                .WithMany(group => group.GroupPosts)
                .HasForeignKey(groupPost => groupPost.GroupId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<GroupPost>()
                .HasOne(groupPost => groupPost.Post)
                .WithMany(post => post.GroupPosts)
                .HasForeignKey(groupPost => groupPost.PostId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
