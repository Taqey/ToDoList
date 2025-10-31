using Microsoft.AspNetCore.Identity;

namespace ToDoList.Domain.Entities
{
	public class ApplicationUser:IdentityUser
	{
		public List<Item>? Items { get; set; }=new List<Item>();
		public List<List>?Lists { get; set; }=new List<List>();
	}
}
