using InventoryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))); // UseSqlServer for SQL Server if needed

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); 

app.UseRouting();



app.MapControllers();

app.UseAuthorization();
app.MapRazorPages();

app.MapHub<InventoryHub>("/InventoryHub");

app.Run();