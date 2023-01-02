// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Taarafo.Core.Models.GroupMemberships;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<GroupMembership> GroupMemberships { get; set; }
    }
}
