// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taarafo.Core.Models.GroupPosts;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<GroupPost> GroupPosts { get; set; }

        public async ValueTask<GroupPost> InsertGroupPostAsync(GroupPost groupPost) =>
            await InsertAsync(groupPost);

        public IQueryable<GroupPost> SelectAllGroupPosts() =>
            SelectAll<GroupPost>();

        public async ValueTask<GroupPost> UpdateGroupPostAsync(GroupPost groupPost) =>
            await UpdateAsync(groupPost);
            
        public async ValueTask<GroupPost> DeleteGroupPostAsync(GroupPost groupPost) =>
            await DeleteAsync(groupPost);
    }
}
