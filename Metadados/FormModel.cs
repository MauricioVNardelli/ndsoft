using Metadados.MyClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Metadados
{
  public enum TypeTextBox
  {
    Text,
    Date,
    DateTime,
    CPF,
    CNPJ
  }

  public abstract class FormModel
  {
    //--Private
    private Form FPrincipal;

    private Panel FPanelTop;
    private Panel FPanelFill;
    private TabControl FTabControl;
    private TabPage FTabComplement;
    private Panel FPanelBottom;

    private Button FButtonSave;
    private Button FButtonClose;
    private Button FButtonEdit;

    private MySQLCommand FSQLCommand = new MySQLCommand();
    private MySQLQuery FSQLQuery = new MySQLQuery();
    private DataSet FDataSet = new DataSet();        

    private readonly Dictionary<string, List<Control>> FListControlLine = new Dictionary<string, List<Control>>();
    private readonly Dictionary<string, Control> FListControlTab = new Dictionary<string, Control>();
    private Dictionary<string, string> FListSQLJoinTranslation = new Dictionary<string, string>();

    private string FTable;
    private string FSQLFieldTranslation = "";

    //--Protected
    protected bool IsEditing = false;
    protected bool IsInserting = false;
    protected int Handle;

    protected abstract void ConfigureForm();

    public FormModel()
    {
      FPrincipal = new Form();
      FPrincipal.Size = new Size(678, 342);
      FPrincipal.Name = "FormModel";
      FPrincipal.MaximizeBox = false;
      FPrincipal.MinimizeBox = false;
      FPrincipal.StartPosition = FormStartPosition.CenterScreen;
      FPrincipal.FormBorderStyle = FormBorderStyle.FixedSingle;
      FPrincipal.KeyPreview = true;

      string className = this.ToString();
      FTable = className.Substring(className.IndexOf("_", 0, className.Length) - 2);

      FPrincipal.KeyPress += FPrincipal_KeyPress;

      ConfigureComponentForm();
      ConfigureForm();
      ConfigureComponentSize();      
    }

    private void FPrincipal_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == 27) //ESC
        this.ButtonClose_Click(sender, null);
    }

    public void ShowModal(int prHandle)
    {
      FSQLQuery.SQL = $" SELECT DESCRICAO FROM MD_TABELA WHERE NOME = @NAME ";
      FSQLQuery.ParamByName("NAME").AsString = FTable;
      FSQLQuery.Execute();

      FPrincipal.Text = FSQLQuery.FieldByName("DESCRICAO").AsString();

      string vSQL = " SELECT A.* ";

      if (FSQLFieldTranslation != "")
      {
        vSQL += ", " + FSQLFieldTranslation;
      }

      vSQL += $" FROM {FTable} A ";

      foreach (var listValue in FListSQLJoinTranslation)
      {
        vSQL += listValue.Value;
      }

      vSQL += $" WHERE A.HANDLE = {prHandle} ";

      FSQLQuery.SQL = vSQL;
      FSQLQuery.Execute();
      FSQLQuery.DataTable.TableName = FTable;

      FDataSet.Tables.Add(FSQLQuery.DataTable);

      this.Handle = prHandle;

      if (this.Handle == 0)
      {
        IsInserting = true;
        FSQLCommand.InsertWithDataTable(FTable);        
      }
      
      FSQLCommand.DataTable = FSQLQuery.DataTable;

      ConfigureFormPermission();

      FPrincipal.ShowDialog();
    }

    private void ConfigureComponentForm()
    {
      FPanelFill = new Panel();
      FPanelFill.Parent = FPrincipal;
      FPanelFill.Dock = DockStyle.Fill;
      FPanelFill.Padding = new Padding(13, 0, 13, 0);

      FPanelTop = new Panel();
      FPanelTop.Parent = FPrincipal;
      FPanelTop.Dock = DockStyle.Top;
      FPanelTop.Height = 50;
      FPanelTop.Padding = new Padding(15, 20, 15, 10);

      FPanelBottom = new Panel();
      FPanelBottom.Parent = FPrincipal;
      FPanelBottom.Dock = DockStyle.Bottom;
      FPanelBottom.Height = 50;
      FPanelBottom.Padding = new Padding(15, 10, 15, 10);

      FTabControl = new TabControl();
      FTabControl.Parent = FPanelFill;
      FTabControl.Dock = DockStyle.Fill;
      FTabControl.Parent = FPanelFill;

      FTabComplement = new TabPage();
      FTabComplement.Text = "Complemento";
      FTabComplement.Name = "COMPLEMENT";
      FTabComplement.Parent = FTabControl;
      FTabComplement.UseVisualStyleBackColor = true;

      FListControlTab.Add("COMPLEMENTO", FTabComplement);

      FButtonSave = new Button();
      FButtonSave.Parent = FPanelBottom;
      FButtonSave.Text = "Gravar";
      FButtonSave.Height = 30;
      FButtonSave.Dock = DockStyle.Left;
      FButtonSave.Click += ButtonSave_Click;

      FButtonClose = new Button();
      FButtonClose.Parent = FPanelBottom;
      FButtonClose.Text = "Fechar";
      FButtonClose.Height = 30;
      FButtonClose.Dock = DockStyle.Right;
      FButtonClose.Click += ButtonClose_Click;

      FButtonEdit = new Button();
      FButtonEdit.Parent = FPanelBottom;
      FButtonEdit.Text = "Editar";
      FButtonEdit.Height = 30;
      FButtonEdit.Dock = DockStyle.Left;
      FButtonEdit.Click += ButtonEdit_Click;
    }

    private void ButtonEdit_Click(object sender, EventArgs e)
    {
      FSQLCommand.Edit(FTable, Handle);
      
      IsEditing = true;
      ConfigureFormPermission();
    }

    private void ButtonSave_Click(object sender, EventArgs e)
    {      
      if (FSQLCommand.InTransaction)
      {
        FSQLCommand.Post();
      }
      
      IsEditing = false;
      ConfigureFormPermission();
    }

    private void ButtonClose_Click(object sender, EventArgs e)
    {
      FPrincipal.Close();
    }

    private void ConfigureFormPermission()
    {
      bool vCanReadOnly = !IsEditing && !IsInserting;

      foreach (KeyValuePair<string, List<Control>> entry in FListControlLine)
      {
        List<Control> vListLine = entry.Value;

        foreach (Control myControl in vListLine)
        {
          if (myControl is MyTextBox)
          {
            MyTextBox myTxtBox = (MyTextBox)myControl;
            myTxtBox.ReadOnly = vCanReadOnly;
          }
          else
          if (myControl is MyMaskedTextBox)
          {
            MyMaskedTextBox myMskTxtBox = (MyMaskedTextBox)myControl;
            myMskTxtBox.ReadOnly = vCanReadOnly;
          }
        }
      }

      FButtonEdit.Visible = !IsEditing && !IsInserting;
      FButtonSave.Visible = IsEditing || IsInserting;
    }

    protected void AddComponentTextBox(string prFieldName, string prTitle, int prLine, string prTabPage)
    {
      Control parent;

      if (!FListControlTab.TryGetValue(prTabPage, out parent))
      {
        parent = FPanelTop;
      }      

      MyTextBox myTxtBox = new MyTextBox(prTitle, FTable + "." + prFieldName, FDataSet);
      myTxtBox.Parent = parent;
      myTxtBox.ReadOnly = true;
      myTxtBox.PercentageWidth = 100;
      myTxtBox.Top = GetTopComponent(prLine, prTabPage, myTxtBox);
    }

    protected void AddComponentTextMask(string prFieldName, string prTitle, TypeTextBox prType, int prLine, string prTabPage)
    {
      Control parent;

      if (!FListControlTab.TryGetValue(prTabPage, out parent))
      {
        parent = FPanelTop;
      }

      MyMaskedTextBox myMaskTxtBox = new MyMaskedTextBox(prTitle, FTable + "." + prFieldName, FDataSet, prType);
      myMaskTxtBox.Parent = parent;
      myMaskTxtBox.ReadOnly = true;
      myMaskTxtBox.Top = GetTopComponent(prLine, prTabPage, myMaskTxtBox);
    }

    protected void AddComponentTextTable(string prFieldName, string prTitle, string prTable, string prFieldTranslation, int prLine, string prTabPage, int prPercentageWidth = 0)
    {
      Control parent;

      if (!FListControlTab.TryGetValue(prTabPage, out parent))
      {
        parent = FPanelTop;
      }

      string vLeftJoinSQL;
      string vKeyList = prTable + "." + prFieldName;
      string vAlias = GetAlias(FListSQLJoinTranslation.Count + 1);

      FListSQLJoinTranslation.TryGetValue(vKeyList, out vLeftJoinSQL);
           
      if (vLeftJoinSQL == null)
      {
        vLeftJoinSQL = $" LEFT JOIN {prTable} {vAlias} ON {vAlias}.HANDLE = A.{prFieldName} ";

        FListSQLJoinTranslation.Add(vKeyList, vLeftJoinSQL);
      }

      string fieldNameDataSet = prFieldName + "_TRANSLATION";

      if (FSQLFieldTranslation != "")
        FSQLFieldTranslation += ", ";

      FSQLFieldTranslation += vAlias + "." + prFieldTranslation + " " + fieldNameDataSet;

      MyTextTable myTxtTable = new MyTextTable(prTitle, FTable + "." + fieldNameDataSet, FDataSet);
      myTxtTable.Parent = parent;
      myTxtTable.ReadOnly = true;
      myTxtTable.PercentageWidth = prPercentageWidth;
      myTxtTable.Top = GetTopComponent(prLine, prTabPage, myTxtTable);      
    }

    private string GetAlias(int prNumber)
    {
      switch(prNumber)
      {
        case 1: return "B";
        case 2: return "C";
        case 3: return "D";
        case 4: return "E";
        case 5: return "F";
        case 6: return "G";
        case 7: return "H";
        case 8: return "I";
        case 9: return "J";
        case 10: return "K";
        case 11: return "L";
        case 12: return "M";
        case 13: return "N";
        case 14: return "O";

        default: return "0";
      }
    }

    protected void AddTabPage(string prName, string prTitle)
    {
      TabPage vNewTabPage = new TabPage();

      vNewTabPage.Text = prTitle;
      vNewTabPage.Name = prName;
      vNewTabPage.Parent = FTabControl;
      vNewTabPage.UseVisualStyleBackColor = true;

      FListControlTab.Add(prName, vNewTabPage);
    }

    private int GetTopComponent(int prLinha, string prTabPage, Control prComponente)
    {
      string vKey = prLinha.ToString() + "|" + prTabPage;
      List<Control> vListControl;

      if (!FListControlLine.ContainsKey(vKey))
      {
        vListControl = new List<Control>();

        FListControlLine.Add(vKey, vListControl);
      }
      else
      {
        FListControlLine.TryGetValue(vKey, out vListControl);
      }

      vListControl.Add(prComponente);

      switch (prLinha)
      {
        case 0: return 24;
        case 1: return 24;
        case 2: return 64;
        case 3: return 104;
        case 4: return 144;

        default: return 0;
      }
    }

    private void ConfigureComponentSize()
    {
      foreach (var entry in FListControlLine)
      {
        List<Control> myListControl = entry.Value;
        Control vControl = myListControl[0];

        int leftoverWidth = (vControl.Parent is TabPage ? FTabComplement.Parent.Width - 20 : vControl.Parent.Width - 26);        
        int vLeft = 0;
        int vCount = 0;

        foreach (Control myControl in entry.Value)
        {
          vCount++;

          if (vLeft == 0)
            vLeft = myControl.Parent is TabPage ? 10 : 13;

          myControl.Left = vLeft;

          if (vCount == entry.Value.Count && !(myControl is MyMaskedTextBox))
          {
            myControl.Width = leftoverWidth;
          } 
          else
          {
            IMyControl iMyControl = myControl as IMyControl;

            if (myControl is IMyControl && iMyControl.PercentageWidth != 0)
              myControl.Width = (leftoverWidth * iMyControl.PercentageWidth / 100);
          }

          vLeft += 3 + myControl.Width;
          leftoverWidth = leftoverWidth - vLeft;
        }
      }
    }
  }
}
