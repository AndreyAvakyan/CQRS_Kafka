using Confluent.Kafka;
using CQRS_Core.Domain;
using CQRS_Core.Handlers;
using CQRS_Core.Infrastructure;
using CQRS_Core.Producers;
using Post_Cmd.Api.Commands;
using Post_Cmd.Domain.Aggregates;
using Post_Cmd.Infrastructure.Config;
using Post_Cmd.Infrastructure.Dispatchers;
using Post_Cmd.Infrastructure.Handlers;
using Post_Cmd.Infrastructure.Producers;
using Post_Cmd.Infrastructure.Repositories;
using Post_Cmd.Infrastructure.Stores;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventProducer, EventProducer>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();

//register command handler methods
var commandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
var dispathcer = new CommandDispatcher();
dispathcer.RegisterHandler<NewPostCommand>(commandHandler.HandleAsync);
dispathcer.RegisterHandler<EditMessageCommand>(commandHandler.HandleAsync);
dispathcer.RegisterHandler<LikePostCommand>(commandHandler.HandleAsync);
dispathcer.RegisterHandler<AddCommentCommand>(commandHandler.HandleAsync);
dispathcer.RegisterHandler<EditCommentCommand>(commandHandler.HandleAsync);
dispathcer.RegisterHandler<RemoveCommentCommand>(commandHandler.HandleAsync);
dispathcer.RegisterHandler<DeletePostCommand>(commandHandler.HandleAsync);
builder.Services.AddSingleton<ICommandDispatcher>(_ => dispathcer);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()){
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();