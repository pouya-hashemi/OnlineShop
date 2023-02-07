using Microsoft.Extensions.FileProviders;
using OnlineShop.Application.Common;
using OnlineShop.Domain.Common;
using OnlineShop.Infrastructure.Common;
using OnlineShop.WebApi.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDomain();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWebApi();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("default",opt =>
    {
        opt.AllowAnyHeader()
            .AllowAnyOrigin();
    });
});

var app = builder.Build();
app.ApplyMigrations();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(builder.Configuration["FileStoringBasePath"]),
    RequestPath = new PathString(builder.Configuration["FileDownloadBaseUrl"]),
    EnableDefaultFiles = true
});

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("default");
app.UseMiddleware<AppExceptionHandlerMiddleware>();

app.MapControllers();


app.Run();