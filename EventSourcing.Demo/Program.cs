using Amazon.DynamoDBv2;
using EventSourcing.Demo;
using EventSourcing.Demo.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterDataStorageServices(builder.Configuration);
builder.Services.AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>();

//mediator for cqrs
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
