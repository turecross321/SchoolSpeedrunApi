using System.Runtime.InteropServices.JavaScript;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SchoolSpeedrunApi.Services;
using SchoolSpeedrunApi.Types.Database;
using SchoolSpeedrunApi.Types.RequestBodies;
using SchoolSpeedrunApi.Types.ResponseBodies;

namespace SchoolSpeedrunApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(AppDbContext db, IPhotoDatastore datastore) : ControllerBase
{
    [HttpPost("requestRegistration")]
    public IActionResult RequestRegistration([FromBody] RequestRegistrationRequest request)
    {
        DbUser? user = db.Users.FirstOrDefault(u => u.CardGuid == request.CardGuid);
        if (user != null)
            return BadRequest();
        
        EntityEntry<DbRegistration> entry = db.Registrations.Add(new DbRegistration(request.CardGuid));
        db.SaveChanges();
        
        return Ok(entry.Entity);
    }

    [HttpGet("registrations/{guid}/scanned")]
    public IActionResult IsRegistrationScanned([FromRoute] Guid guid)
    {
        DbRegistration? registration = db.Registrations.FirstOrDefault(r => r.Id == guid);
        if (registration == null)
            return NotFound();

        return Ok(new IsRegistrationScannedResponse(registration.Scanned));
    }

    [HttpGet("registrations/{guid}")]
    public IActionResult GetRegistration([FromRoute] Guid guid)
    {
        DbRegistration? registration = db.Registrations.FirstOrDefault(r => r.Id == guid);
        if (registration == null || DateTimeOffset.UtcNow > registration.ExpiryDate)
            return NotFound();
        
        if (registration.Scanned)
            return Forbid("Already scanned");

        registration.Scanned = true;
        db.SaveChanges();

        return Ok(registration);
    }
    
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        DbRegistration? registration = db.Registrations.FirstOrDefault(r => r.Id == request.RegistrationGuid);
        if (registration == null)
            return NotFound();

        if (DateTimeOffset.UtcNow > registration.ExpiryDate)
            return NotFound();
        
        DbUser? user = db.Users.FirstOrDefault(u => u.CardGuid == registration.CardGuid);
        if (user != null)
            return NotFound();
        
        if (!Regex.IsMatch(request.Username, @"^[a-zA-ZåäöÅÄÖ0-9]{3,36}$"))
        {
            return BadRequest("Invalid username. Should be \"^[a-zA-ZåäöÅÄÖ0-9]{3,36}$\"");
        }

        DbUser? userWithName = db.Users.FirstOrDefault(u => u.Username == request.Username);
        if (userWithName != null) 
            return BadRequest("Username taken");
        
        EntityEntry<DbUser> entry = db.Users.Add(new DbUser
        {
            CardGuid = registration.CardGuid,
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

    [HttpGet("cardGuid/{guid}")]
    public Task<IActionResult> GetUserWithCardGuid(string guid)
    {
        DbUser? user = db.Users.FirstOrDefault(u => u.CardGuid == guid);
        if (user == null)
            return Task.FromResult<IActionResult>(NotFound());

        return Task.FromResult<IActionResult>(Ok(user));
    }
    
    [HttpGet("id/{id}")]
    public Task<IActionResult> GetUserWithId(int id)
    {
        DbUser? user = db.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
            return Task.FromResult<IActionResult>(NotFound());

        return Task.FromResult<IActionResult>(Ok(user));
    }
}