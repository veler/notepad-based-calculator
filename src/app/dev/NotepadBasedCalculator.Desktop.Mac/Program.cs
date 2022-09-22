using Avalonia;
using Avalonia.Controls;
using NotepadBasedCalculator.Core.Mef;

namespace NotepadBasedCalculator.Desktop.Mac
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(
                    args,
                    shutdownMode: ShutdownMode.OnMainWindowClose);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            return
                AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .With(new X11PlatformOptions
                {
                    EnableMultiTouch = true
                })
                .With(AppDelegate.Init())
                .With(new MefComposer(new[]
                {
                    typeof(Program).Assembly,
                    typeof(App).Assembly
                }).Provider);
        }
    }
}
