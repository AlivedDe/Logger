using System;
using System.IO;
using System.Threading.Tasks;

namespace Logger.IO
{
    public class FileSystemAdapter : IFileSystem
    {
        /// <summary>
        /// Creates a System.IO.StreamWriter that appends UTF-8 encoded text to an existing file, or to a new file if the specified file does not exist.
        /// </summary>
        /// <param name="fileName">File to create</param>
        /// <param name="text">Text to append</param>
        /// <exception cref="ArgumentException">path is a zero-length string, contains only white space, or contains one or more 
        /// invalid characters as defined by System.IO.Path.InvalidPathChars.</exception>
        /// <exception cref="ArgumentNullException">path is null or contents is empty.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. 
        /// For example, on Windows-based platforms, paths must be less than 248 characters, 
        /// and file names must be less than 260 characters.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="UnauthorizedAccessException">path specified a file that is read-only. -or- This operation is not supported 
        /// on the current platform. -or- path specified a directory. -or- The caller does 
        /// not have the required permission.</exception>
        /// <exception cref="NotSupportedException">path is in an invalid format.</exception>
        public async Task AppendFileAsync(string fileName, string text)
        {
            using (StreamWriter fileStream = File.AppendText(fileName))
            {
                await fileStream.WriteLineAsync(text).ConfigureAwait(false);
            };
        }
    }
}
