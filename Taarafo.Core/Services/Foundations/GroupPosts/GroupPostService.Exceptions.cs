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
        private delegate ValueTask<GroupPost> ReturningGroupPostFunction();

        private async ValueTask<GroupPost> TryCatch(ReturningGroupPostFunction returningGroupPostFunction)
        {
            try
            {
                return await returningGroupPostFunction();
            }
            catch (InvalidGroupPostException invalidGroupPostException)
            {
                throw CreateAndLogValidationException(invalidGroupPostException);
            }
            catch (NotFoundGroupPostException notFoundGroupPostException)
            {
                throw CreateAndLogValidationException(notFoundGroupPostException);
            }
            catch (SqlException sqlException)
            {
                var failedGroupPostStorageException =
                    new FailedGroupPostStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedGroupPostStorageException);
            }
        }

        private GroupPostValidationException CreateAndLogValidationException(Xeption exception)
        {
            var groupPostValidationException = new GroupPostValidationException(exception);
            this.loggingBroker.LogError(groupPostValidationException);

            return groupPostValidationException;
        }

        private GroupPostDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var groupPostDependencyException = new GroupPostDependencyException(exception);
            this.loggingBroker.LogCritical(groupPostDependencyException);

            return groupPostDependencyException;
        }
    }
}
