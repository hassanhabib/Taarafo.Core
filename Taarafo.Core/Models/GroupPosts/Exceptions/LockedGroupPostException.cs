//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Teams.Exceptions
{
    public class LockedGroupPostException : Xeption
    {
        public LockedGroupPostException(Exception innerException)
            : base(message: "GrouPost is locked, please try again.", innerException)
        { }
    }
}