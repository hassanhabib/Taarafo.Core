// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsGroupMembershipException =
                    new AlreadyExistsGroupMembershipException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsGroupMembershipException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedGroupMembershipStorageException =
                    new FailedGroupMembershipStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedGroupMembershipStorageException);
            }

        }

        private Exception CreateAndLogDependencyException(Xeption exception)
        {
            var groupMembershipDependencyException = new GroupMembershipDependencyException(exception);
            this.loggingBroker.LogError(groupMembershipDependencyException);

            return groupMembershipDependencyException;
        }

        private Exception CreateAndLogDependencyValidationException(Xeption exception)
        {
            var groupMembershipDependencyValidationException = new GroupMembershipDependencyValidationException(exception);
            this.loggingBroker.LogError(groupMembershipDependencyValidationException);

            return groupMembershipDependencyValidationException;
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