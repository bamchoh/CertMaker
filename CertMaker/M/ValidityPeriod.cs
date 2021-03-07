using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;

using Prism.Mvvm;

namespace CertMaker.M
{
    public class ValidityPeriod : BindableBase, IDisposable, INotifyPropertyChanged
    {
        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set { this.SetProperty(ref date, value); }
        }

        public List<int> Hours { get; set; }

        public int HoursIndex { get; set; }

        public List<int> Minutes { get; set; }

        public int MinutesIndex { get; set; }

        public List<int> Seconds { get; set; }

        public int SecondsIndex { get; set; }

        public ValidityPeriod(DateTime baseTime)
        {
            Date = baseTime;

            Hours = new List<int>();
            for(int i = 0; i < 24; i++)
            {
                Hours.Add(i);
            }

            HoursIndex = Date.Hour;

            Minutes = new List<int>();
            for (int i = 0; i < 60; i++)
            {
                Minutes.Add(i);
            }

            MinutesIndex = Date.Minute;

            Seconds = new List<int>();
            for (int i = 0; i < 60; i++)
            {
                Seconds.Add(i);
            }

            SecondsIndex = Date.Second;
        }

        public override string ToString()
        {
            var date = new DateTime(
                Date.Year,
                Date.Month,
                Date.Day,
                Hours[HoursIndex],
                Minutes[MinutesIndex],
                Seconds[SecondsIndex]
                ).ToUniversalTime();

            return string.Format("{0}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}Z",
                date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
        }

        public void Dispose()
        {
            // NOPE;
        }
    }
}
