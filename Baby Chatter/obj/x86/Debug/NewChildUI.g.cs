﻿#pragma checksum "C:\Users\William\Documents\Visual Studio 2012\Projects\Baby Chatter\Baby Chatter\NewChildUI.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "BDBD9333716DFB27649405A8E4048403"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
    
    
    public partial class NewChildUI : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Primitives.Popup PictureMenu;
        
        internal System.Windows.Controls.StackPanel PicturePopupStackPanel;
        
        internal System.Windows.Controls.Button PictureFromCameraButton;
        
        internal System.Windows.Controls.Button RemovePictureButton;
        
        internal System.Windows.Controls.Button CancelPictureButton;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.Image ChildImageControl;
        
        internal System.Windows.Controls.Button PictureButton;
        
        internal System.Windows.Controls.TextBox NameField;
        
        internal Microsoft.Phone.Controls.DatePicker DatePicker;
        
        internal System.Windows.Controls.TextBlock DateField;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Baby%20Chatter;component/NewChildUI.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.PictureMenu = ((System.Windows.Controls.Primitives.Popup)(this.FindName("PictureMenu")));
            this.PicturePopupStackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("PicturePopupStackPanel")));
            this.PictureFromCameraButton = ((System.Windows.Controls.Button)(this.FindName("PictureFromCameraButton")));
            this.RemovePictureButton = ((System.Windows.Controls.Button)(this.FindName("RemovePictureButton")));
            this.CancelPictureButton = ((System.Windows.Controls.Button)(this.FindName("CancelPictureButton")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.ChildImageControl = ((System.Windows.Controls.Image)(this.FindName("ChildImageControl")));
            this.PictureButton = ((System.Windows.Controls.Button)(this.FindName("PictureButton")));
            this.NameField = ((System.Windows.Controls.TextBox)(this.FindName("NameField")));
            this.DatePicker = ((Microsoft.Phone.Controls.DatePicker)(this.FindName("DatePicker")));
            this.DateField = ((System.Windows.Controls.TextBlock)(this.FindName("DateField")));
        }
    }
}

