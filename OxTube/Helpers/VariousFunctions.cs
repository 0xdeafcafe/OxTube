using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace OxTube.Helpers
{
    public static class VariousFunctions
    {
        public static readonly byte[][] HexColourScale = new byte[][]
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
            new byte[] { 0xFF, 0x00, 0x00},     // Red
        };

        public static string GetStringFromResources(string path)
        {
            string output = string.Empty;

            var ResrouceStream = Application.GetResourceStream(new Uri(path, UriKind.Relative));

            if (ResrouceStream != null)
            {
                using (Stream myFileStream = ResrouceStream.Stream)
                {
                    if (myFileStream.CanRead)
                    {
                        StreamReader myStreamReader = new StreamReader(myFileStream);

                        //read the content here
                        output =  myStreamReader.ReadToEnd();
                    }
                }
            }

            // Remove Possibility of Memory Leak
            ResrouceStream = null;

            return output;
        }
        public static Uri CreateAPIUri(string stopCode, string stopDirection)
        {
            return new Uri(string.Format("http://www.xrax.me/OxTube/NaPAN.aspx?sid={0}&dir={1}", stopCode, stopDirection));
        }
        public static Color GetARGBFromTime(int minutes)
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
    }
}
