using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using BabyChatterData;
using Microsoft.Phone.Tasks;
using Microsoft.Devices;
namespace Baby_Chatter
{
    public partial class ChildEditing : PhoneApplicationPage, INotifyPropertyChanged
    {
        bool BirthdateSet;
        bool NewPictureSet;     // use to determine if a different picture than the original has been set
  
        public ChildEditing()
        {
            // Need to get child via the GetChild method rather than just getting child from SelectedChild property because for some reason, 
            // when the child long list selector is updated, the most current version of the child is not loaded.
            Child child = App.ViewModel.GetChild(App.ViewModel.SelectedChild.name);
            ChildName = child.name;
            Birthdate = child.birthdate;
            Picture = Utilities.GetImageFromDisk(child.pictureURL);

            InitializeComponent();
            DataContext = this;

            BirthdateSet = false;
            NewPictureSet = false;

            PropertyChanged += ChildEditing_PropertyChanged;
        }

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
                _ChildName = value;
                NotifyPropertyChanged("ChildName");
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

        private void DatePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            // date picker may not be initialized when this method is called
            if (DatePicker == null)
            {
                return;
            }

            // check if there are any words said before the new date
            Child child = App.ViewModel.SelectedChild;

            var words = from Word w in child.Words
                        where w.date < (DateTime)DatePicker.Value
                        select w;

            if (words.Count() > 0)
            {
                MessageBox.Show("You cannot set a birthdate for your child that is later than the date of the earliest word your child has said.");
                DatePicker.Value = Birthdate;
            }
            else
            {
                Birthdate = (DateTime)DatePicker.Value;
            }
        }

        /*
         * Picture menu event handlers.
         */

        private void TakePictureButton_Click(object sender, RoutedEventArgs e)
        {
            PictureMenu.IsOpen = false;
            EnablePageInteraction();
            OpenPhotoChooserTask();
        }

        private void RemovePictureButton_Click(object sender, RoutedEventArgs e)
        {
            PictureMenu.IsOpen = false;
            EnablePageInteraction();
            Picture = null;
            PictureButton.Opacity = 1;
            PictureButton.IsHitTestVisible = true;
            NewPictureSet = true;
        }

        private void CancelPictureButton_Click(object sender, RoutedEventArgs e)
        {
            PictureMenu.IsOpen = false;
            EnablePageInteraction();
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
            Child child;

            if (NewPictureSet)
            {
                child = new Child()
                {
                    name = ChildName,
                    birthdate = Birthdate,
                    pictureURL = Utilities.SaveChildImageToDisk(ChildName, Picture)
                };
            }
            else
            {
                child = new Child()
                {
                    name = ChildName,
                    birthdate = Birthdate,
                    pictureURL = App.ViewModel.SelectedChild.pictureURL
                };
            }

            App.ViewModel.UpdateChild(child);

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
                NewPictureSet = true;
            }

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
            EnablePageInteraction();
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

        void ChildEditing_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Birthdate":
                    DateField.Text = Birthdate.ToShortDateString();
                    break;
                case "Picture":
                    ChildImageControl.Source = Picture;
                    break;
                default:
                    break;
            }
        }

        /*
         * Enable all controls on this page
         */
        private void EnablePageInteraction()
        {
            PictureButton.IsHitTestVisible = true;
            NameField.IsHitTestVisible = true;
            DatePicker.IsHitTestVisible = true;
        }

        /*
         * Disable all controls on this page.
         */
        private void DisablePageInteraction()
        {
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