using Metadados.MyClass;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Metadados
{
  public class MyTextBox : TextBox, IMyControl
  {
    private Label FLabel;

    public MyTextBox(string prTitle, string prField, DataSet prDataSet)
    {
      this.DataBindings.Add("Text", prDataSet, prField);

      FLabel = new Label();
      FLabel.Text = prTitle;
      FLabel.AutoSize = true;
    }

    protected override void OnReadOnlyChanged(EventArgs e)
    {
      base.BackColor = SystemColors.Window;

      if (base.ReadOnly)
      {
        base.ForeColor = Color.Gray;
      }
      else
      {
        base.ForeColor = Color.Black;
      }
    }

    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
      FLabel.Top = y - 14;
      FLabel.Left = x;

      base.SetBoundsCore(x, y, width, height, specified);
    }

    protected override void OnParentChanged(EventArgs e)
    {
      base.OnParentChanged(e);

      if (this.Parent != null)
      {
        FLabel.Parent = this.Parent;
      }
    }

    public int PercentageWidth { set; get; }
  }
}
