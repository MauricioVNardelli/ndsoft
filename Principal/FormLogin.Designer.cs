namespace Principal
{
  partial class FormLogin
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
      txtUser = new TextBox();
      lblUser = new Label();
      lblPassword = new Label();
      txtPassword = new TextBox();
      btnEntrar = new Button();
      pictureBox1 = new PictureBox();
      ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
      SuspendLayout();
      // 
      // txtUser
      // 
      txtUser.Location = new Point(150, 58);
      txtUser.Name = "txtUser";
      txtUser.Size = new Size(159, 23);
      txtUser.TabIndex = 0;
      // 
      // lblUser
      // 
      lblUser.AutoSize = true;
      lblUser.Location = new Point(150, 40);
      lblUser.Name = "lblUser";
      lblUser.Size = new Size(47, 15);
      lblUser.TabIndex = 1;
      lblUser.Text = "Usuário";
      // 
      // lblPassword
      // 
      lblPassword.AutoSize = true;
      lblPassword.Location = new Point(150, 84);
      lblPassword.Name = "lblPassword";
      lblPassword.Size = new Size(39, 15);
      lblPassword.TabIndex = 3;
      lblPassword.Text = "Senha";
      // 
      // txtPassword
      // 
      txtPassword.Location = new Point(150, 103);
      txtPassword.Name = "txtPassword";
      txtPassword.Size = new Size(159, 23);
      txtPassword.TabIndex = 2;
      // 
      // btnEntrar
      // 
      btnEntrar.Location = new Point(315, 58);
      btnEntrar.Name = "btnEntrar";
      btnEntrar.Size = new Size(75, 70);
      btnEntrar.TabIndex = 4;
      btnEntrar.Text = "Entrar";
      btnEntrar.UseVisualStyleBackColor = true;
      btnEntrar.Click += btnEntrar_Click;
      // 
      // pictureBox1
      // 
      pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
      pictureBox1.Location = new Point(12, 23);
      pictureBox1.Name = "pictureBox1";
      pictureBox1.Size = new Size(132, 130);
      pictureBox1.TabIndex = 5;
      pictureBox1.TabStop = false;
      // 
      // FormLogin
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(403, 177);
      Controls.Add(btnEntrar);
      Controls.Add(lblPassword);
      Controls.Add(txtPassword);
      Controls.Add(lblUser);
      Controls.Add(txtUser);
      Controls.Add(pictureBox1);
      FormBorderStyle = FormBorderStyle.FixedSingle;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "FormLogin";
      StartPosition = FormStartPosition.CenterScreen;
      Text = "NDSoft";
      ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private TextBox txtUser;
    private Label lblUser;
    private Label lblPassword;
    private TextBox txtPassword;
    private Button btnEntrar;
    private PictureBox pictureBox1;
  }
}