// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Taarafo.Core.Models.GroupMemberships;
using Taarafo.Core.Models.GroupMemberships.Exceptions;
using Xeptions;

namespace Taarafo.Core.Services.Foundations.GroupMemberships
{
    public partial class GroupMembershipService
    {
        private delegate ValueTask<GroupMembership> ReturningGroupMembershipFuntion();

        private async ValueTask<GroupMembership> TryCatch(ReturningGroupMembershipFuntion returningGroupMembershipFuntion)
        {
            try
            {
                return await returningGroupMembershipFuntion();
            }
            catch (NullGroupMembershipException nullGroupMembershipException)
            {
                throw CreateAndLogValidationException(nullGroupMembershipException);
            }
            catch (InvalidGroupMembershipException invalidGroupMembershipException)
            {
                throw CreateAndLogValidationException(invalidGroupMembershipException);
            }
            catch (SqlException sqlException)
            {
                var failedGroupMembershipStorageException =
                    new FailedGroupMembershipStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedGroupMembershipStorageException);
            }
        }

        private Exception CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var groupMembershipDependencyException = new GroupMembershipDependencyException(exception);
            this.loggingBroker.LogCritical(groupMembershipDependencyException);

            return groupMembershipDependencyException;
        }

        private GroupMembershipValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var groupMembershipValidationException =
                new GroupMembershipValidationException(exception);

            this.loggingBroker.LogError(groupMembershipValidationException);

            return groupMembershipValidationException;
        }
    }
}