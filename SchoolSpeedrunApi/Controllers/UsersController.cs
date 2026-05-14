using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SchoolSpeedrunApi.Services;
using SchoolSpeedrunApi.Types.Database;
using SchoolSpeedrunApi.Types.RequestBodies;

namespace SchoolSpeedrunApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(AppDbContext db, IPhotoDatastore datastore) : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register([FromBody]  RegisterRequest request)
    {
        DbUser? user = db.Users.FirstOrDefault(u => u.CardGuid == request.CardGuid);
        if (user != null)
            return BadRequest();
        
        if (!Regex.IsMatch(request.Username, @"^[a-zA-ZåäöÅÄÖ0-9]{3,36}$"))
        {
            return BadRequest("Invalid username. Should be \"^[a-zA-ZåäöÅÄÖ0-9]{3,36}$\"");
        }
        
        EntityEntry<DbUser> entry = db.Users.Add(new DbUser
        {
            CardGuid = request.CardGuid,
            Username = request.Username,
            RegistrationDate = DateTimeOffset.UtcNow,
            SchoolProgram = request.SchoolProgram
        });
        user = entry.Entity;
        db.SaveChanges();
        return Ok(user);
    }
    
    [HttpPost("{guid}/setProfilePicture")]
    public async Task<IActionResult> UploadProfilePicture(IFormFile? file, string guid)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");
        
        DbUser? user = db.Users.FirstOrDefault(u => u.CardGuid == guid);
        if (user == null)
            return NotFound("User not found");

        using MemoryStream ms = new();
        await file.CopyToAsync(ms);

        string hash = await datastore.SaveAsync(ms);
        
        user.ProfilePictureHash = hash;
        await db.SaveChangesAsync();
        
        return Ok();
    }
    
    [HttpGet("{id}/profilePicture")]
    public async Task<IActionResult> GetProfilePicture(int id)
    {
        DbUser? user = db.Users.FirstOrDefault(u => u.Id == id);
        if (user?.ProfilePictureHash == null)
            return NotFound();
        
        MemoryStream? ms = await datastore.RetrieveAsync(user.ProfilePictureHash);
        if (ms == null)
            return NotFound();

        return File(ms, "application/octet-stream");
    }

    [HttpGet("{guid}")]
    public Task<IActionResult> GetUserWithGuid(string guid)
    {
        DbUser? user = db.Users.FirstOrDefault(u => u.CardGuid == guid);
        if (user == null)
            return Task.FromResult<IActionResult>(NotFound());

        return Task.FromResult<IActionResult>(Ok(user));
    }
}