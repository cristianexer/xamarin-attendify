
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Attendify.Properties
{
    [Activity(Label = "MenuActivity")]
    public class MenuActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.menu);
            TextView message = FindViewById<TextView>(Resource.Id.textView1);
            TextView status = FindViewById<TextView>(Resource.Id.status);
            Button register_attendance = FindViewById<Button>(Resource.Id.button1);
            Button show_attendance = FindViewById<Button>(Resource.Id.button2);

            if (Intent.HasExtra("status"))
            {
                status.Text = Intent.GetStringExtra("status");
            }
            else
            {
                status.Text = "";
            }

            message.Text = "Student ID: " + Intent.Extras.GetString("student_id");


            register_attendance.Click += (object sender, EventArgs e) => {

                Intent intent = new Intent(this, typeof(AttendActivity));
                intent.PutExtra("student_id", Intent.Extras.GetString("student_id"));
                intent.PutExtra("token", Intent.Extras.GetString("token"));
                StartActivity(intent);

            };
            show_attendance.Click += (object sender, EventArgs e) => {

                Intent intent = new Intent(this, typeof(StatusActivity));
                intent.PutExtra("student_id", Intent.Extras.GetString("student_id"));
                intent.PutExtra("token", Intent.Extras.GetString("token"));
                StartActivity(intent);
            };

            // Create your application here
        }
    }
}
