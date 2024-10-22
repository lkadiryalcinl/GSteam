using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Global.Services
{
    public static class MassTransitExtension
    {
        public static IServiceCollection AddMassTransitWithRabbitMq<T>(this IServiceCollection services, IConfiguration configuration, string endpointPrefix, Dictionary<string, Type> consumers)
            where T : class, IConsumer
        {
            // MassTransit ayarlarını RabbitMQ config'den çek
            var massTransitConfs = new MassTransitConfs();
            configuration.GetSection("RabbitMQ").Bind(massTransitConfs);

            services.AddMassTransit(opt =>
            {
                // Tüketicileri (consumers) eklemek için
                foreach (var consumer in consumers)
                {
                    opt.AddConsumer(consumer.Value); // Her consumer'ı ekle
                }

                opt.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(endpointPrefix, false));

                // RabbitMQ ile bağlantı ve receive endpoint yapılandırması
                opt.UsingRabbitMq((context, config) =>
                {
                    config.Host(massTransitConfs.Host, "/", host =>
                    {
                        host.Username(massTransitConfs.Username);
                        host.Password(massTransitConfs.Password);
                    });

                    // Tüketici için receive endpoint yapılandırmaları
                    foreach (var consumer in consumers)
                    {
                        config.ReceiveEndpoint($"{endpointPrefix}-{consumer.Key}", e =>
                        {
                            e.UseMessageRetry(r => r.Interval(5, 5));
                            e.ConfigureConsumer(context, consumer.Value); // Consumer'ı yapılandır
                        });
                    }

                    config.ConfigureEndpoints(context); // Tüm consumer'lar merkezi olarak yapılandırılır
                });
            });

            return services;
        }
    }
}

