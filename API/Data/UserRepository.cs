using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public class UserRepository(DataContext context) : IUserRepository
{
    
    public async Task<IEnumerable<AppUser>> GetAllAsync() 
        => await context.Users.ToListAsync();

    public async Task<AppUser?> GetByIdAsync(int id)
        => await context.Users.FindAsync(id);

    public async Task<AppUser?> GetByUsernameAsync(string username)
        => await context.Users.SingleOrDefaultAsync(x => x.UserName == username);

    public async Task<bool> SaveAllAsync()
         => await context.SaveChangesAsync() > 0;

    public void Update(AppUser user)
        => context.Entry(user).State = EntityState.Modified;
}
