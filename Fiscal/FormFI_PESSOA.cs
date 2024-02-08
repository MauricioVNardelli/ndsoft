using Metadados;

namespace Fiscal
{
  public class FormFI_PESSOA : FormModel
  {
    public FormFI_PESSOA()
    {
      //
    }

    protected override void ConfigureForm()
    {
      AddComponentTextBox("NOME", "Nome", 1, "");

      AddComponentTextTable("TIPO", "Tipo", "FI_TIPOPESSOA", "NOME", 1, "COMPLEMENTO", 50);
      AddComponentTextMask("CPFCNPJ", "CPF", TypeTextBox.CPF , 1, "COMPLEMENTO");
      AddComponentTextMask("DATANASCIMENTO", "Nascimento", TypeTextBox.Date, 1, "COMPLEMENTO");      
    }
  }
}
