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
using Baby_Chatter;

namespace Baby_Chatter
{
    public partial class WordUI : PhoneApplicationPage
    {
        public WordUI()
        {
            InitializeComponent();

            this.DataContext = App.ViewModel;
        }

        #region Event Handlers

        private void ApplicationBarAddIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewWordUI.xaml", UriKind.Relative));
        }

        private void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LongListSelector selector = (LongListSelector)sender;

            if (selector.SelectedItem == null)
                return;

            // Since NavigationService.Navigate(object) does not seem to be available, need to set the current word selected in the view model and then 
            // load the  word page which will then load that word
            App.ViewModel.SelectedWord = (Word)selector.SelectedItem;
            NavigationService.Navigate(new Uri("/WordViewer.xaml", UriKind.Relative));

            // reset selector
            selector.SelectedItem = null;
        }

        #endregion

        private void EditWord_Click(object sender, RoutedEventArgs e)
        {
            Word selectedWord = (Word)((MenuItem)sender).DataContext;

            if (selectedWord == null)
                return;

            // Since NavigationService.Navigate(object) does not seem to be available, need to set the current selected word in the view model and then load the word editing page which will
            // then load the currently selected word and setup the page correctly from there
            App.ViewModel.SelectedWord = selectedWord;
            NavigationService.Navigate(new Uri("/WordEditing.xaml", UriKind.Relative));
        }

        private void DeleteWord_Click(object sender, RoutedEventArgs e)
        {
            Word selectedWord = (Word)((MenuItem)sender).DataContext;

            if (selectedWord == null)
                return;

            App.ViewModel.RemoveWord(selectedWord.word);
        }

        private void ApplicationBarMenuItemSortAlphabetical_Click(object sender, EventArgs e)
        {
            App.ViewModel.WordGroupingType = ViewModels.ApplicationViewModel.GroupingType.GroupAlphabetic;
        }

        private void ApplicationBarMenuItemSortByDate_Click(object sender, EventArgs e)
        {
            App.ViewModel.WordGroupingType = ViewModels.ApplicationViewModel.GroupingType.GroupByDate;

        }

        private void ApplicationBarMenuItemSortByAge_Click(object sender, EventArgs e)
        {
            App.ViewModel.WordGroupingType = ViewModels.ApplicationViewModel.GroupingType.GroupByDate;
        }

        private void AppliciationBarMenuItemEmailWords_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (ChildImage.Source == null)
            {
                ChildImage.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}