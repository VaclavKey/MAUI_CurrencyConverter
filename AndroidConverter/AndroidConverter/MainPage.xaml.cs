using static AndroidConverter.ConverterViewModel;
namespace AndroidConverter
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }


        public void RUBClicked(object sender, EventArgs e)
        {
            CurrentCurrencyAbbr = "RUB";
            CurrencyClicked();
        }

        public void USDClicked(object sender, EventArgs e)
        {
            CurrentCurrencyAbbr = "USD";
            CurrencyClicked();
        }

        public void EURClicked(object sender, EventArgs e)
        {
            CurrentCurrencyAbbr = "EUR";
            CurrencyClicked();
        }

        public void GBPClicked(object sender, EventArgs e)
        {
            CurrentCurrencyAbbr = "GBP";
            CurrencyClicked();
        }

        public void JPYClicked(object sender, EventArgs e)
        {
            CurrentCurrencyAbbr = "JPY";
            CurrencyClicked();
        }

        public void BYNClicked(object sender, EventArgs e)
        {
            CurrentCurrencyAbbr = "BYN";
            CurrencyClicked();
        }

        public void PLNClicked(object sender, EventArgs e)
        {
            CurrentCurrencyAbbr = "PLN";
            CurrencyClicked();
        }

        public void CNYClicked(object sender, EventArgs e)
        {
            CurrentCurrencyAbbr = "CNY";
            CurrencyClicked();
        }

        public void TRYClicked(object sender, EventArgs e)
        {
            CurrentCurrencyAbbr = "TRYC";
            CurrencyClicked();
        }

        public void KZTClicked(object sender, EventArgs e)
        {
            CurrentCurrencyAbbr = "KZT";
            CurrencyClicked();
        }


        public async void CurrencyClicked()
        {
            await Navigation.PushAsync(new ConverterPage());
        }

        public void DisplayAlertClick(object sender, EventArgs e)
        {
            DisplayAlert("Кнопка нажата", "Вы нажали кнопку", "ОК");
        }


        private bool isButtonProcessing = false;

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            if (isButtonProcessing) return;

            isButtonProcessing = true;

            var button = sender as Button;
            if (button != null)
            {
                button.IsEnabled = false;

                await Task.Delay(500);

                button.IsEnabled = true;
                isButtonProcessing = false;
            }
        }
    }
}
