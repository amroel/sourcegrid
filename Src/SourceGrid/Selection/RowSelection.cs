using System;
using System.Collections.Generic;
using System.Text;

namespace SourceGrid.Selection
{
	public class RowSelection : SelectionBase
	{
		private Decorators.DecoratorSelection mDecorator;
		private RangeMergerByRows mList = new RangeMergerByRows();
		
		public RowSelection()
		{
		}

		public override void BindToGrid(GridVirtual p_grid)
		{
			base.BindToGrid(p_grid);

			mDecorator = new Decorators.DecoratorSelection(this);
			Grid.Decorators.Add(mDecorator);
		}

		public override void UnBindToGrid()
		{
			Grid.Decorators.Remove(mDecorator);

			base.UnBindToGrid();
		}

		public override bool IsSelectedColumn(int column)
		{
			return false;
		}

		public override void SelectColumn(int column, bool select)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override bool IsSelectedRow(int row)
		{
			return mList.IsSelectedRow(row);
		}

		public override void SelectRow(int row, bool select)
		{
			SgRange rowRange = Grid.Rows.GetRange(row);
			if (select && mList.IsSelectedRow(row) == false)
			{
				// if multi selection is false, remove all previously selected rows
				var activePosition = this.ActivePosition;
				if (this.EnableMultiSelection == false)
					this.Grid.Selection.ResetSelection(false);
				// continue with adding selection
				mList.AddRange(rowRange);
				this.ActivePosition = activePosition;
				OnSelectionChanged(new RangeRegionChangedEventArgs(rowRange, SgRange.Empty));
			} else
				if (!select && mList.IsSelectedRow(row))
			{
				mList.RemoveRange(rowRange);
				OnSelectionChanged(new RangeRegionChangedEventArgs(SgRange.Empty, rowRange));
			}
		}

		public override bool IsSelectedCell(Position position)
		{
			return IsSelectedRow(position.Row);
		}

		public override void SelectCell(Position position, bool select)
		{
			SelectRow(position.Row, select);
		}

		public override bool IsSelectedRange(SgRange range)
		{
			for (int r = range.Start.Row; r <= range.End.Row; r++)
			{
				if (IsSelectedRow(r) == false)
					return false;
			}

			return true;
		}

		private SgRange NormalizeRange(SgRange range)
		{
			return new SgRange(range.Start.Row, 0, range.End.Row, Grid.Columns.Count - 1);
		}
		
		public override void SelectRange(SgRange range, bool select)
		{
			SgRange normalizedRange = NormalizeRange(range);
			if (select)
				mList.AddRange(normalizedRange); else
				mList.RemoveRange(normalizedRange);
			OnSelectionChanged(new RangeRegionChangedEventArgs(normalizedRange, SgRange.Empty));
		}

		protected override void OnResetSelection()
		{
			RangeRegion prevRange = GetSelectionRegion();

			mList.Clear();

			OnSelectionChanged(new RangeRegionChangedEventArgs(null, prevRange));
		}

		public override bool IsEmpty()
		{
			return mList.IsEmpty();
		}

		public override RangeRegion GetSelectionRegion()
		{
			RangeRegion region = new RangeRegion();

			if (Grid.Columns.Count == 0)
				return region;
			foreach (SgRange selectedRange in mList.GetSelectedRowRegions(0, Grid.Columns.Count))
			{
				region.Add(ValidateRange(selectedRange));
			}
			return region;
		}

		public override bool IntersectsWith(SgRange rng)
		{
			for (int r = rng.Start.Row; r <= rng.End.Row; r++)
			{
				if (IsSelectedRow(r))
					return true;
			}

			return false;
		}
	}
}
