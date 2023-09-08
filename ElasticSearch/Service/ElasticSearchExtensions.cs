using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ElasticSearch.Model;
using Nest;

namespace ElasticSearch.Service
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var elkConfiguration = configuration.GetSection("ELKConfiguration").Get<ELKConfiguration>();
            if (elkConfiguration == null)
            {
                throw new ArgumentException("ELKConfiguration section is missing or invalid.");
            }

            var settings = new ConnectionSettings(new Uri(elkConfiguration.Uri))
                .PrettyJson()
                .DefaultIndex(elkConfiguration.Index);

            AddDefaultMappings(settings);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            CreateIndex(client, elkConfiguration.Index);
        }

        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings.DefaultMappingFor<Product>(m => m.Ignore(p => p.Price));
        }

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName,
                index => index.Map<Product>(x => x.AutoMap())
            );
        }
    }
}
