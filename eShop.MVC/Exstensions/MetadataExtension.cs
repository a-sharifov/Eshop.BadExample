using Grpc.Core;

namespace eShop.MVC.Exstensions;

public class MetadataExtension
{
    public static Metadata CreateAuthorizationBearerMetadata(string token)
    {
        return new Metadata
        {
            { "Authorization", "Bearer " + token }
        };
    }

}
