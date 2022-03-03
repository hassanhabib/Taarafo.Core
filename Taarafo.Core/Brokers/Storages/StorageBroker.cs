// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Taarafo.Core.Models.Profiles;
using Taarafo.Core.Models.GroupMemberships;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker : EFxceptionsContext, IStorageBroker
    {
        private readonly IConfiguration configuration;

        public StorageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            AddCommentReferences(modelBuilder);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = this.configuration
                .GetConnectionString(name: "DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }

        private void AddGroupMembershipsReferences(ModelBuilder modelBuilder)
        {
            throw new NotImplementedException();
        }

        public override void Dispose() { }

        public ValueTask<GroupMembership> InsertGroupMembershipAsync(GroupMembership groupMembership)
        {
            throw new NotImplementedException();
        }

        public IQueryable<GroupMembership> SelectAllGroupMemberships()
        {
            throw new NotImplementedException();
        }

        public ValueTask<GroupMembership> SelectGroupMembershipByIdAsync(Guid groupMembershipId)
        {
            throw new NotImplementedException();
        }

        public void onModelCreating(ModelBuilder modelBuilder)
        {
            throw new NotImplementedException();
        }
    }
}
