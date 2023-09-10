// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.PostReports.Exceptions
{
    public class NotFoundPostReportException : Xeption
    {
        public NotFoundPostReportException(Guid postReportId)
           : base(message: $"Couldn't find postReport with id: {postReportId}.")
        { }
    }
}