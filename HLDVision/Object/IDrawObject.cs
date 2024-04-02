using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLDVision
{
    public delegate void RefreshHandler(object sender, EventArgs arg);

    public delegate void Drawing(System.Drawing.Graphics gdi);

    public interface IDrawObject
    {
        void Draw(System.Drawing.Graphics gdi);

        event RefreshHandler Refresh;
        void OnRefresh(object sender, EventArgs arg);
    }
}
