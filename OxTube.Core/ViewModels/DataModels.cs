#if WINDOWS_PHONE
using System.Windows.Media;
#endif

namespace OxTube.Core.ViewModels
{
    public static class DataModels
    {
        public class StopInfo
        {
            public string StopName { get; set; }
            public uint StopCode { get; set; }
            public string StopURL { get; set; }
            public string StopDirection { get; set; }

            public string StopDirectionFrendly { get; set; }
        }
        public class TimeEntry
        {
            public string ServiceName { get; set; }
            public string Destination { get; set; }
            public int ArrivalTime { get; set; }

            public string ArrivalTimeFriendly { get; set; }
#if WINDOWS_PHONE
            public SolidColorBrush BackgroundColour { get; set; }
#endif
        }
    }
}
