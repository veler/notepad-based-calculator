using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using NotepadBasedCalculator.Desktop.Platform;

namespace NotepadBasedCalculator.Desktop
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                IMefProvider mefProvider = AvaloniaLocator.Current.GetService<IMefProvider>()!;
                Guard.IsNotNull(mefProvider);
                mefProvider.Import<IPlatformInitializer>().Initialize();

                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
