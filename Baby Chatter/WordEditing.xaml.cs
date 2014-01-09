using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BabyChatterData;
using System.ComponentModel;
using Microsoft.Phone.Tasks;
using Microsoft.Devices;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Baby_Chatter
{
    public partial class WordEditing : PhoneApplicationPage
    {
        bool WordSaved;
        private SolidColorBrush OriginalButtonTextColor;
        private bool DiaryEntryActive;
        private bool PictureUpdated;

        public WordEditing()
        {
            // init fields
            Word word = App.ViewModel.SelectedWord;
            Picture = Utilities.GetImageFromDisk(word.pictureURL);
            Diary = word.diary;
            Date = word.date;
            _Word = word.word;

            PictureUpdated = false;

            InitializeComponent();
            DataContext = this;

            PropertyChanged += WordEditing_PropertyChanged;
        }

        #region Properties

        private BitmapImage _Picture;
        public BitmapImage Picture
        {
            get
            {
                return _Picture;
            }
            set
            {
                _Picture = value;
                NotifyPropertyChanged("Picture");
                NotifyPropertyChanged("PictureThumb");
            }
        }
        public ImageSource PictureThumb
        {
            get
            {
                return Utilities.GetThumbnail(Picture);
            }
        }

        private string _diary;
        public string Diary
        {
            get
            {
                return _diary;
            }
            set
            {
                _diary = value;
                NotifyPropertyChanged("Diary");
            }
        }

        private DateTime _Date;
        public DateTime Date
        {
            get
            {
                return _Date;
            }
            set
            {
                _Date = value;
                NotifyPropertyChanged("Date");
            }
        }

        private string _Word;
        public string Word
        {
            get
            {
                return _Word;
            }
        }

        #endregion

        /*
         * Handle return from diary entry controller.
         */
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // if returning from the diary entry page
            if (DiaryEntryActive)
            {
                Diary = (string)PhoneApplicationService.Current.State["DiaryEntry"];
                DiaryEntryActive = false;
            }
        }
        /*
         * Since Image does not implement INotifyPropertyChanged, PictureImage.Source needs to be updated manually.
         */
        void WordEditing_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "PictureThumb":
                    PictureImage.Source = PictureThumb;
                    break;
                case "Diary":
                    DiaryBox.Text = Diary;
                    break;
                case "Date":
                    DateField.Text = Date.ToShortDateString();
                    break;
            }
        }


        private void TakePictureButton_Click(object sender, RoutedEventArgs e)
        {
            PictureMenu.IsOpen = false;
            EnablePageInteraction();
            EnableApplicationBarButtonItems();
            OpenPhotoChooserTask();
        }

        private void RemovePictureButton_Click(object sender, RoutedEventArgs e)
        {
            PictureMenu.IsOpen = false;
            EnablePageInteraction();
            EnableApplicationBarButtonItems();
            Picture = null;
            PictureButton.Foreground = OriginalButtonTextColor;
            PictureButton.Content = "add \n picture";
            PictureUpdated = true;
        }

        private void CancelPictureButton_Click(object sender, RoutedEventArgs e)
        {
            PictureMenu.IsOpen = false;
            EnablePageInteraction();
        }

        private void DatePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            DatePicker picker = (DatePicker)sender;

            // avoid this event handler on page initialization.  When the page is initialized, the date picker's value will be null
            if (picker.Value == null)
            {
                return;
            }

            DateTime date = (DateTime)picker.Value;

            if (date > DateTime.Today)
            {
                MessageBox.Show("You cannot enter a date for a word that is later than today.");
            }
            else
            {
                this.Date = date;
            }
        }

        private void DiaryBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            // setup the text to pass to the DiaryEntry page
            PhoneApplicationService.Current.State["DiaryEntry"] = DiaryBox.Text;
            DiaryEntryActive = true;
            NavigationService.Navigate(new Uri("/DiaryEntry.xaml", UriKind.Relative));
        }

        private void PictureButton_Click(object sender, RoutedEventArgs e)
        {
            DisablePageInteraction();

            // if picture is not set, we only need to open the photo chooser task
            if (Picture == null)
            {
                OpenPhotoChooserTask();
            }
            else
            {
                PictureMenu.IsOpen = true;
            }
        }

        #region Helper Methods

        public void EnablePageInteraction()
        {
            EnableApplicationBarButtonItems();
            WordBox.IsEnabled = true;
            DatePicker.IsEnabled = true;
            DiaryBox.IsEnabled = true;
            PictureButton.IsEnabled = true;
        }

        public void DisablePageInteraction()
        {
            DisableApplicationBarButtonItems();
            WordBox.IsEnabled = false;
            DatePicker.IsEnabled = false;
            DiaryBox.IsEnabled = false;
            PictureButton.IsEnabled = false;
        }

        public void EnableApplicationBarButtonItems()
        {
            ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = true;
        }

        public void DisableApplicationBarButtonItems()
        {
            ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = false;
        }

        private void OpenPhotoChooserTask()
        {
            PhotoChooserTask photoTask = new PhotoChooserTask();
            photoTask.Completed += PhotoTask_Completed;

            // show camera button in photo chooser
            // check if the device has a camera
            // if it does, set the camera button
            try
            {
                photoTask.ShowCamera = Camera.IsCameraTypeSupported(CameraType.Primary);
            }
            // catch the unauthorized access exception that seems to come up in the simulator
            catch (System.UnauthorizedAccessException exception)
            {
                photoTask.ShowCamera = false;
            }

            photoTask.Show();
        }

        void PhotoTask_Completed(object sender, PhotoResult e)
        {
            // set up child's photo in the image control if an image was selected
            if (e.ChosenPhoto != null)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.SetSource(e.ChosenPhoto);
                Picture = bitmap;
                PictureButton.Content = "change \n picture";
                OriginalButtonTextColor = (SolidColorBrush)PictureButton.Foreground;
                PictureButton.Foreground = new SolidColorBrush(Colors.White);
                PictureUpdated = true;
            }

            EnableApplicationBarButtonItems();
            EnablePageInteraction();
        }

        private void OpenCameraCaptureTask()
        {
            CameraCaptureTask cameraTask = new CameraCaptureTask();
            cameraTask.Completed += cameraTask_Completed;
            cameraTask.Show();
        }

        void cameraTask_Completed(object sender, PhotoResult e)
        {
            EnableApplicationBarButtonItems();
            EnablePageInteraction();
        }

        private void ShowPictureMenu()
        {
            DisablePageInteraction();

            // reset buttons
            PictureFromCameraButton.Visibility = System.Windows.Visibility.Visible;
            RemovePictureButton.Visibility = System.Windows.Visibility.Visible;

            /* try block needed here to capture System.UnauthorzedAccessException
               for now, until I have a device to test with, I'll assume this means a camera isn't available
               and just disable the button
             */
            try
            {
                if (!PhotoCamera.IsCameraTypeSupported(CameraType.Primary))
                {
                    PictureFromCameraButton.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            catch (System.UnauthorizedAccessException e)
            {
                PictureFromCameraButton.Visibility = System.Windows.Visibility.Collapsed;
            }

            if (Picture == null)
            {
                RemovePictureButton.Visibility = System.Windows.Visibility.Collapsed;
            }

            PictureMenu.IsOpen = true;
        }

        #endregion

        #region INotifyPropertyChanged Interface Implementation

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

        /*
         * Validate the word fields and save the word to the database for the current child.
         */
        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            // check if the user has set a word
            if (WordBox.Text == "" || WordBox.Text == null)
            {
                MessageBox.Show("You need to enter a word before saving this entry.");
                WordBox.Text = "";
                return;
            }

            // is the date set before the child was born
            if (DatePicker.Value < App.ViewModel.CurrentChild.birthdate)
            {
                MessageBox.Show("You cannot set a date for the new word that is earlier than when your child was born.");
                DatePicker.Value = DateTime.Today;
                return;
            }

            // is the date set after today
            if (DatePicker.Value > DateTime.Today)
            {
                MessageBox.Show("You cannot set a date for the new word that is later than today.");
                DatePicker.Value = DateTime.Today;
                return;
            }

            Word newWord;

            if (PictureUpdated)
            {
                newWord = new Word()
                {
                    word = Word,
                    date = Date,
                    diary = Diary,
                    pictureURL = Utilities.SavePhotoToDisk(Picture),
                    childName = App.ViewModel.CurrentChild.name
                };
            }
            else
            {
                newWord = new Word()
                {
                    word = Word,
                    date = Date,
                    diary = Diary,
                    pictureURL = App.ViewModel.SelectedWord.pictureURL,
                    childName = App.ViewModel.CurrentChild.name
                };
            }

            App.ViewModel.UpdateWord(newWord);

            WordSaved = true;

            NavigationService.GoBack();
        }
    }
}