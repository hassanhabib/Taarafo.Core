// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

		private async ValueTask<T> InsertAsync<T>(T @object)
		{
			var broker = new StorageBroker(this.configuration);
			broker.Entry(@object).State = EntityState.Added;
			await broker.SaveChangesAsync();

			return @object;
		}

		private async ValueTask<T> SelectAsync<T>(params object[] objectIds) where T : class =>
			await FindAsync<T>(objectIds);
		
		private async ValueTask<T> DeleteAsync<T>(T @object)
		{
			var broker = new StorageBroker(this.configuration);
			broker.Entry(@object).State-= EntityState.Deleted;
			await broker.SaveChangesAsync();

			return @object;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			AddCommentConfigurations(modelBuilder);
			AddGroupPostConfigurations(modelBuilder);
			AddPostImpressionConfigurations(modelBuilder);
		}
       
		private async ValueTask<T> UpdateAsync<T>(T @object)
        {
            var broker = new StorageBroker(this.configuration);
            broker.Entry(@object).State = EntityState.Modified;
            await broker.SaveChangesAsync();

            return @object;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			string connectionString = this.configuration
				.GetConnectionString(name: "DefaultConnection");

			optionsBuilder.UseSqlServer(connectionString);
		}

		public override void Dispose() { }
	}
}
