using System.ComponentModel;
using ShUtilities.Windows;

namespace ShUtilitiesTest.Mocks
{
    public class PropertyChangedMock: INotifyPropertyChanged
    {
        public long Direct
        {
            get => _direct;
            set
            {
                if (_direct != value)
                {
                    _direct = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Direct)));
                }
            }
        }
        private long _direct;

        public long SetAndRaise
        {
            get => _setAndRaise;
            set => this.SetAndRaise(ref _setAndRaise, value, PropertyChanged);
        }
        private long _setAndRaise;

        public long SetAndRaiseWithReflection
        {
            get => _setAndRaiseWithReflection;
            set => this.SetAndRaise(ref _setAndRaiseWithReflection, value);
        }
        private long _setAndRaiseWithReflection;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}