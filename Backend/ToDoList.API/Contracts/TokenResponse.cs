namespace ToDoList.API.Contracts
{
	public class TokenResponse
	{
		public string Token { get; set; }
		public DateTime? ExpiresOn {  get; set; }
	}
}
