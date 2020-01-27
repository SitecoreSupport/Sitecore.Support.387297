    using Sitecore;
    using Sitecore.Configuration;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Globalization;
    using Sitecore.Web;
    using Sitecore.Web.UI.WebControls;
    using System;
    using System.Web.UI;

namespace Sitecore.Shell.Controls.TreeviewEx
{
    public class TreeviewExPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Language language;
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(e, "e");
            Sitecore.Web.UI.WebControls.TreeviewEx child = MainUtil.GetBool(WebUtil.GetQueryString("mr"), false) ? new MultiRootTreeview() : new Sitecore.Web.UI.WebControls.TreeviewEx();
            this.Controls.Add(child);
            child.ID = WebUtil.GetQueryString("treeid");
            string queryString = WebUtil.GetQueryString("db", Sitecore.Client.ContentDatabase.Name);
            Database database = Factory.GetDatabase(queryString);
            Assert.IsNotNull(database, queryString);
            ID itemId = ShortID.DecodeID(WebUtil.GetQueryString("id"));
            string str2 = WebUtil.GetQueryString("la");
            if (string.IsNullOrEmpty(str2) || !Language.TryParse(str2, out language))
            {
                language = Sitecore.Context.Language;
            }
            Item item = database.GetItem(itemId, language);
            if (item != null)
            {
                child.ParentItem = item;
            }
        }
    }
}
