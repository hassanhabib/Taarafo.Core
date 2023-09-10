// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Models.PostImpressions.Exceptions;
using Taarafo.Core.Services.Foundations.PostImpressions;
using Taarafo.Core.Services.Processings.PostImpressions;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

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

        private static PostImpression CreateRandomPostImpression() =>
            CreatePostImpressionFiller().Create();

        private static IQueryable<PostImpression> CreateRandomPostImpressions() =>
            CreatePostImpressionFiller().Create(count: GetRandomNumber()).AsQueryable();

        public static TheoryData DependencyExceptions()
        {
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new PostImpressionDependencyException(innerException),
                new PostImpressionServiceException(innerException)
            };
        }

        public static TheoryData DependencyValidationExceptions()
        {
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new PostImpressionValidationException(innerException),
                new PostImpressionDependencyValidationException(innerException)
            };
        }

        public static IQueryable<PostImpression> CreateRandomPostImpressions(PostImpression postImpression)
        {
            List<PostImpression> randomPostImpressions =
                CreateRandomPostImpressions().ToList();

            randomPostImpressions.Add(postImpression);

            return randomPostImpressions.AsQueryable();
        }

        private static string GetRandomMessage() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Filler<PostImpression> CreatePostImpressionFiller()
        {
            var filler = new Filler<PostImpression>();

            filler.Setup().OnType<DateTimeOffset>()
                .Use(GetRandomDateTimeOffset());

            return filler;
        }
    }
}