namespace ContosoFlowers
{
    using System.Configuration;
    using Autofac;
    using BotAssets.Dialogs;
    using Dialogs;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Internals.Fibers;
    using Microsoft.Bot.Builder.Scorables;
    using Microsoft.Bot.Connector;
    using Services.Models;

    public class ContosoFlowersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ContosoFlowersDialogFactory>()
                .Keyed<IContosoFlowersDialogFactory>(FiberModule.Key_DoNotSerialize)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<RootDialog>()
                .As<IDialog<object>>()
                .InstancePerDependency();

            builder.RegisterType<SettingsScorable>()
                .As<IScorable<IActivity, double>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FlowerCategoriesDialog>()
                .InstancePerDependency();

            builder.RegisterType<BouquetsDialog>()
                .InstancePerDependency();

            builder.RegisterType<AddressDialog>()
                .InstancePerDependency();

            builder.RegisterType<SavedAddressDialog>()
              .InstancePerDependency();

            builder.RegisterType<SettingsDialog>()
             .InstancePerDependency();

            // Service dependencies
            builder.RegisterType<Services.InMemoryOrdersService>()
                .Keyed<Services.IOrdersService>(FiberModule.Key_DoNotSerialize)
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<Services.InMemoryBouquetRepository>()
                .Keyed<Services.IRepository<Bouquet>>(FiberModule.Key_DoNotSerialize)
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<Services.InMemoryFlowerCategoriesRepository>()
                .Keyed<Services.IRepository<FlowerCategory>>(FiberModule.Key_DoNotSerialize)
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<Services.BingLocationService>()
                .WithParameter("bingMapsKey", ConfigurationManager.AppSettings["MicrosoftBingMapsKey"])
                .Keyed<Services.ILocationService>(FiberModule.Key_DoNotSerialize)
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}