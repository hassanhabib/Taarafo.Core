﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.GroupMemberships.Exceptions
{
    public class InvalidGroupMembershipException : Xeption
    {
        public InvalidGroupMembershipException()
            : base(
                message: "Invalid GroupMembership. Please correct the errors and try again.")
        { }
        
        public InvalidGroupMembershipException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}