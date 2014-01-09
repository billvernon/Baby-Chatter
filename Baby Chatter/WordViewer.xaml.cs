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
using System.Windows.Media.Imaging;

namespace Baby_Chatter
{
    public partial class WordViewer : PhoneApplicationPage
    {
        public WordViewer()
        {
            InitializeComponent();
            this.DataContext = App.ViewModel;
            SetupView();
        }

        /*
         * Setup the view.  Set fields visibility based on whether a particular Word attribute is set.
         */
        private void SetupView()
        {
            Word word = App.ViewModel.SelectedWord;

            if (string.IsNullOrEmpty(word.diary))
            {
                DiaryContainer.Visibility = Visibility.Collapsed;
            }

            if (string.IsNullOrEmpty(word.pictureURL))
            {
                PictureContainer.Visibility = Visibility.Collapsed;
            }
        }

        private void DiaryBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }
    }
}