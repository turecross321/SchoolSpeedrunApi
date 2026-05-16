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
        return db.BestRuns().ToList();
    }
}