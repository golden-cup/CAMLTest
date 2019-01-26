using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMLTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var site = new SPSite("http://sharepoint.medieval.ml/sites/m"))
            {
                var web = site.RootWeb;
                var list = web.Lists.TryGetList("Projects");
                if (list == null)
                    return;
                var items = QueryItems(list);
                ShowItems(items);

                //RaiseRates(items);
                //Console.WriteLine("\nAfter Raise\n");
                //ShowItems(items);
                Console.ReadKey();
            }
        }
        static SPListItemCollection QueryItems(SPList list)
        {
            var query = new SPQuery();

            //query.ViewFields =
            //   "<FieldRef Name='Title' />" +
            //   "<FieldRef Name='ID' />" +
            //   "<FieldRef Name='ProjectGV' />";

            //query.Query =
            //"<OrderBy>" +
            //" <FieldRef Name='ID' />" +
            //"</OrderBy>" +
            ////"<Where>" +
            ////" <Contains>" +
            ////" <FieldRef Name='Title' />" +
            ////" <Value Type='Text'>q3</Value>" +
            ////" </Contains>" +
            ////"</Where>";

            //query.Query = "<Where><Or><In><FieldRef Name='ProjectGV' /><Values></Values></In><Contains><FieldRef Name='ProjectGV'/><Value Type='Text'>Q3</Value></Contains></Or></Where>";

            query.Query =
                @"
            <Where>
            <Or>
                <In>
                    <FieldRef Name='ProjectGV' />
                    <Values>
                        <Value Type='Text'>B1</Value>
                        <Value Type='Text'>A2</Value>
                        <Value Type='Text'>huy</Value>
                    </Values>
                </In>
                <Contains>
                    <FieldRef Name='ProjectGV'/>
                    <Value Type='Text'>G23</Value>
                  </Contains>
              </Or>
            </Where>
            ";

            return list.GetItems(query);
        }
        //static void RaiseRates(SPListItemCollection items)
        //{
        //    foreach (SPListItem item in items)
        //    {
        //        var employee = Convert.ToBoolean(item["Employee"]);
        //        var rate = Convert.ToDouble(item["Salary_x002f_Rate"]);
        //        var newRate = employee ? rate + 1 : rate + 0.1;
        //        item["Salary_x002f_Rate"] = newRate;
        //        item.Update();
        //    }
        //}
        static void ShowItems(SPListItemCollection items)
        {
            foreach (SPListItem item in items)
            {
                Console.WriteLine("shaew {0} is , {1}",
                item.Title,
                item["ID"]);
            }
        }
    }
}
