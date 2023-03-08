// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
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

        [HttpPost]
        public async ValueTask<ActionResult<PostReport>> PostReportAsync(PostReport postReport)
        {
            try
            {
                PostReport addedPostReport =
                    await this.postReportService.AddPostReportAsync(postReport);

                return Created(addedPostReport);
            }
            catch (PostReportValidationException postReportValidationException)
            {
                return BadRequest(postReportValidationException.InnerException);
            }
            catch (PostReportDependencyValidationException postReportDependencyValidationException)
                when (postReportDependencyValidationException.InnerException is InvalidPostReportReferenceException)
            {
                return FailedDependency(postReportDependencyValidationException.InnerException);
            }
            catch (PostReportDependencyValidationException postReportDependencyValidationException)
                when (postReportDependencyValidationException.InnerException is AlreadyExistsPostReportException)
            {
                return Conflict(postReportDependencyValidationException.InnerException);
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