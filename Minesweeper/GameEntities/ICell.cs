using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.GameEntities
{
    interface ICell
    {
        public enum CellState
        {
            Hidden,
            Revealed,
            Pressed,
            Flagged
        }
        public int Value { get; }
    }
}
