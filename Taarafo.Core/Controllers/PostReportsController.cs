// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Taarafo.Core.Models.PostReports;
using Taarafo.Core.Models.PostReports.Exceptions;
using Taarafo.Core.Services.Foundations.PostReports;

namespace Taarafo.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostReportsController : RESTFulController
    {
        private readonly IPostReportService postReportService;

        public PostReportsController(IPostReportService postReportService) =>
            this.postReportService = postReportService;

        [HttpGet]
        public ActionResult<IQueryable<PostReport>> GetAllPostReports()
        {
            try
            {
                IQueryable<PostReport> retrievedPostReports =
                    this.postReportService.RetrieveAllPostReports();

                return Ok(retrievedPostReports);
            }
            catch (PostReportDependencyException postReportDependencyException)
            {
                return InternalServerError(postReportDependencyException.InnerException);
            }
            catch (PostReportServiceException postReportServiceException)
            {
                return InternalServerError(postReportServiceException.InnerException);
            }
        }
    }
}
