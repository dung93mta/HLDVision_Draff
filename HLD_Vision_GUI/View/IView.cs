using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLD_Vision_GUI.View
{
    public interface IView
    {
        object DataContext { get; set; }
    }

    public enum ViewName
    {
        MainFrame,
        Auto,
        Vision,
        Teaching,
        Data,
        Log,
    }
}
