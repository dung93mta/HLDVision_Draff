using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLDVision
{
    public class HldNotifyCollection<T> : List<T>
    {
        new public void Add(T item)
        {
            base.Add(item);
            NotifyAddItem(item);
        }

        new public void Clear()
        {
            base.Clear();

            if (ClearCollection != null)
            {
                ClearCollection(this, null);
            }
        }

        public delegate void AddItemEventHandler(object sender, T item);

        public event AddItemEventHandler AddItem;

        protected void NotifyAddItem(T item)
        {
            if (AddItem != null)
            {
                AddItem(this, item);
            }
        }

        public event EventHandler ClearCollection;

    }
}
