using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Contracts;
using ToDoList.Application.Interfaces.Services;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Implementaion.Services
{
	public class RegisterService : IRegisterService
	{
		private readonly IUserService _userService;

		public RegisterService(IUserService userService)
		{
			_userService = userService;
		}
		public async Task<dtoRegisterResult> Register(dtoRegister dto)
		{
			ApplicationUser? check=null;
			check=await _userService.GetByEmailAsync(dto.Email);
			if (check!=null)
			{
				return new dtoRegisterResult { Message = "Email already exists" };
			}
			check=await _userService.GetByUsernameAsync(dto.UserName);
			if (check != null)
			{
				return new dtoRegisterResult { Message = "Username already exists" };
			}
			ApplicationUser user=new ApplicationUser { Email = dto.Email,UserName=dto.UserName,PhoneNumber=dto.PhoneNumber,FirstName=dto.FirstName,LastName=dto.LastName };
			var result=await _userService.CreateUserAsync(user, dto.Password);
			if (result.Succeeded)
			{
				return new dtoRegisterResult { Message = "Registered Successfuly " ,IsRegistered=true};
			}
			return new dtoRegisterResult { Message = "Error Occured while Registering" };
		}
	}
}
