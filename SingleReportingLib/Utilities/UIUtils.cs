using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;



    public class UIUtils
    {
        public static void MoveSelectedItemBetweenLists(ListBox listFrom, ListBox listTo)
        {
            for (int i = listFrom.Items.Count - 1; i >= 0; i--)
            {
                ListItem item = listFrom.Items[i];

                if (!item.Selected) continue;

                ListItem newItem = new ListItem(item.Text, item.Value);
                listTo.Items.Add(newItem);
                listFrom.Items.Remove(item);
            }
        }



        public static void SortListItems(ListBox listToSort, string[] itemsToPlaceFirst)
        {
            List<string> tempItems = new List<string>();

            Dictionary<string, string> tempPairs = new Dictionary<string, string>();

            foreach (ListItem listItem in listToSort.Items)
            {
                tempItems.Add(listItem.Text);
                tempPairs.Add(listItem.Text, listItem.Value);
            }

            tempItems.Sort();

            listToSort.Items.Clear();
            foreach (string placeFirstItems in itemsToPlaceFirst)
            {
                if (tempItems.Contains(placeFirstItems))
                {
                    ListItem newItem = new ListItem(placeFirstItems, tempPairs[placeFirstItems]);
                    listToSort.Items.Add(newItem);
                }
                tempItems.Remove(placeFirstItems);
                tempPairs.Remove(placeFirstItems);
            }

            foreach (string item in tempItems)
            {
                ListItem newItem = new ListItem(item, tempPairs[item]);
                listToSort.Items.Add(newItem);
            }
        }



        public static void SortListItemsByValues(ListBox listToSort, string[] valuesToPlaceFirst)
        {

            List<string> tempItems = new List<string>();
            Dictionary<string, string> tempPairs = new Dictionary<string, string>();

            foreach (ListItem listItem in listToSort.Items)
            {
                tempItems.Add(listItem.Value);
                tempPairs.Add(listItem.Value, listItem.Text);
            }

            tempItems.Sort();

            listToSort.Items.Clear();
            foreach (string placeFirstValue in valuesToPlaceFirst)
            {
                if (tempItems.Contains(placeFirstValue))
                {
                    ListItem newItem = new ListItem(tempPairs[placeFirstValue], placeFirstValue);
                    listToSort.Items.Add(newItem);
                }
                tempItems.Remove(placeFirstValue);
                tempPairs.Remove(placeFirstValue);
            }

            foreach (string item in tempItems)
            {
                ListItem newItem = new ListItem(tempPairs[item], item);
                listToSort.Items.Add(newItem);
            }
        }
    }

