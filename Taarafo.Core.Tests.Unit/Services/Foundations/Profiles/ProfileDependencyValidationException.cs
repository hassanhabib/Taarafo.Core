// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xeptions;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Profiles
{
    public class ProfileDependencyValidationException : Xeption
    {
        public ProfileDependencyValidationException(Xeption innerException)
            : base(message: "Profile dependency validation occurred, please try again.", innerException)
        { }
    }
}
