namespace ForeignExchange.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using System;

    public class MainViewModel
    {
        #region Properties
        public string Amount
        {
            get;
            set;
        }

        public ObservableCollection<Rate> Rates
        {
            get;
            set;
        }

        public Rate SourceRate
        {
            get;
            set;
        }

        public Rate TargetRate
        {
            get;
            set;
        }

        public bool IsRunning
        {
            get;
            set;
        }

        public bool IsEnabled
        {
            get;
            set;
        }

        public sbyte Result { get; set; }

        #endregion

        public MainViewModel()
        {

        }

        #region Commands

        public ICommand ConvertCommand
        {
            get
            {
                return new RelayCommand(Convert);
            }
        }

        private void Convert()
        {
            
            throw new NotImplementedException();
        }

        #endregion
    }
}
