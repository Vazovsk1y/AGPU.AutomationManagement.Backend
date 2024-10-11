using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.DAL.PostgreSQL.Extensions;
using AGPU.AutomationManagement.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddApplication();
builder.Services.AddDataAccessLayer(builder.GetDatabaseSettings());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
    app.MigrateDatabase();
}

app.MapControllers();
app.Run();