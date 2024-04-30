using hackernews.Core.Service;

namespace hackernews.Core
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IStoryService, StoryService>();
            services.AddSingleton<IHackerNewsClient, HackerNewsHttpClient>();

            return services;
        }

    }
}
