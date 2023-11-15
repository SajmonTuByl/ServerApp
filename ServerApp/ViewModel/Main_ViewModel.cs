﻿using ServerApp.Model;
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
using Google.Protobuf.WellKnownTypes;

namespace ServerApp.ViewModel
{
    public class SensorSample
    {
        public DateTime DateTime { get; set; }
        public double Value { get; set; }
    }

    public class Date : INotifyPropertyChanged
    {
        private string fromDate;
        private string toDate;
        private string fromHours;
        private string toHours;
        private string fromMinutes;
        private string toMinutes;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public string FromDate 
        {
            get
            { 
                return fromDate;
            }
            set
            {
                fromDate = value;
                OnPropertyChanged("FromDate");
            }
        }
        public string ToDate
        {
            get
            {
                return toDate;
            }
            set
            {
                toDate = value;
                OnPropertyChanged("ToDate");
            }
        }
        public string FromHours
        {
            get
            {
                return fromHours;
            }
            set
            {

                if (Int16.TryParse(value, out short val) && val>=0 && val <=24)
                {
                    fromHours = val.ToString();
                }
                OnPropertyChanged("FromHours");
            }
        }
        public string ToHours
        {
            get
            {
                return toHours;
            }
            set
            {

                if (Int16.TryParse(value, out short val) && val >= 0 && val <= 24)
                {
                    toHours = val.ToString();
                }
                OnPropertyChanged("ToHours");
            }
        }
        public string FromMinutes
        {
            get
            {
                return fromMinutes;
            }
            set
            {

                if (Int16.TryParse(value, out short val) && val >= 0 && val <= 24)
                {
                    fromMinutes = val.ToString();
                }
                OnPropertyChanged("FromMinutes");
            }
        }
        public string ToMinutes
        {
            get
            {
                return toMinutes;
            }
            set
            {

                if (Int16.TryParse(value, out short val) && val >= 0 && val <= 24)
                {
                    toMinutes = val.ToString();
                }
                OnPropertyChanged("ToMinutes");
            }
        }

        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }

        public Date()
        {
            FromDateTime = DateTime.Now;
            ToDateTime = DateTime.Now;

            FromDate = FromDateTime.Date.ToString();
            ToDate = ToDateTime.Date.ToString();
            FromHours = FromDateTime.Hour.ToString();
            ToHours = ToDateTime.Hour.ToString();
            FromMinutes = FromDateTime.Minute.ToString();
            ToMinutes = ToDateTime.Minute.ToString();
        }
        
        public DateTime GetFromDateTime()
        {
            FromDateTime = DateTime.Parse(FromDate.ToString() + " " + FromHours + ":" + FromMinutes);
            return FromDateTime;
        }
        public DateTime GetToDateTime()
        {
            ToDateTime = DateTime.Parse(ToDate.ToString() + " " + ToHours + ":" + ToMinutes);
            return ToDateTime;
        }
    }

    public class Main_ViewModel : INotifyPropertyChanged
    {
        private string serverIp;
        private string serverPort;
        private string serverStatus;

        private string dbIp;
        private string dbPort;
        private string dbStatus;

        private SensorObj_Model selectedSensor;
        private LineSeries series1;

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

        public SensorObj_Model SelectedSensor
        {
            get => selectedSensor;
            set
            {
                selectedSensor = value;
                OnPropertyChanged("SelectedSensor");
                
                if (selectedSensor.SensorName != null && selectedSensor.Samples != null)
                {
                    Series1.Title = SelectedSensor.SensorName;
                    Series1.Values = SelectedSensor.Samples;
                }
                
            }
        }
        public LineSeries Series1
        {
            get => series1;
            set
            {
                series1 = value;
                OnPropertyChanged("Series1");
            }
        }

        // Formatowanie osi X
        //public Func<double, string> Formatter { get; set; } = value => new DateTime((long)value).ToString("HH:mm");
        public Func<double, string> XFormatter { get; set; } = value => new DateTime((long)value).ToString("T");
        public Func<double, string> YFormatter { get; set; } = value => value.ToString("N2");

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
            SelectedSensor = new SensorObj_Model();
            YFormatter = value => value.ToString("N2") + SelectedSensor.SensorUnit;

            Series1 = new LineSeries
            {
                Title = SelectedSensor.SensorName,
                Values = SelectedSensor.Samples,
                Fill = Brushes.Transparent,
                //LabelPoint = point => point.Y + "K"
            };

            var dayConfig = Mappers.Xy<SensorSample>()
              .X(dateModel => dateModel.DateTime.Ticks) // / TimeSpan.FromDays(1).Ticks)
              .Y(dateModel => dateModel.Value);

            Series = new SeriesCollection(dayConfig)
            {
                Series1,
                //Series2,
                //Series3...
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
