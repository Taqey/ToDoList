using ToDoList.Application.Contracts;

namespace ToDoList.API.Contracts
{
	public class AddOrRemoveItem
	{
		public int ItemId { get; set; }
		public string ItemName { get; set; }
		public string? ItemDescription { get; set; }
		public bool IsCompleted { get; set; } = false;
		public int ListId { get; set; }

		public string ListName { get; set; }
		public string? ListDescription { get; set; }
		public List<dtoItem>? Items { get; set; } = new List<dtoItem>();
	}
}
