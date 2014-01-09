using System;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Collections.ObjectModel;
using BabyChatterData;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;
using Baby_Chatter;

namespace Baby_Chatter.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged{

        public ApplicationViewModel() : base()
        {
            WordGroupingType = GroupingType.GroupAlphabetic;
        }
    
        #region Properties

        /*
         * Get all of the children stored in the application.
         */
        public ObservableCollection<Child> Children
        {
            get
            {
                using (BabyChatterDataContext db = new BabyChatterDataContext())
                {
                    return new ObservableCollection<Child>(db.children.ToList());
                }
            }
        }

        /*
         *  The current child being worked with.  Needed for setting up NewChild page and referencing the correct child for the new word page
         */
        public Child CurrentChild {get; set;}

        #endregion

        /*
         * Use SelectedWord to store a selected word so that the new view controller can access it.
         */
        public Word SelectedWord { get; set; }

        /*
         * Use SelectedChild to store a selected child so that the new view controller can access it.
         */
        public Child SelectedChild { get; set; }

        public enum GroupingType { GroupAlphabetic, GroupByDate };

        /*
         * Get or set how the current child's words are grouped.  Set to GroupAlphabetic by default.
         */
        private GroupingType _WordGroupingType;
        public GroupingType WordGroupingType
        {
            get
            {
                return _WordGroupingType;
            }
            set
            {
                _WordGroupingType = value;
                // notify that the word list has been updated
                NotifyPropertyChanged("CurrentChildsWords");
            }
        }

        /*
         * Get a grouped list of the current child's words.  The words will be grouped based on the value of WordGroupingType.
         * If WordGroupingType is set to GroupAlphabetic, then the Words will be grouped into lists, sorted alphabetically in
         * ascending order by the first letter of the words in each group.  If WordGroupingType is set to GroupByDate, then the
         * Words will be grouped into Lists, sorted by date in descending order by the month and year the words in each group
         * was said.
         */
        public List<KeyedList<string, List<Word>>> CurrentChildsWords
        {
            get
            {
                if (WordGroupingType == GroupingType.GroupAlphabetic)
                {
                    return (from Word w in CurrentChild.Words
                            group w by w.word.Substring(0,1).ToUpper() into groupedList
                            orderby groupedList.Key
                            select new KeyedList<string, List<Word>>(groupedList.Key, groupedList.ToList<Word>())).ToList();


                }
                // else WordGroupingType = GroupingType.GroupByDate
                else
                {
                    return (from Word w in CurrentChild.Words
                            group w by new DateTime(w.date.Year, w.date.Month, 1).ToString("Y") into groupedList
                            orderby Convert.ToDateTime((string)groupedList.Key) descending
                            select new KeyedList<string, List<Word>>(groupedList.Key, groupedList.ToList<Word>())).ToList();
                }
            }
        }

        /*
         * Insert a new child into the database.
         */
        public void AddNewChild(Child newChild)
        {
            using (BabyChatterDataContext db = new BabyChatterDataContext())
            {
                db.children.InsertOnSubmit(new Child(){
                                                name = newChild.name,
                                                birthdate = newChild.birthdate,
                                                pictureURL = newChild.pictureURL
                                            });
                db.SubmitChanges();
                Children.Add(newChild);
            }

            NotifyPropertyChanged("Children");
        }

        /*
         * Update the supplied word with the new values, if any in word.
         */
        public void UpdateChild(Child updatedChild)
        {
            using (BabyChatterDataContext db = new BabyChatterDataContext())
            {
                Child existingChild = (from Child c in db.children
                                       where c.name == updatedChild.name
                                       select c).First();

                string oldPictureURL = existingChild.pictureURL;
                existingChild.name = updatedChild.name;
                existingChild.birthdate = updatedChild.birthdate;
                existingChild.pictureURL = (existingChild.pictureURL == updatedChild.pictureURL) ? existingChild.pictureURL : updatedChild.pictureURL;

                db.SubmitChanges();

                // if picture was updated, delete old picture
                if (oldPictureURL != existingChild.pictureURL && !String.IsNullOrEmpty(oldPictureURL))
                {
                    Utilities.DeleteImageFromDisk(oldPictureURL);
                }

                NotifyPropertyChanged("Children");
            }
        }

