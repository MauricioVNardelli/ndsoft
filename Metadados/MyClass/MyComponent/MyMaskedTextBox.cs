using Metadados.MyClass;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Metadados
{
  public class MyMaskedTextBox : MaskedTextBox
  {
    private Label FLabel;

    public MyMaskedTextBox(string prTitle, string prField, DataSet prDataSet, TypeTextBox prType)
    {
      FLabel = new Label();
      FLabel.Text = prTitle;
      FLabel.AutoSize = true;

      this.DataBindings.Add("Text", prDataSet, prField);
      this.TextAlign = HorizontalAlignment.Center;

      switch (prType)
      {
        case TypeTextBox.Date: 
          this.Mask = "00/00/0000"; 
          this.Width = 75;
        break;
        
        case TypeTextBox.DateTime: 
          this.Mask = "00/00/0000 90:00";
          this.Width = 95;
        break;

        case TypeTextBox.CPF:
          this.Mask = "000,000,000-00";
          this.Width = 95;
          break;

        case TypeTextBox.CNPJ:
          this.Mask = "00,000,000/0000-00";
          this.Width = 130;
          break;
      }
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
  }
}
