namespace ForeignExchange.ViewModels
{
    using ForeignExchange.Helpers;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class MainViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        ApiService apiService;
        #endregion

        #region Attributes
        bool _isRunning;
        bool _isEnabled;
        string _result;
        string _status;
        Rate _sourceRate;
        Rate _targetRate;
        ObservableCollection<Rate> _rates;
        #endregion

        #region Properties

        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Status)));
                }
            }
        }

        public string Amount
        {
            get;
            set;
        }       

        public ObservableCollection<Rate> Rates
        {
            get
            {
                return _rates;
            }
            set
            {
                if (_rates != value)
                {
                    _rates = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Rates)));
                }
            }
        }

        public Rate SourceRate
        {
            get
            {
                return _sourceRate;
            }
            set
            {
                if (_sourceRate != value)
                {
                    _sourceRate = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(SourceRate)));
                }
            }
        }

        public Rate TargetRate
        {
            get
            {
                return _targetRate;
            }
            set
            {
                if (_targetRate != value)
                {
                    _targetRate = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(TargetRate)));
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsRunning)));
                }    
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }

        public string Result
        {
            get
            {
                return _result;
            }
            set
            {
                if (_result != value)
                {
                    _result = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Result)));
                }
            }
        }

        #endregion

        #region Constructors
        public MainViewModel()
        {
            apiService = new ApiService(); 
            LoadRates();
        }

        
        #endregion

        #region Methods

       async void LoadRates()
        {
            IsRunning = true;
            Result = Lenguages.Loading;

            var connection = await apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                IsRunning = false;
                Result = connection.Message;
                return;
            }

            var response = await apiService.GetList<Rate>(
                "http://apiexchange.azurewebsites.net",
                "/api/Rates");

            if (!response.IsSuccess)
            {
                IsRunning = false;
                Result = response.Message;
                return;
            }

            Rates = new ObservableCollection<Rate>((List<Rate>)response.Result);
            IsRunning = false;
            IsEnabled = true;
            Result = Lenguages.Ready;
            Status = Lenguages.Status;
        }

        #endregion

        #region Commands

        public ICommand SwitchCommand
        {
            get
            {
                return new RelayCommand(Switch);
            }
        }

        void Switch()
        {
            var aux = SourceRate;
            SourceRate = TargetRate;
            TargetRate = aux;
            Convert();
        }

        public ICommand ConvertCommand
        {
            get
            {
                return new RelayCommand(Convert);
            }
        }

        async private void Convert()
        {
            try
            {
                if (string.IsNullOrEmpty(Amount))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        Lenguages.Error,
                        Lenguages.AmountValidation,
                        Lenguages.Accept);
                    return;
                }

                decimal amount = 0;
                if (!decimal.TryParse(Amount, out amount))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        Lenguages.Error,
                        Lenguages.AmountNumericValidation,
                        Lenguages.Accept);
                    return;
                }

                if (SourceRate == null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        Lenguages.Error,
                        Lenguages.SourceRateValidation,
                        Lenguages.Accept);
                    return;
                }

                if (TargetRate == null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        Lenguages.Error,
                        Lenguages.TargetRateValidation,
                        Lenguages.Accept);
                    return;
                }

                var amountConverted = amount /
                                      (decimal)SourceRate.TaxRate *
                                      (decimal)TargetRate.TaxRate;
                Result = string.Format(
                    "{0} {1:C2} = {2} {3:C2}",
                    SourceRate.Code,
                    amount,
                    TargetRate.Code,
                    amountConverted);
            }
            catch (Exception ex)
            {
                Result = ex.Message;
                throw;
            }            
        }

        #endregion
    }
}
