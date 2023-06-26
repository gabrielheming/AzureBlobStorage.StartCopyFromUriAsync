using System.Reflection;

namespace AzureBlobStorage.StartCopyFromUriAsync.Bug;

public class AzureBlobsStorageConfiguration
{
    public string ContainerName { get; set; } = String.Empty;

    [ConnectionString]
    public string? DefaultEndpointsProtocol { get; set; }
    [ConnectionString]
    public string? AccountName { get; set; }
    [ConnectionString]
    public string? AccountKey { get; set; }
    [ConnectionString]
    public string? EndpointSuffix { get; set; }
    public string? DirectoryRoot { get; set; }
    public string ConnectionString {
        get {
            string ret = String.Empty;

            this.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(_ => _.CustomAttributes.ToList().Any(attr => attr.AttributeType == typeof(ConnectionString)))
                .Where(_ => _.PropertyType == typeof(string))
                .Where(_ => {
                    object? value = _.GetValue(this);
                    return value != null && !String.IsNullOrWhiteSpace(value.ToString());
                })
                .Select(_ => {
                    ConnectionString attribute = (ConnectionString)Attribute.GetCustomAttribute(_, typeof(ConnectionString), true)!;

                    return new
                    {
                        Name = attribute.Name,
                        Value = _.GetValue(this),
                        Attribute = attribute
                    };
                })
                .OrderBy(x => x.Attribute.Order)
                .ToList()
                .ForEach(_ => {
                    ret += String.Format("{0}={1};", _.Name, _.Value);
                });

            return ret;
        }
    }
}