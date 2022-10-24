// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Taarafo.Core.Models.GroupPosts;
using Taarafo.Core.Models.GroupPosts.Exceptions;

namespace Taarafo.Core.Services.Foundations.GroupPosts
{
    public partial class GroupPostService
    {
        public static void ValidateGroupPostOnAdd(GroupPost groupPost)
        {
            ValidateGroupPostIsNotNull(groupPost);
        }

        private static void ValidateGroupPostIsNotNull(GroupPost groupPost)
        {
            if(groupPost is null)
            {
                throw new NullGroupPostException();
            }
        }
    }
}
