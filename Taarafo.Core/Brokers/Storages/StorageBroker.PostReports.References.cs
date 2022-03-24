// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Taarafo.Core.Models.Posts;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void AddReportedPostsReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostReport>()
                .HasOne(postReport => postReport.Post)
                .WithMany(post => post.ReportedPosts)
                .HasForeignKey(postReport => postReport.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PostReport>()
                .HasOne(postReport => postReport.Profile)
                .WithMany(profile => profile.ReportedPosts)
                .HasForeignKey(postReport => postReport.ReporterId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}