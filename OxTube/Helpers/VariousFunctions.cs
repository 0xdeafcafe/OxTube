using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OxTube.Helpers
{
    public static class VariousFunctions
    {
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
    }
}
