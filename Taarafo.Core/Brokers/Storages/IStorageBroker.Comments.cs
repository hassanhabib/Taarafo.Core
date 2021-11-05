// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.Comments;

namespace Taarafo.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Comment> InsertCommentAsync(Comment comment);
        IQueryable<Comment> SelectAllComments();
        ValueTask<Comment> SelectCommentByIdAsync(Guid commentId);
        ValueTask<Comment> UpdateCommentAsync(Comment comment);
        ValueTask<Comment> DeleteCommentAsync(Comment comment);
    }
}
