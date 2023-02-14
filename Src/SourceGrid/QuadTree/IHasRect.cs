﻿using SourceGrid;

namespace QuadTreeLib
{
    /// <summary>
    /// An interface that defines and object with a rectangle
    /// </summary>
    public interface IHasRect
    {
        SgRange Rectangle { get; }
    }
}
