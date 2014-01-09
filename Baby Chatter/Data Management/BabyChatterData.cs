using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Microsoft.Phone.Data.Linq;
using Microsoft.Phone.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using Baby_Chatter;

namespace BabyChatterData
{
    public class BabyChatterDataContext : DataContext
    {
        public const string DBConnectionString = "data source = isostore:/database.sdf";

        #region Constructors 

        public BabyChatterDataContext()
            : base(DBConnectionString)
        {}

        #endregion

        public Table<Word> words;
        public Table<Child> children;
    }
    
    [Table]
    public class Word : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private string _word;

        [Column(IsPrimaryKey = true, CanBeNull = false)]
        public string word
        {
            get
            {
                return _word;
            }
            set
            {
                OnPropertyChanging("word");
                _word = value;
                OnPropertyChanged("word");
            }
        }

        public DateTime _date;

        [Column(CanBeNull=false)]
        public DateTime date
        {
            get
            {
                return _date;
            }
            set
            {
                OnPropertyChanging("date");
                _date = value;
                OnPropertyChanged("date");
            }
        }

        private string _diary;

        [Column]
        public string diary
        {
            get
            {
                return _diary;
            }
            set
            {
                OnPropertyChanging("diary");
                _diary = value;
                OnPropertyChanged("diary");
            }
        }

        private string _pictureURL;

        [Column]
        public string pictureURL
        {
            get
            {
                return _pictureURL;
            }
            set
            {
                OnPropertyChanging("pictureURL");
                _pictureURL = value;
                OnPropertyChanged("pictureURL");
            }
        }

        /*
         * Get the picture associated with this word.
         */
        public WriteableBitmap PictureThumb
        {
            get
            {
                return Utilities.GetThumbnail(Utilities.GetImageFromDisk(pictureURL));
            }
        }


        private string _childName;

        [Column]
        public string childName
        {
            get
            {
                return _childName;
            }
            set
            {
                OnPropertyChanging("child");
                _childName = value;
                OnPropertyChanged("child");
            }
        }

        #region  Property Changes

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanging(string property)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(property));
            }
        }

        #endregion
    }

    [Table]
    public class Child : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constructors

        #endregion
        private string _name;

        [Column(IsPrimaryKey=true, CanBeNull=false)]
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                OnPropertyChanging("name");
                _name = value;
                OnPropertyChanged("name");
            }
        }

        private DateTime _birthdate;

        [Column(CanBeNull=false)]
        public DateTime birthdate
        {
            get
            {
                return _birthdate;
            }
            set
            {
                OnPropertyChanging("birthdate");
                _birthdate = value;
                OnPropertyChanged("birthdate");
            }
        }

        private string _pictureURL;

        [Column]
        public string pictureURL
        {
            get
            {
                return _pictureURL;
            }
            set
            {
                OnPropertyChanging("pictureURL");
                _pictureURL = value;
                OnPropertyChanged("pictureURL");
            }
        }

        public ObservableCollection<Word> Words
        {
            get
            {
                using (BabyChatterDataContext db = new BabyChatterDataContext())
                {
                    var words = from Word word in db.words
                                where word.childName == name
                                select word;

                    return new ObservableCollection<Word>(words.ToList<Word>());
                }
            }
        }

        public BitmapImage ChildImage
        {
            get
            {
                return Utilities.GetImageFromDisk(pictureURL);
            }
        }
        
        /*
         * Since Words is not stored as a property, there is no way locally to update when a child's words are updated.  Calling this will 
         * notify the property tracking system that Words has been updated.
         */
        public void NotifyWordsChanged()
        {
            OnPropertyChanged("Words");
        }


        #region  Property Changes

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanging(string property)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(property));
            }
        }

        #endregion
    }
}
