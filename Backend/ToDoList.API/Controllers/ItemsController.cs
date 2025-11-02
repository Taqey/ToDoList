using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ToDoList.API.Contracts;
using ToDoList.Application.Contracts;
using ToDoList.Application.Interfaces.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoList.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
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
			return await _service.GetItems();
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
		public async Task Post([FromBody] ItemDto dto)
		{
			var item=new dtoItem { Name =dto.Name,Description=dto.Description,IsCompleted=dto.IsCompleted};
			await _service.CreateItem(item);
		}

		// PUT api/<ItemsController>/5
		[HttpPut("{id}")]
		public async Task Put(int id,[FromBody] ItemDto dto)
		{
			var item = new dtoItem { Name = dto.Name, Description = dto.Description, IsCompleted = dto.IsCompleted ,Id=id};
			await _service.UpdateItem(item);
		}

		// DELETE api/<ItemsController>/5
		[HttpDelete("{id}")]
		public async Task Delete(int id)
		{
			await _service.DeleteItem(id);
		}
	}
}