        /*
         *  Remove a child from the database and the child list
         */
        public void RemoveChild(string childName)
        {
            using (BabyChatterDataContext db = new BabyChatterDataContext())
            {
                Child childToRemove = (from Child child in db.children
                                       where child.name == childName
                                       select child).ToList<Child>().ElementAt(0);

                if (childToRemove != null)
                {
                    db.children.DeleteOnSubmit(childToRemove);
                    db.SubmitChanges();
                    NotifyPropertyChanged("Children");
                }
            }
        } 

        public bool DoesChildExist(string childName)
        {
            childName = Utilities.CapitalizeString(childName);
            using (BabyChatterDataContext db = new BabyChatterDataContext())
            {
                var children = from Child child in db.children
                               where child.name == childName
                               select child;

                return (children.ToList().Count > 0);
            }
        }

        /*
         * Get a specific child by name.
         */
        public Child GetChild(string name)
        {
            using (BabyChatterDataContext db = new BabyChatterDataContext())
            {
                Child child = (from Child c in db.children
                               where c.name == name
                               select c).First();
                return child;
            }
        }

        /*
         * Check if the current child has said a word.  Return the Word if the child has or null otherwise.
         */
        public Word GetWordForCurrentChild(string word)
        {
            using (BabyChatterDataContext db = new BabyChatterDataContext())
            {
                var wordMatch = (from Word _word in db.words
                            where _word.word == word
                            select _word).ToList();

                if (wordMatch.Count() == 0)
                {
                    return null;
                }

                return wordMatch.ElementAt(0);
            }
        }

        /*
         * Insert a new Word into the database.
         */
        public void AddNewWord(Word newWord)
        {
            using (BabyChatterDataContext db = new BabyChatterDataContext())
            {
                Word newWordCopy = new Word()
                {
                    word = newWord.word,
                    date = newWord.date,
                    diary = newWord.diary,
                    pictureURL = newWord.pictureURL,
                    childName = newWord.childName
                };
                
                db.words.InsertOnSubmit(newWordCopy);

                db.SubmitChanges();
            }

            NotifyPropertyChanged("CurrentChildsWords");

            if (CurrentChild != null)
            {
                CurrentChild.NotifyWordsChanged();
            }
        }

        /*
         * Update the supplied word with the new values, if any in word.
         */
        public void UpdateWord(Word word)
        {
            // save the old picture url
            Word oldWord = GetWordForCurrentChild(word.word);
            string oldPictureURL = oldWord.pictureURL;

            using (BabyChatterDataContext db = new BabyChatterDataContext())
            {
                var existingWord = (from Word w in db.words
                                    where w.word == word.word && w.childName == word.childName
                                    select w).First();

                existingWord.word = word.word;
                existingWord.date = word.date;
                existingWord.diary = word.diary;
                existingWord.pictureURL = word.pictureURL;

                db.SubmitChanges();

                // notify that the words have changed so the long list selector updates the word list
                NotifyPropertyChanged("CurrentChildsWords");
                
                // delete old image, if needed
                if (oldPictureURL != word.pictureURL && !String.IsNullOrEmpty(oldPictureURL))
                {
                    Utilities.DeleteImageFromDisk(oldPictureURL);
                }
            }
        }

        /*
         *  Remove a Word from the database and the Word list
         */
        public void RemoveWord(string word)
        {
            using (BabyChatterDataContext db = new BabyChatterDataContext())
            {
                Word wordToRemove = (from Word _word in db.words
                                    where _word.word == word && _word.childName == CurrentChild.name
                                    select _word).ToList<Word>().ElementAt(0);

                // delete from databse and Word list
                if (wordToRemove != null)
                {
                    db.words.DeleteOnSubmit(wordToRemove);
                    db.SubmitChanges();
                }
            }
            NotifyPropertyChanged("CurrentChildsWords");
            CurrentChild.NotifyWordsChanged();
        }

        /*
         * Check if the current child has said a word already.
         */
        public bool DoesWordExist(string word)
        {
            using (BabyChatterDataContext db = new BabyChatterDataContext())
            {
                var words = (from Word _word in CurrentChild.Words
                             where _word.word == word
                             select _word).ToList();

                return (words.Count() > 0);
            }
        }

        /*
         * Return the Word object associated with a particular word.
         */
        public Word getWord(string word)
        {
            using (BabyChatterDataContext db = new BabyChatterDataContext())
            {
                var alreadyExists = (from Word _word in CurrentChild.Words
                                    where _word.word == word
                                    select _word).ToList();

                if (alreadyExists.Count() == 0)
                {
                    return null;
                }

                return alreadyExists.ElementAt(0);
            }
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}