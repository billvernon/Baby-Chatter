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

namespace Baby_Chatter
{
    public partial class ChildrenUI : PhoneApplicationPage
    {
        #region Constructors

        public ChildrenUI()
        {

            InitializeComponent();

            this.DataContext = App.ViewModel;
        }

        #endregion

        #region Event Handlers

        private void ApplicationBarAddButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewChildUI.xaml", UriKind.Relative));
        }

        private void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LongListSelector selector = (LongListSelector)sender;

            if (selector.SelectedItem == null)
                return;

            // Since NavigationService.Navigate(object) does not seem to be available, need to set the current child in the view model and then load the child's word page which will
            // then load the current child and setup the page correctly from there
            App.ViewModel.CurrentChild = (Child)selector.SelectedItem;
            NavigationService.Navigate(new Uri("/WordUI.xaml", UriKind.Relative));

            // reset selector
            selector.SelectedItem = null;
        }

        private void EditChild_Click(object sender, RoutedEventArgs e)
        {
            App.ViewModel.SelectedChild = (Child)((MenuItem)sender).DataContext;

            // Since NavigationService.Navigate(object) does not seem to be available, need to set the current selected child in the view model and then load the child editing page which will
            // then load the currently selected child and setup the page correctly from there
            NavigationService.Navigate(new Uri("/ChildEditing.xaml", UriKind.Relative));
        }

        private void DeleteChild_Click(object sender, RoutedEventArgs e)
        {
            Child selectedChild = (Child)((MenuItem)sender).DataContext;

            if (selectedChild == null)
                return;

            App.ViewModel.RemoveChild(selectedChild.name);
        }

        #endregion
    }


}