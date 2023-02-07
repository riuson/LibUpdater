using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LibUpdater.Utils
{
    public class Hasher
    {
        public string HashStream(Stream stream)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(stream);
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString().ToLower();
            }
        }

        public Task<string> HashStreamAsync(Stream stream)
        {
            return Task<string>.Run(() => this.HashStream(stream));
        }
    }
}