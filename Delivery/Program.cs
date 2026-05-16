namespace Delivery
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var apiHost = ApiHost.Start();
            Application.Run(new Login());
            apiHost.DisposeAsync().AsTask().GetAwaiter().GetResult();
        }
    }
}