using System;
using System.Collections.ObjectModel;

namespace GroceryList.ViewModel
{
    public class GroceryItem
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string ImageUrl { get; set; }
        public bool Completed { get; set; }

        public GroceryItem()
        {
        }
    }
    public class GroupedGroceryItem : ObservableCollection<GroceryItem>
    {
        public string LongName { get; set; }
        public string ShortName { get; set; }
    }
}
