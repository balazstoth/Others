using System.ComponentModel;

namespace Manager
{
    class Binding : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string txt)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(txt));
        }
    }
}
