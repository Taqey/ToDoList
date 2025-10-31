namespace ToDoList.Domain.Entities
{
	public class Date
	{
		public int Id { get; set; }
		public DateOnly date { get; set; }

		public List<ItemDate>? ItemDates { get; set; } = new List<ItemDate>();


	}
}
