
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Nfc;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Attendify.Properties;

namespace Attendify
{
    [Activity(Label = "AttendActivity")]
    public class AttendActivity : Activity, NfcAdapter.ICreateNdefMessageCallback, NfcAdapter.IOnNdefPushCompleteCallback
    {
        private NfcAdapter _nfcAdapter;
        private string token;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.attend_layout);

            token = Intent.Extras.GetString("token");
            _nfcAdapter = NfcAdapter.GetDefaultAdapter(this);

            if (_nfcAdapter == null)
            {
                Intent intent = new Intent(this, typeof(NoNFCActivity));
                StartActivity(intent);
            }
            else
            {
                NfcAdapter adapter = NfcAdapter.GetDefaultAdapter(this);
                adapter.SetNdefPushMessageCallback(this, this);
                adapter.SetOnNdefPushCompleteCallback(this, this);
            }
        }
            
        public NdefMessage CreateNdefMessage(NfcEvent e)
        {
           
            NdefRecord txt = NdefRecord.CreateTextRecord("token",token);
            NdefMessage message = new NdefMessage(new[] { txt });
            return message;
        }

        public void OnNdefPushComplete(NfcEvent e)
        {

            Intent intent = new Intent(this, typeof(MenuActivity));
            intent.PutExtra("status","Attendance registerd");
            StartActivity(intent);
        }
    }
}

