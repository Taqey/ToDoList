using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoList.API.Contracts;
using ToDoList.Application.Contracts;
using ToDoList.Application.Interfaces.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoList.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListsController : ControllerBase
	{
		private readonly IListService _service;

		public ListsController(IListService service)
		{
			_service = service;
		}
		// GET: api/<ListsController>
		[HttpGet]
		public async Task<List<dtoList>> Get()
		{
			var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var lists = await _service.ShowLists(UserId);
			return lists;
		}

		// GET api/<ListsController>/5
		[HttpGet("{id}")]
		public async Task<dtoList> Get(int id)
		{

			return await _service.ShowList(id);
		}

		// POST api/<ListsController>
		[HttpPost]
		public async Task Post([FromBody] ListDto dto)
		{
			var UserId=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var List = new dtoList { Name = dto.Name, Description = dto.Description };
			await _service.CreateList(List,UserId);

		}

		// PUT api/<ListsController>/5
		[HttpPut("EditList/{id}")]
		public async Task EditList(int id, [FromBody] ListDto dto)
		{
			var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var list = new dtoList { Id = id, Name = dto.Name, Description = dto.Description, Items = dto.Items };
			await _service.UpdateList(list, UserId);
		}

		[HttpPut("AddItem/{id}")]
		public async Task AddItem(int id, [FromBody] int itemId)
		{

			await _service.AddItemToList(id, itemId);
		}

		[HttpPut("RemoveItem/{id}")]
		public async Task RemoveItem(int id, [FromBody] int itemId)
		{

			await _service.RemoveItemToList(id, itemId);
		}


		// DELETE api/<ListsController>/5
		[HttpDelete("{id}")]
		public async Task Delete(int id)
		{
			await _service.RemoveList(id);
		}
	}
}
