// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Taarafo.Core.Models.PostImpressions;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void AddPostImpressionConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostImpression>()
                   .HasKey(postImpression =>
                       new { postImpression.PostId, postImpression.ProfileId });

            modelBuilder.Entity<PostImpression>()
                .HasOne(postImpression => postImpression.Post)
                .WithMany(post => post.PostImpressions)
                .HasForeignKey(postImpression => postImpression.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PostImpression>()
               .HasOne(postImpression => postImpression.Profile)
               .WithMany(profile => profile.PostImpressions)
               .HasForeignKey(postImpression => postImpression.ProfileId)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
