using Metadados;

namespace Fiscal
{
  internal class ManagementFI_PESSOA : FormManagement
  {
    //
    protected override string GetSQL()
    {
      return " SELECT A.HANDLE, " +
             "        A.STATUS, " +
             "        A.NOME,  " +
             "        A.CPFCNPJ, " +
             "        B.NOME TIPO  " +
             "   FROM FI_PESSOA A " +
             "   LEFT JOIN FI_TIPOPESSOA B ON B.HANDLE = A.TIPO ";
    }
  }
}
