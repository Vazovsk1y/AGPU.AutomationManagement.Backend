using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.DAL.PostgreSQL.Extensions;
using AGPU.AutomationManagement.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebApi();
builder.Services.AddApplication(builder.GetAuthSettings());
builder.Services.AddDataAccessLayer(builder.GetDatabaseSettings());

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MigrateDatabase();

    app.UseCors(e => e
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin());
}

// TODO: Настроить CORS для Production.

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();