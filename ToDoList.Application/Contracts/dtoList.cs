namespace ToDoList.Application.Contracts
{
	public class dtoList
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public List<dtoItem>? Items { get; set; } = new List<dtoItem>();
	}
}
