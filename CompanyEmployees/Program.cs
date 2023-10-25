using CompanyEmployees.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(),
    "./nlog.config"));

// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts(); // Will add middleware for using HSTS, which adds the Strict - Transport - Security header

app.UseHttpsRedirection();


app.UseStaticFiles(); // Enables using static files for the request. If we don’t set a path to the static files directory, it will use a wwwroot folder in our project by default

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");

app.UseAuthorization(); // Used to add the middleware for the redirection from HTTP to HTTPS.

app.MapControllers(); // Adds the endpoints from controller actions to the IEndpointRouteBuilder 

app.Run();
