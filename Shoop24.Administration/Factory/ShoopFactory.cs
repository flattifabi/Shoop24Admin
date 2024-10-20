
using Shoop24.Administration.Services.Resolver;

namespace Shoop24.Administration.Services;

public static class ShoopFactory
{
    private static UserResolver _userResolver;

    public static UserResolver GetUserResolver()
    {
        if(_userResolver == null)
        {
            _userResolver = new UserResolver();
        }
        return _userResolver;
    }
}