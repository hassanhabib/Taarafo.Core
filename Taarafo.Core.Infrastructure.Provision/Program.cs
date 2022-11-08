// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Taarafo.Core.Infrastructure.Provision.Services.Processings;

namespace Taarafo.Core.Infrastructure.Provision
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			ICloudManagementProcessingService cloudManagementProcessingService =
				new CloudManagementProcessingService();

			await cloudManagementProcessingService.ProcessAsync();
		}
	}
}