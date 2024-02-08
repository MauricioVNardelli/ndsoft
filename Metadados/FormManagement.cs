using System;
using System.Drawing;
using System.Windows.Forms;

namespace Metadados
{
  public abstract class FormManagement
  {
    string FTable;
    private string FClassName;

    Form FForm;
    MyDataGridView FDataGridView;    

    public FormManagement()
    {
      FForm = new Form();
      FClassName = string.Empty;
    }

    public void Show(Control prSender, string prTableName, string prClassName)
    {
      FTable = prTableName;
      FClassName = prClassName;

      FForm.TopLevel = false;
      FForm.FormBorderStyle = FormBorderStyle.None;
      FForm.Dock = DockStyle.Fill;
      FForm.Parent = prSender;

      ConfigureForm(prTableName);

      FForm.Show();
    }

    private void ConfigureForm(string prTableName)
    {
      TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
      tableLayoutPanel.Dock = DockStyle.Fill;
      tableLayoutPanel.RowCount = 2;
      tableLayoutPanel.Parent = FForm;
      tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60)); // Altura do primeiro Panel
      tableLayoutPanel.RowStyles.Add(new RowStyle()); //  Retante do espaço disponível

      Panel panel1 = new Panel();
      panel1.Dock = DockStyle.Fill;

      Button buttonClose = new Button();
      buttonClose.Text = "Fechar";
      buttonClose.Parent = panel1;
      buttonClose.Dock = DockStyle.Right;
      buttonClose.Click += ButtonClose_Click;

      Button buttonInsert = new Button();
      buttonInsert.Text = "Inserir";
      buttonInsert.Parent = panel1;
      buttonInsert.Dock = DockStyle.Left;
      buttonInsert.Click += ButtonInsert_Click;

      Panel panel2 = new Panel();
      panel2.Dock = DockStyle.Fill;

      tableLayoutPanel.Controls.Add(panel1, 0, 0);
      tableLayoutPanel.Controls.Add(panel2, 0, 1);

      FDataGridView = new MyDataGridView(prTableName);
      FDataGridView.Parent = panel2;
      FDataGridView.Dock = DockStyle.Fill;
      FDataGridView.SQL = GetSQL();
      FDataGridView.BackgroundColor = SystemColors.Window;
      FDataGridView.BorderStyle = BorderStyle.None;
      FDataGridView.ReadOnly = true;
    }

    private void ButtonInsert_Click(object sender, EventArgs e)
    {
      var type = Type.GetType(FClassName);
      string vNamespace = type.Namespace;

      var typeForm = Type.GetType($"{vNamespace}.Form" + FTable + $", {vNamespace}");

      if (type != null)
      {
        var frmModel = Activator.CreateInstance(typeForm) as FormModel;

        if (frmModel != null)
        {
          frmModel.ShowModal(0);
        }
        else
          throw new Exception("Table class not found! " + Environment.NewLine + "Class: " + FTable);
      }
    }

    private void ButtonClose_Click(object sender, EventArgs e)
    {
      FForm.Close();
    }

    protected abstract string GetSQL();
  }
}
