﻿#pragma checksum "C:\Users\William\Documents\Visual Studio 2012\Projects\Baby Chatter\Baby Chatter\Copy of NewWordUI.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FCD433E5B3D9E24041257AC31F5FBBB3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Baby_Chatter;
using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Baby_Chatter {
    
    
    public partial class NewWordUI : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Primitives.Popup PictureMenu;
        
        internal System.Windows.Controls.StackPanel PicturePopupStackPanel;
        
        internal System.Windows.Controls.Button PictureFromCameraButton;
        
        internal System.Windows.Controls.Button RemovePictureButton;
        
        internal System.Windows.Controls.Button CancelPictureButton;
        
        internal System.Windows.Controls.Primitives.Popup MovieMenu;
        
        internal System.Windows.Controls.StackPanel MoviePopupStackPanel;
        
        internal System.Windows.Controls.Button MovieFromCameraButton;
        
        internal System.Windows.Controls.Button MovieFromAlbumButton;
        
        internal System.Windows.Controls.Button RemoveMovieButton;
        
        internal System.Windows.Controls.Button CancelMovieButton;
        
        internal System.Windows.Controls.TextBox WordBox;
        
        internal Baby_Chatter.DateTimeToStringConverter DateTimeToStringConverter;
        
        internal Microsoft.Phone.Controls.DatePicker DatePicker;
        
        internal System.Windows.Controls.TextBlock DateField;
        
        internal System.Windows.Controls.TextBox DiaryBox;
        
        internal System.Windows.Controls.CheckBox FacebookCheckBox;
        
        internal System.Windows.Controls.Button PictureButton;
        
        internal System.Windows.Controls.Image PictureImage;
        
        internal System.Windows.Controls.Button MovieButton;
        
        internal System.Windows.Controls.Image MovieImage;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Baby%20Chatter;component/Copy%20of%20NewWordUI.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.PictureMenu = ((System.Windows.Controls.Primitives.Popup)(this.FindName("PictureMenu")));
            this.PicturePopupStackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("PicturePopupStackPanel")));
            this.PictureFromCameraButton = ((System.Windows.Controls.Button)(this.FindName("PictureFromCameraButton")));
            this.RemovePictureButton = ((System.Windows.Controls.Button)(this.FindName("RemovePictureButton")));
            this.CancelPictureButton = ((System.Windows.Controls.Button)(this.FindName("CancelPictureButton")));
            this.MovieMenu = ((System.Windows.Controls.Primitives.Popup)(this.FindName("MovieMenu")));
            this.MoviePopupStackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("MoviePopupStackPanel")));
            this.MovieFromCameraButton = ((System.Windows.Controls.Button)(this.FindName("MovieFromCameraButton")));
            this.MovieFromAlbumButton = ((System.Windows.Controls.Button)(this.FindName("MovieFromAlbumButton")));
            this.RemoveMovieButton = ((System.Windows.Controls.Button)(this.FindName("RemoveMovieButton")));
            this.CancelMovieButton = ((System.Windows.Controls.Button)(this.FindName("CancelMovieButton")));
            this.WordBox = ((System.Windows.Controls.TextBox)(this.FindName("WordBox")));
            this.DateTimeToStringConverter = ((Baby_Chatter.DateTimeToStringConverter)(this.FindName("DateTimeToStringConverter")));
            this.DatePicker = ((Microsoft.Phone.Controls.DatePicker)(this.FindName("DatePicker")));
            this.DateField = ((System.Windows.Controls.TextBlock)(this.FindName("DateField")));
            this.DiaryBox = ((System.Windows.Controls.TextBox)(this.FindName("DiaryBox")));
            this.FacebookCheckBox = ((System.Windows.Controls.CheckBox)(this.FindName("FacebookCheckBox")));
            this.PictureButton = ((System.Windows.Controls.Button)(this.FindName("PictureButton")));
            this.PictureImage = ((System.Windows.Controls.Image)(this.FindName("PictureImage")));
            this.MovieButton = ((System.Windows.Controls.Button)(this.FindName("MovieButton")));
            this.MovieImage = ((System.Windows.Controls.Image)(this.FindName("MovieImage")));
        }
    }
}
