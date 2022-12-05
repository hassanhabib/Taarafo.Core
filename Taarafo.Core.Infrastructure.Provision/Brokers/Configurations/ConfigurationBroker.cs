// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.IO;
using Microsoft.Extensions.Configuration;
using Taarafo.Core.Infrastructure.Provision.Models.Configurations;

namespace Taarafo.Core.Infrastructure.Provision.Brokers.Configurations
{
	public class ConfigurationBroker : IConfigurationBroker
	{
		public CloudManagementConfiguration GetConfigurations()
		{
			IConfigurationRoot configurationRoot = new ConfigurationBuilder()
				.SetBasePath(basePath: Directory.GetCurrentDirectory())
				.AddJsonFile(path: "Taarafo.Core.Infrastructure.Provision\\appSettings.json", optional: false)
				.Build();

			return configurationRoot.Get<CloudManagementConfiguration>();
		}
	}
}
