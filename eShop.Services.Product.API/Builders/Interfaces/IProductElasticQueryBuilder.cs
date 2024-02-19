using eShop.ServicesProduct.API.Models;
using eShop.ServicesProduct.API.Protos.Product;
using Nest;

namespace eShop.ServicesProduct.API.Builders.Interfaces;

public interface IProductElasticQueryBuilder
{
    public QueryContainerDescriptor<ProductIndex> Build(QueryContainerDescriptor<ProductIndex> queryContainerDescriptor, SearchFilter filter);
    public QueryContainerDescriptor<ProductIndex> BuildById(QueryContainerDescriptor<ProductIndex> queryContainerDescriptor, int id);
}