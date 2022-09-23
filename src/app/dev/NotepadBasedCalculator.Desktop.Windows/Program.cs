using Avalonia;
using NotepadBasedCalculator.Core.Mef;

namespace NotepadBasedCalculator.Desktop.Windows
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            return
                AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .With(new Win32PlatformOptions
                {
                    UseWindowsUIComposition = true,
                    CompositionBackdropCornerRadius = 8f,
                })
                .With(new MefComposer(new[]
                {
                    typeof(Program).Assembly,
                    typeof(App).Assembly
                }).Provider);
        }
    }
}
