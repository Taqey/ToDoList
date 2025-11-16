using Microsoft.AspNetCore.Identity;

namespace ToDoList.Domain.Entities
{
	public class ApplicationUser:IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public List<Item>? Items { get; set; }=new List<Item>();
		public List<List>?Lists { get; set; }=new List<List>();
		public List<RefreshToken>? RefreshTokens { get; set; } =new List<RefreshToken>();
	}
}
