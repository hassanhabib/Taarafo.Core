// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Taarafo.Core.Tests.Acceptance.Brokers;
using Xunit;

namespace Taarafo.Core.Tests.Acceptance.Apis.Homes
{
    [Collection(nameof(ApiTestCollection))]
    public class HomeApiTests
    {
        private readonly ApiBroker apiBroker;

        public HomeApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        [Fact]
        public async Task ShouldReturnHomeMessageAsync()
        {
            // given
            string expectedMesage =
                "Thank you Mario! But the princess is in another castle!";

            // when
            string actualMessage =
                await this.apiBroker.GetHomeMessage();

            // then
            actualMessage.Should().Be(expectedMesage);
        }
    }
}
