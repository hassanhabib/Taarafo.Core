// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Taarafo.Core.Models.Groups;
using Taarafo.Core.Models.Groups.Exceptions;
using Taarafo.Core.Services.Foundations.Groups;

namespace Taarafo.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : RESTFulController
    {
        private readonly IGroupService groupService;

        public GroupsController(IGroupService groupService) =>
            this.groupService = groupService;

        [HttpPost]
        public async ValueTask<ActionResult<Group>> GroupPostAsync(Group group)
        {
            try
            {
                Group addedGroup =
                    await this.groupService.AddGroupAsync(group);

                return Created(addedGroup);
            }
            catch (GroupValidationException groupValidationException)
            {
                return BadRequest(groupValidationException.InnerException);
            }
            catch (GroupDependencyValidationException groupDependencyValidationException)
                when (groupDependencyValidationException.InnerException is InvalidGroupReferenceException)
            {
                return FailedDependency(groupDependencyValidationException.InnerException);
            }
            catch (GroupDependencyValidationException groupDependencyValidationException)
                when (groupDependencyValidationException.InnerException is AlreadyExistsGroupException)
            {
                return Conflict(groupDependencyValidationException.InnerException);
            }
            catch (GroupDependencyException groupDependencyException)
            {
                return InternalServerError(groupDependencyException);
            }
            catch (GroupServiceException groupServiceException)
            {
                return InternalServerError(groupServiceException);
            }
        }
    }
}