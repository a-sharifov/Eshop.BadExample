using eShop.ServicesProduct.API.Models;
using Nest;

namespace eShop.ServicesProduct.API.Extensions;

public static class ElasticSearchExtension
{
    public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
    {
        var uri = configuration["ELKConfiguration:Uri"];
        var defaultIndex = configuration["ELKConfiguration:Index"];

        var settings = new ConnectionSettings(new Uri(uri))
                           .DefaultIndex(defaultIndex)
                           .DefaultMappingFor<Product>(m => m
                               .PropertyName(p => p.Id, "id")
                               .PropertyName(p => p.Name, "name")
                               .PropertyName(p => p.Description, "description")
                               .PropertyName(p => p.ProductType, "productType")
                               .PropertyName(p => p.ProductBrand, "productBrand")
                               .PropertyName(p => p.ProductSeller, "productSeller")
                           )
                           .DefaultFieldNameInferrer(f => f.ToLowerInvariant())
                           .EnableDebugMode();

        var client = new ElasticClient(settings);
        services.AddSingleton<IElasticClient>(client);

        CreateIndex(client, defaultIndex);
    }

    private static void CreateIndex(IElasticClient client, string indexName)
    {
        var createIndexResponse = client.Indices.Create(indexName, c => c
            .Settings(s => s
                .Analysis(a => a
                    .TokenFilters(tf => tf
                        .NGram("ngram_filter", ng => ng
                            .MinGram(2)
                            .MaxGram(20)
                        )
                        .Stop("stop_filter", sf => sf
                            .StopWords("_english_")
                        )
                    )
                    .Analyzers(an => an
                        .Custom("ngram_analyzer", ca => ca
                            .Tokenizer("standard")
                            .Filters("lowercase", "ngram_filter", "stop_filter")
                        )
                    )
                )
            )
            .Map<Product>(m => m
                .AutoMap()
                .Properties(props => props
                    .Text(t => t
                        .Name(p => p.ProductSeller)
                        .Analyzer("ngram_analyzer")
                    )
                    .Text(t => t
                        .Name(p => p.Name)
                        .Analyzer("ngram_analyzer")
                    )
                    .Text(t => t
                        .Name(p => p.ProductType)
                        .Analyzer("ngram_analyzer")
                        .SearchAnalyzer("standard")
                    )
                    .Text(t => t
                        .Name(p => p.ProductBrand)
                        .Analyzer("ngram_analyzer")
                    )
                    .Text(t => t
                        .Name(p => p.Description)
                        .Analyzer("ngram_analyzer")
                    )
                )
            ));

        if (!createIndexResponse.IsValid)
        {
            throw new Exception($"Failed to create index {indexName}: {createIndexResponse.DebugInformation}");
        }
    }
}