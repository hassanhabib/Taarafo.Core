// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taarafo.Core.Models.Comments;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Comment> Comments { get; set; }

        public async ValueTask<Comment> InsertCommentAsync(Comment comment) =>
            await InsertAsync(comment);

        public IQueryable<Comment> SelectAllComments() =>
            SelectAll<Comment>();

        public async ValueTask<Comment> SelectCommentByIdAsync(Guid commentId) =>
            await SelectAsync<Comment>(commentId);

        public async ValueTask<Comment> UpdateCommentAsync(Comment comment) =>
            await UpdateAsync(comment);

        public async ValueTask<Comment> DeleteCommentAsync(Comment comment) =>
            await DeleteAsync(comment);
    }
}
