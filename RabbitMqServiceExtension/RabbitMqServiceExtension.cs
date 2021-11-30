using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMqServiceExtension.AsyncMessageService;

namespace RabbitMqServiceExtension;

public static class RabbitMqServiceExtension
{
    public static IServiceCollection AddRabbitMqService(this IServiceCollection services,
        Action<RabbitMqSettings> settingOptions)
    {
        // Set values for Rabbit Mq settings.
        services.Configure<RabbitMqSettings>(settings =>
        {
            //Default settings
            settings.Channel = "common_exchange";
            settings.Type = "topic";
        }).Configure(settingOptions);

        services.AddSingleton<IRabbitMqService, RabbitMqService>(provider =>
        {
            try
            {
                var logger = provider.GetRequiredService<ILogger<RabbitMqService>>();
                var options = provider.GetRequiredService<IOptions<RabbitMqSettings>>();
                return new RabbitMqService(logger, options);
            }
            catch (Exception e)
            {
                var logger = provider.GetRequiredService<ILogger<RabbitMqNotWorkingService>>();
                return new RabbitMqNotWorkingService(logger);
            }
        });
        return services;
    }
}