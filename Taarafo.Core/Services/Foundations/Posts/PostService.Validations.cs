// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.Posts;
using Taarafo.Core.Models.Posts.Exceptions;

namespace Taarafo.Core.Services.Foundations.Posts
{
    public partial class PostService
    {
        private static void ValidatePost(Post post)
        {
            ValidatePostIsNotNull(post);
        }

        private static void ValidatePostIsNotNull(Post post)
        {
            if(post is null)
            {
                throw new NullPostException();
            }
        }
    }
}
