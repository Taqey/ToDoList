﻿namespace ToDoList.API.Contracts
{
	public class ItemDto
	{
		public string Name { get; set; }
		public string? Description { get; set; }
		public bool IsCompleted { get; set; } = false;
	}
}
