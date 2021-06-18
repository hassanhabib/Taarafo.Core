// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using Microsoft.EntityFrameworkCore;
using Taarafo.Core.Models.Posts;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Post> Posts { get; set; }

        public IQueryable<Post> SelectAllPosts() => this.Posts;
    }
}
