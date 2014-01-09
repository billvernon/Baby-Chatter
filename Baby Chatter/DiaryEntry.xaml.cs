using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Baby_Chatter
{
    public partial class DiaryEntry : PhoneApplicationPage
    {
        public DiaryEntry()
        {
            InitializeComponent();
        }

        private void ApplicationBarCancelButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            // check if text needs to be loaded in order to mirror the DiaryEntry field of NewWordUI
            if ((string)PhoneApplicationService.Current.State["DiaryEntry"] != "" &&
                (string)PhoneApplicationService.Current.State["DiaryEntry"] != null)
            {
                DiaryText.Text = (string)PhoneApplicationService.Current.State["DiaryEntry"];
            }
        }

        private void ApplicationBarSaveButton_Click(object sender, EventArgs e)
        {
            // setup a dictionary entry to give NewWord access to the diary text
            PhoneApplicationService.Current.State["DiaryEntry"] = DiaryText.Text;

            NavigationService.GoBack(); 
        }

        private void DiaryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (DiaryText.Text != "")
            {
                // enable save button
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = true;
            }
            else
            {
                // disable save button
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = false;
            }
        }
    }

}