// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using System.Runtime.Serialization;
using Moq;
using Taarafo.Core.Brokers.DateTimes;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.GroupPosts;
using Taarafo.Core.Services.Foundations.GroupPosts;
using Taarafo.Core.Services.Foundations.Groups;
using Tynamix.ObjectFiller;
using Xeptions;
using Taarafo.Core.Models.Groups;
using Taarafo.Core.Models.Posts;
using System.Linq;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.GroupPosts
{
    public partial class GroupPostServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IGroupPostService groupPostService;

        public GroupPostServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.groupPostService = new GroupPostService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 1, max: 10).GetValue();

        private static GroupPost CreateRandomGroupPost(DateTimeOffset dates) =>
            CreateGroupPostFiller(dates).Create();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static GroupPost CreateRandomGroupPost() =>
            CreateGroupPostFiller(GetRandomDateTimeOffset()).Create();

        private static IQueryable<GroupPost> CreateRandomGroupPosts()
        {
            return CreateGroupPostFiller( GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber()).AsQueryable();
        }

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));
        
        private static string GetRandomMessage() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static Filler<GroupPost> CreateGroupPostFiller(DateTimeOffset dates)
        {
            var filler = new Filler<GroupPost>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
