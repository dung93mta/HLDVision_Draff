using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace HLDVision.Core
{
    public class ImageTreeView : TreeView
    {
        public ImageTreeView()
        {
            ImageList = new ImageList();
            //ImageList.Images.Add(new Icon(""));
            
            //ImageList.Images.Add("", new Icon(@"C:\Users\SDV\Desktop\icon\1.ico"));
            //ImageList.Images.Add("Input", new Icon(@"C:\Users\SDV\Desktop\icon\2.ico"));
            //ImageList.Images.Add("Output", new Icon(@"C:\Users\SDV\Desktop\icon\3.ico"));
            //ImageList.Images.Add("Acquisition", new Icon(@"C:\Users\SDV\Desktop\icon\4.ico"));
        }
    }
}
