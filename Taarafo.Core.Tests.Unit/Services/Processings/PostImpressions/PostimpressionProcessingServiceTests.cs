// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using Moq;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Services.Foundations.PostImpressions;
using Taarafo.Core.Services.Processings.PostImpressions;
using Tynamix.ObjectFiller;

namespace Taarafo.Core.Tests.Unit.Services.Processings.PostImpressions
{
    public partial class PostImpressionProcessingServiceTests
    {
        private readonly Mock<IPostImpressionService> postImpressionServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IPostImpressionProcessingService postImpressionProcessingService;

        public PostImpressionProcessingServiceTests()
        {
            this.postImpressionServiceMock = new Mock<IPostImpressionService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.postImpressionProcessingService = new PostImpressionProcessingService(
                postImpressionService: this.postImpressionServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private IQueryable<PostImpression> CreateRandomPostImpressions() =>
            CreatePostImpressionFiller().Create(count: GetRandomNumber()).AsQueryable();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Filler<PostImpression> CreatePostImpressionFiller()
        {
            var filler = new Filler<PostImpression>();
            filler.Setup().OnType<DateTimeOffset>().Use(GetRandomDateTimeOffset());

            return filler;
        }
    }
}
