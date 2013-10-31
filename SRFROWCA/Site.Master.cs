using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

namespace SRFROWCA
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Highlihgt current menue item.
            HighlightSelectedMenuItem();

            // Check user is loggedin
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                // If loggedin user is not Admin then remove admin menue items from master page.
                if (!HttpContext.Current.User.IsInRole("Admin") && !HttpContext.Current.User.IsInRole("CountryAdmin"))
                {
                    // Remove admin menue items
                    RemoveAdminMenuItems();
                }
                // if user is loggedin, hide user/password and register and show loginview.
                ShowHideLoginControls(true);
            }
            else
            {
                // If user is not logged in then remove all menue items which
                // only authenticated user has to view.
                RemoveAuthenticatedMenuItems();

                // Hide loginview and show user/password and register link.
                ShowHideLoginControls(false);
            }
        }

        // If user is not loged in then show login control and hide loginview and vice versa
        private void ShowHideLoginControls(bool isVisible)
        {
            //loginDiv.Visible = !isVisible;
            //HeadLoginView.Visible = isVisible;
        }

        // Remove menue items from menue list.
        // These items only needed to show when user is loggedin.
        private void RemoveAuthenticatedMenuItems()
        {
            MenuItemCollection menuItems = NavigationMenu.Items;
            List<MenuItem> adminItems = new List<MenuItem>();
            foreach (MenuItem menuItem in menuItems)
            {
                if (menuItem.Text == "My Activities")
                    adminItems.Add(menuItem);

                if (!HttpContext.Current.User.IsInRole("Admin"))
                {
                    if (menuItem.Text == "Organizations")
                        adminItems.Add(menuItem);

                    if (menuItem.Text == "Data Feeds")
                        adminItems.Add(menuItem);

                    if (menuItem.Text == "Users")
                        adminItems.Add(menuItem);

                    if (menuItem.Text == "Emergencies")
                        adminItems.Add(menuItem);

                    if (menuItem.Text == "Offices")
                        adminItems.Add(menuItem);

                    if (menuItem.Text == "Clusters")
                        adminItems.Add(menuItem);

                    if (menuItem.Text == "Str Objectives")
                        adminItems.Add(menuItem);

                    if (menuItem.Text == "Objectives")
                        adminItems.Add(menuItem);

                    if (menuItem.Text == "Indicators")
                        adminItems.Add(menuItem);

                    if (menuItem.Text == "Activity")
                        adminItems.Add(menuItem);

                    if (menuItem.Text == "Data")
                        adminItems.Add(menuItem);

                    if (menuItem.Text == "Upload Activities")
                        adminItems.Add(menuItem);

                    if (menuItem.Text == "Data Entry")
                        adminItems.Add(menuItem);

                    if (menuItem.Text == "Data Feeds")
                        adminItems.Add(menuItem);
                }
            }

            foreach (MenuItem item in adminItems)
            {
                menuItems.Remove(item);
            }
        }

        // Remove menue items from menue list.
        // These items only available to Admin.
        private void RemoveAdminMenuItems()
        {
            MenuItemCollection menuItems = NavigationMenu.Items;
            List<MenuItem> adminItems = new List<MenuItem>();
            foreach (MenuItem menuItem in menuItems)
            {
                if (menuItem.Text == "Organizations")
                    adminItems.Add(menuItem);

                if (menuItem.Text == "Users")
                    adminItems.Add(menuItem);

                if (menuItem.Text == "Emergencies")
                    adminItems.Add(menuItem);

                if (menuItem.Text == "Offices")
                    adminItems.Add(menuItem);

                if (menuItem.Text == "Clusters")
                    adminItems.Add(menuItem);

                if (menuItem.Text == "Str Objectives")
                    adminItems.Add(menuItem);
                
                if (menuItem.Text == "Objectives")
                    adminItems.Add(menuItem);

                if (menuItem.Text == "Indicators")
                    adminItems.Add(menuItem);

                if (menuItem.Text == "Activity")
                    adminItems.Add(menuItem);

                if (menuItem.Text == "Data")
                    adminItems.Add(menuItem);

                if (menuItem.Text == "Upload Activities")
                    adminItems.Add(menuItem);

                if (menuItem.Text == "Data Feeds")
                    adminItems.Add(menuItem);
            }

            foreach (MenuItem item in adminItems)
            {
                menuItems.Remove(item);
            }
        }

        // Change colour of currenly selected menue item.
        private void HighlightSelectedMenuItem()
        {
            // Get current URL            
            string myURL = Request.Url.AbsoluteUri;

            // Loop through all the menues.
            foreach (MenuItem mi in NavigationMenu.Items)
            {
                // Get filename from path.
                string pageName = System.IO.Path.GetFileName(mi.NavigateUrl);

                // If current URL contains file name then highlight this menue item and exit.
                if (myURL.Contains(pageName))
                {
                    mi.Selected = true;
                    break;
                }
            }
        }

        // Gets the ASP.NET application's virtual application root path on the server.
        private static string VirtualFolder
        {
            get { return HttpContext.Current.Request.ApplicationPath; }
        }

        // This property is to use it in markup where css and js files
        // use this to create virtualpath.
        protected string BaseURL
        {
            get
            {

                return string.Format("http://{0}{1}",
                                     HttpContext.Current.Request.ServerVariables["HTTP_HOST"],
                                     (VirtualFolder.Equals("/")) ? string.Empty : VirtualFolder);
            }
        }
    }
}
