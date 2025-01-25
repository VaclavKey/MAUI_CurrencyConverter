using System.Windows.Input;

namespace AndroidConverter
{
    internal class MainViewModel 
    {
        public double BorderHeight { get; set; }

        public ICommand ButtonCommand { get; }

        private bool canExecute = true;

        public MainViewModel()
        {
            BorderHeight = (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density) * 0.1;

            ButtonCommand = new Command(async () => await ExecuteButtonCommand(), () => canExecute);        
        }

        public async Task ExecuteButtonCommand()
        {
            canExecute = false;
            (ButtonCommand as Command)?.ChangeCanExecute();

            await Task.Delay(1000);

            canExecute = true;
            (ButtonCommand as Command)?.ChangeCanExecute();
        }
    }
}
