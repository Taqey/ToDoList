namespace ToDoList.Domain.Entities
{
	public class List
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public List<Item>? Items { get; set; } = new List<Item>();
		public ApplicationUser? User { get; set; }


	}
}
