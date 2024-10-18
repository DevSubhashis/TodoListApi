using Microsoft.EntityFrameworkCore;
using TodoListApi.Data;
using TodoListApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("TodoListConnection")));

builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
