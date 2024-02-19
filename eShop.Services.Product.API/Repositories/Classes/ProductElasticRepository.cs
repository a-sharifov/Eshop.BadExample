using Elasticsearch.Net;
using eShop.ServicesProduct.API.Builders.Interfaces;
using eShop.ServicesProduct.API.Models;
using eShop.ServicesProduct.API.Protos.Product;
using eShop.ServicesProduct.API.Repositories.Interfaces;
using Nest;
using System.Text.Json;

namespace eShop.ServicesProduct.API.Repositories.Classes;

public class ProductElasticRepository : IProductElasticRepository
{
    private readonly IElasticClient _elasticClient;
    private readonly IProductElasticQueryBuilder _queryBuilder;
    
    public ProductElasticRepository(IElasticClient elasticClient, IProductElasticQueryBuilder queryBuilder) =>
       (_elasticClient, _queryBuilder) = (elasticClient, queryBuilder);

    public async Task<List<int>> FindProductIdListAsync(SearchFilter filter)
    {
        var searchResponse = await _elasticClient.SearchAsync<ProductIndex>(
           s => s.Query(x => _queryBuilder.Build(x, filter))
           .Skip(filter.Skip)
           .Take(filter.Take == 0 ? null : filter.Take));

        return searchResponse.Documents.Select(pI => pI.Id).ToList();
    }

    public async Task AddProductIndexAsync(ProductIndex productIndex)
    {
        await _elasticClient.IndexDocumentAsync(productIndex);
        await _elasticClient.Indices.RefreshAsync();
    }

    public async Task UpdateProductIndexAsync(ProductIndex productIndex)
    {
        await _elasticClient.UpdateByQueryAsync<ProductIndex>(u => u
        .Query(q => q.Term(t => t.Field(pI => pI.Id).Value(productIndex.Id)))
        .Script(s => s.Source("ctx._source = value")
                      .Params(p => p.Add("value", productIndex))
               ));

        await _elasticClient.Indices.RefreshAsync();
    }

    public async Task DeleteProductIndexAsync(int id)
    {
        await _elasticClient.DeleteByQueryAsync<ProductIndex>(s =>
            s.Query(x => _queryBuilder.BuildById(x, id)));
        await _elasticClient.Indices.RefreshAsync();
    }

    public async Task<int> GetLengthAsync(SearchFilter filter)
    {
        var searchResponse = await _elasticClient.SearchAsync<ProductIndex>(
           s => s.Query(x => _queryBuilder.Build(x, filter)));
        return (int)searchResponse.Total;
    }
}