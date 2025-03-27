using CommandLine;

namespace ToolConfigEditor
{
    internal class Options
    {
        [Option('p', "project", Required = false, HelpText = "Project to open")]
        public string ProjectFile { get; set; } = "";
    }

    internal static class Program
    {
        private static string? projectFile;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions);

            Application.Run(new MainWindow(projectFile));

        }

        static void RunOptions(Options opts)
        {
            try
            {
                projectFile = opts.ProjectFile;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occured: {ex.Message}");
            }
        }
    }
}