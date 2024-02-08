using Metadados.MyClass;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Metadados
{
  public class MyTextTable : TextBox, IMyControl
  {
    private Label FLabel;
    private Button FBtnSearch;

    public MyTextTable(string prTitle, string prField, DataSet prDataSet)
    {
      this.DataBindings.Add("Text", prDataSet, prField);

      FLabel = new Label();
      FLabel.Text = prTitle;
      FLabel.AutoSize = true; 
      
      FBtnSearch = new Button();
      FBtnSearch.Width = 20;
      FBtnSearch.FlatStyle = FlatStyle.Flat;
      FBtnSearch.BackColor = Color.White;
      FBtnSearch.ForeColor = Color.Black;
      FBtnSearch.FlatAppearance.BorderColor = Color.Gray;
      FBtnSearch.FlatAppearance.BorderSize = 0;
      FBtnSearch.Dock = DockStyle.Right;
      FBtnSearch.TextAlign = ContentAlignment.MiddleCenter;
      FBtnSearch.Image = Image.FromFile("C:\\Users\\mauri\\source\\repos\\NDSoft\\Metadados\\src\\search_enable.png");
      FBtnSearch.Click += FBtnSearch_Click;
    }

    private void FBtnSearch_Click(object sender, EventArgs e)
    {
      throw new NotImplementedException();    }

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

      FBtnSearch.Left = x + width;
      FBtnSearch.Top = y;

      base.SetBoundsCore(x, y, width, height, specified);
    }

    protected override void OnParentChanged(EventArgs e)
    {
      base.OnParentChanged(e);

      if (this.Parent != null)
      {
        FLabel.Parent = this.Parent;
        FBtnSearch.Parent = this;
      }
    }

    public int PercentageWidth { set; get; }
  }
}
