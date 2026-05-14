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
            .FirstOrDefault(u => u.CardGuid == request.CardGuid);
        
        if (user == null)
            return BadRequest();

        DbLocation? lastLocation = user.Locations.LastOrDefault();
        EntityEntry<DbLocation> currentLocationEntry = db.Locations
            .Add(new DbLocation { Position = request.Position, Date = request.Date, UserCardGuid = user.CardGuid });
        DbLocation currentLocation = currentLocationEntry.Entity;
        await db.SaveChangesAsync();

        DbRun? run = null;
        
        // If the last location has not been part of any runs, and it was in a different position than this new one, we create a run
        if (lastLocation?.StartRuns.Count == 0 && lastLocation.EndRuns.Count == 0 && lastLocation.Position != currentLocation.Position)
        {
            EntityEntry<DbRun> runEntry = db.Runs.Add(new DbRun(lastLocation, currentLocation, user));
            run = runEntry.Entity;
            await db.SaveChangesAsync();
        }
        
        
        return Ok(new SubmitLocationResponse(currentLocation, run));
    }

    /// <summary>
    /// Gets locations posted within the last 10 minutes but only the latest for each user
    /// </summary>
    /// <returns></returns>
    [HttpGet("recent")]
    public IEnumerable<DbLocation> GetRecentLocations()
    {
        DateTime earliest = DateTime.Now.Subtract(TimeSpan.FromMinutes(10));

        return db.Locations
            .Where(l => l.Date >= earliest)
            .GroupBy(l => l.UserCardGuid)
            .Select(g => g.OrderByDescending(l => l.Date).First())
            .ToList();
    }
}