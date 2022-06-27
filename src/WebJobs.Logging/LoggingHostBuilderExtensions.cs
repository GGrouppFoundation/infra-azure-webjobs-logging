using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Hosting;

public static class LoggingHostBuilderExtensions
{
    public static IHostBuilder ConfigureStandardLogging(this IHostBuilder hostBuilder)
    {
        _ = hostBuilder ?? throw new ArgumentNullException(nameof(hostBuilder));

        return hostBuilder.ConfigureLogging(ConfigureLogging);
    }

    private static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder builder)
        =>
        builder.AddConsole().AddApplicationInsights(context.Configuration);

    private static ILoggingBuilder AddApplicationInsights(this ILoggingBuilder builder, IConfiguration configuration)
    {
        var instrumentationKey = configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
        if (string.IsNullOrEmpty(instrumentationKey))
        {
            return builder;
        }

        return builder.AddApplicationInsightsWebJobs(o => o.InstrumentationKey = instrumentationKey);
    }
}