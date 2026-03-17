using System.Security.Cryptography;

namespace SchoolSpeedrunApi.Services;

public class PhotoDatastore : IPhotoDatastore
{
    private readonly string _storagePath;

    public PhotoDatastore(IConfiguration configuration)
    {
        _storagePath = configuration.GetValue<string>("PhotoStoragePath") ?? "Photos";
        if (!Directory.Exists(_storagePath))
            Directory.CreateDirectory(_storagePath);
    }

    public async Task<string> SaveAsync(MemoryStream photoStream)
    {
        if (photoStream == null || photoStream.Length == 0)
            throw new ArgumentException("Photo stream cannot be null or empty.");

        // Compute SHA256 hash
        string hash;
        using (SHA256 sha = SHA256.Create())
        {
            photoStream.Position = 0;
            byte[] hashBytes = await sha.ComputeHashAsync(photoStream);
            hash = Convert.ToHexStringLower(hashBytes);
        }

        string filePath = Path.Combine(_storagePath, hash);
        if (File.Exists(filePath)) return hash;
        photoStream.Position = 0;
        await using FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        await photoStream.CopyToAsync(fs);

        return hash;
    }

    public async Task<MemoryStream?> RetrieveAsync(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("Hash cannot be null or empty.");

        string filePath = Path.Combine(_storagePath, hash);
        if (!File.Exists(filePath))
            return null;

        MemoryStream ms = new MemoryStream();
        await using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        await fs.CopyToAsync(ms);
        ms.Position = 0;
        return ms;
    }
}