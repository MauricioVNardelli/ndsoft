using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Metadados
{
  public partial class MyDataGridView : DataGridView
  {
    private string FManagementFormName;
    private int FManagementFormHandle;
    private MySQLQuery FMySQLQuery;
    private int FIndexColumHeaderClick;

    ContextMenuStrip FHeaderContextMenuStrip;

    public MyDataGridView(string prManagementFormName)
    {
      FMySQLQuery = new MySQLQuery();
      FManagementFormName = prManagementFormName;

      ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
      contextMenuStrip.Items.Add("Salvar");
      contextMenuStrip.Items[0].Click += Save_Click;

      FHeaderContextMenuStrip = new ContextMenuStrip();
      FHeaderContextMenuStrip.Items.Add("Ocultar");
      FHeaderContextMenuStrip.Items[0].Click += HeaderCell_Click;

      this.ContextMenuStrip = contextMenuStrip;
      this.CellDoubleClick += Double_Click;
      this.CellMouseDown += CellMouseDown_Click;

      //Template grid
      this.RowTemplate.Height = 20;
      this.GridColor = Color.Gray;
      this.RowHeadersVisible = false;
      this.AllowUserToResizeRows = false;
      this.AllowUserToAddRows = false;
      this.AllowUserToDeleteRows = false;

      MySQLQuery mySQLCommand = new MySQLQuery();
      mySQLCommand.SQL = " SELECT HANDLE FROM MD_TELAGERENCIAMENTO WHERE NOME = @VALOR ";
      mySQLCommand.ParamByName("VALOR").AsString = prManagementFormName;
      mySQLCommand.Execute();

      FManagementFormHandle = mySQLCommand.FieldByName("HANDLE").AsInt();
    }

    private void CellMouseDown_Click(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right && e.RowIndex == -1) // Verifica se o clique ocorreu no cabeçalho da coluna
      {
        FIndexColumHeaderClick = e.ColumnIndex;
      }
    }

    private void Double_Click(object sender, EventArgs e)
    {
      var type = Type.GetType("Fiscal.Form" + FManagementFormName + ", Fiscal");

      int rowIndex = this.SelectedCells[0].RowIndex;

      if (type != null)
      {
        var frmModel = Activator.CreateInstance(type) as FormModel;

        if (frmModel != null)
        {
          int vHandle = (int)this.Rows[rowIndex].Cells["HANDLE"].Value;
          frmModel.ShowModal(vHandle);

          FMySQLQuery.Refresh();
          this.Refresh();
        }
        else
          throw new Exception("Table class not found! " + Environment.NewLine + "Class: " + FManagementFormName);
      }
    }

    private void HeaderCell_Click(object sender, EventArgs e)
    {
      MySQLQuery mySQLCommand = new MySQLQuery();
      mySQLCommand.SQL = " UPDATE MD_TELAGERENCIAMENTOCAMPO " +
                         "    SET EHVISIVEL = @VALOR " +
                         "  WHERE TELAGERENCIAMENTO = " + FManagementFormHandle.ToString() +
                         "    AND NOME = @COLUMNNAME ";
      mySQLCommand.ParamByName("VALOR").AsString = "N";
      mySQLCommand.ParamByName("COLUMNNAME").AsString = this.Columns[FIndexColumHeaderClick].Name;
      mySQLCommand.Execute();

      ConfigureColumn();
    }

    private void Save_Click(object sender, EventArgs e)
    {
      MySQLCommand mySQLCommand = new MySQLCommand();
      MySQLQuery mySQLQuery = new MySQLQuery();

      mySQLQuery.SQL = " SELECT HANDLE FROM MD_TELAGERENCIAMENTO WHERE NOME = @NOME ";
      mySQLQuery.ParamByName("NOME").AsString = FManagementFormName;
      mySQLQuery.Execute();

      int vHandle = mySQLQuery.FieldByName("HANDLE").AsInt();

      mySQLCommand.BeginTransaction();
      try
      {
        mySQLCommand.SQL = " DELETE MD_TELAGERENCIAMENTOCAMPO " +
                           "  WHERE TELAGERENCIAMENTO = @VALOR ";
        mySQLCommand.ParamByName("VALOR").AsInt = vHandle;
        mySQLCommand.ExecuteDML();

        foreach (DataGridViewColumn column in this.Columns)
        {
          mySQLCommand.Insert("MD_TELAGERENCIAMENTOCAMPO");
          mySQLCommand.AddWithValue("INDICECOLUNA", column.Index);
          mySQLCommand.AddWithValue("NOME", column.Name);
          mySQLCommand.AddWithValue("DESCRICAO", column.HeaderText);
          mySQLCommand.AddWithValue("TELAGERENCIAMENTO", vHandle);
          mySQLCommand.AddWithValue("LARGURA", column.Width);
          mySQLCommand.AddWithValue("EHVISIVEL", column.Visible);
          mySQLCommand.Post();
        }

        mySQLCommand.CommitTransaction();
      } catch { 
        mySQLCommand.RollbackTransaction();
      }      
    }

    public string SQL
    {
      set
      {
        if (value != "")
        {
          FMySQLQuery.SQL = value;
          FMySQLQuery.Execute();

          this.DataSource = FMySQLQuery.DataTable;
          this.Refresh();

          ConfigureColumn();
        }
      }
    }

    private void ConfigureColumn()
    {
      MySQLQuery myCommand = new MySQLQuery();
      myCommand.SQL = " SELECT A.* " +
                      "   FROM MD_TELAGERENCIAMENTOCAMPO A " +
                      "  WHERE A.TELAGERENCIAMENTO = " + FManagementFormHandle.ToString();
      myCommand.Execute();

      while (!myCommand.Eof())
      {
        DataGridViewColumn vColumn = this.Columns[myCommand.FieldByName("NOME").AsString()];

        if (vColumn.Name == "STATUS_IMAGE")
        {
          vColumn.Width = 15;
          vColumn.HeaderText = "";
        }
        else
        {
          vColumn.Width = myCommand.FieldByName("LARGURA").AsInt();
          vColumn.HeaderText = myCommand.FieldByName("DESCRICAO").AsString();
          vColumn.Visible = myCommand.FieldByName("EHVISIVEL").AsBool();

          if (vColumn.ValueType == typeof(int) || vColumn.ValueType == typeof(float) || vColumn.ValueType == typeof(DateTime))
          {
            vColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
          }

          if (vColumn.HeaderCell.ContextMenuStrip != FHeaderContextMenuStrip)
          {
            vColumn.HeaderCell.ContextMenuStrip = FHeaderContextMenuStrip;
          }
        }

        myCommand.Next();
      }
    }
  }
}
