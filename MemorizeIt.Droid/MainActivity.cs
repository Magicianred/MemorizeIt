using System;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FileMemoryStorage;
using GoogleMemorySupplier;
using MemorizeIt.MemorySourceSupplier;
using MemorizeIt.MemoryStorage;

namespace MemorizeIt.Droid
{
    [Activity(Label = "MemorizeIt.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            store = new FileSystemMemoryStorage();
            UpdateMemoryTableUI();
            btnUpdate.Click += btnUpdate_Click;
        }

        private void UpdateMemoryTableUI()
        {
            tlData.RemoveAllViews();
            foreach (var memoryItem in store.Items)
            {
                TableRow tr = new TableRow(this);
                var rowColor = Color.White;
                tr.SetBackgroundColor(rowColor);

                var cellColor = Color.Black;
                var txtVal1 = new TextView(this) {Text = memoryItem.Values[0]};
                txtVal1.SetPadding(1, 1, 1, 1);
                tr.AddView(txtVal1);
                txtVal1.SetBackgroundColor(cellColor);
                var txtVal2 = new TextView(this) {Text = memoryItem.Values[1]};
                txtVal2.SetPadding(1, 1, 1, 1);
                txtVal2.SetBackgroundColor(cellColor);
                tr.AddView(txtVal2);
                tlData.AddView(tr);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateMemoryTableUI();
        }

        protected TableLayout tlData
        {
            get { return FindViewById<TableLayout>(Resource.Id.tlData); }
        }

        protected Button btnUpdate
        {
            get { return this.FindViewById<Button>(Resource.Id.btnUpdate); }
        }

        private IMemoryStorage store;
    }
}

