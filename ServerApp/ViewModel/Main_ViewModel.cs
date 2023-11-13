using ServerApp.Model;
using ServerApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

//using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;
using LiveCharts.Configurations;

namespace ServerApp.ViewModel
{
    public class SensorSample
    {
        public DateTime DateTime { get; set; }
        public double Value { get; set; }
    }

    public class Main_ViewModel : INotifyPropertyChanged
    {
        private string serverIp;
        private string serverPort;
        private string serverStatus;

        private string dbIp;
        private string dbPort;
        private string dbStatus;

        private int selectedSensorParentId;
        private int selectedSensorSensorId;

        public ObservableCollection<DeviceObj_Model> DevicesList { get; set; }
        public ObservableCollection<SensorObj_Model> SensorsList { get; set; }

        public string ServerIp
        {
            get => serverIp;
            set
            {
                serverIp = value;
                OnPropertyChanged("ServerIp");
            }
        }
        public string ServerPort
        {
            get => serverPort;
            set
            {
                serverPort = value;
                OnPropertyChanged("ServerPort");
            }
        }
        public string ServerStatus
        {
            get => serverStatus;
            set
            {
                serverStatus = value;
                OnPropertyChanged("ServerStatus");
            }
        }

        public string DbIp
        {
            get => dbIp;
            set
            {
                dbIp = value;
                OnPropertyChanged("Db");
            }
        }
        public string DbPort
        {
            get => dbPort;
            set
            {
                dbPort = value;
                OnPropertyChanged("DbPort");
            }
        }
        public string DbStatus
        {
            get => dbStatus;
            set
            {
                dbStatus = value;
                OnPropertyChanged("DbStatus");
            }
        }

        public int SelectedSensorParentId
        {
            get => selectedSensorParentId;
            set
            {
                selectedSensorParentId = value;
                OnPropertyChanged("SelectedSensorParentId");
            }
        }
        public int SelectedSensorSensorId
        {
            get => selectedSensorSensorId;
            set
            {
                selectedSensorSensorId = value;
                OnPropertyChanged("SelectedSensorSensorId");
            }
        }

        // Formatowanie osi X
        //public Func<double, string> Formatter { get; set; } = value => new DateTime((long)value).ToString("HH:mm");
        public Func<double, string> Formatter { get; set; } = value => new DateTime((long)value).ToString("d");

        // Zbiór serii, które zostaną pokazane na wykresie
        public SeriesCollection Series { get; set; }

        // Zbiór wartości zmierzonych próbek
        // Do zbioru serii SeriesCollection należy dodać nową serię, do której podpinamy poniższy zbiór wartości próbek
        public ChartValues<SensorSample> Samples { get; set; } = new ChartValues<SensorSample>();
        /* Dodawanie próbek
           Samples.Add(new SensorSample
                {
                    DateTime = DateTime.Now,
                    Value = 5
                });
        */

        public Main_ViewModel()
        {
            DevicesList = new ObservableCollection<DeviceObj_Model>();
            SensorsList = new ObservableCollection<SensorObj_Model>();

            var dayConfig = Mappers.Xy<SensorSample>()
              .X(dateModel => dateModel.DateTime.Ticks) // / TimeSpan.FromDays(1).Ticks)
              .Y(dateModel => dateModel.Value);

            Series = new SeriesCollection(dayConfig)
            {
                new LineSeries
                {
                    Title = "Google Rank",
                    Values = Samples,
                    Fill = Brushes.Transparent,
                },
            };

            ServerIp = "127.0.0.1";
            ServerPort = "11000";
            ServerStatus = "Nieuruchomiony";
            DbIp = "127.0.0.1";
            DbPort = "3306"; //orcl
            DbStatus = "Niepodłączona";
        }       

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
