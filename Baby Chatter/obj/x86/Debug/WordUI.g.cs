﻿#pragma checksum "C:\Users\William\Documents\Visual Studio 2012\Projects\Baby Chatter\Baby Chatter\WordUI.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "EDC161EAE1B90CE46CAE2607B240E0B3"
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
    
    
    public partial class WordUI : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Baby_Chatter.WordCountConverter WordCountConverter;
        
        internal Baby_Chatter.DateTimeOrStringToStringConverter DateTimeOrStringToStringConverter;
        
        internal System.Windows.Controls.Image ChildImage;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Baby%20Chatter;component/WordUI.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.WordCountConverter = ((Baby_Chatter.WordCountConverter)(this.FindName("WordCountConverter")));
            this.DateTimeOrStringToStringConverter = ((Baby_Chatter.DateTimeOrStringToStringConverter)(this.FindName("DateTimeOrStringToStringConverter")));
            this.ChildImage = ((System.Windows.Controls.Image)(this.FindName("ChildImage")));
        }
    }
}

