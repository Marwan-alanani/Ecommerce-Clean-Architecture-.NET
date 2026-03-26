using Newtonsoft.Json;

namespace ECommerce_Clean_Arch.Infrastructure.Extensions;

public static class JsonConvertExtensions
{
    public static T? Deserialize<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(
            json,
            new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = new JsonPrivateResolver()
            }
        );
    }
}