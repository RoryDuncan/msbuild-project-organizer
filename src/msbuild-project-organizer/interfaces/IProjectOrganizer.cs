namespace MSBuildProjectOrganizer
{
    /// <summary>
    /// An interface for organizing Project files (.csproj)
    /// </summary>
    public interface IProjectOrganizer
    {
        /// <summary>
        /// Reads, organizes, and saves a csproj file
        /// </summary>
        /// <param name="input">The file to read and organize</param>
        /// <param name="output">A file where the results should be saved — defaults to <see param="input" /></param>
        void Run(string input, string output);
    }
}