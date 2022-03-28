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
        private static void AddPostReportsReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostReport>()
                .HasOne(postReport => postReport.Post)
                .WithMany(post => post.PostsReported)
                .HasForeignKey(postReport => postReport.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PostReport>()
                .HasOne(postReport => postReport.Profile)
                .WithMany(profile => profile.PostsReported)
                .HasForeignKey(postReport => postReport.ReporterId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}