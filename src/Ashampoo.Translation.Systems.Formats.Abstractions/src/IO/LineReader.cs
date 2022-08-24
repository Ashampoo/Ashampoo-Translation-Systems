namespace Ashampoo.Translation.Systems.Formats.Abstractions.IO;

/// <summary>
/// Utility class for reading lines from a file.
/// Allows for peeking at the next line and skipping empty lines.
/// </summary>
public class LineReader : IDisposable
{
    private readonly TextReader textReader;
    private string? lastLine;

    /// <summary>
    /// The number of the line that is currently being read.
    /// </summary>
    public int LineNumber { get; private set; }

    /// <summary>
    /// Creates a new instance of the <see cref="LineReader"/> class.
    /// </summary>
    /// <param name="reader">
    /// The <see cref="TextReader"/> to read from.
    /// </param>
    public LineReader(TextReader reader)
    {
        // This should create a Thread-Safe StreamReader.
        textReader = TextReader.Synchronized(reader);
    }

    /// <summary>
    /// Reads a line of characters asynchronously and returns the data as a string.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous read operation. The value of the TResult parameter contains the next line from the text reader,
    /// or is null if all of the characters have been read.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The number of characters in the next line is larger than <see cref="Int32.MaxValue"/>.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The text reader has been disposed.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The reader is currently in use by a previous read operation
    /// </exception>
    public async Task<string?> ReadLineAsync()
    {
        if (lastLine is null)
        {
            LineNumber++;
            return await textReader.ReadLineAsync();
        }

        var line = lastLine;
        lastLine = null;
        return line;
    }
    
    /// <summary>
    /// Peeks at the next line in the text reader.
    /// Only one line can be peeked at at a time.
    /// Multiple calls to <see cref="PeekLineAsync"/> will return the same line.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous peek operation.
    /// The value of the TResult parameter contains the next line from the text reader,
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The number of characters in the next line is larger than <see cref="Int32.MaxValue"/>.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The text reader has been disposed.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The reader is currently in use by a previous read operation
    /// </exception>
    public async Task<string?> PeekLineAsync()
    {
        if (lastLine is not null) return lastLine;

        LineNumber++;
        lastLine = await textReader.ReadLineAsync();

        return lastLine;
    }
    
    /// <summary>
    /// Skips all empty lines in the text reader, until the next non-empty line is found.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing an asynchronous operation.</returns>
    public async Task SkipEmptyLinesAsync()
    {
        while (await PeekLineAsync() == string.Empty) await ReadLineAsync();
    }
    
    /// <summary>
    /// Determine whether the text reader has any more lines.
    /// </summary>
    /// <returns>
    /// A boolean value indicating whether the text reader has any more lines.
    /// </returns>
    public async Task<bool> HasMoreLinesAsync()
    {
        /*
        EndOfStream won't work with all async. stream implementations.
        Therefore we check the peeked line for null.
        */
        return await PeekLineAsync() is not null;
    }
    
    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="LineReader"/> and optionally releases the managed resources.
    /// </summary>
    public void Dispose()
    {
        textReader.Dispose();
        GC.SuppressFinalize(this);
    }
    
    /// <summary>
    /// Destructor.
    /// </summary>
    ~LineReader()
    {
        Dispose();
    }
}