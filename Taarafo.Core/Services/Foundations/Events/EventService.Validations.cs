// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Taarafo.Core.Models.Events;
using Taarafo.Core.Models.Events.Exceptions;

namespace Taarafo.Core.Services.Foundations.Events
{
    public partial class EventService
    {
        public void ValidateEventOnAdd(Event @event)
        {
            ValidateEventIsNotNull(@event);

        }

        private void ValidateEventIsNotNull(Event @event)
        {
            if (@event is null)
            {
                throw new NullEventException();
            }
        }
    }
}
