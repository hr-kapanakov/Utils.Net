using System.Windows;
using Autofac;
using Utils.Net.Interfaces;
using Utils.Net.Managers;

namespace Utils.Net.Sample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IContainer Container { get; private set; }

        public App()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<NavigationManager>().As<INavigationManager>().SingleInstance();
            Container = builder.Build();
        }
    }
}
