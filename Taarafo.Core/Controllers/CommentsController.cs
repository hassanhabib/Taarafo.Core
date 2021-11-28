// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Taarafo.Core.Models.Comments;
using Taarafo.Core.Models.Comments.Exceptions;
using Taarafo.Core.Services.Foundations.Comments;

namespace Taarafo.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : RESTFulController
    {
        private readonly ICommentService commentService;

        public CommentsController(ICommentService commentService) =>
            this.commentService = commentService;

        [HttpPost]
        public async ValueTask<ActionResult<Comment>> PostCommentAsync(Comment comment)
        {
            try
            {
                Comment addedComment =
                    await this.commentService.AddCommentAsync(comment);

                return Created(addedComment);
            }
            catch (CommentValidationException commentValidationException)
            {
                return BadRequest(commentValidationException.InnerException);
            }
            catch (CommentDependencyValidationException commentValidationException)
                when (commentValidationException.InnerException is InvalidCommentReferenceException)
            {
                return FailedDependency(commentValidationException.InnerException);
            }
            catch (CommentDependencyValidationException commentDependencyValidationException)
               when (commentDependencyValidationException.InnerException is AlreadyExistsCommentException)
            {
                return Conflict(commentDependencyValidationException.InnerException);
            }
            catch (CommentDependencyException commentDependencyException)
            {
                return InternalServerError(commentDependencyException);
            }
            catch (CommentServiceException commentServiceException)
            {
                return InternalServerError(commentServiceException);
            }
        }

        [HttpDelete("{commentId}")]
        public async ValueTask<ActionResult<Comment>> DeleteCommentByIdAsync(Guid commentId)
        {
            try
            {
                Comment deletedComment =
                    await this.commentService.RemoveCommentByIdAsync(commentId);

                return Ok(deletedComment);
            }
            catch (CommentValidationException commentValidationException)
                when (commentValidationException.InnerException is NotFoundCommentException)
            {
                return NotFound(commentValidationException.InnerException);
            }
            catch (CommentValidationException commentValidationException)
            {
                return BadRequest(commentValidationException.InnerException);
            }
            catch (CommentDependencyValidationException commentDependencyValidationException)
                when (commentDependencyValidationException.InnerException is LockedCommentException)
            {
                return Locked(commentDependencyValidationException.InnerException);
            }
            catch (CommentDependencyValidationException commentDependencyValidationException)
            {
                return BadRequest(commentDependencyValidationException);
            }
            catch (CommentDependencyException commentDependencyException)
            {
                return InternalServerError(commentDependencyException);
            }
            catch (CommentServiceException commentServiceException)
            {
                return InternalServerError(commentServiceException);
            }
        }
    }
}
