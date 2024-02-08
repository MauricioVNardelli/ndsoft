using Metadados;

namespace Principal
{
  public partial class FormMain : Form
  {
    public FormMain()
    {
      InitializeComponent();
    }

    private void OpenForm(string prClassName, string prTableName)
    {
      var type = Type.GetType(prClassName);

      if (type != null)
      {
        FormManagement? frmModel = Activator.CreateInstance(type) as FormManagement;

        if (frmModel != null)
        {
          frmModel.Show(this.pnlManagement, prTableName, prClassName);
        }
        else
          throw new Exception("Table class not found! " + Environment.NewLine + "Class: " + prClassName);
      }
    }

    private void PessoaToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.OpenForm("Fiscal.ManagementFI_PESSOA, Fiscal", "FI_PESSOA");
    }

    private void btnFormModel_Click(object sender, EventArgs e)
    {
      FormExample formExample = new FormExample();
      formExample.ShowDialog();
    }
  }
}