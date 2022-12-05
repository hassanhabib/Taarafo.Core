// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace Taarafo.Core.Infrastructure.Provision.Brokers.Clouds
{
	public partial class CloudBroker
	{
		public async ValueTask<IResourceGroup> CreateResourceGroupAsync(string resourceGroupName)
		{
			return await this.azure.ResourceGroups
				.Define(name: resourceGroupName)
				.WithRegion(region: Region.USWest3)
				.CreateAsync();
		}

		public async ValueTask<bool> CheckResourceGroupExistAsync(string resourceGroupName) =>
			await this.azure.ResourceGroups.ContainAsync(resourceGroupName);

		public async ValueTask DeleteResourceGroupAsync(string resourceGroupName) =>
			await this.azure.ResourceGroups.DeleteByNameAsync(resourceGroupName);
	}
}
