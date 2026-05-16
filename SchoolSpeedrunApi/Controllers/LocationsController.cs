using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SchoolSpeedrunApi.Types.Database;
using SchoolSpeedrunApi.Types.RequestBodies;
using SchoolSpeedrunApi.Types.ResponseBodies;

namespace SchoolSpeedrunApi.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationsController(AppDbContext db) : ControllerBase
{
    [HttpPost("submit")]
    public async Task<IActionResult> SubmitLocation([FromBody] SubmitLocationRequest request)
    {
        DbUser? user = db.Users
            .Include(dbUser => dbUser.Locations)
            .ThenInclude(dbLocation => dbLocation.StartRuns)
            .Include(dbUser => dbUser.Locations)
            .ThenInclude(dbLocation => dbLocation.EndRuns)
            .Include(dbUser => dbUser.Runs)
            .FirstOrDefault(u => u.CardGuid == request.CardGuid);
        
        if (user == null)
            return BadRequest();

        DbLocation? lastLocation = user.Locations.LastOrDefault();
        EntityEntry<DbLocation> currentLocationEntry = db.Locations
            .Add(new DbLocation { Position = request.Position, Date = request.Date, UserId = user.Id });
        DbLocation currentLocation = currentLocationEntry.Entity;
        await db.SaveChangesAsync();
        
        DbRun? previousBestRun = user.Runs.MinBy(r => r.Milliseconds);
        DbRun? newRun = null;
        
        // If the last location has not been part of any runs, and it was in a different position than this new one, we create a run
        if (lastLocation?.StartRuns.Count == 0 && lastLocation.EndRuns.Count == 0 && lastLocation.Position != currentLocation.Position)
        {
            EntityEntry<DbRun> runEntry = db.Runs.Add(new DbRun(lastLocation, currentLocation, user));
            newRun = runEntry.Entity;
            await db.SaveChangesAsync();
        }
        
        
        return Ok(new SubmitLocationResponse(currentLocation, newRun, previousBestRun));
    }

    /// <summary>
    /// Gets locations posted within the last 10 minutes but only the latest for each user
    /// </summary>
    /// <returns></returns>
    [HttpGet("recent")]
    public IEnumerable<DbLocation> GetRecentLocations()
    {
        DateTimeOffset earliest = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromMinutes(10));

        // 1. Get the latest location ID for every active user within the timeframe
        var latestLocationIds = db.Locations
            .Where(l => l.Date >= earliest)
            .GroupBy(l => l.UserId)
            .Select(g => g.OrderByDescending(l => l.Date).Select(l => l.Id).First());

        // 2. Fetch the full objects only for those IDs, 
        // and ONLY if they haven't started/ended a run.
        return db.Locations
            .Include(l => l.User)
            .Where(l => latestLocationIds.Contains(l.Id))
            .Where(l => !l.StartRuns.Any() && !l.EndRuns.Any())
            .ToList();
    }
}