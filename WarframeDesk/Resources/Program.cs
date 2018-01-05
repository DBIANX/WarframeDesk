﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace WarframeDesk.Resources
{
    public struct Alert
    {
        public string ID;
        public string Title;
        public string Description;
        public string Start_Date;
        public string Faction;
        public string Expiry_Date;
    }

    public struct Invasion
    {
        public string ID;
        public string Title;
        public string Start_Date;
    }

    public struct Outbreak
    {
        public string ID;
        public string Title;
        public string Start_Date;
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
       
    }
}