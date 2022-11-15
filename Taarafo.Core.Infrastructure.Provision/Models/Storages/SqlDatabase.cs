// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Microsoft.Azure.Management.Sql.Fluent;

namespace Taarafo.Core.Infrastructure.Provision.Models.Storages
{
	public class SqlDatabase
	{
		public string ConnectionString { get; set; }
		public ISqlDatabase Database { get; set; }
	}
}
