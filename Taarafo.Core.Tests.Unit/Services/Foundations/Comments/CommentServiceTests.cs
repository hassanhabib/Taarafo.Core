// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Brokers.DateTimes;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.Comments;
using Taarafo.Core.Services.Foundations.Comments;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Comments
{
	public partial class CommentServiceTests
	{
		private readonly Mock<IStorageBroker> storageBrokerMock;
		private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
		private readonly Mock<ILoggingBroker> loggingBrokerMock;
		private readonly ICommentService commentService;

		public CommentServiceTests()
		{
			this.storageBrokerMock = new Mock<IStorageBroker>();
			this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
			this.loggingBrokerMock = new Mock<ILoggingBroker>();

			this.commentService = new CommentService(
				storageBroker: this.storageBrokerMock.Object,
				dateTimeBroker: this.dateTimeBrokerMock.Object,
				loggingBroker: this.loggingBrokerMock.Object);
		}

		private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
			actualException => actualException.SameExceptionAs(expectedException);

		private static IQueryable<Comment> CreateRandomComments()
		{
			return CreateCommentFiller(date: GetRandomDateTimeOffset())
				.Create(count: GetRandomNumber())
					.AsQueryable();
		}

		private static Comment CreateRandomModifyComment(DateTimeOffset dates)
		{
			int randomDaysInPast = GetRandomNegativeNumber();
			Comment randomComment = CreateRandomComment(dates);

			randomComment.CreatedDate =
				randomComment.CreatedDate.AddDays(randomDaysInPast);

			return randomComment;
		}

		private static string GetRandomMessage() =>
			new MnemonicString(wordCount: GetRandomNumber()).GetValue();

		private static SqlException GetSqlException() =>
			(SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

		private static ForeignKeyConstraintConflictException GetForeignKeyException() =>
			(ForeignKeyConstraintConflictException)FormatterServices
				.GetUninitializedObject(typeof(ForeignKeyConstraintConflictException));

		private static int GetRandomNumber() =>
			new IntRange(min: 2, max: 10).GetValue();

		private static int GetRandomNegativeNumber() =>
			-1 * new IntRange(min: 2, max: 10).GetValue();

		public static TheoryData MinutesBeforeOrAfter()
		{
			int randomNumber = GetRandomNumber();
			int randomNegativeNumber = GetRandomNegativeNumber();

			return new TheoryData<int>
			{
				randomNumber,
				randomNegativeNumber
			};
		}

		private static Comment CreateRandomComment() =>
			CreateCommentFiller(date: GetRandomDateTimeOffset()).Create();

		private static Comment CreateRandomComment(DateTimeOffset date) =>
			CreateCommentFiller(date).Create();

		private static DateTimeOffset GetRandomDateTimeOffset() =>
			new DateTimeRange(earliestDate: new DateTime()).GetValue();

		public static IEnumerable<object[]> InvalidMinuteCases()
		{
			int randomMoreThanMinuteFromNow = GetRandomNumber();
			int randomMoreThanMinuteBeforeNow = GetRandomNegativeNumber();

			return new List<object[]>
			{
				new object[] { randomMoreThanMinuteFromNow },
				new object[] { randomMoreThanMinuteBeforeNow }
			};
		}
		private static Filler<Comment> CreateCommentFiller(DateTimeOffset date)
		{
			var filler = new Filler<Comment>();

			filler.Setup()
				.OnType<DateTimeOffset>().Use(date);

			return filler;
		}
	}
}
