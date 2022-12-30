// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

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

        public async ValueTask<GroupPost> DeleteGroupPostAsync(GroupPost groupPost) =>
            await DeleteAsync(groupPost);
    }
}
