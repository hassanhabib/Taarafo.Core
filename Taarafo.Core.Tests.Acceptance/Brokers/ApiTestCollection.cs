// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xunit;

namespace Taarafo.Core.Tests.Acceptance.Brokers
{
    [CollectionDefinition(nameof(ApiTestCollection))]
    public class ApiTestCollection : ICollectionFixture<ApiBroker>
    { }
}