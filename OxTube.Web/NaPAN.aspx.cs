using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;
using HtmlAgilityPack;

namespace OxTube.Web
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly JavaScriptSerializer _json = new JavaScriptSerializer();
        private IList<StopInfo> _toOxford = new List<StopInfo>();
        private IList<StopInfo> _toLondon = new List<StopInfo>();

        public class StopInfo
        {
            public string StopName { get; set; }
            public uint StopCode { get; set; }
            public string StopURL { get; set; }
        }
        public class TimeEntry
        {
            public string ServiceName { get; set; }
            public string Destination { get; set; }
            public int ArrivalTime { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Create Argument Storage
            string stopID, direction, returnString = "";

            // Store Data in them
            try
            {
                stopID = Request.QueryString["sid"];
                direction = Request.QueryString["dir"].ToLower();
            }
            catch
            {
                stopID = null;
                direction = null;
            }

            // Error Checking
            if (String.IsNullOrEmpty(stopID) || String.IsNullOrEmpty(direction))
            {
#if DEBUG
                stopID = "69347426";
                direction = "to";
#else
                returnString = "missing params";
                Response.Write(returnString);
                return;
#endif
            }

            // Parse JSON
#if DEBUG
            _toOxford = _json.Deserialize<IList<StopInfo>>(File.ReadAllText(Server.MapPath("/App_Data/StopData/") + "TowardsOxford.json"));
            _toLondon = _json.Deserialize<IList<StopInfo>>(File.ReadAllText(Server.MapPath("/App_Data/StopData/") + "TowardsLondon.json"));
#else
            _toOxford = _json.Deserialize<IList<StopInfo>>(File.ReadAllText(@"D:\Hosting\8582025\html\OxTube\App_Data\StopData\TowardsOxford.json"));
            _toLondon = _json.Deserialize<IList<StopInfo>>(File.ReadAllText(@"D:\Hosting\8582025\html\OxTube\App_Data\StopData\TowardsLondon.json"));
#endif

            // Get the Stop that we want
            var userRequestedStop = GetStopInfoFromID(stopID, direction);

            // Error Checking
            if (userRequestedStop == null)
            {
#if DEBUG
                
#else
                returnString = "invalid stopid or direction";
                Response.Write(returnString);
                return;
#endif
            }

            // Begin Screen Scraping
            var wb = new WebClient();
            string pageHTML = null;
            try
            {
                pageHTML = wb.DownloadString(userRequestedStop.StopURL);
            }
            catch
            {
                pageHTML = null;
            }

            // Error Checking
            if (pageHTML == null)
            {
#if DEBUG
                
#else
                returnString = "unable to parse page";
                Response.Write(returnString);
                return;
#endif
            }

            var arrivalTimes = new List<TimeEntry>();

            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(pageHTML);
                arrivalTimes.AddRange(from obj in doc.DocumentNode.SelectNodes("//table")
                                      where obj.OuterHtml.Contains("tblfullwidth selectable sortable-onload-3r rowstyle-alt colstyle-alt sortable-onload-3r no-arrow colstyle-alt rowstyle-alt paginate-10max-pages-7 paginationcallback-callbackTest-calculateTotalRating")
                                      from entry in obj.ChildNodes["tbody"].ChildNodes
                                      where entry.Name == "tr"
                                      select entry.ChildNodes
                                      into collection select new TimeEntry
                                                                 {
                                                                     ServiceName = collection[1].InnerText, Destination = collection[3].InnerText, ArrivalTime = (collection[5].InnerText.ToLower() == "due") ? 0 : int.Parse(collection[5].InnerText.ToLower().Replace(" ", "").Replace("mins", ""))
                                                                 });
            }
// ReSharper disable EmptyGeneralCatchClause
            catch
// ReSharper restore EmptyGeneralCatchClause
            {

            }

            if (arrivalTimes.Count == 0)
            {
#if DEBUG

#else
                returnString = "unable to parse page";
                Response.Write(returnString);
                return;
#endif
            }

            // Create JSON
            returnString = _json.Serialize(arrivalTimes);
            Response.Write(returnString);
        }

        private StopInfo GetStopInfoFromID(string stopID, string direction)
        {
            switch (direction)
            {
                case "to":
                    foreach (var stop in _toOxford.Where(stop => stop.StopCode.ToString(CultureInfo.InvariantCulture) == stopID))
                    {
                        return stop;
                    }
                    break;
                case "tl":
                    foreach (var stop in _toLondon.Where(stop => stop.StopCode.ToString(CultureInfo.InvariantCulture) == stopID))
                    {
                        return stop;
                    }
                    break;
            }

            return null;
        }
    }
}