// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;
using Taarafo.Core.Infrastructure.Provision.Models.Storages;

namespace Taarafo.Core.Infrastructure.Provision.Brokers.Clouds
{
	public partial interface ICloudBroker
	{
		ValueTask<ISqlServer> CreateSqlServerAsync(
			string sqlServerName,
			IResourceGroup resourceGroup);

		ValueTask<ISqlDatabase> CreateSqlDatabaseAsync(
			string sqlDatabasename,
			ISqlServer sqlServer);

		SqlDatabaseAccess GetAdminAccess();
	}
}
