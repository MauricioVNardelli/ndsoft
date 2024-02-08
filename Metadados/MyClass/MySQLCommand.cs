using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace Metadados
{ 

  public class MySQLCommand
  {
    private SqlCommand FSQLCommand;
    private DataTable FDataTable;

    private string FSQL = "";
    private string FTabela = "";
    private int FHandle = 0;
    private int FIndice = 0;
    private bool FInTransaction = false;
    private bool FIsInserting = false;
    private List<ColumnsToInsert> FListColumnsToInsert;

    public bool InTransaction { get => FInTransaction; }

    public MySQLCommand()
    {
      FSQLCommand = new SqlCommand();
      FSQLCommand.Connection = MySystem.GetConnection();

      FListColumnsToInsert = new List<ColumnsToInsert>();
    }

    public void BeginTransaction()
    {
      if (!FInTransaction)
      {
        SqlTransaction transaction = MySystem.GetConnection().BeginTransaction();

        FSQLCommand.Transaction = transaction;

        FInTransaction = true;
      }
    }

    public void Edit(string prTable, int prHandle)
    {
      FTabela = prTable;
      FHandle = prHandle;

      if (!FInTransaction)
      {
        this.BeginTransaction();
      }

      FSQLCommand.CommandText = $" UPDATE {prTable} SET HANDLE = HANDLE WHERE HANDLE = {prHandle}";
      FSQLCommand.ExecuteNonQuery();
    }

    public void Post()
    {
      if (FIsInserting && FListColumnsToInsert.Count >= 1)
        PrepareQueryForInsert();      
      
      else if (FIsInserting)
        PrepareQueryForInsertWithDataTable();       
      
      else
        PrepareQueryForUpdate();

      FSQLCommand.ExecuteNonQuery();

      if (FInTransaction)
      {
        this.CommitTransaction();
      }
    }

    private void PrepareQueryForInsertWithDataTable()
    {
      string vSQL = this.SQL + " ( ";
      int vCount = FDataTable.Columns.Count;

      foreach (DataColumn dataColumn in FDataTable.Columns)
      {
        if (!dataColumn.ColumnName.Contains("_TRANSLATION"))
        {
          if (vCount != 1)
            vSQL += dataColumn.ColumnName + ", ";
          else
            vSQL += dataColumn.ColumnName;
        }
      }

      vSQL += " ) VALUES ( ";

      for (int i = 0; i < FDataTable.Columns.Count - 1; i++)
      {
        if (!FDataTable.Columns[i].ColumnName.Contains("_TRANSLATION"))
        { 
          if (i != FDataTable.Columns.Count - 1)
            vSQL += $"VALUE" + i.ToString() + ", ";
          else
            vSQL += $"VALUE" + i.ToString();
        }
      }

      FSQLCommand.CommandText = vSQL;
      FSQLCommand.Parameters["VALUE0"].Value = this.NewHandle(FTabela);

      vCount = 1;
      foreach (DataColumn dataColumn in FDataTable.Columns)
      {
        if (!dataColumn.ColumnName.Contains("_TRANSLATION"))
        {
          FSQLCommand.Parameters[$"VALUE" + vCount.ToString()].Value = dataColumn.ToString();
        }
      }
    }

    private void PrepareQueryForInsert()
    {
      int vCount = 0;
      string vColumns = "";
      string vValues = "";

      foreach (ColumnsToInsert columnsToInsert in FListColumnsToInsert)
      {
        vCount++;

        if (vCount == 1)
        {
          vColumns += columnsToInsert.Field;
          vValues += "@VALUE" + vCount.ToString();
        }
        else
        {
          vColumns += ", " + columnsToInsert.Field;
          vValues += ", @VALUE" + vCount.ToString();
        }
      }

      this.SQL += " ( " + vColumns + " ) VALUES ( " + vValues + " ) ";

      vCount = 0;
      foreach (ColumnsToInsert columnsToInsert in FListColumnsToInsert)
      {
        vCount++;

        switch (columnsToInsert.TypeValue)
        {
          case TypeValue.Numeric:
            this.ParamByName("VALUE" + vCount.ToString()).AsFloat = float.Parse(columnsToInsert.Value);
            break;

          case TypeValue.Integer:
            this.ParamByName("VALUE" + vCount.ToString()).AsInt = int.Parse(columnsToInsert.Value);
            break;

          case TypeValue.DateTime:
            this.ParamByName("VALUE" + vCount.ToString()).AsDateTime = DateTime.Parse(columnsToInsert.Value);
            break;

          default:
            this.ParamByName("VALUE" + vCount.ToString()).AsString = columnsToInsert.Value;
            break;
        }
      }
    }

    private void PrepareQueryForUpdate()
    {
      string commandText = $" UPDATE {FTabela} SET ";
      int columnCount = 0;

      FSQLCommand.Parameters.Clear();

      foreach (DataColumn dataColumn in FDataTable.Columns)
      {
        if (dataColumn.ColumnName != "STATUS_IMAGE" && dataColumn.ColumnName != "HANDLE" && !dataColumn.ColumnName.Contains("_TRANSLATION"))
        {
          columnCount++;

          if (columnCount == 1)
            commandText += dataColumn.ColumnName + $" = @VALUE{columnCount}";
          else
            commandText += ", " + dataColumn.ColumnName + $" = @VALUE{columnCount}";
        }
      }

      commandText += $" WHERE HANDLE = {FHandle} ";
      FSQLCommand.CommandText = commandText;

      columnCount = 0;
      foreach (DataColumn dataColumn in FDataTable.Columns)
      {
        if (dataColumn.ColumnName != "STATUS_IMAGE" && dataColumn.ColumnName != "HANDLE" && !dataColumn.ColumnName.Contains("_TRANSLATION"))
        {
          columnCount++;
          string param = $"VALUE{columnCount}";

          if (FDataTable.Rows[0][dataColumn.ColumnName].ToString() == "")
            FSQLCommand.Parameters.AddWithValue(param, DBNull.Value);
          else
            FSQLCommand.Parameters.AddWithValue(param, FDataTable.Rows[0][dataColumn.ColumnName].ToString());
        }
      }
    }

    public void CommitTransaction()
    {
      if (FInTransaction)
      {
        FSQLCommand.Transaction.Commit();

        FInTransaction = false;
      }
    }

    public void RollbackTransaction()
    {
      if (FInTransaction)
      {
        FSQLCommand.Transaction.Rollback();

        FInTransaction = false;
      }
    }

    public string SQL
    {
      get { return FSQL; }
      set
      {
        FSQLCommand.Parameters.Clear();

        FSQL = value;

        FSQLCommand.CommandText = value;
      }
    }

    public int ExecuteDML()
    {
      return FSQLCommand.ExecuteNonQuery();
    }    

    public DataTable DataTable
    {
      set
      {
        FDataTable = value;
      }
      get
      {
        return FDataTable;
      }
    }

    public SqlCommand Command
    {
      get
      {
        return FSQLCommand;
      }
    }

    public Param ParamByName(string prName)
    {
      Param param = new Param(FSQLCommand, prName);

      return param;
    }

    public MyDataRow FieldByName(string prFieldName)
    {
      DataRow dataRow = FDataTable.Rows[FIndice];

      return new MyDataRow(FDataTable.Rows[FIndice], prFieldName);
    }

    public void AddWithValue(string prFieldName, int value)
    {
      ColumnsToInsert columnsToInsert = new ColumnsToInsert { Field = prFieldName, Value = value.ToString(), TypeValue = TypeValue.Integer };
      FListColumnsToInsert.Add(columnsToInsert);
    }

    public void AddWithValue(string prFieldName, float value)
    {
      ColumnsToInsert columnsToInsert = new ColumnsToInsert { Field = prFieldName, Value = value.ToString(), TypeValue = TypeValue.Numeric };
      FListColumnsToInsert.Add(columnsToInsert);
    }

    public void AddWithValue(string prFieldName, DateTime value)
    {
      ColumnsToInsert columnsToInsert = new ColumnsToInsert { Field = prFieldName, Value = value.ToString(), TypeValue = TypeValue.DateTime };
      FListColumnsToInsert.Add(columnsToInsert);
    }

    public void AddWithValue(string prFieldName, string value)
    {
      ColumnsToInsert columnsToInsert = new ColumnsToInsert { Field = prFieldName, Value = value.ToString(), TypeValue = TypeValue.String };
      FListColumnsToInsert.Add(columnsToInsert);
    }

    public void AddWithValue(string prFieldName, bool value)
    {
      string vValue = (value) ? "S" : "N";

      ColumnsToInsert columnsToInsert = new ColumnsToInsert { Field = prFieldName, Value = vValue, TypeValue = TypeValue.String };
      FListColumnsToInsert.Add(columnsToInsert);
    }

    public void Insert(string prTable)
    {
      FListColumnsToInsert.Clear();

      this.SQL = "INSERT " + prTable;
      int newHandle = NewHandle(prTable);

      ColumnsToInsert columnsToInsert = new ColumnsToInsert { Field = "HANDLE", Value = newHandle.ToString(), TypeValue = TypeValue.Integer };
      FListColumnsToInsert.Add(columnsToInsert);

      FIsInserting = true;
    }

    public void InsertWithDataTable(string prTable)
    {
      this.SQL = "INSERT " + prTable;
      FIsInserting = true;
    }

    public int NewHandle(string prTable)
    {
      MySQLQuery cmd = new MySQLQuery();
      cmd.SQL = " SELECT COALESCE(MAX(HANDLE), 0) HANDLE FROM " + prTable;
      cmd.Command.Transaction = FSQLCommand.Transaction;
      cmd.Execute();

      return cmd.FieldByName("HANDLE").AsInt() + 1;
    }
  }
}
