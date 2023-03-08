// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
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

        [HttpGet("{postReportId}")]
        public async ValueTask<ActionResult<PostReport>> GetReportByIdAsync(Guid postReportId)
        {
            try
            {
                return await this.postReportService.RetrievePostReportByIdAsync(postReportId);
            }
            catch (PostReportDependencyException postReportDependencyException)
            {
                return InternalServerError(postReportDependencyException.InnerException);
            }
            catch (PostReportValidationException postReportValidationException)
                when (postReportValidationException.InnerException is InvalidPostReportException)
            {
                return BadRequest(postReportValidationException.InnerException);
            }
            catch (PostReportValidationException postReportValidationException)
                when (postReportValidationException.InnerException is NotFoundPostReportException)
            {
                return NotFound(postReportValidationException.InnerException);
            }
            catch (PostReportServiceException postReportServiceException)
            {
                return InternalServerError(postReportServiceException.InnerException);
            }
        }
    }
}