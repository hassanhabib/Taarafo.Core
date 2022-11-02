// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace Taarafo.Core.Infrastructure.Provision.Services.Processings
{
	public interface ICloudManagementProcessingService
	{
		ValueTask ProcessAsync();
	}
}
