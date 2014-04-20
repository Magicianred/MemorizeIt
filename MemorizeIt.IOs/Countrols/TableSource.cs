using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using MemorizeIt.Model;
using System.Linq;

namespace MemorizeIt.IOs.Controls
{
	public class TableSource : UITableViewSource {
		protected IList<DictionaryItem> tableItems;
		protected string cellIdentifier = "TableCell";
		public TableSource (IEnumerable<DictionaryItem> items)
		{
			tableItems = items.ToList();
		}
		public override int RowsInSection (UITableView tableview, int section)
		{
			return tableItems.Count;
		}
		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			// request a recycled cell to save memory
			UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
			// if there are no cells to reuse, create a new one
			if (cell == null) {
				cell = new UITableViewCell (UITableViewCellStyle.Value1, cellIdentifier);
				cell.SelectionStyle = UITableViewCellSelectionStyle.None;
			}
			var t = tableItems [indexPath.Row];
			cell.TextLabel.Text = string.Format ("{1}({0})", t.SuccessCount, t.Values [0]);/*, t.Values[1]);*/
			cell.DetailTextLabel.Text = t.Values [1];
			return cell;
		}
	}
}

