using API.DataEntities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public class UserRepository(DataContext context) : IUserRepository
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

    public async Task<bool> SaveAllAsync()
         => await context.SaveChangesAsync() > 0;

    public void Update(AppUser user)
        => context.Entry(user).State = EntityState.Modified;
}
