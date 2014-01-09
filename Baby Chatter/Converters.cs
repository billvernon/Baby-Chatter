using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using System.Data.Linq;
using BabyChatterData;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace Baby_Chatter
{
    public class WordCountConverter : IValueConverter
    {
        /*
         * Count the Words in a List of KeyedLists of Words or a ObservableCollection of Words and return a string 
         * of the number of words counted, "X words".
         */
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int count = 0;

            if (value.GetType() == typeof(ObservableCollection<Word>))
            {
                count = ((ObservableCollection<Word>)value).Count();
            }
            else
            {
                List<KeyedList<string, List<Word>>> lists = (List<KeyedList<string, List<Word>>>)value;


                foreach (KeyedList<string, List<Word>> k in lists)
                {
                    count += k.Count();
                }
            }


            if (count == 1)
            {
                return "1 word";
            }
            else
            {
                return count + " words";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class WordEntitySetToObservableCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            EntitySet<Word> set = (EntitySet<Word>)value;
            return new System.Collections.ObjectModel.ObservableCollection<Word>(set);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /*
     * Convert a DateTime object to a short date string.
     */
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime)value).ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /*
     *  Convert a DateTime or a String to a String.  This class is useful when the object type can be either a DateTime or a String and we are unable to differentiate between the two
     *  in XAML.
     */
    public class DateTimeOrStringToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() == typeof(DateTime))
            {
                return ((DateTime)value).ToShortDateString();
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
