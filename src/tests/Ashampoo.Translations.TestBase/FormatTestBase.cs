using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ashampoo.Translations.Formats.Abstractions;

namespace Ashampoo.Translations.TestBase
{
    public abstract class FormatTestBase<T> where T : IFormat, new()
    {
        /// <summary>
        /// Creates a name of the format implementation.
        /// </summary>
        private string formatName = Regex.Replace(typeof(T).Name, @"Format$", "");

        /// <summary>
        /// Creates a instance of T.
        /// </summary>
        /// <returns></returns>
        protected T CreateFormat()
        {
            return new T();
        }

        /// <summary>
        /// Creates an absolute file name.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected string GetAbsoluteFileName(string fileName)
        {
            var append = Path.Combine("_TestFiles_", fileName);
            return Path.Combine(Directory.GetCurrentDirectory(), append);
        }

        /// <summary>
        /// Creates a new instance of T and calls T.Read(fileName)
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected T CreateAndReadFromFile(string fileName, FormatReadOptions? options = null)
        {
            using var stream = CreateFileInStream(fileName);

            var format = CreateFormat();
            format.Read(stream, options);

            stream.Close();
            stream.Dispose();

            return format;
        }

        protected async Task<T> CreateAndReadFromFileAsync(string fileName, FormatReadOptions? options = null)
        {
            await using var stream = CreateFileInStream(fileName);

            var format = CreateFormat();
            await format.ReadAsync(stream, options);

            stream.Close();
            return format;
        }

        protected string CreateTempFileName()
        {
            return Path.GetTempFileName();
        }

        protected Stream CreateFileInStream(string fileName)
        {
            var absoluteFileName = GetAbsoluteFileName(fileName);
            return new FileStream(absoluteFileName, FileMode.Open, FileAccess.Read);
        }

        protected Stream CreateFileOutStream(string fileName)
        {
            var absoluteFileName = GetAbsoluteFileName(fileName);
            return new FileStream(absoluteFileName, FileMode.Create, FileAccess.ReadWrite);
        }

        protected void WriteNormalizedFile(IFormat format, string fileName)
        {
            var absoluteFileName = GetAbsoluteFileName($"normalized_{fileName}");
            using var outStream = new FileStream(absoluteFileName, FileMode.Create, FileAccess.ReadWrite);
            format.Write(outStream);
            outStream.Close();
            outStream.Dispose();
        }
    }
}