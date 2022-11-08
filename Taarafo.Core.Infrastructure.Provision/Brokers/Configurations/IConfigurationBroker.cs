// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Taarafo.Core.Infrastructure.Provision.Models.Configurations;

namespace Taarafo.Core.Infrastructure.Provision.Brokers.Configurations
{
	public interface IConfigurationBroker
	{
		CloudManagementConfiguration GetConfigurations();
	}
}
