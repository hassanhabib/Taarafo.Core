// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Taarafo.Core.Models.Events.Exceptions;
using Taarafo.Core.Models.Events;
using System.Data;
using System.Reflection.Metadata;
using System;

namespace Taarafo.Core.Services.Foundations.Events
{
    public partial class EventService
    {

        private static void ValidateEvent(Event @event)
        {
            ValidateEventNotNull(@event);

            Validate(
                    (Rule: IsInvalid(@event.Id), Parameter: nameof(Event.Id)),
                    (Rule: IsInvalid(@event.Name), Parameter: nameof(Event.Name)),
                    (Rule: IsInvalid(@event.Date), Parameter: nameof(Event.Date)),
                    (Rule: IsInvalid(@event.Location), Parameter: nameof(Event.Location)),
                    (Rule: IsInvalid(@event.Image), Parameter: nameof(Event.Image)),
                    (Rule: IsInvalid(@event.CreatedBy), Parameter: nameof(Event.CreatedBy)),
                    (Rule: IsInvalid(@event.CreatedDate), Parameter: nameof(Event.CreatedDate)),
                    (Rule: IsInvalid(@event.UpdatedBy), Parameter: nameof(Event.UpdatedBy)),
                    (Rule: IsInvalid(@event.UpdatedDate), Parameter: nameof(Event.UpdatedDate)));
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrEmpty(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTime date) => new
        {       
            Condition = date == default,
            Message = "Date is required"
        };

        private static void ValidateEventNotNull(Event @event)
        {
            if (@event is null)
            {
                throw new NullEventException();
            }
        }

        private static void  Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidEventException = new InvalidEventException();

            foreach((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidEventException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidEventException.ThrowIfContainsErrors();
        }
    }
}
