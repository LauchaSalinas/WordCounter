using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace WordCounter
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            try
            {
                var builder = MauiApp.CreateBuilder();
                builder
                    .UseMauiApp<App>()
                    .UseMauiCommunityToolkit()
                    .ConfigureFonts(fonts =>
                    {
                        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    });

                builder.Services.AddMauiBlazorWebView();

    #if DEBUG
    		    builder.Services.AddBlazorWebViewDeveloperTools();
    		    builder.Logging.AddDebug();
#endif
                return builder.Build();

            }
            catch (Exception ex)
            {
                string runTimeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                using (StreamWriter outputFile = new StreamWriter(runTimeDirectory + "\\log.txt", true))
                {
                    outputFile.WriteLine(ex.ToString());
                }
            }
            throw new Exception("Error at start");
        }
    }
}
