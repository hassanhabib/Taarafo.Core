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

        [HttpDelete("{postImpressionId}")]
        public async ValueTask<ActionResult<PostImpression>> DeletePostImpressionByIdAsync(PostImpression postImpression)
        {
            try
            {
                PostImpression deletePostImpression =
                    await this.postImpressionService.RemovePostImpressionAsync(postImpression);

                return Ok(deletePostImpression);
            }
            catch (PostImpressionValidationException postImpressionValidationException)
                when (postImpressionValidationException.InnerException is NotFoundPostImpressionException)
            {
                return NotFound(postImpressionValidationException.InnerException);
            }
            catch (PostImpressionValidationException postImpressionValidationException)
            {
                return BadRequest(postImpressionValidationException.InnerException);
            }
            catch (PostImpressionDependencyValidationException postImpressionDependencyValidationException)
                when (postImpressionDependencyValidationException.InnerException is LockedPostImpressionException)
            {
                return Locked(postImpressionDependencyValidationException.InnerException);
            }
            catch (PostImpressionDependencyValidationException postImpressionDependencyValidationException)
            {
                return BadRequest(postImpressionDependencyValidationException.InnerException);
            }
            catch (PostImpressionDependencyException postImpressionDependencyException)
            {
                return InternalServerError(postImpressionDependencyException.InnerException);
            }
            catch (PostImpressionServiceException postImpressionServiceException)
            {
                return InternalServerError(postImpressionServiceException.InnerException);
            }
        }
    }
}
