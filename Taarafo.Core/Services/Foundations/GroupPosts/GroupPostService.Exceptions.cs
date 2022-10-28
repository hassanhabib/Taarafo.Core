// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Taarafo.Core.Models.GroupPosts;
using Taarafo.Core.Models.GroupPosts.Exceptions;
using Xeptions;

namespace Taarafo.Core.Services.Foundations.GroupPosts
{
    public partial class GroupPostService
    {
        private delegate ValueTask<GroupPost> ReturningPostFuntion();

        private async ValueTask<GroupPost> TryCatch(ReturningPostFuntion returningPostFuntion)
        {
            try
            {
                return await returningPostFuntion();
            }
            catch (NullGroupPostException nullGroupPostException)
            {
                throw CreateAndLogValidationException(nullGroupPostException);
            }
            catch (InvalidGroupPostException invalidGroupPostException)
            {
                throw CreateAndLogValidationException(invalidGroupPostException);
            }
            catch (SqlException sqlException)
            {
                var failedGroupPostStorageException =
                    new FailedGroupPostStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(
                    failedGroupPostStorageException);
            }
        }

        private GroupPostValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var groupPostValidationException =
                new GroupPostValidationException(exception);

            this.loggingBroker.LogError(groupPostValidationException);

            return groupPostValidationException;
        }

        private GroupPostDependencyException CreateAndLogCriticalDependencyException(
            Xeption exception)
        {
            var groupPostDependencyException =
                new GroupPostDependencyException(exception);

            this.loggingBroker.LogCritical(groupPostDependencyException);

            return groupPostDependencyException;
        }
    }
}
