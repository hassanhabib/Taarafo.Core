// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.PostReports.Exceptions
{
	public class FailedPostReportStorageException : Xeption
	{
		public FailedPostReportStorageException(Exception innerException)
			: base(message: "Failed postReport storage error occurred, contact support.", innerException)
		{ }
	}
}
