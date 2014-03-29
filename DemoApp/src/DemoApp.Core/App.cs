using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;
using DemoApp.Core.ViewModels;

namespace DemoApp.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<SessionsViewModel>();
        }
    }
}
