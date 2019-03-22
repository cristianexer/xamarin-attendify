
using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Attendify
{
    [Activity(Label = "StatusActivity")]
    public class StatusActivity : Activity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.status_layout);

            ListView att_list = FindViewById<ListView>(Resource.Id.attendanceList);

            string student_id = Intent.Extras.GetString("student_id");
            string token = Intent.Extras.GetString("token");

            JsonValue res = await Task.Run(()=> Helper.GetAttendance(token, student_id));

 
            if (res["response"] == "Success")
            {
                att_list.Adapter = new ArrayAdapter<string>(this, Resource.Layout.attendance_item_layout, Resource.Id.textView1);
                ArrayAdapter<string> adapter = (ArrayAdapter<string>)FindViewById<ListView>(Resource.Id.attendanceList).Adapter;
                foreach (JsonValue att in res["attendance"])
                {
                    adapter.Add($"{att["building_name"]}{att["room_id"]} - {att["created_at"]}");
                }

            }
           
        }
    }
}
