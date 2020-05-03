using System.ComponentModel;
using System.Runtime.CompilerServices;
using SbNotifierDevice.Annotations;

namespace SbNotifierDevice.ViewModels
{
    public abstract class BasePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Version => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString();
    }
}