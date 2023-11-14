using Serilog;

namespace IPhoneFileRenameTool
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.File("log.txt") // Set the path to your log file
            .CreateLogger();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            IPhoneFileRanameForm mainForm;
            if (args.Length > 0 )
            {
                mainForm = new IPhoneFileRanameForm(args[0]);
            }
            else
            {
                mainForm = new IPhoneFileRanameForm();
            }
            Application.Run(mainForm);
        }
    }
}