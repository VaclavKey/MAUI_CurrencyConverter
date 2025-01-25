using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AndroidConverter
{
    internal class ConverterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public ObservableCollection<string> Currencies { get; set; }
        public static string CurrentCurrencyAbbr { get; set; }


        private decimal? amountRUB;
        public decimal? AmountRUB
        {
            get => amountRUB;
            set
            {
                if (amountRUB != value)
                {
                    amountRUB = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AmountRUB"));
                }
            }
        }
       
      
        private decimal? amountUSD;
        public decimal? AmountUSD
        {
            get => amountUSD;
            set
            {
                if (amountUSD != value)
                {
                    amountUSD = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AmountUSD"));
                }
            }
        }
       
       
        private decimal? amountEUR;
        public decimal? AmountEUR
        {
            get => amountEUR;
            set
            {
                if (amountEUR != value)
                {
                    amountEUR = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AmountEUR"));
                }
            }
        }
        
       
        private decimal? amountGBP;
        public decimal? AmountGBP
        {
            get => amountGBP;
            set
            {
                if (amountGBP != value)
                {
                    amountGBP = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AmountGBP"));
                }
            }
        }
       
       
        private decimal? amountJPY;
        public decimal? AmountJPY
        {
            get => amountJPY;
            set
            {
                if (amountJPY != value)
                {
                    amountJPY = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AmountJPY"));
                }
            }
        }
        
       
        private decimal? amountBYN;
        public decimal? AmountBYN
        {
            get => amountBYN;
            set
            {
                if (amountBYN != value)
                {
                    amountBYN = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AmountBYN"));
                }
            }
        }
       
       
        private decimal? amountPLN;
        public decimal? AmountPLN
        {
            get => amountPLN;
            set
            {
                if (amountPLN != value)
                {
                    amountPLN = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AmountPLN"));
                }
            }
        }
        
       
        private decimal? amountCNY;
        public decimal? AmountCNY
        {
            get => amountCNY;
            set
            {
                if (amountCNY != value)
                {
                    amountCNY = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AmountCNY"));
                }
            }
        }
       
       
        private decimal? amountTRY;
        public decimal? AmountTRY
        {
            get => amountTRY;
            set
            {
                if (amountTRY != value)
                {
                    amountTRY = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AmountTRY"));
                }
            }
        }
       

        private decimal? amountKZT;
        public decimal? AmountKZT
        {
            get => amountKZT;
            set
            {
                if (amountKZT != value)
                {
                    amountKZT = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AmountKZT"));
                }
            }
        }



        public double CurrencyBtnWidth { get; }
        public double CurrencyEntryWidth { get; }
        public double CurrencyImgSize { get; }
        public double CurrencyFieldHeight { get; }
        public double AbbrZoneWidth { get; }
        public double AmountZoneWidth { get; }
        public double CurrencyChoiceFieldHeight { get; }
        public double MainCurrencyImgSize { get; }

       
        
        private string mainCurrencyImg;
        public string MainCurrencyImg
        {
            get => mainCurrencyImg;
            set
            {
                if (mainCurrencyImg != value)
                {
                    mainCurrencyImg = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MainCurrencyImg"));
                }
            }
        }


        private string mainCurrencyAbbr;
        public string MainCurrencyAbbr
        {
            get => mainCurrencyAbbr;
            set
            {
                if (mainCurrencyAbbr != value)
                {
                    mainCurrencyAbbr = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MainCurrencyAbbr"));
                    if (MainCurrencyAbbr != null)
                    MainCurrencyImg = MainCurrencyAbbr.ToLower() + ".png";
                    MainAmount = 0;
                }
            }
        }


        private decimal mainAmount;
        public decimal MainAmount
        {
            get => mainAmount;
            set
            {
                if (mainAmount != value)
                {
                    mainAmount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MainAmount"));
                    _ = RunLoadDataAsync();
                }
            }
        }


        public ConverterViewModel()
        {
            Currencies = new ObservableCollection<string>
            {
                "RUB",
                "USD",
                "EUR",
                "GBP",
                "JPY",
                "BYN",
                "PLN",
                "CNY",
                "TRY",
                "KZT"
            };

            if (CurrentCurrencyAbbr != null)
            {
                MainCurrencyAbbr = CurrentCurrencyAbbr;
                MainCurrencyImg = CurrentCurrencyAbbr.ToLower() + ".png";
            }

            double screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            double screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
            

            CurrencyBtnWidth = screenWidth * 0.3;
            CurrencyEntryWidth = screenWidth * 0.6;
            MainCurrencyImgSize = screenWidth * 0.15;
            CurrencyChoiceFieldHeight = screenWidth * 0.3;

            AbbrZoneWidth = screenWidth * 0.3;
            AmountZoneWidth = screenWidth * 0.7;
            CurrencyImgSize = screenWidth * 0.12;
            CurrencyFieldHeight = screenHeight * 0.08;
        }
        
        private async Task RunLoadDataAsync()
        {
            await Convert();
        }

        private async Task Convert()
        {
            AmountRUB = MainAmount * await CurrencyConverter.GetRate(MainCurrencyAbbr, "RUB");
            AmountUSD = MainAmount * await CurrencyConverter.GetRate(MainCurrencyAbbr, "USD");
            AmountEUR = MainAmount * await CurrencyConverter.GetRate(MainCurrencyAbbr, "EUR");
            AmountGBP = MainAmount * await CurrencyConverter.GetRate(MainCurrencyAbbr, "GBP");
            AmountJPY = MainAmount * await CurrencyConverter.GetRate(MainCurrencyAbbr, "JPY");
            AmountBYN = MainAmount * await CurrencyConverter.GetRate(MainCurrencyAbbr, "BYN");
            AmountPLN = MainAmount * await CurrencyConverter.GetRate(MainCurrencyAbbr, "PLN");
            AmountCNY = MainAmount * await CurrencyConverter.GetRate(MainCurrencyAbbr, "CNY");
            AmountTRY = MainAmount * await CurrencyConverter.GetRate(MainCurrencyAbbr, "TRY");
            AmountKZT = MainAmount * await CurrencyConverter.GetRate(MainCurrencyAbbr, "KZT");
        }
    }
}
