using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using GroceryList.ViewModel;
using Newtonsoft.Json.Linq;

namespace GroceryList.Datasource
{
    public static class GroceryDatasource
    {
        public static async Task<ObservableCollection<GroceryItem>> GetData()
        {
            //ObservableCollection<GroceryItem> list = new ObservableCollection<GroceryItem> {
            //    new GroceryItem() { Name = "Apple", Completed = false, Location = "HEB (right), WF (left)" },
            //    new GroceryItem() { Name = "Banana", Completed = false, Location = "HEB (right), WF (left)" },
            //    new GroceryItem() { Name = "Raisins", Completed = false, Location = "HEB (A6), WF (2)" },
            //};

            //// Printing list above as JSON so we can manually input inside Google Firebase
            //JObject o = JObject.FromObject(new
            //{
            //    //Table = list
            //    GroceryList = list.ToDictionary(x => x.Name, x => x)
            //});
            //Console.WriteLine(o);


            var mL = await Query();
            return mL;
        }



        // Firebase code

        public static FirebaseClient Authenticate()
        {
            var APP_SECRET = "ENTER_YOUR_APP_SECRET_HERE";
            var CLIENT_URL = "https://UPDATE_HERE.firebaseio.com/";

            var auth = APP_SECRET; // your app secret
            var firebaseClient = new FirebaseClient(
              CLIENT_URL,
              new FirebaseOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult(auth)
              });
            return firebaseClient;
        }

        public static async Task<ObservableCollection<GroceryItem>> Query()
        {
            var firebase = Authenticate();
            var result = await firebase
              .Child("GroceryList")
              .OrderByKey()              
              .OnceAsync<GroceryItem>();

            foreach (var item in result)
            {
                Console.WriteLine($"{item.Key} is {item.Object.Name}.");
            }

            var groceryList = result.Select(x => x.Object);                 
            return new ObservableCollection<GroceryItem>(groceryList);
        }

        public static async Task UpdateItem(GroceryItem groceryItem)
        {
            var firebase = Authenticate();
            await firebase
                  .Child("GroceryList")
                  .Child(groceryItem.Name)
                  .PutAsync(groceryItem);
        }


    }
 
}
