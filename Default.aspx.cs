using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZendeskSSOWeb
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = "testuser";
            string password = "password123";
            string subdomain = "capsystems";

            string ZenUser = "Sean Ruud"; //"Jen Goodman"
            string userEmail = "sruud@capsystems.com";// "jgoodman@capsystems.com"; // This should be retrieved after authentication
            string sharedSecret = "jTKFgexkas2ZqRAYM0nJlhdsMAreZC8uWFztU8ERqPl3TKEH";
            string articleId = "26493446562836-Client-Intake";

            if (AuthenticateUser(username, password))
            {
                ZendeskRedirector redirector = new ZendeskRedirector();
               string _Url = redirector.RedirectToZendeskArticle(subdomain, userEmail, ZenUser, sharedSecret, articleId);
                Response.Redirect(_Url);
            }
            else
            {
                Console.WriteLine("Authentication failed. Please check your credentials.");
            }
        }
        public static bool AuthenticateUser(string username, string password)
        {
            // Replace this with your actual authentication logic
            if (username == "testuser" && password == "password123")
            {
                return true;
            }
            return false;
        }
    }
}