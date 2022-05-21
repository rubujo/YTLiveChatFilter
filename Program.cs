using Microsoft.Extensions.DependencyInjection;

namespace YTLiveChatFilter;

internal static class Program
{
    private static IServiceProvider? ServiceProvider { get; set; }

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

        ConfigureServices();

        Application.Run((MainForm)ServiceProvider?.GetService(typeof(MainForm))!);
    }

    // 參考：https://docs.microsoft.com/zh-tw/archive/msdn-magazine/2019/may/net-core-3-0-create-a-centralized-pull-request-hub-with-winforms-in-net-core-3-0
    private static void ConfigureServices()
    {
        ServiceCollection services = new();

        services.AddHttpClient()
            .AddSingleton<MainForm>();

        ServiceProvider = services.BuildServiceProvider();
    }
}