using Microsoft.EntityFrameworkCore;

namespace SchoolSpeedrunApi.Types.Database;

public partial class AppDbContext
{
    public IOrderedQueryable<DbRun> BestRuns(DbUser? excludeUser = null)
    {
        // Start with the base query
        var query = this.Runs.AsQueryable();

        // 1. Filter the main list if a user is provided
        if (excludeUser != null)
        {
            query = query.Where(r => r.UserId != excludeUser.Id);
        }

        return query
            .Include(r => r.User)
            .Include(r => r.StartLocation)
            .Include(r => r.EndLocation)
            // 2. The subquery must also respect the exclusion to ensure 
            // we are only comparing against the visible pool of runs.
            .Where(r => !this.Runs
                .Where(r2 => excludeUser == null || r2.UserId != excludeUser.Id)
                .Any(r2 => r2.UserId == r.UserId && r2.Milliseconds < r.Milliseconds))
            .OrderBy(r => r.Milliseconds);
    }
}