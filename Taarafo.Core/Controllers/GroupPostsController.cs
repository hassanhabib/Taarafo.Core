// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Taarafo.Core.Models.GroupPosts;
using Taarafo.Core.Models.GroupPosts.Exceptions;
using Taarafo.Core.Models.GroupPosts.Exceptions;
using Taarafo.Core.Services.Foundations.GroupPosts;

namespace Taarafo.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupPostsController : RESTFulController
    {
        private readonly IGroupPostService groupPostService;

        public GroupPostsController(IGroupPostService groupPostService) =>
            this.groupPostService = groupPostService;

        [HttpPost]
        public async ValueTask<ActionResult<GroupPost>> PostGroupPostAsync(GroupPost groupPost)
        {
            try
            {
                GroupPost addedGroupPost =
                    await this.groupPostService.AddGroupPostAsync(groupPost);

                return Created(addedGroupPost);
            }
            catch (GroupPostValidationException groupPostValidationException)
            {
                return BadRequest(groupPostValidationException.InnerException);
            }
            catch (GroupPostDependencyValidationException groupPostDependencyValidationException)
                when (groupPostDependencyValidationException.InnerException is InvalidGroupPostReferenceException)
            {
                return FailedDependency(groupPostDependencyValidationException.InnerException);
            }
            catch (GroupPostDependencyValidationException groupPostDependencyValidationException)
                when (groupPostDependencyValidationException.InnerException is AlreadyExistsGroupPostException)
            {
                return Conflict(groupPostDependencyValidationException.InnerException);
            }
            catch (GroupPostDependencyException groupPostDependencyException)
            {
                return InternalServerError(groupPostDependencyException.InnerException);
            }
            catch (GroupPostServiceException groupPostServiceException)
            {
                return InternalServerError(groupPostServiceException.InnerException);
            }
        }
    }
}
