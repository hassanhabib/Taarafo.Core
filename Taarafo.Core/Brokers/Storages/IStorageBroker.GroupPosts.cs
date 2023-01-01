// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.GroupPosts;

namespace Taarafo.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<GroupPost> InsertGroupPostAsync(GroupPost groupPost);
        IQueryable<GroupPost> SelectAllGroupPosts();
        ValueTask<GroupPost> SelectGroupPostByIdAsync(Guid groupId, Guid postId);
        ValueTask<GroupPost> UpdateGroupPostAsync(GroupPost groupPost);
        ValueTask<GroupPost> DeleteGroupPostAsync(GroupPost groupPost);
    }
}
