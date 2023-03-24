// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
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

        [HttpGet]
        public ActionResult<IQueryable<Group>> GetAllGroups()
        {
            try
            {
                IQueryable<Group> retrievedGroups =
                    this.groupService.RetrieveAllGroups();

                return Ok(retrievedGroups);
            }
            catch (GroupDependencyException groupDependencyException)
            {
                return InternalServerError(groupDependencyException.InnerException);
            }
            catch (GroupServiceException groupServiceException)
            {
                return InternalServerError(groupServiceException.InnerException);
            }
        }

        [HttpGet("{groupId}")]
        public async ValueTask<ActionResult<Group>> GetGroupByIdAsync(Guid groupId)
        {
            try
            {
                Group retrievedGroupById =
                    await this.groupService.RemoveGroupByIdAsync(groupId);

                return Ok(retrievedGroupById);
            }
            catch (GroupValidationException groupValidationException)
                when (groupValidationException.InnerException is NotFoundGroupException)
            {
                return NotFound(groupValidationException.InnerException);
            }
            catch (GroupValidationException groupValidationException)
            {
                return BadRequest(groupValidationException.InnerException);
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

        [HttpPut]
        public async ValueTask<ActionResult<Group>> PutGroupAsync(Group group)
        {
            try
            {
                Group modifiedGroup =
                    await this.groupService.ModifyGroupAsync(group);

                return Ok(modifiedGroup);
            }
            catch (GroupValidationException groupValidationException)
                when (groupValidationException.InnerException is NotFoundGroupException)
            {
                return NotFound(groupValidationException.InnerException);
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
            {
                return BadRequest(groupDependencyValidationException.InnerException);
            }
            catch (GroupDependencyException groupDependencyException)
            {
                return InternalServerError(groupDependencyException.InnerException);
            }
            catch (GroupServiceException groupServiceException)
            {
                return InternalServerError(groupServiceException.InnerException);
            }
        }

        [HttpDelete("{groupId}")]
        public async ValueTask<ActionResult<Group>> DeleteGroupByIdAsync(Guid groupId)
        {
            try
            {
                Group deletedGroup =
                    await this.groupService.RemoveGroupByIdAsync(groupId);

                return Ok(deletedGroup);
            }
            catch (GroupValidationException groupValidationException)
                when (groupValidationException.InnerException is NotFoundGroupException)
            {
                return NotFound(groupValidationException.InnerException);
            }
            catch (GroupValidationException groupValidationException)
            {
                return BadRequest(groupValidationException.InnerException);
            }
            catch (GroupDependencyValidationException groupDependencyValidationException)
                when (groupDependencyValidationException.InnerException is LockedGroupException)
            {
                return Locked(groupDependencyValidationException.InnerException);
            }
            catch (GroupDependencyValidationException groupDependencyValidationException)
            {
                return BadRequest(groupDependencyValidationException.InnerException);
            }
            catch (GroupDependencyException groupDependencyException)
            {
                return InternalServerError(groupDependencyException.InnerException);
            }
            catch (GroupServiceException groupServiceException)
            {
                return InternalServerError(groupServiceException.InnerException);
            }
        }
    }
}