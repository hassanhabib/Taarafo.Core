// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Taarafo.Core.Models.Events.Exceptions;
using Taarafo.Core.Models.Events;

namespace Taarafo.Core.Services.Foundations.Events
{
    public partial class EventService
    {
        private static void ValidateEventNotNull(Event @event)
        {
            if (@event is null)
            {
                throw new NullEventException();
            }
        }
    }
}
