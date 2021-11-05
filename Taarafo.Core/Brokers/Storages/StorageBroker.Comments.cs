// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;
using Taarafo.Core.Models.Comments;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Comment> Comments { get; set; }

        public async ValueTask<Comment> InsertCommentAsync(Comment comment)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Comment> commentEntityEntry =
                await broker.Comments.AddAsync(comment);

            await broker.SaveChangesAsync();

            return commentEntityEntry.Entity;
        }
    }
}
