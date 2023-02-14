using System;
using System.Collections.Generic;
using System.Text;

namespace SourceGrid.Decorators
{
    public class DecoratorHighlight : DecoratorBase
    {
        private SgRange mRange = SgRange.Empty;
        /// <summary>
        /// Gets or sets the range to draw
        /// </summary>
        public SgRange Range
        {
            get { return mRange; }
            set { mRange = value; }
        }


        public override bool IntersectWith(SgRange range)
        {
            return Range.IntersectsWith(range);
        }

        public override void Draw(RangePaintEventArgs e)
        {
        }
    }
}
