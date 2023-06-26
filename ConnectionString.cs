using System.Runtime.CompilerServices;

namespace AzureBlobStorage.StartCopyFromUriAsync.Bug;

public class ConnectionString : System.Attribute
{
    public string Name { get; set; }

    public int Order { get; set; }

    public ConnectionString([CallerMemberName] string name = "")
    {
        this.Name = name;
        this.Order = 9999;
    }
}