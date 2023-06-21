using Amazon.DynamoDBv2;
using EventSourcing.Demo.Ioc;
using EventSourcing.Demo.Models;
using EventSourcing.Demo.Models.Events.V1;
using EventSourcing.Demo.Repositories;
using EventSourcing.Demo.Services;
using Trainline.EventSourcing.DynamoDB.Repository;
using Trainline.EventSourcing.EventPublishing;
using Trainline.EventSourcing.Mapping;
using Trainline.EventSourcing.Repository;

namespace EventSourcing.Demo.Extensions;

public static class Services
{
    public static IServiceCollection RegisterDataStorageServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAggregateRepository<Ledger>, AggregateRepository<Ledger>>();
        services.AddScoped<ILedgerAggregateRepository, LedgerAggregateRepository>();
        services.AddScoped<ILedgerProjectionRepository, LedgerProjectionRepository>();
        services.AddScoped<ILedgerProjectionService, LedgerProjectionService>();

        services.AddSingleton<IEventSubscriberResolver, EventSubscriberResolver>();

        services.AddSingleton<IEventMapper, TypeNamespaceEventMapper>(_ => new TypeNamespaceEventMapper(new[] 
        { typeof(LedgerCreated).Assembly }));


        //DynamoDB
        services
            .AddDefaultAWSOptions(configuration.GetAWSOptions())
            .AddAWSService<IAmazonDynamoDB>()
            .Configure<DynamoDBSettings>(s =>
            {
                s.TableName = "event";
                s.EnvironmentName = "test";
            })
            .AddSingleton<IEventRepository, DynamoDBEventRepository>();


        //services
        //       .AddScoped<IEventSubscriber<LedgerCreated, Ledger>,
        //           SnsPublishOrderUpdatedEventSubscriber<LedgerCreated>>();


        services.AddScoped<IEventPublisher, EventPublisher>(
            provider =>
            {
                var eventSubscriberResolver = provider.GetService<IEventSubscriberResolver>();
                return new EventPublisher(eventSubscriberResolver);
            });

        return services;
    }
}
