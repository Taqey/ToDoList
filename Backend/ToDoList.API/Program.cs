using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using ToDoList.Application.Implementaion.Services;
using ToDoList.Application.Interfaces.Services;
using ToDoList.Domain.Repositories;
using ToDoList.Infrastructure.Implementation;
using ToDoList.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ToDoList.Infrastructure;
using ToDoList.Infrastructure.Implementation.Services;
using ToDoList.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ToDoList.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddOpenApi();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(options =>
			{
				options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,$"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
				options.SwaggerDoc("v1",new OpenApiInfo {Title="ToDoListAPI",Contact=new OpenApiContact { Email="atakieeldeen@gmail.com",Name="Taqeyy"},Version="v1" });
				options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,new OpenApiSecurityScheme { Name= "Authorization", In=ParameterLocation.Header,Type=SecuritySchemeType.ApiKey,Scheme=JwtBearerDefaults.AuthenticationScheme});
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = JwtBearerDefaults.AuthenticationScheme
							},
							In = ParameterLocation.Header,
							Name = "Authorization"
						},
						new List<string>()
					}
				});
			});

			builder.Services.AddDbContext<AppDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
			});
			builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
			builder.Services.AddIdentityCore<ApplicationUser>(options =>
			{ options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireDigit= true;
				options.Password.RequireLowercase= true;
				options.Password.RequireUppercase= true;
				options.Password.RequiredUniqueChars = 1;
				options.Password.RequiredLength = 8;

			})
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<AppDbContext>()
				.AddDefaultTokenProviders();
			builder.Services.AddHttpContextAccessor();

			builder.Services.AddScoped(typeof(IRepository<>),typeof(Repository<>));
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddScoped<IListService,ListService>();
			builder.Services.AddScoped<IItemService, ItemService>();
			builder.Services.AddScoped<ILoginService, LoginService>();
			builder.Services.AddScoped<IRegisterService, RegisterService>();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IRefreshToken, Refresh>();
			builder.Services.AddScoped<IJwtToken, JwtToken>();
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAll",
					builder => builder.AllowAnyOrigin()
									  .AllowAnyMethod()
									  .AllowAnyHeader());
			});
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => 
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateAudience = true,
					ValidateIssuer = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = builder.Configuration["Jwt:Issuer"],
					ValidAudience = builder.Configuration["Jwt:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
					ClockSkew=TimeSpan.Zero
				};
				});
			var app = builder.Build();
			

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseCors("AllowAll");
			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
