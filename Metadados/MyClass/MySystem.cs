using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Metadados
{
  public static class MySystem
  {
    private static SqlConnection FConnection;

    public static SqlConnection GetConnection()
    {
      if (FConnection == null)
      {
        string fileContent = File.ReadAllText("C:/Users/mauri/source/repos/NDSoft/Principal/config.json");
        Config loadedConfig = JsonSerializer.Deserialize<Config>(fileContent);

        string password = Encoding.UTF8.GetString(System.Convert.FromBase64String(loadedConfig.Password));
        string connectionString = $"Data Source={loadedConfig.Server};Initial Catalog={loadedConfig.DataBase};UID={loadedConfig.User};PWD={password};";

        FConnection = new SqlConnection();
        FConnection.ConnectionString = connectionString;
        FConnection.Open();
      }

      return FConnection;
    }

    public static class Status
    {
      public const int Cadastrado = 1;
      public const int Encerrado = 2;
      public const int Ativo = 3;
      public const int Cancelado = 4;
      public const int EmExecucao = 5;
      public const int AgModificacao = 6;
    }
  }
}
