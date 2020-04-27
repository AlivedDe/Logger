using System.Threading.Tasks;

namespace Logger
{
    /// <summary>
    /// Access to computer file system
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Creates a System.IO.StreamWriter that appends UTF-8 encoded text to an existing file, or to a new file if the specified file does not exist.
        /// </summary>
        /// <param name="fileName">File to create</param>
        /// <param name="text">Text to append</param>
        Task AppendFileAsync(string fileName, string text);
    }
}