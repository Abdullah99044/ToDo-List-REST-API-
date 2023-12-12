using Microsoft.AspNetCore.Identity;
using Model.Models;
using TodoList.Data;
using TodoList.Services;
using TodoList.Services.ToDoListsServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


//Add custom services

builder.Services.AddScoped< IListsServices , ListServices>();
builder.Services.AddScoped< ITodoListServices, ToDoListServices>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Add database

builder.Services.AddDbContext<ApplicationDbContext>();

//Add authorization and authentication

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<Users>().AddEntityFrameworkStores<ApplicationDbContext>();

 

//Add CORS policy

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5173")
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();


        //Later for security
        // builder.WithOrigins(URL)
        //        .AllowAnyHeader()
        //          .WithMethods("GET", "POST", "PATCH");

    });
});


//Add Auto mapper


builder.Services.AddAutoMapper(typeof(Program).Assembly);




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.MapIdentityApi<Users>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
