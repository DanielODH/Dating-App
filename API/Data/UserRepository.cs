using API.DataEntities;
using API.DTOs;
using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    
    public async Task<IEnumerable<AppUser>> GetAllAsync() 
        => await context.Users
                .Include(u => u.Photos)
                .ToListAsync();

    public async Task<AppUser?> GetByIdAsync(int id)
        => await context.Users
                .Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<AppUser?> GetByUsernameAsync(string username)
        => await context.Users
                .Include(u => u.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);

    public async Task<MemberResponse?> GetMemberAsync(string username)
        => await context.Users
                 .Where(u => u.UserName == username)
                 .ProjectTo<MemberResponse>(mapper.ConfigurationProvider)
                 .SingleOrDefaultAsync();

   public async Task<PagedList<MemberResponse>> GetMembersAsync(UserParams userParams)
    {
        var query = context.Users.ProjectTo<MemberResponse>(mapper.ConfigurationProvider);
        return await PagedList<MemberResponse>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
    }

    public async Task<bool> SaveAllAsync()
         => await context.SaveChangesAsync() > 0;

    public void Update(AppUser user)
        => context.Entry(user).State = EntityState.Modified;
}
