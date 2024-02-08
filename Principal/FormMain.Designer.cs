namespace Principal
{
  partial class FormMain
  {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      menuStrip1 = new MenuStrip();
      cadastroToolStripMenuItem = new ToolStripMenuItem();
      pessoaToolStripMenuItem = new ToolStripMenuItem();
      pnlManagement = new Panel();
      menuStrip1.SuspendLayout();
      SuspendLayout();
      // 
      // menuStrip1
      // 
      menuStrip1.Dock = DockStyle.Left;
      menuStrip1.Items.AddRange(new ToolStripItem[] { cadastroToolStripMenuItem });
      menuStrip1.Location = new Point(0, 0);
      menuStrip1.Name = "menuStrip1";
      menuStrip1.Size = new Size(72, 385);
      menuStrip1.TabIndex = 0;
      menuStrip1.Text = "menuStrip1";
      // 
      // cadastroToolStripMenuItem
      // 
      cadastroToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { pessoaToolStripMenuItem });
      cadastroToolStripMenuItem.Name = "cadastroToolStripMenuItem";
      cadastroToolStripMenuItem.Size = new Size(59, 19);
      cadastroToolStripMenuItem.Text = "Cadastro";
      // 
      // pessoaToolStripMenuItem
      // 
      pessoaToolStripMenuItem.Name = "pessoaToolStripMenuItem";
      pessoaToolStripMenuItem.Size = new Size(110, 22);
      pessoaToolStripMenuItem.Text = "Pessoa";
      pessoaToolStripMenuItem.Click += PessoaToolStripMenuItem_Click;
      // 
      // pnlManagement
      // 
      pnlManagement.Dock = DockStyle.Fill;
      pnlManagement.Location = new Point(72, 0);
      pnlManagement.Name = "pnlManagement";
      pnlManagement.Size = new Size(709, 385);
      pnlManagement.TabIndex = 1;
      // 
      // FormMain
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(781, 385);
      Controls.Add(pnlManagement);
      Controls.Add(menuStrip1);
      MainMenuStrip = menuStrip1;
      Name = "FormMain";
      Text = "NDSoft";
      WindowState = FormWindowState.Maximized;
      menuStrip1.ResumeLayout(false);
      menuStrip1.PerformLayout();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private MenuStrip menuStrip1;
    private ToolStripMenuItem cadastroToolStripMenuItem;
    private ToolStripMenuItem pessoaToolStripMenuItem;
    private Panel pnlManagement;
  }
}