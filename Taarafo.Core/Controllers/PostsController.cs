// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Taarafo.Core.Models.Posts;
using Taarafo.Core.Models.Posts.Exceptions;
using Taarafo.Core.Services.Foundations.Posts;

namespace Taarafo.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : RESTFulController
    {
        private readonly IPostService postService;

        public PostsController(IPostService postService) =>
            this.postService = postService;

        [HttpPost]
        public async ValueTask<ActionResult<Post>> PostPostAsync(Post post)
        {
            try
            {
                Post AddedPost = 
                    await this.postService.AddPostAsync(post);

                return Created(AddedPost);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message); 
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Post>> GetAllPosts()
        {
            try
            {
                IQueryable<Post> retrievedPosts =
                    this.postService.RetrieveAllPosts();

                return Ok(retrievedPosts);
            }
            catch (PostDependencyException postDependencyException)
            {
                return Problem(postDependencyException.Message);
            }
            catch (PostServiceException postServiceException)
            {
                return Problem(postServiceException.Message);
            }
        }
    }
}
