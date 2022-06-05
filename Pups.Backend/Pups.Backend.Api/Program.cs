using Microsoft.OpenApi.Models;
using Pups.Backend.Api.Data;
using Pups.Backend.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSqlServer<MessengerContext>(
    builder.Configuration.GetConnectionString("ApiContext"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Pups Messenger API",
        Version = "v1.0"
    });

    var filePath = Path.Combine(AppContext.BaseDirectory, "Pups.Backend.Api.xml");
    options.IncludeXmlComments(filePath);
});

builder.Services.AddTransient<IUserService, MsSqlUserService>();
builder.Services.AddTransient<IChatService, MsSqlChatService>();
builder.Services.AddTransient<IChatMemberService, MsSqlChatMemberService>();
builder.Services.AddTransient<IMessageService, MsSqlMessageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
