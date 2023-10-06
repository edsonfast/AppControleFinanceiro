using AppControleFinanceiro.Views;
using System.Globalization;

namespace AppControleFinanceiro {
    public partial class App : Application {
        public App(TransactionList listPage) {
            InitializeComponent();
            var culture = new CultureInfo("pt-BR"); // Replace with your desired culture or language code
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            //MainPage = new NavigationPage(new TransactionList(listPage)); //TransactionList(); //AppShell();
            MainPage = new NavigationPage(listPage);
        }
    }
}