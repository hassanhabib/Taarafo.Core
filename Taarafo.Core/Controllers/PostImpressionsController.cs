// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Models.PostImpressions.Exceptions;
using Taarafo.Core.Services.Foundations.PostImpressions;

namespace Taarafo.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostImpressionsController : RESTFulController
    {
        private readonly IPostImpressionService postImpressionService;

        public PostImpressionsController(IPostImpressionService postImpressionService) =>
            this.postImpressionService = postImpressionService;

        [HttpPost]
        public async ValueTask<ActionResult<PostImpression>> PostPostImpressionAsync(PostImpression postImpression)
        {
            try
            {
                PostImpression addedPostImpression =
                    await this.postImpressionService.AddPostImpressions(postImpression);

                return Created(addedPostImpression);
            }
            catch (PostImpressionValidationException postImpressionValidationException)
            {
                return BadRequest(postImpressionValidationException.InnerException);
            }
            catch (PostImpressionDependencyValidationException postImpressionDependencyValidationException)
                when (postImpressionDependencyValidationException.InnerException is InvalidPostImpressionReferenceException)
            {
                return FailedDependency(postImpressionDependencyValidationException.InnerException);
            }
            catch (PostImpressionDependencyValidationException postImpressionDependencyValidationException)
                when (postImpressionDependencyValidationException.InnerException is AlreadyExistsPostImpressionException)
            {
                return Conflict(postImpressionDependencyValidationException.InnerException);
            }
            catch (PostImpressionDependencyException postImpressionDependecyException)
            {
                return InternalServerError(postImpressionDependecyException.InnerException);
            }
            catch (PostImpressionServiceException postImpressionServiceException)
            {
                return InternalServerError(postImpressionServiceException.InnerException);
            }
        }
    }
}