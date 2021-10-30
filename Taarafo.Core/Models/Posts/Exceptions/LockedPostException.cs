using System;
using Xeptions;

namespace Taarafo.Core.Models.Posts.Exceptions
{
    public class LockedPostException : Xeption
    {
        public LockedPostException(Exception innerException)
            : base(message: "Locked teacher record exception, please try again later.", innerException)
        { }
    }
}
