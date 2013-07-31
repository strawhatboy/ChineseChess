using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Strawhat.Games
{
    [XmlRoot]
    public abstract class GameObject : INotifyPropertyChanged
    {
        private string _Name;

        [XmlElement]
        public string Name
        {
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    this.OnPropertyChanged("Name");
                }
            }
            get
            {
                return _Name;
            }
        }

        private string _Description;

        [XmlElement]
        public string Description
        {
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    this.OnPropertyChanged("Description");
                }
            }
            get
            {
                return _Description;
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
