using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSpeedrunApi.Types.Database;

namespace SchoolSpeedrunApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RunsController(AppDbContext db) : ControllerBase
{
    [HttpGet("bestUnique")]
    public IEnumerable<DbRun> GetBestUniqueRuns()
    {
        return db.Runs
            .Include(r => r.User)
            .Include(r => r.StartLocation)
            .Include(r => r.EndLocation)
            .Where(r => !db.Runs.Any(r2 => r2.UserId == r.UserId && r2.Milliseconds < r.Milliseconds))
            .OrderBy(r => r.Milliseconds)
            .ToList();
    }
}