 using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Devices;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using Microsoft.Phone.Tasks;
using BabyChatterData;
using Microsoft.Phone.Tasks;

namespace Baby_Chatter
{
    public partial class NewChildUI : PhoneApplicationPage, INotifyPropertyChanged
    {
        private Microsoft.Phone.Shell.ApplicationBarIconButton SaveButton;
        private bool BirthdateSet;

        #region Constructors

        public NewChildUI()
        {
            // setup initial property values
            Birthdate = DateTime.Today;

            // BirthdateString should be set to "date" in order to signify what the field is for when the page is first loaded
            BirthdateString = "birthdate";
           
            InitializeComponent();
            DataContext = this;

            SaveButton = (Microsoft.Phone.Shell.ApplicationBarIconButton)ApplicationBar.Buttons[0];

            BirthdateSet = false;

            // initialize to true so that the date picker is active upon page loading
            // this is a workaround of a bug further described in the property description
            DatePicker.IsHitTestVisible = true;
        }

        #endregion

        #region Properties

        private string _ChildName;

        public string ChildName
        {
            get
            {
                return _ChildName;
            }
            set
            {
                // if value is set and the value is not the initial value for the field
                if (value != null && value != "name")
                {
                    _ChildName = Utilities.CapitalizeString(value);
                    NotifyPropertyChanged("ChildName");
                }
            }
        }

        private DateTime _Birthdate;

        public DateTime Birthdate
        {
            get
            {
                return _Birthdate;
            }
            set
            {
                _Birthdate = value;
                NotifyPropertyChanged("Birthdate");
                
                // update BirthdateString here so the birthdate text field, which is bound to BirthdateString, gets updated
                BirthdateString = value.ToShortDateString();
            }
        }

        // used in order to update to update the birthdate text field
        private string _BirthdateString;

        public string BirthdateString
        {
            get
            {
                return _BirthdateString;
            }
            set
            {
                _BirthdateString = value;
                NotifyPropertyChanged("BirthdateString");
            }
        }

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
            }
        }

        #endregion

        #region Event Handlers

        /*
         * Name text block event handlers
         */
        private void NameField_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NameField.Text == "name")
            {
                NameField.Text = "";
            }
        }

        private void NameField_LostFocus(object sender, RoutedEventArgs e)
        {
            if (NameField.Text == "" || NameField.Text == null)
            {
                NameField.Text = "name";
                return;
            }

            if (App.ViewModel.DoesChildExist(NameField.Text))
            {
                MessageBox.Show("You have already entered a child named " + ChildName + ".");
                return;
            }

            ChildName = NameField.Text;
        }

        /*
         * DatePicker event handlers
         */
        private void DatePicker_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            // DatePicker does not update Birthdate if the value doesn't change so check if the birthdate field still displays "birthdate".
            // If it does, then when the date picker is tapped, set the birthdate to show the current value on the date picker
            if (DateField.Text == "birthdate")
            {
                BirthdateString = Birthdate.ToShortDateString();
            }

            BirthdateSet = true;

        }

        private void DatePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {            
            // When loading page, DatePicker is null.  This function is called as the binding to the birthdate field happens to initialize date picker.  However,
            // DatePicker is not initialized yet and the next bit of code attempts to pull the date value from DatePicker, which is still null.  Therefore, identify 
            // when DatePicker is initializing and just return.
            if (DatePicker == null)
            {
                return;
            }

            if (DatePicker.Value > DateTime.Today)
            {
                MessageBox.Show("You cannot set a birthdate for your child that is later than today.");

                Birthdate = DateTime.Today;
            }
        }

        /*
         * Picture menu event handlers.
         */

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
            PictureButton.Opacity = 1;
            PictureButton.IsHitTestVisible = true;
        }

        private void CancelPictureButton_Click(object sender, RoutedEventArgs e)
        {
            PictureMenu.IsOpen = false;
            EnablePageInteraction();
            EnableApplicationBarButtonItems();
        }

        private void PictureButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            // if picture is not set, we only need to show the photo chooser task
            if (Picture == null)
            {
                OpenPhotoChooserTask();
            }
            else
            {
                ShowPictureMenu();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (NameField.Text == "" || NameField.Text == "name" || NameField.Text == null)
            {
                MessageBox.Show("You need to enter a name for your child before you can save your child.");
                return;
            }
            else
            {
                // it's possible ChildName may not be set yet if the user hasn't taken the focus off of NameField
                ChildName = NameField.Text;
            }

            // if date has not been set yet
            if (!BirthdateSet)
            {
                MessageBox.Show("You need to enter a birthdate for your child before you can save your child.");
                return;
            }

            string pictureURL = Utilities.SaveChildImageToDisk(ChildName, Picture);

            Child child = new Child()
            {
                name = this.ChildName,
                birthdate = this.Birthdate,
                pictureURL = pictureURL
            };

            App.ViewModel.AddNewChild(child);

            NavigationService.GoBack();
        }   
    
        #endregion
         
        #region Helper Methods

        private void OpenPhotoChooserTask()
        {
            PhotoChooserTask photoTask = new PhotoChooserTask();
            photoTask.ShowCamera = true;
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

            // setup crop box size to match the image box size
            photoTask.PixelHeight = (int)ChildImageControl.Height;
            photoTask.PixelWidth = (int)ChildImageControl.Width;


            photoTask.Show();
        }

        private void PhotoTask_Completed(object sender, PhotoResult e)
        {
            // set up child's photo in the image control if an image was selected
            if (e.ChosenPhoto != null)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.SetSource(e.ChosenPhoto);
                Picture = bitmap;
                PictureButton.Opacity = 0;
                PictureButton.IsHitTestVisible = false;
                
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

        private void cameraTask_Completed(object sender, PhotoResult e)
        {
            EnableApplicationBarButtonItems();
            EnablePageInteraction();
        }

        private void EnableApplicationBarButtonItems()
        {
            SaveButton.IsEnabled = true;
        }

        private void DisableApplicationBarButtonItems()
        {
            SaveButton.IsEnabled = false;
        }

        private void ShowPictureMenu()
        {
            DisablePageInteraction();

            if (Picture == null)
            {
                RemovePictureButton.Visibility = System.Windows.Visibility.Collapsed;
            }

            PictureMenu.IsOpen = true;
        }

        private void PictureFromCameraButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PictureFromAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            OpenCameraCaptureTask();
        }

        /*
         * Enable all controls on this page
         */
        private void EnablePageInteraction()
        {
            EnableApplicationBarButtonItems();
            PictureButton.IsHitTestVisible = true;
            NameField.IsHitTestVisible = true;
            DatePicker.IsHitTestVisible = true;
        }

        /*
         * Disable all controls on this page.
         */
        private void DisablePageInteraction()
        {
            DisableApplicationBarButtonItems();
            PictureButton.IsHitTestVisible = false;
            NameField.IsHitTestVisible = false;
            DatePicker.IsHitTestVisible = false;
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