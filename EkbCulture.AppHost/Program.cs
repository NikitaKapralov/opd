using EkbCulture.AppHost.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.Swagger;
using Swashbuckle.SwaggerUi;
using Swashbuckle.AspNetCore.Swagger;


var builder = WebApplication.CreateBuilder(args); //������ builder

// 2. ��������� ������� �� ������ builder.Build()
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer(); // 3. ��������� Swagger �� Build()
builder.Services.AddSwaggerGen();

var app = builder.Build(); // 4. ������ ����������

// 5. ��������� middleware ����� Build()
app.UseSwagger();
app.UseSwaggerUI();

// 6. �������� ����������� � ��
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    Console.WriteLine("Database connected successfully!");
}

app.Run(); // 7. ��������� ����������