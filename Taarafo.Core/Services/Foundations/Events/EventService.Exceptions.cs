// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Taarafo.Core.Models.Events;
using Taarafo.Core.Models.Events.Exceptions;
using Xeptions;

namespace Taarafo.Core.Services.Foundations.Events
{
    public partial class EventService
    {
        private delegate ValueTask<Event> ReturningEventFunction(); 

        private async ValueTask<Event> TryCatch(ReturningEventFunction returningEventFunction)
        {
            try
            {
                return await returningEventFunction();
            }
            catch(NullEventException nullEventException)
            {
                throw CreateAndLogValidationException(nullEventException);
            }
            catch(InvalidEventException invalidEventException)
            {
                throw CreateAndLogValidationException(invalidEventException);
            }
            catch(SqlException sqlException)
            {
                var failedEventStorageException =
                    new FailedEventStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedEventStorageException);
            }
        }

        private EventValidationException CreateAndLogValidationException(Xeption exception)
        {
            EventValidationException eventValidationException = new EventValidationException(exception);
            this.loggingBroker.LogError(eventValidationException);

            return eventValidationException;
        }

        private EventDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var eventDependencyException = new EventDependencyException(exception);
            this.loggingBroker.LogCritical(eventDependencyException);

            return eventDependencyException;
        }
    }
}
