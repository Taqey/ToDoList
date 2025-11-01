namespace ToDoList.Domain.Entities
{
	public class Item
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsCompleted { get; set; } = false;
		public int? ListId { get; set; }       
		public List? List { get; set; }
		public List<ItemDate>? ItemDates { get; set; } = new List<ItemDate>();
		public ApplicationUser? User { get; set; }

	}
}
