using System;
using System.IO;
using System.Reflection;
#if WINDOWS_PHONE
using System.Windows;
using System.Windows.Media;
#endif

namespace OxTube.Core.Helpers
{
    public static class VariousFunctions
    {
        public static readonly byte[][] HexColourScale = new[]
        {
            new byte[] { 0x00, 0xFF, 0x00},     // Green
            new byte[] { 0x11, 0xFF, 0x00},
            new byte[] { 0x22, 0xFF, 0x00},
            new byte[] { 0x33, 0xFF, 0x00},
            new byte[] { 0x44, 0xFF, 0x00},
            new byte[] { 0x55, 0xFF, 0x00},
            new byte[] { 0x66, 0xFF, 0x00},
            new byte[] { 0x77, 0xFF, 0x00},
            new byte[] { 0x88, 0xFF, 0x00},
            new byte[] { 0x99, 0xFF, 0x00},
            new byte[] { 0xAA, 0xFF, 0x00},
            new byte[] { 0xBB, 0xFF, 0x00},
            new byte[] { 0xCC, 0xFF, 0x00},
            new byte[] { 0xDD, 0xFF, 0x00},
            new byte[] { 0xEE, 0xFF, 0x00},
            new byte[] { 0xFF, 0xFF, 0x00},     // Yellow
            new byte[] { 0xFF, 0xEE, 0x00},
            new byte[] { 0xFF, 0xDD, 0x00},
            new byte[] { 0xFF, 0xCC, 0x00},
            new byte[] { 0xFF, 0xBB, 0x00},
            new byte[] { 0xFF, 0xAA, 0x00},
            new byte[] { 0xFF, 0x99, 0x00},
            new byte[] { 0xFF, 0x88, 0x00},
            new byte[] { 0xFF, 0x77, 0x00},
            new byte[] { 0xFF, 0x66, 0x00},
            new byte[] { 0xFF, 0x55, 0x00},
            new byte[] { 0xFF, 0x44, 0x00},
            new byte[] { 0xFF, 0x33, 0x00},
            new byte[] { 0xFF, 0x22, 0x00},
            new byte[] { 0xFF, 0x11, 0x00},
            new byte[] { 0xFF, 0x00, 0x00}      // Red
        };

#if WINDOWS_PHONE
        public static string GetStringFromResources(string path)
        {
            var output = string.Empty;
            var resrouceStream = Application.GetResourceStream(new Uri(path, UriKind.Relative));

            if (resrouceStream != null)
            {
                using (var myFileStream = resrouceStream.Stream)
                {
                    if (myFileStream.CanRead)
                    {
                        var myStreamReader = new StreamReader(myFileStream);

                        //read the content here
                        output =  myStreamReader.ReadToEnd();
                    }
                }
            }

            return output;
        }
        public static Color GetArgbFromTime(int minutes)
        {
            // Return fixed value for over 60 mins
            if (minutes > 60)
                return Color.FromArgb(
                    0xFF,
                    HexColourScale[HexColourScale.Length - 1][0],
                    HexColourScale[HexColourScale.Length - 1][1],
                    HexColourScale[HexColourScale.Length - 1][2]);

            // Return fixed value for 0 minutes
            if (minutes == 0)
                return Color.FromArgb(
                    0xFF,
                    HexColourScale[0][0],
                    HexColourScale[0][1],
                    HexColourScale[0][2]);

            // Calculate dynamic value for 1-59 minutes
            int index = minutes / 3;
            return Color.FromArgb(
                    0xFF,
                    HexColourScale[index][0],
                    HexColourScale[index][1],
                    HexColourScale[index][2]);
        }
#endif

        public static string GetStringFromCoreResources(string path)
        {
            try
            {
#if WP7
                var stream = Assembly.Load("OxTube.Core").GetManifestResourceStream("OxTube.Core." + path.Replace("/", ".").Replace("\\", "."));

                if (stream != null)
                {
                    var reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }
                return null;
#elif WP8
                var stream = Assembly.Load(new AssemblyName("OxTube.Core")).GetManifestResourceStream("OxTube.Core." + path.Replace("/", ".").Replace("\\", "."));

                if (stream == null)
                {
                    return null;
                }
                var reader = new StreamReader(stream);
                return reader.ReadToEnd();
#endif
                return null;
            }
            catch { return null; }
        }
        public static Uri CreateAPIUri(string stopCode, string stopDirection)
        {
            var randomNonce = new Random().Next(0, 1000000);
            return new Uri(string.Format("http://www.xrax.me/OxTube/NaPAN.aspx?sid={0}&dir={1}&nonce={2}", stopCode, stopDirection, randomNonce));
        }
    }
}
