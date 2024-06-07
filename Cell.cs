using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper3
{
    internal class Cell
    {
        public bool IsMine { get; set; }
        public bool IsOpen { get; set; }
        public bool HasFlag { get; set; }
        public int MinesNearby { get; set; }

        public Cell()
        {
            IsMine = false;
            IsOpen = false;
            HasFlag = false;
            MinesNearby = 0;
        }
    }
}
