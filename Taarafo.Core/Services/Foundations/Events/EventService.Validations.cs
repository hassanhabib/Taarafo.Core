// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Taarafo.Core.Models.Events;
using Taarafo.Core.Models.Events.Exceptions;

namespace Taarafo.Core.Services.Foundations.Events
{
    public partial class EventService
    {
        public void ValidateEventOnAdd(Event @event)
        {
            ValidateEventIsNotNull(@event);

            Validate(
                (Rule: IsInvalid(@event.Id), Parameter: nameof(Event.Id)),
                (Rule: IsInvalid(@event.Location), Parameter: nameof(Event.Location)),
                (Rule: IsInvalid(@event.Date), Parameter: nameof(Event.Date)),
                (Rule: IsInvalid(@event.CreatedDate), Parameter: nameof(Event.CreatedDate)),
                (Rule: IsNotRecent(@event.CreatedDate), Parameter: nameof(Event.CreatedDate)),
                (Rule: IsInvalid(@event.CreatedBy), Parameter: nameof(Event.CreatedBy))
            );

        }

        private void ValidateEventIsNotNull(Event @event)
        {
            if (@event is null)
            {
                throw new NullEventException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTimeOffset();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidEventException =
                new InvalidEventException();

            foreach ((dynamic rule, string parameter) in validations)
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
