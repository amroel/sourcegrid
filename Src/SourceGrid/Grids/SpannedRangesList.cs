using System;
using System.Collections.Generic;

namespace SourceGrid
{
	/// <summary>
	/// A is simple List of Ranges.
	/// Uses simple iterating over list to find
	/// required range
	/// </summary>
public class SpannedRangesList: List<SgRange>, ISpannedRangesCollection
	{
		public void Update(SgRange oldRange, SgRange newRange)
		{
			int index = base.IndexOf(oldRange);
			if (index <0 )
				throw new RangeNotFoundException();
			this[index] = newRange;
		}
		
		public void Redim(int rowCount, int colCount)
		{
			// just do nothing, nothing needed
		}
		
		public new void Remove(SgRange range)
		{
			int index = base.IndexOf(range);
			if (index < 0)
				throw new RangeNotFoundException();
			base.RemoveAt(index);
		}
		
		
		/// <summary>
		/// Returns first intersecting region
		/// </summary>
		/// <param name="pos"></param>
		public SgRange? GetFirstIntersectedRange(Position pos)
		{
			for (int i = 0; i < this.Count; i++)
			{
				var range = this[i];
				if (range.Contains(pos))
					return range;
			}
			return null;
		}
		
		public List<SgRange> GetRanges(SgRange range)
		{
			var result = new List<SgRange>();
			for (int i = 0; i < this.Count; i++)
			{
				var r = this[i];
				if (r.Contains(range))
					result.Add(r);
			}
			return result;
		}
		
		public SgRange? FindRangeWithStart(Position start)
		{
			for (int i = 0; i < this.Count; i++)
			{
				var range = this[i];
				if (range.Start.Equals(start))
					return range;
			}
			return null;
		}
	}
}
