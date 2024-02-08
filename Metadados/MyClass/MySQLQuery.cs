using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace Metadados
{
  public class MySQLQuery
  {
    private SqlCommand FSQLCommand;
    private DataTable FDataTable;
    
    private string FSQL = "";
    private int FIndice = 0;    

    public MySQLQuery()
    {
      FSQLCommand = new SqlCommand();
      FSQLCommand.Connection = MySystem.GetConnection();
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

    public void Execute()
    {
      SqlDataReader sqlDataReader = FSQLCommand.ExecuteReader();

      FDataTable = new DataTable();

      if (FDataTable.Columns["STATUS_IMAGE"] == null) 
        FDataTable.Columns.Add("STATUS_IMAGE", typeof(string));
        
      FDataTable.Load(sqlDataReader);

      sqlDataReader.Close();

      FIndice = 0;
    }

    public void Refresh()
    {
      SqlDataReader sqlDataReader = FSQLCommand.ExecuteReader();
      
      FDataTable.Clear();
      FDataTable.Load(sqlDataReader);

      sqlDataReader.Close();

      FIndice = 0;
    }

    public DataTable DataTable
    {
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

    public int RecordCount
    {
      get
      {
        return this.FDataTable.Rows.Count;
      }
    }

    public int CurrentReg()
    {
      return FIndice;
    }

    public bool Eof()
    {
      return FIndice > RecordCount - 1;
    }

    public void Next()
    {
      FIndice++;
    }

    public void First()
    {
      FIndice = 0;
    }

    public MyDataRow FieldByName(string prFieldName)
    {
      DataRow dataRow = FDataTable.Rows[FIndice];

      return new MyDataRow(dataRow, prFieldName);
    }

    public int NewHandle(string prTable, SqlTransaction prTransaction)
    {
      MySQLQuery cmd = new MySQLQuery();
      cmd.SQL = " SELECT MAX(HANDLE) HANDLE FROM " + prTable;
      cmd.Command.Transaction = prTransaction;
      cmd.Execute();

      return cmd.FieldByName("HANDLE").AsInt() + 1;
    }
  }
}
