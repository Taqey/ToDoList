using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Domain.Entities
{
	public class ItemDate
	{
		public int Id { get; set; }

		public int? ItemId { get; set; }
		public int? DateId { get; set; }
		public Date? Date { get; set; }
		public Item? Item { get; set; }	

	}
}
