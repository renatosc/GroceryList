using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GroceryList.ViewModel;

namespace GroceryList.Datasource
{
    public class GroceryDatasource
    {
        public ObservableCollection<GroceryItem> GetData()
        {
            ObservableCollection<GroceryItem> list = new ObservableCollection<GroceryItem> {
                new GroceryItem() { Name = "Apple", Completed = false, Location = "HEB (right), WF (left)" },
                new GroceryItem() { Name = "Banana", Completed = false, Location = "HEB (right), WF (left)" },
                new GroceryItem() { Name = "Raisins", Completed = true, Location = "HEB (A6), WF (2)" },
                new GroceryItem() { Name = "Sweet Potato", Completed = true, Location = "HEB (right), WF (left)" }
            };

            return list;
        }

    }
}
