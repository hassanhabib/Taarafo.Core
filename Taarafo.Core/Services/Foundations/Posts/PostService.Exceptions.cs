// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using Taarafo.Core.Models.Posts;
using Taarafo.Core.Models.Posts.Exceptions;

namespace Taarafo.Core.Services.Foundations.Posts
{
    public partial class PostService
    {
        private delegate IQueryable<Post> ReturningPostsFunction();

        private IQueryable<Post> TryCatch(ReturningPostsFunction returningPostsFunction)
        {
            try
            {
                return returningPostsFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
        }

        private PostDependencyException CreateAndLogCriticalDependencyException(
            Exception exception)
        {
            var postDependencyException = new PostDependencyException(exception);
            this.loggingBroker.LogCritical(postDependencyException);

            return postDependencyException;
        }
    }
}
