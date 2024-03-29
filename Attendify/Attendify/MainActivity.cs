﻿using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Android.Content;
using System.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Attendify
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);


            //ImageView logo = FindViewById<ImageView>(Resource.Id.logo);
            //logo.SetImageURI(Android.Net.Uri.Parse(Helper.logoPath));

            TextView emailLabel = FindViewById<TextView>(Resource.Id.emailLabel);
            TextView passwordLabel = FindViewById<TextView>(Resource.Id.passwordLabel);
            TextView errorLabel = FindViewById<TextView>(Resource.Id.errorLabel);
            EditText emailInput = FindViewById<EditText>(Resource.Id.emailInput);
            EditText passwordInput = FindViewById<EditText>(Resource.Id.passwordInput);
            Button loginButton = FindViewById<Button>(Resource.Id.login);


            loginButton.Click += async (object sender, EventArgs e) =>
            {
                errorLabel.Text = "...processing...";
                bool valid = true;
                if (emailInput.Text == "" || passwordInput.Text == "")
                {
                    errorLabel.Text = "No Email or Password";
                    valid = false;
                }

                if (valid)
                {

                    JsonValue res = await Task.Run(() => Helper.Login(emailInput.Text, passwordInput.Text));

                    errorLabel.Text = res["response"];

                    if(res["response"] == "Success")
                    {
                        string token = res["token"];
                        JsonValue user = await Helper.parseJwtAsync(token);
                        string student_id = user["student_id"];
                        emailInput.Text = "";
                        passwordInput.Text = "";
                        Intent intent = new Intent(this, typeof(Properties.MenuActivity));
                        intent.PutExtra("student_id",student_id);
                        intent.PutExtra("token",token);
                        StartActivity(intent);

                    }   
                    else
                    { 
                        errorLabel.Text = res["response"];
                    }



                }




            };




        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

    }
}

