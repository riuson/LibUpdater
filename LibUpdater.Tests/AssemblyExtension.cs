using System.Reflection;

namespace LibUpdater.Tests
{
    /// <summary>
    /// Helper methods for working with resources.
    /// </summary>
    public static class AssemblyExtension
    {
        /// <summary>
        /// Gets stream from embedded resource of <paramref name="assembly" /> with path like <paramref name="resourcePath" />.
        /// </summary>
        /// <param name="assembly">Assembly to search resource in.</param>
        /// <param name="resourcePath">Partial path to the resource.</param>
        /// <returns>Stream from resource.</returns>
        /// <exception cref="ArgumentException">Resource with specified path was not found.</exception>
        /// <exception cref="ArgumentException">Resource path is null, empty or whitespace.</exception>
        public static Stream GetEmbeddedResource(
            this Assembly assembly,
            string resourcePath)
        {
            if (string.IsNullOrWhiteSpace(resourcePath))
            {
                throw new ArgumentException("Resource path is null, empty or whitespace.");
            }

            var names = assembly.GetManifestResourceNames();
            var name = names.First(x => x.ToLower().Contains(resourcePath.ToLower()));
            var result = assembly.GetManifestResourceStream(name);

            return result ?? throw new ArgumentException($"Resource {resourcePath} was not found.");
        }

        /// <summary>
        /// Gets embedded resource names by filter.
        /// </summary>
        /// <param name="assembly">Assembly to search resource in.</param>
        /// <param name="acceptName">Check that found resource name is accepted.</param>
        /// <returns>Collection of names.</returns>
        public static IEnumerable<string> GetEmbeddedResourceNames(
            this Assembly assembly,
            Func<string, bool> acceptName)
        {
            var names = assembly.GetManifestResourceNames();
            return names.Where(acceptName);
        }

        /// <summary>
        /// Gets embedded resource names by filter from used assemblies.
        /// </summary>
        /// <param name="acceptName">Check that found resource name is accepted.</param>
        /// <returns>Collection of names.</returns>
        public static IEnumerable<string> GetEmbeddedResourceNames(Func<string, bool> acceptName) =>
            Assembly.GetExecutingAssembly().GetManifestResourceNames();

        /// <summary>
        /// Find resource, read it and return as string.
        /// </summary>
        /// <param name="resourcePath">Path to resource.</param>
        /// <returns>Content of resource as string.</returns>
        public static string ReadResource(string resourcePath)
        {
            using var scriptStream = GetEmbeddedResource(resourcePath);
            using var streamReader = new StreamReader(scriptStream);
            return streamReader.ReadToEnd();
        }

        /// <summary>
        /// Asynchronously find resource, read it and return as string.
        /// </summary>
        /// <param name="resourcePath">Path to resource.</param>
        /// <returns>Content of resource as string.</returns>
        public static async Task<string> ReadResourceAsync(string resourcePath)
        {
            await using var resourceStream = GetEmbeddedResource(resourcePath);
            using var streamReader = new StreamReader(resourceStream);
            return await streamReader.ReadToEndAsync();
        }

        /// <summary>
        /// Gets stream from embedded resource from used assemblies with path like <paramref name="resourcePath" />.
        /// </summary>
        /// <param name="resourcePath">Partial path to the resource.</param>
        /// <returns>Stream from resource.</returns>
        /// <exception cref="ArgumentException">Resource with specified path was not found.</exception>
        /// <exception cref="ArgumentException">Resource path is null, empty or whitespace.</exception>
        public static Stream GetEmbeddedResource(string resourcePath)
        {
            return Assembly.GetExecutingAssembly().GetEmbeddedResource(resourcePath);
        }
    }
}