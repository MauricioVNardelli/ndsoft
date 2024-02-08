using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadados
{
  internal class MySQL
  {
  }

  public class Param
  {
    private string FNameParam;
    private SqlCommand FSQLCommand;

    public Param(SqlCommand prSQLCommand, string prNameParam)
    {
      FSQLCommand = prSQLCommand;
      FNameParam = prNameParam;
    }

    public string AsString
    {
      set
      {
        FSQLCommand.Parameters.Add("@" + FNameParam, SqlDbType.VarChar);
        FSQLCommand.Parameters["@" + FNameParam].IsNullable = true;
        FSQLCommand.Parameters["@" + FNameParam].Value = value;
      }
    }

    public DateTime AsDateTime
    {
      set
      {
        FSQLCommand.Parameters.Add("@" + FNameParam, SqlDbType.DateTime);
        FSQLCommand.Parameters["@" + FNameParam].IsNullable = true;
        FSQLCommand.Parameters["@" + FNameParam].Value = value;
      }
    }

    public void AsNull(SqlDbType sqlDbType)
    {
      FSQLCommand.Parameters.Add("@" + FNameParam, sqlDbType);
      FSQLCommand.Parameters["@" + FNameParam].IsNullable = true;

      if (sqlDbType == SqlDbType.Int)
        FSQLCommand.Parameters["@" + FNameParam].Value = null;
      else
        FSQLCommand.Parameters["@" + FNameParam].Value = null;
    }

    public int AsInt
    {
      set
      {
        FSQLCommand.Parameters.Add("@" + FNameParam, SqlDbType.Int);
        FSQLCommand.Parameters["@" + FNameParam].IsNullable = true;
        FSQLCommand.Parameters["@" + FNameParam].Value = value;
      }
    }

    public bool AsBool
    {
      set
      {
        FSQLCommand.Parameters.Add("@" + FNameParam, SqlDbType.VarChar);
        FSQLCommand.Parameters["@" + FNameParam].IsNullable = true;

        string vValue = (value) ? vValue = "S" : vValue = "N";

        FSQLCommand.Parameters["@" + FNameParam].Value = vValue;
      }
    }

    public float AsFloat
    {
      set
      {
        FSQLCommand.Parameters.Add("@" + FNameParam, SqlDbType.Float);
        FSQLCommand.Parameters["@" + FNameParam].IsNullable = true;
        FSQLCommand.Parameters["@" + FNameParam].Value = value;
      }
    }
  }

  public class MyDataRow
  {
    private DataRow FDataRow;
    private string FField;

    public MyDataRow(DataRow prDataRow, string prField)
    {
      FDataRow = prDataRow;
      FField = prField;
    }

    public string AsString()
    {
      return FDataRow[FField].ToString();
    }

    public int AsInt()
    {
      return Convert.ToInt32(FDataRow[FField].ToString());
    }

    public DateTime AsDateTime()
    {
      return Convert.ToDateTime(FDataRow[FField].ToString());
    }

    public DateTime AsDate()
    {
      DateTime dt = Convert.ToDateTime(FDataRow[FField].ToString());

      return new DateTime(dt.Year, dt.Month, dt.Day);
    }

    public double AsFloat()
    {
      return Convert.ToDouble(FDataRow[FField].ToString());
    }

    public bool AsBool()
    {
      if (FDataRow[FField].ToString() == "S")
      {
        return true;
      }
      else
        return false;
    }
  }

  public enum TypeValue
  {
    Integer,
    String,
    Numeric,
    DateTime
  }

  public struct ColumnsToInsert
  {
    public string Field;
    public string Value;
    public TypeValue TypeValue;
  }
}
