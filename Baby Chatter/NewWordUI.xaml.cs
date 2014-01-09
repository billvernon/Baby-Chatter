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
    public partial class NewWordUI : PhoneApplicationPage, INotifyPropertyChanged
    {
        private bool DateSet;
        private bool DiaryEntryActive;
        private SolidColorBrush OriginalButtonTextColor;

        /*
         *  Needed because when the save button is pressed before the word field has lost focus, the word is saved
         *  and the word field lost focus method then  an error message to the user that the word has already been added
         *  even though the word field contains the word that was just entered
         */
        bool WordSaved;       

        public NewWordUI()
        {
            InitializeComponent();

            Date = DateTime.Today;
            DatePicker.Value = Date;
            DiaryEntryActive = false;
            WordSaved = false;

            this.DataContext = this;

            this.PropertyChanged += HandlePropertyChanged;
        }

        /*
        * Handle return from diary entry controller.
        */
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // if returning from the diary entry page
            if (DiaryEntryActive)
            {
                DiaryBox.Text = (string)PhoneApplicationService.Current.State["DiaryEntry"];
                DiaryEntryActive = false;
            }
        }

        #region Properties
        
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

        private BitmapImage _Picture;
        private BitmapImage Picture
        {
            get
            {
                return _Picture;
            }
            set
            {
                _Picture = value;
                NotifyPropertyChanged("Picture");
            }
        }

        public WriteableBitmap PictureThumb
        {
            get
            {
                if (Picture != null)
                {
                    return Utilities.GetThumbnail(Picture);
                }

                return null;
            }
        }


        #endregion

        #region Event Handlers

        /*
         * When Picture is changed, signal that PictureThumb is updated as well.
         */
        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Picture")
            {
                NotifyPropertyChanged("PictureThumb");
            }
        }

        /*
         * Word Box Event Handlers
         */
        private void WordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string word = WordBox.Text;

            if (word == "")
            {
                return;
            }

            // did the user already enter the word
            if (App.ViewModel.DoesWordExist(word) && WordSaved == false)
            {
                Word foundWord = App.ViewModel.GetWordForCurrentChild(word);
                MessageBox.Show(App.ViewModel.CurrentChild.name + " already said \"" + word + "\" on " + foundWord.date.ToShortDateString() + ".");
                WordBox.Text = "";
            }
        }

        /*
         * Date Picker Event Handlers
         */
        private void DatePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            // This method can be called when the DatePicker is being initialized.  The binding of value to date causes this method to be called.
            // So if DatePicker is still null, it is being initialized and ignore the rest of the code in this method.
            if (DatePicker == null)
            {
                return;
            }

            if (DatePicker.Value > DateTime.Today)
            {
                MessageBox.Show("You cannot set a date for this word that is later than today.");
                Date = DateTime.Today;
            }
            else if (DatePicker.Value < App.ViewModel.CurrentChild.birthdate)
            {
                MessageBox.Show("You cannot set a date for this word that occurred before your child was born.");
                Date = DateTime.Today;
            }
        }

        /*
         * Diary Box Event Handlers
         */

        private void DiaryBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            // setup the text to pass to the DiaryEntry page
            PhoneApplicationService.Current.State["DiaryEntry"] = DiaryBox.Text;
            DiaryEntryActive = true;
            NavigationService.Navigate(new Uri("/DiaryEntry.xaml", UriKind.Relative));
        }

        /*
         * Picture Button Event Handlers
         */
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
        }

        private void CancelPictureButton_Click(object sender, RoutedEventArgs e)
        {
            PictureMenu.IsOpen = false;
            EnablePageInteraction();
        }


        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            // check if the user has set a word
            if (WordBox.Text == "" || WordBox.Text == null)
            {
                MessageBox.Show("You need to enter a word before saving this entry.");
                WordBox.Text = "";
                return;
            }

            // does the word already exist
            if (App.ViewModel.DoesWordExist(WordBox.Text))
            {
                // dont' need an error message here as an error message will already be presented by the word field lost focus handler
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

            Word newWord = new Word(){
                           word = WordBox.Text,
                           date = (DateTime)DatePicker.Value,
                           diary = DiaryBox.Text,
                           pictureURL = Utilities.SavePhotoToDisk(Picture),
                           childName = App.ViewModel.CurrentChild.name
                            };

            App.ViewModel.AddNewWord(newWord);

            WordSaved = true;

            NavigationService.GoBack();
        }

        #endregion

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
    }   
}