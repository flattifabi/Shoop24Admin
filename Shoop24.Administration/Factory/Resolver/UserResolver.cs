using System.Runtime.CompilerServices;
using RestSharp;
using Shoop24.Administration.Services.Interfaces;

namespace Shoop24.Administration.Services.Resolver;

public class UserResolver : ResolverBase
{

    public UserResolver()
    {
        GetClient();
    }

    public void Methode()
    {
        Console.WriteLine("asd");
    }
    
}