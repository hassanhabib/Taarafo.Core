// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using Taarafo.Core.Models.Events;
using Taarafo.Core.Models.Events.Exceptions;
using Xeptions;

namespace Taarafo.Core.Services.Foundations.Events
{
    public partial class EventService
    {
        private delegate IQueryable<Event> ReturningEventsFunction();

        private IQueryable<Event> TryCatch(ReturningEventsFunction returningEventsFunction)
        {
            try
            {
                return returningEventsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedEventStorageException = new FailedEventStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedEventStorageException);
            }
            catch (Exception exception)
            {
                var failedEventServiceException =
                    new FailedEventServiceException(exception);

                throw CreateAndLogServiceException(failedEventServiceException);
            }
        }

        private EventDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var eventDependencyException = new EventDependencyException(exception);
            this.loggingBroker.LogCritical(eventDependencyException);

            return eventDependencyException;
        }

        private EventServiceException CreateAndLogServiceException(Xeption exception)
        {
            var eventServiceException = new EventServiceException(exception);
            this.loggingBroker.LogError(eventServiceException);

            return eventServiceException;
        }
    }
}
