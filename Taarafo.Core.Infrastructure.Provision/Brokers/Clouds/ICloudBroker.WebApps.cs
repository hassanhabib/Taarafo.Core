// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;

namespace Taarafo.Core.Infrastructure.Provision.Brokers.Clouds
{
	public partial interface ICloudBroker
	{
		ValueTask<IWebApp> CreateWebAppAsync(
			string webAppName,
			string databaseConnectionString,
			IAppServicePlan plan,
			IResourceGroup resourceGroup);
	}
}
