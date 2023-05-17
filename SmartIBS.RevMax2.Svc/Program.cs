using SmartIBS.RevMax2.Svc;
using SmartIBS.RevMax2.Svc.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var url = builder.Configuration.GetValue(typeof(string),"BaseUrl");

if(!string.IsNullOrEmpty(url.ToString()) )
    builder.WebHost.UseUrls(new[] { url.ToString() });

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.ConfigureKestrel(k =>  k.AddServerHeader = false);

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Logger.LogInformation(RevMax.GetDeviceDetail());
app.Run();

