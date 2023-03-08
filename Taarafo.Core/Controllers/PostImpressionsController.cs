// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
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

        [HttpGet]
        public ActionResult<IQueryable<PostImpression>> GetAllPostImpressions()
        {
            try
            {
                IQueryable<PostImpression> retrievedPostImpressions =
                    this.postImpressionService.RetrieveAllPostImpressions();

                return Ok(retrievedPostImpressions);
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
