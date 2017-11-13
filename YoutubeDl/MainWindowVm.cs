using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using YoutubeDl.Annotations;

namespace YoutubeDl
{
    public class MainWindowVm : INotifyPropertyChanged { 

        private string _logText;

        private int _formatNum;

        private List<YoutubeInfo> _formats;


        private YoutubeInfo _selected;

        private string _savePath;
        //private MainWindow _mainWindow;

//        public MainWindowVm(MainWindow mainWindow)
//        {
//
//
//        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        

        [XmlIgnore]
        public string LogText           
        {
            get => _logText;
            set
            {
                if (value == _logText) return;
                _logText = value;
                OnPropertyChanged();

            }
        }

        public int FormatNum    
        {
            get { return _formatNum; }
            set
            {
                if (value == _formatNum) return;
                _formatNum = value;
                OnPropertyChanged();
            }
        }

        public string SavePath  
        {
            get { return _savePath; }
            set
            {
                if (value == _savePath) return;
                _savePath = value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        public List<YoutubeInfo> Formats
        {
            get { return _formats; }
            set
            {
                if (Equals(value, _formats)) return;
                _formats = value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        public YoutubeInfo Selected
        {
            get { return _selected; }
            set
            {
                if (Equals(value, _selected)) return;
                _selected = value;
                OnPropertyChanged();
                FormatNum = value.FormatNum;
            }
        }


        public void Serialize()
        {
            var sr = new XmlSerializer(this.GetType());
            using (var s = new FileStream("config.xml", FileMode.OpenOrCreate|FileMode.Truncate))
            {
                sr.Serialize(s, this);
            }

        }

        public static MainWindowVm Deserialize()
        {
            var sr = new XmlSerializer(typeof(MainWindowVm));
            using (var s = new FileStream("config.xml", FileMode.OpenOrCreate))
            {
                return (MainWindowVm)sr.Deserialize(s);
            }
        }
    }
}