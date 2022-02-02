// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Profiles.Exceptions
{
    public class NullProfileException : Xeption
    {
        public NullProfileException()
            : base(message: "Profile is null.")
        {

        }
    }
}
