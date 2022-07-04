// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Taarafo.Core.Infrastructure.Build.Services;

var integrationService = new IntegrationService();
integrationService.GenerateBuildScripts();
integrationService.GenerateProvisionScripts();