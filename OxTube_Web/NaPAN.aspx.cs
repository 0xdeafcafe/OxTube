using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;
using HtmlAgilityPack;

namespace OxTube_Web
{
    public partial class _Default : System.Web.UI.Page
    {
        private JavaScriptSerializer json = new JavaScriptSerializer();
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

        public string return_string = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Create Argument Storage
            string stopID = null;
            string direction = null;

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
                error_code = "missing params";
                Response.Write(return_string);
                return;
#endif
            }

            // Parse JSON
            string pathTO = File.ReadAllText(Server.MapPath("/App_Data/StopData/") + "TowardsOxford.json");

            _toOxford = json.Deserialize<IList<StopInfo>>(pathTO);
            _toLondon = json.Deserialize<IList<StopInfo>>(File.ReadAllText(Server.MapPath("/App_Data/StopData/") + "TowardsLondon.json"));

            // Get the Stop that we want
            StopInfo userRequestedStop = GetStopInfoFromID(stopID, direction);

            // Error Checking
            if (userRequestedStop == null)
            {
#if DEBUG
                
#else
                error_code = "invalid stopid or direction";
                Response.Write(return_string);
                return;
#endif
            }

            // Begin Screen Scraping
            WebClient wb = new WebClient();
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
                error_code = "unable to parse page";
                Response.Write(return_string);
                return;
#endif
            }

            IList<TimeEntry> ArrivalTimes = new List<TimeEntry>();

            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(pageHTML);
                foreach (HtmlNode obj in doc.DocumentNode.SelectNodes("//table"))
                {
                    // Check it's the time one
                    if (obj.OuterHtml.Contains("tblfullwidth selectable sortable-onload-3r rowstyle-alt colstyle-alt sortable-onload-3r no-arrow colstyle-alt rowstyle-alt paginate-10max-pages-7 paginationcallback-callbackTest-calculateTotalRating"))
                    {
                        // It's the Time One :D
                        foreach (HtmlNode entry in obj.ChildNodes["tbody"].ChildNodes)
                        {
                            if (entry.Name == "tr")
                            {
                                HtmlNodeCollection collection = entry.ChildNodes;

                                /*
                                1	::	Service Name
                                3	::	Destination
                                5	::	Arrival Time
                                7	::	Provider Image 
                                */

                                ArrivalTimes.Add(new TimeEntry()
                                {
                                    ServiceName = collection[1].InnerText,
                                    Destination = collection[3].InnerText,
                                    ArrivalTime = (collection[5].InnerText.ToLower() == "due") ? 0 : int.Parse(collection[5].InnerText.ToLower().Replace(" ", "").Replace("mins", ""))
                                });
                            }
                        }
                    }
                }
            }
            catch { }

            if (ArrivalTimes.Count == 0)
            {
#if DEBUG

#else
                error_code = "unable to parse page";
                Response.Write(return_string);
                return;
#endif
            }

            // Create JSON
            string return_string = json.Serialize(ArrivalTimes);
            Response.Write(return_string);
        }

        private StopInfo GetStopInfoFromID(string stopID, string direction)
        {
            if (direction == "to")
            {
                foreach (StopInfo stop in _toOxford)
                {
                    if (stop.StopCode.ToString() == stopID)
                    {
                        return stop;
                    }
                }
            }
            else if (direction == "tl")
            {
                foreach (StopInfo stop in _toLondon)
                {
                    if (stop.StopCode.ToString() == stopID)
                    {
                        return stop;
                    }
                }
            }

            return null;
        }
    }
}