using Asp.Versioning;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Model.Models;
using Serilog;
using System;
using System.Threading.RateLimiting;
using TodoList.Data;
using TodoList.MiddleWare;
using TodoList.Services;
using TodoList.Services.ToDoListsServices;
using TodoList.Services.TodoTasksService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


//Add custom services

builder.Services.AddScoped< IListsServices , ListServices>();
builder.Services.AddScoped< ITodoListServices, ToDoListServices>();
builder.Services.AddScoped< ITodoTasksService , TodoTasksService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Add database

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});



//Add authorization and authentication

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<Users>().AddEntityFrameworkStores<ApplicationDbContext>();

//Add versioning

builder.Services.AddApiVersioning(options =>
{

    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1,0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(

        new HeaderApiVersionReader("x-version")
    );
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

 
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



//Add security headers 

builder.Services.AddTransient<securityHeadersMiddleWare>();


//Add Inmemeory cache

builder.Services.AddSingleton<MyMemoryCache>();


//Add rate limiting based on Ip address

builder.Services.AddRateLimiter(options => { 
 
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
     
    options.AddPolicy("FixedWindow", HttpContext =>

        RateLimitPartition.GetFixedWindowLimiter(
            
            partitionKey : HttpContext.Connection.RemoteIpAddress?.ToString(),
            factory : _ => new FixedWindowRateLimiterOptions
            {

                PermitLimit = 10,
                Window = TimeSpan.FromSeconds(10)

            }  
        )
    );
});


//Add logger 

Log.Logger = new LoggerConfiguration().MinimumLevel
                                        .Error()
                                        .WriteTo
                                        .Console()
                                        .WriteTo.File("logs/ApiLog-.txt", rollingInterval: RollingInterval.Day)
                                        .CreateLogger();

builder.Host.UseSerilog();



//Add Global error handler

builder.Services.AddTransient<GlobalExceptionHandlingMiddlewarecs>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<GlobalExceptionHandlingMiddlewarecs>();

app.UseHttpsRedirection();

app.UseCors();

app.MapIdentityApi<Users>();

app.UseAuthorization();

app.UseRateLimiter();

app.UseSerilogRequestLogging();

app.UseMiddleware<securityHeadersMiddleWare>();

app.MapControllers();

app.Run();
