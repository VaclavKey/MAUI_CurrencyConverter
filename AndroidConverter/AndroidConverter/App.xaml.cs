
namespace AndroidConverter
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override async void OnStart()
        {

            CurrencyConverter.InitializeCache();
            await CurrencyConverter.UpdateRates();
        }
    }
}
