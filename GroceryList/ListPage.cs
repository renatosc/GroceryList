using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using GroceryList.Datasource;
using GroceryList.ViewModel;
using Xamarin.Forms;

namespace GroceryList
{

    //public class MyImageCell : ImageCell
    //{
    //    public bool Completed;

    //    public MyImageCell()
    //    {
    //        var deleteAction = new MenuItem { Text = Completed ? "Mark as Pending" : "Mark as Complete", IsDestructive = true }; // red background
    //        deleteAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
    //        deleteAction.Clicked += async (sender, e) =>
    //        {
    //            var mi = ((MenuItem)sender);
    //            //Debug.WriteLine("Delete Context Action clicked: " + mi.CommandParameter);
    //        };
    //        // add to the ViewCell's ContextActions property
            
    //        ContextActions.Add(deleteAction);
    //    }
    //}

    public class ListPage : ContentPage
    {
        private ObservableCollection<GroupedGroceryItem> grouped { get; set; }
        private ListView lstView;
        private ObservableCollection<GroceryItem> items;

        public ListPage()
        {
            

            ToolbarItem itemReset = new ToolbarItem() { Text = "Reset" };
            ToolbarItems.Add(itemReset);
            itemReset.Clicked += OnResetTap;

            ToolbarItem itemRefresh = new ToolbarItem() { Text = "Refresh" };
            ToolbarItems.Add(itemRefresh);
            itemRefresh.Clicked += OnRefreshTap;

            lstView = new ListView();
            lstView.IsGroupingEnabled = true;
            lstView.GroupDisplayBinding = new Binding("LongName");
            lstView.ItemTemplate = new DataTemplate(typeof(ImageCell));
            lstView.ItemTemplate.SetBinding(ImageCell.TextProperty, "Name");
            lstView.ItemTemplate.SetBinding(ImageCell.DetailProperty, "Location");

            lstView.ItemTapped += Handle_ItemTapped;

            ReloadData();

            Content = lstView;

            this.Title = "My Grocery List";

        }


        async Task ReloadData()
        {
            Debug.WriteLine("ON RELOAD DATA");
            
            grouped = new ObservableCollection<GroupedGroceryItem>();
            var groupPending = new GroupedGroceryItem() { LongName = "Pending", ShortName = "P" };
            var groupCompleted = new GroupedGroceryItem() { LongName = "Completed", ShortName = "C" };

            if (items == null)            
                items = await GroceryDatasource.GetData();
            

            foreach (GroceryItem item in items)
            {
                if (item.Completed)
                {
                    groupCompleted.Add(item);
                }
                else
                {
                    groupPending.Add(item);
                }
            }

            grouped.Add(groupPending);
            grouped.Add(groupCompleted);

            Debug.WriteLine("groupPending.count=" + groupPending.Count);
            Debug.WriteLine("groupCompleted.count=" + groupCompleted.Count);

            lstView.ItemsSource = grouped;
        }

        void OnResetTap(object sender, EventArgs e)
        {     
            foreach (GroceryItem item in items)
            {
                item.Completed = false;
            }
            ReloadData();
        }

        void OnRefreshTap(object sender, EventArgs e)
        {
            items = null;
            ReloadData();
        }
       
        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            GroceryItem item = (GroceryItem)e.Item;
            bool isCompleted = item.Completed;
            Debug.WriteLine("isCompleted="+ isCompleted);
            bool result = await DisplayAlert(item.Name, "Mark as " + (isCompleted ? "Pending" : "Completed") + "?", "OK", "Cancel");
            if (result)
            {                         
                item.Completed = !item.Completed;
                ReloadData();
                GroceryDatasource.UpdateItem(item);
            }


            Debug.WriteLine(result);

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }


        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
        }

    }
}

