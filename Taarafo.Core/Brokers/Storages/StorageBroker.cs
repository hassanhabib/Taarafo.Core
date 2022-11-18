﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

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
		public async ValueTask<T> DeleteProfileAsync()
		{
			var broker = await 
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			AddCommentConfigurations(modelBuilder);
			AddGroupPostConfigurations(modelBuilder);
			AddPostImpressionConfigurations(modelBuilder);
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
