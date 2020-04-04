using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using PlainApiGateway.TestServer.ViewModel;

namespace PlainApiGateway.TestServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public sealed class PostController : ControllerBase
    {
        private static readonly List<PostViewModel> Content = new List<PostViewModel>
        {
            new PostViewModel
            {
                Id = 1,
                UserId = 1,
                Title = "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
                Body = "uia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
            },
            new PostViewModel
            {
                Id = 2,
                UserId = 1,
                Title = "qui est esse",
                Body = "est rerum tempore vitae\nsequi sint nihil reprehenderit dolor beatae ea dolores neque\nfugiat blanditiis voluptate porro vel nihil molestiae ut reiciendis\nqui aperiam non debitis possimus qui neque nisi nulla"
            }
        };

        [HttpGet("all")]
        [ProducesResponseType(typeof(List<PostViewModel>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return this.Ok(Content);
        }

        [HttpGet("allxml")]
        [Produces(ContentType.Xml)]
        [ProducesResponseType(typeof(List<PostViewModel>), StatusCodes.Status200OK)]
        public IActionResult GetAllXml()
        {
            return this.Ok(Content);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PostViewModel), StatusCodes.Status200OK)]
        public IActionResult GetById([FromRoute]int id)
        {
            var post = Content.FirstOrDefault(a => a.Id == id);
            if (post == null)
            {
                return this.NotFound();
            }

            return this.Ok(post);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PostViewModel), StatusCodes.Status200OK)]
        public IActionResult GetByUserId([FromQuery(Name = "userId")]int userId)
        {
            var posts = Content.Where(a => a.UserId == userId).ToList();

            return this.Ok(posts);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PostViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(PostViewModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(PostViewModel), StatusCodes.Status204NoContent)]
        public IActionResult Add([FromBody]PostViewModel post)
        {
            if (post == null)
            {
                return this.BadRequest();
            }

            bool doesPostExist = Content.Any(a => a.Id == post.Id);
            if (doesPostExist)
            {
                return this.Conflict();
            }

            Content.Add(post);

            return this.NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PostViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(PostViewModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(PostViewModel), StatusCodes.Status204NoContent)]
        public IActionResult Edit([FromRoute]int id, [FromBody]PostViewModel post)
        {
            if (post == null || id != post.Id)
            {
                return this.BadRequest();
            }

            var postToEdit = Content.FirstOrDefault(a => a.Id == post.Id);
            if (postToEdit == null)
            {
                return this.NotFound();
            }

            Content.Remove(postToEdit);
            Content.Add(post);

            return this.NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(PostViewModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(PostViewModel), StatusCodes.Status204NoContent)]
        public IActionResult Delete([FromRoute]int id)
        {
            var postToEdit = Content.FirstOrDefault(a => a.Id == id);
            if (postToEdit == null)
            {
                return this.NotFound();
            }

            Content.Remove(postToEdit);

            return this.NoContent();
        }
    }
}
