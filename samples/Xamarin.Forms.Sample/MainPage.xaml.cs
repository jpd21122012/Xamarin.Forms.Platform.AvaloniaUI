﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Forms.Sample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        
        async void OnButtonClicked(object sender, EventArgs args)
        {
            Console.WriteLine("test");
            Application.Current.Quit();
        }
    }
}
