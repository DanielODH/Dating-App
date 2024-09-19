using API.Entities;

namespace API.Services;
    public interface ITokenServices
    {
        string CreateToken(AppUser user);
    }