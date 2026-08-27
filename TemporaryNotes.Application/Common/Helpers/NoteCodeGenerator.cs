using System.Security.Cryptography;

namespace TemporaryNotes.Application.Common.Helpers
{
    public static class NoteCodeGenerator
    {
        public static string Generate() =>
            Convert.ToHexString(RandomNumberGenerator.GetBytes(6))
                .ToLowerInvariant();
    }
}
