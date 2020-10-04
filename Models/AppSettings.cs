namespace csproj_sorter.Models
{
    public class AppSettings
    {
        /// <summary>
        /// Sets header title in console window
        /// </summary>
        public string ConsoleTitle { get; set; }
        /// <summary>
        /// The default name of the file configuration to look for when running the application
        /// </summary>
        public string DefaultConfigFileName { get; set; }

        public string Description { get; set; }
    }
}
