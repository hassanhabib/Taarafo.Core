// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
            catch(NullGroupPostException nullGroupPostException)
            {
                throw CreateAndLogValidationException(nullGroupPostException);
            }
            catch (InvalidGroupPostException invalidGroupPostException)
            {
                throw CreateAndLogValidationException(invalidGroupPostException);
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
    }
}
