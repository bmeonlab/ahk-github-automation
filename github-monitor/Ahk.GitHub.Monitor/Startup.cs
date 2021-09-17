﻿using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Ahk.GitHub.Monitor.Startup))]

namespace Ahk.GitHub.Monitor
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddMemoryCache(setup =>
            {
                setup.ExpirationScanFrequency = TimeSpan.FromMinutes(4);
            });
            builder.Services.AddSingleton<Services.IGitHubClientFactory, Services.GitHubClientFactory>();

            var eventHandlers = registerEventHandlers(builder.Services);
            builder.Services.AddSingleton<Services.IEventDispatchService, Services.EventDispatchService>(sp => new Services.EventDispatchService(sp, eventHandlers));

            var configuration = new ConfigurationBuilder().AddEnvironmentVariables("AHK_").Build();
            builder.Services.Configure<GitHubMonitorConfig>(configuration);

            addGradeStoreIntegration(builder, configuration);
        }

        private static Services.EventDispatchConfigBuilder registerEventHandlers(IServiceCollection services)
        {
            return new Services.EventDispatchConfigBuilder(services)
                .Add<EventHandlers.BranchProtectionRuleHandler>(EventHandlers.BranchProtectionRuleHandler.GitHubWebhookEventName)
                .Add<EventHandlers.IssueCommentEditDeleteHandler>(EventHandlers.IssueCommentEditDeleteHandler.GitHubWebhookEventName)
                .Add<EventHandlers.PullRequestOpenDuplicateHandler>(EventHandlers.PullRequestOpenDuplicateHandler.GitHubWebhookEventName)
                .Add<EventHandlers.PullRequestReviewToAssigneeHandler>(EventHandlers.PullRequestReviewToAssigneeHandler.GitHubWebhookEventName)
                .Add<EventHandlers.GradeCommandHandler>(EventHandlers.GradeCommandHandler.GitHubWebhookEventName);
        }

        private static void addGradeStoreIntegration(IFunctionsHostBuilder builder, IConfigurationRoot configuration)
        {
            var config = configuration.Get<GitHubMonitorConfig>();

            if (!string.IsNullOrEmpty(config?.EventsQueueConnectionString))
            {
                builder.Services.AddSingleton<Services.IGradeStore, Services.GradeStoreAzureQueue>();
                builder.Services.AddAzureClients(az =>
                {
                    az.ConfigureDefaults(opts => opts.Diagnostics.IsLoggingEnabled = false);
                    az.AddQueueServiceClient(connectionString: config.EventsQueueConnectionString)
                        .WithName(Services.GradeStoreAzureQueue.QueueClientName)
                        .ConfigureOptions(options =>
                        {
                            options.MessageEncoding = Azure.Storage.Queues.QueueMessageEncoding.Base64;
                            options.Retry.Mode = Azure.Core.RetryMode.Exponential;
                            options.Retry.MaxRetries = 5;
                            options.Retry.MaxDelay = TimeSpan.FromSeconds(100);
                        });
                });
            }
            else
            {
                builder.Services.AddSingleton<Services.IGradeStore, Services.GradeStoreNoop>();
            }
        }
    }
}