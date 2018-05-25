using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Utils.Net.Helpers;

namespace Utils.NetTests
{
    public static class UITester
    {
        private static Thread thread;
        private static Type applicationType;

        public static Application Application { get; private set; }
        public static Dispatcher Dispatcher => Application?.Dispatcher;
        public static Window MainWindow => Application?.MainWindow;

        public static void Init(Type applicationType)
        {
            if (Application == null)
            {
                UITester.applicationType = applicationType;

                thread = new Thread(ThreadRun)
                {
                    Name = "Main (UI)",
                    IsBackground = true
                };
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();

                WaitFor(
                    () => Dispatcher == null || Dispatcher.Invoke(() => Application?.MainWindow == null || !Application.MainWindow.IsLoaded),
                    TimeSpan.FromSeconds(15));
            }
        }

        public static T Get<T>(Func<T, bool> condition = null) where T : DependencyObject
        {
            return Dispatcher?.CheckAndInvoke(() => MainWindow.FindVisualDescendant(condition));
        }

        public static void Stop()
        {
            Dispatcher?.CheckAndInvoke(() => MainWindow?.Close());
            thread.Join();
        }


        private static void ThreadRun()
        {
            Application = applicationType.GetConstructor(new Type[0]).Invoke(new object[0]) as Application;
            applicationType.GetMethod("InitializeComponent").Invoke(Application, null);
            Application.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Application.Run();
        }

        public static void WaitFor(Func<bool> condition, TimeSpan timeout)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            while (condition())
            {
                if (stopwatch.Elapsed > timeout)
                {
                    return;
                }
            }
        }
    }
}
