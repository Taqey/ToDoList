using Microsoft.AspNetCore.Authorization;
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
	[Authorize]
	public class ItemsController : ControllerBase
	{
		private readonly IItemService _service;

		public ItemsController(IItemService service)
		{
			_service = service;
		}
		// GET: api/<ItemsController>
		[HttpGet]
		public async Task<List<dtoItem>> Get()
		{
			var UserId=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			return await _service.GetItems(UserId);
		}

		// GET api/<ItemsController>/5
		[HttpGet("{id}")]
		public async Task<dtoItem> Get(int id)
		{
			var item= await _service.GetItem(id);
			return item;
			
		}

		// POST api/<ItemsController>
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] ItemDto dto)
		{
			var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var item=new dtoItem { Name =dto.Name,Description=dto.Description,IsCompleted=dto.IsCompleted};
			await _service.CreateItem(item,UserId);
			return CreatedAtAction(nameof(Get), new { id = item.Id }, item);

		}

		// PUT api/<ItemsController>/5
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id,[FromBody] ItemDto dto)
		{
			var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var item = new dtoItem { Name = dto.Name, Description = dto.Description, IsCompleted = dto.IsCompleted ,Id=id};
			await _service.UpdateItem(item, UserId);
			return Ok(item);

		}

		// DELETE api/<ItemsController>/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _service.DeleteItem(id);
			return NoContent();
		}
	}
}
