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
        public static void AddPostImpressionsReferences(ModelBuilder modelBuilders )
        {
            modelBuilders.Entity<PostImpression>()
                .HasOne(postImpression => postImpression.Post)
                .WithMany(post => post.PostImpressions)
                .HasForeignKey(postImpression => postImpression.PostId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilders.Entity<PostImpression>()
                .HasOne(postImpression => postImpression.Profile)
                .WithOne(profile => profile.PostImpression)
                .OnDelete(DeleteBehavior.NoAction);
                
        }
    }
}
