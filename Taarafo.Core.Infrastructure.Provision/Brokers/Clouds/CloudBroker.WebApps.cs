// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.AppService.Fluent.Models;
using Microsoft.Azure.Management.ResourceManager.Fluent;

namespace Taarafo.Core.Infrastructure.Provision.Brokers.Clouds
{
    public partial class CloudBroker
    {
        public async ValueTask<IWebApp> CreateWebAppAsync(
            string webAppName,
            string databaseConnectionString,
            IAppServicePlan plan,
            IResourceGroup resourceGroup)
        {
            return await this.azure.AppServices.WebApps
                .Define(webAppName)
                .WithExistingWindowsPlan(plan)
                .WithExistingResourceGroup(resourceGroup)
                .WithNetFrameworkVersion(NetFrameworkVersion.Parse("v7.0"))
                .WithConnectionString(
                    name: "DefaultConnect",
                    value: databaseConnectionString,
                    type: ConnectionStringType.SQLAzure)
                .CreateAsync();
        }
    }
}
