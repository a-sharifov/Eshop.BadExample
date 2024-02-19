using eShop.ServicesProduct.API.Builders.Interfaces;
using eShop.ServicesProduct.API.Models;
using Nest;

namespace eShop.ServicesProduct.API.Builders.Classes;

public class ProductElasticQueryBuilder : IProductElasticQueryBuilder
{
    public QueryContainerDescriptor<ProductIndex> Build(QueryContainerDescriptor<ProductIndex> queryContainerDescriptor, SearchFilter filter)
    {
        queryContainerDescriptor.Bool(bQ =>
        {
            bQ.Must(qC =>
            {
                AddProductSearch(qC, filter);
                return qC;
            });

            bQ.Filter(qC =>
            {
                AddPrice(qC, filter);
                AddProductSeller(qC, filter);
                AddProductType(qC, filter);
                AddProductBrand(qC, filter);
                return qC;
            });

            return bQ;
        });

        return queryContainerDescriptor;
    }

    public QueryContainerDescriptor<ProductIndex> BuildById(QueryContainerDescriptor<ProductIndex> queryContainerDescriptor, int id)
    {
        queryContainerDescriptor.Term(t => t.Field(pI => pI.Id).Value(id));
        return queryContainerDescriptor;
    }

    private void AddPrice(QueryContainerDescriptor<ProductIndex> query, SearchFilter filter)
    {
        if (filter.MaxPrice >= filter.MinPrice && filter.MaxPrice != 0)
        {
            query.Range(r => r.Field(pI => pI.Price)
                              .GreaterThanOrEquals(filter.MinPrice)
                              .LessThanOrEquals(filter.MaxPrice));
        }
    }

    private void AddProductSearch(QueryContainerDescriptor<ProductIndex> query, SearchFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.ProductName))
        {
            query.MultiMatch(mM => mM.Fields(
                f => f.Field(pI => pI.Name, boost: 2)
                      .Field(pI => pI.ProductBrand)
                      .Field(pI => pI.ProductSeller)
                      .Field(pI => pI.ProductType))
            .Query(filter.ProductName)
            .Fuzziness(Fuzziness.Auto));
        }
    }

    private void AddProductSeller(QueryContainerDescriptor<ProductIndex> query, SearchFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.ProductSeller))
        {
            query.Term(tQ => tQ.Field(pI => pI.ProductSeller).Value(filter.ProductSeller));
        }
    }

    private void AddProductType(QueryContainerDescriptor<ProductIndex> query, SearchFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.ProductType))
        {
            query.Term(tQ => tQ.Field(pI => pI.ProductType).Value(filter.ProductType));
        }
    }

    private void AddProductBrand(QueryContainerDescriptor<ProductIndex> query, SearchFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.ProductBrand))
        {
            query.Term(tQ => tQ.Field(pI => pI.ProductBrand).Value(filter.ProductBrand));
        }
    }
}