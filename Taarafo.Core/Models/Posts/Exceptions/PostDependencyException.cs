using System;

namespace Taarafo.Core.Models.Posts.Exceptions
{
    public class PostDependencyException : Exception
    {
        public PostDependencyException(Exception innerException) :
            base("Post dependency error occured, contact support.", innerException)
        { }
    }
}
