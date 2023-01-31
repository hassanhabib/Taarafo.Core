// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Taarafo.Core.Models.PostReports;
using Taarafo.Core.Models.PostReports.Exceptions;

namespace Taarafo.Core.Services.Foundations.PostReports
{
    public partial class PostReportService
    {
        private static void ValidatePostReport(PostReport postReport)
        {
            ValidatePostReportNotNull(postReport);

            Validate(
                (Rule: IsInvalid(postReport.Id), Parameter: nameof(PostReport.Id)),
                (Rule: IsInvalid(postReport.Details), Parameter: nameof(PostReport.Details)),
                (Rule: IsInvalid(postReport.PostId), Parameter: nameof(PostReport.PostId)),
                (Rule: IsInvalid(postReport.Post), Parameter: nameof(PostReport.Post)),
                (Rule: IsInvalid(postReport.ReporterId), Parameter: nameof(PostReport.ReporterId)),
                (Rule: IsInvalid(postReport.Profile), Parameter: nameof(PostReport.Profile)),
                (Rule: IsInvalid(postReport.CreatedDate), Parameter: nameof(PostReport.CreatedDate)),
                (Rule: IsInvalid(postReport.UpdatedDate), Parameter: nameof(PostReport.UpdatedDate)),

                (Rule: IsNotSame(
                    firstDate: postReport.CreatedDate,
                    secondDate: postReport.UpdatedDate,
                    secondDateName: nameof(PostReport.UpdatedDate)),

                Parameter: nameof(PostReport.CreatedDate)));
        }


        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            InvalidPostReportException invalidPostReportException = new InvalidPostReportException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidPostReportException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidPostReportException.ThrowIfContainsErrors();
        }

        private static void ValidatePostReportNotNull(PostReport postReport)
        {
            if (postReport is null)
            {
                throw new NullPostReportException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Value is required"
        };

        private static dynamic IsInvalid(object @object) => new
        {
            Condition = @object is null,
            Message = "Object is required"
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };
    }
}
