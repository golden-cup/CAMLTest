using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMLTest
{
    class BatchDeleteFromLists
    {
        static void Main(string[] args)
        {
            using (var site = new SPSite("http://sharepoint.medieval.ml/sites/m"))
            {
                var web = site.RootWeb;
                var list = web.Lists.TryGetList("MedEventsLog");
                // We prepare a String.Format with a String.Format, this is why we have a {{0}} 
                string command = String.Format("<Method><SetList Scope=\"Request\">{0}</SetList><SetVar Name=\"ID\">{{0}}</SetVar><SetVar Name=\"Cmd\">Delete</SetVar><SetVar Name=\"owsfileref\">{{1}}</SetVar></Method>", list.ID);
                // We get everything but we limit the result to 100 rows 
                SPQuery q = new SPQuery();
                q.RowLimit = 100;

                // While there's something left 
                while (list.ItemCount > 0)
                {
                    // We get the results 
                    SPListItemCollection coll = list.GetItems(q);

                    StringBuilder sbDelete = new StringBuilder();
                    sbDelete.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?><Batch>");

                    Guid[] ids = new Guid[coll.Count];
                    for (int i = 0; i < coll.Count; i++)
                    {
                        SPListItem item = coll[i];
                        sbDelete.Append(string.Format(command, item.ID.ToString(),""));
                        ids[i] = item.UniqueId;
                    }
                    sbDelete.Append("</Batch>");

                    // We execute it 
                    web.ProcessBatchData(sbDelete.ToString());
                    list.Update();

                }
                web.RecycleBin.DeleteAll();
                site.RecycleBin.DeleteAll();
            }
        }
    }
}
