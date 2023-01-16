//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Taarafo.Core.Models.Events.Exceptions
{
    public class LockedEventException : Xeption
    {
        public LockedEventException(Exception innerException)
            : base(message: "Event is locked, please try again.", innerException)
        { }
    }
}