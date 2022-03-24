// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.Groups;
using Taarafo.Core.Models.Groups.Exceptions;
using Xeptions;

namespace Taarafo.Core.Services.Foundations.Groups
{
    public partial class GroupService : IGroupService
    {
        private delegate IQueryable<Group> RetrieveAllGroupsFunction();

        private IQueryable<Group> TryCatch(RetrieveAllGroupsFunction retrieveAllGroups)
        {
            try
            {
                return retrieveAllGroups();
            }
            catch (SqlException sqlException)
            {
                var failedGroupStorageException =
                    new FailedGroupStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedGroupStorageException);
            }
            catch(Exception exception)
            {
                var failedGroupServiceException =
                    new FailedGroupServiceException(exception);

                throw CreateAndLogServiceException(failedGroupServiceException);
            }
        }

        private GroupDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var groupDependencyException= new GroupDependencyException(exception);
            this.loggingBroker.LogCritical(groupDependencyException);

            return groupDependencyException;
        }

        private GroupServiceException CreateAndLogServiceException(Xeption exception)
        {
            var groupServiceException = new GroupServiceException(exception);
            this.loggingBroker.LogError(groupServiceException);

            return groupServiceException;
        }
    }
}