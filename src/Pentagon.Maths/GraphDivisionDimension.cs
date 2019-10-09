// -----------------------------------------------------------------------
//  <copyright file="GraphDivisionDimension.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths
{
    public struct GraphDivisionDimension
    {
        public GraphDivisionDimension(int top, int left, int bottom, int right)
        {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }

        public int Top { get; }
        public int Left { get; }
        public int Bottom { get; }
        public int Right { get; }

        public int Horizontal => Left + Right;
        public int Vertical => Top + Bottom;
    }
}