using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using WarframeDesk.Resources;
using System.Collections.Generic;
using System;


namespace WarframeDesk
{
    [Activity(Label = "WarframeDesk", MainLauncher = true, Theme = "@style/Tema1")]
    public class MainActivity : Activity
    {
        
        //alerts items
        List<string> items;
        Android.Widget.ListView lvAlerts;
        ArrayAdapter adapter;

        //invasion items
        List<string> invItems;
        ListView lvInvasions;
        ArrayAdapter adapterInv;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbarMain);
            SetActionBar(toolbar);
            ActionBar.Title = "Tenno";

            //listview alerts
            items = new List<string>();
            lvAlerts = FindViewById<Android.Widget.ListView>(Resource.Id.AlertData);
            adapter = new ArrayAdapter(this, Resource.Layout.TextViewItem,items);
            

            //listview invasions
            invItems = new List<string>();
            lvInvasions = FindViewById<ListView>(Resource.Id.AlertData);
            adapterInv = new ArrayAdapter(this, Resource.Layout.TextViewItem, invItems);
            lvInvasions.Adapter = adapter;

            lvAlerts.Adapter = adapterInv;
            WF_Update();
        }

        

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
                ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);
        }
        
        public void WF_Update()
        {
            var wf = new WarframeHandler();

            var alerts = new List<Alert>();
            var invasions = new List<Invasion>();
            var outbreaks = new List<Outbreak>();

            var status = "";
            var response = wf.GetXml(ref status);

            if (status != "OK")
            {
                var message = "Network not responding" + '\n';
                message = message + response;

                
                return;
            }

            wf.GetObjects(response, ref alerts, ref invasions, ref outbreaks);

            

            
            
            //InvasionData.Items.Clear();
            //_idList.Clear();
           

            //AlertData.Items.Clear();
            //InvasionData.Items.Clear();
            //_idList.Clear();   

            for (var i = 0; i < alerts.Count; i++)
            {
                var eTime = Convert.ToDateTime(alerts[i].Expiry_Date);

                var title = alerts[i].Title;
                var titleSp = title.Split('-');
                var place = "";
                

                if(titleSp[1].Contains("("))
                {
                    title = "Botín: " + titleSp[0];
                    place = "Ubicación: " + titleSp[1].TrimStart();
                }
                else
                {
                    title = "Botín: " + titleSp[0] + " + " + titleSp[1];
                    place = "Ubicación: " + titleSp[2];
                }
                
                var description = alerts[i].Description;
               


                var faction = alerts[i].Faction;
                var aId = alerts[i].ID;

                var aSpan = eTime.Subtract(DateTime.Now);
                var aLeft = "";

                if (aSpan.Days != 0)
                {
                    aLeft = aLeft + aSpan.Days + " días ";
                }

                if (aSpan.Hours != 0)
                {
                    aLeft = aLeft + aSpan.Hours + " horas ";
                }

                if (aSpan.Minutes != 0)
                {
                    aLeft = aLeft + aSpan.Minutes + " minutos ";
                }

                aLeft = aLeft + aSpan.Seconds + " segundos restantes";

                items.Clear();

                string[] row = { description, title, faction, aLeft };

                string linea = description + System.Environment.NewLine + title + System.Environment.NewLine + place + System.Environment.NewLine + faction + System.Environment.NewLine + aLeft + System.Environment.NewLine;
                items.Add(linea);

                adapter.Add(linea);
                adapter.NotifyDataSetChanged();
            }
            
            for (var i = 0; i < invasions.Count; i++)
            {
                var title = invasions[i].Title;
                var invId = invasions[i].ID;

                var sTime = Convert.ToDateTime(invasions[i].Start_Date);
                var now = DateTime.Now;
                var span = now.Subtract(sTime);

                var time = "";

                if (span.Hours != 0)
                {
                    time = time + span.Hours + " Hours ";
                }

                time = time + span.Minutes + " Minutes Ago";


                
                char[] charsToTrim = { 'V', 'S', '.'};
                string[] words = title.Split();
                string newTitulo = "";
                int flag = 0;
                foreach (string word in words)
                {
                    if(flag==1)
                    {
                        if ((word.TrimStart(charsToTrim) == "Grineer") || (word.TrimStart(charsToTrim) == "Corpus"))
                        {
                            newTitulo = newTitulo + System.Environment.NewLine;
                        }
                        if (word.TrimStart(charsToTrim) == "-")
                        {
                            newTitulo = newTitulo + System.Environment.NewLine;
                            
                        }
                    }
                    
                    if (word.TrimStart(charsToTrim) != "-")
                    {
                        newTitulo = newTitulo + word.TrimStart(charsToTrim) + " ";
                    }
                    
                    flag = 1;
                }
                    


                string linea = "Invasión" + System.Environment.NewLine + newTitulo + " " + System.Environment.NewLine + time;

                invItems.Add(linea);
                adapterInv.Add(linea);
                adapterInv.NotifyDataSetChanged();

                

                


            }
            /*
            for (var i = 0; i < outbreaks.Count; i++)
            {
                var title = outbreaks[i].Title;
                var oId = outbreaks[i].ID;

                var sTime = Convert.ToDateTime(outbreaks[i].Start_Date);
                var now = DateTime.Now;
                var oSpan = now.Subtract(sTime);

                var oTime = "";

                if (oSpan.Hours != 0)
                {
                    oTime = oTime + oSpan.Hours + " Hours ";
                }

                oTime = oTime + oSpan.Minutes + " Minutes Ago";

                _idList.Add(oId);
                string[] row = { title, "Outbreak", oTime };
                var listViewItem = new ListViewItem(row);
                //InvasionData.Items.Add(listViewItem);
                Invoke(new Action(() => InvasionData.Items.Add(listViewItem)));
            }
            */
            //AlertData.Scrollable = AlertData.Items.Count != 3;
            //InvasionData.Scrollable = InvasionData.Items.Count != 3;
            /*
            Invoke(new Action(() =>
            {
                AlertData.Scrollable = AlertData.Items.Count != 3;
                InvasionData.Scrollable = InvasionData.Items.Count != 3;
                InvasionData.Columns[0].Width = InvasionData.Items.Count > 3 ? 627 : 644;
                AlertData.Columns[3].Width = AlertData.Items.Count > 3 ? 235 : 252;
            }));
            */
            //InvasionData.Columns[0].Width = InvasionData.Items.Count > 3 ? 627 : 644;
            //AlertData.Columns[3].Width = AlertData.Items.Count > 3 ? 235 : 252;
        }

        public bool GameDetection { get; set; } = true;
    }

}

