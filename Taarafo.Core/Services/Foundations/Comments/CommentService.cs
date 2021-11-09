// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Brokers.DateTimes;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.Comments;

namespace Taarafo.Core.Services.Foundations.Comments
{
    public partial class CommentService : ICommentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public CommentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Comment> AddCommentAsync(Comment comment) =>
        TryCatch(async () =>
        {
            ValidateComment(comment);

            return await this.storageBroker.InsertCommentAsync(comment);
        });

        public ValueTask<Comment> RetrieveCommentByIdAsync(Guid commentId) =>
        TryCatch(async () =>
        {
            ValidateCommentId(commentId);

            Comment maybeComment = await this.storageBroker
                .SelectCommentByIdAsync(commentId);

            ValidateStorageComment(maybeComment, commentId);

            return maybeComment;
        });

        public IQueryable<Comment> RetrieveAllComments() =>
        TryCatch(() => this.storageBroker.SelectAllComments());
    }
}
