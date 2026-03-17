namespace SchoolSpeedrunApi.Services;

public interface IPhotoDatastore
{
    /// <summary>
    /// Saves a photo from a MemoryStream and returns its SHA256 hash.
    /// </summary>
    Task<string> SaveAsync(MemoryStream photoStream);

    /// <summary>
    /// Retrieves a photo as a MemoryStream by hash.
    /// </summary>
    Task<MemoryStream?> RetrieveAsync(string hash);
}