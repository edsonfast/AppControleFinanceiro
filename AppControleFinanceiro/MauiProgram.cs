using AppControleFinanceiro.Repo;
using AppControleFinanceiro.Views;
using LiteDB;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace AppControleFinanceiro {
    public static class MauiProgram {
        public static MauiApp CreateMauiApp() {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts => {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .RegisterDatabaseAndRepo()
                .RegisterViews();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        #region Injeção de Dependencia
        public static MauiAppBuilder RegisterDatabaseAndRepo(this MauiAppBuilder mauiAppBuilder) {
            // Instância do DB
            mauiAppBuilder.Services.AddSingleton<LiteDatabase>(
                options => {
                    return new LiteDatabase($"Filename={AppSettings.DatabasePath};Connection=Shared");
                }
            );

            // Instância um repósitorio para cada chamada (tela) "AddTransient"
            mauiAppBuilder.Services.AddTransient<ITransactionRepo, TransactionRepo>(); // posso ter uma classe TransactionRepo para cada tipo de banco mantendo a mesma interface TransactionRepoLiteDB e TransactionRepoRealm

            return mauiAppBuilder;
        }
        public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder) {
            // Motor de injeção de dependencia
            mauiAppBuilder.Services.AddTransient<TransactionAdd>();
            mauiAppBuilder.Services.AddTransient<TransactionEdit>();
            mauiAppBuilder.Services.AddTransient<TransactionList>();

            return mauiAppBuilder;
        }
        #endregion 
    }
}