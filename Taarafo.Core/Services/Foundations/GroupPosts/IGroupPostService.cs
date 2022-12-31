// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.GroupPosts;

namespace Taarafo.Core.Services.Foundations.GroupPosts
{
    public interface IGroupPostService
    {
        ValueTask<GroupPost> AddGroupPostAsync(GroupPost groupPost);
        IQueryable<GroupPost> RetrieveAllGroupPosts();
    }
}
