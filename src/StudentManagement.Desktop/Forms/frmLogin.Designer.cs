using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace StudentManagement.Desktop.Forms
{
    partial class frmLogin
    {
        private IContainer components = null;
        private Panel pnlCard;
        private Label lblBrand;
        private Label lblTitle;
        private Label lblUser;
        private Label lblPass;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblStatus;
        private Label lblHint;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlCard = new System.Windows.Forms.Panel();
            this.lblBrand = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPass = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblHint = new System.Windows.Forms.Label();
            this.pnlCard.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlCard
            //
            this.pnlCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(20)))), ((int)(((byte)(48)))));
            this.pnlCard.Controls.Add(this.lblBrand);
            this.pnlCard.Controls.Add(this.lblTitle);
            this.pnlCard.Controls.Add(this.lblUser);
            this.pnlCard.Controls.Add(this.txtUsername);
            this.pnlCard.Controls.Add(this.lblPass);
            this.pnlCard.Controls.Add(this.txtPassword);
            this.pnlCard.Controls.Add(this.btnLogin);
            this.pnlCard.Controls.Add(this.lblStatus);
            this.pnlCard.Location = new System.Drawing.Point(40, 40);
            this.pnlCard.Name = "pnlCard";
            this.pnlCard.Size = new System.Drawing.Size(380, 330);
            this.pnlCard.TabIndex = 0;
            this.pnlCard.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCard_Paint);
            //
            // lblBrand
            //
            this.lblBrand.AutoSize = true;
            this.lblBrand.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.lblBrand.Location = new System.Drawing.Point(24, 24);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(180, 25);
            this.lblBrand.TabIndex = 0;
            this.lblBrand.Text = "IDEAL HIGH SCHOOL";
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(147)))), ((int)(((byte)(166)))));
            this.lblTitle.Location = new System.Drawing.Point(24, 56);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(170, 19);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Enterprise Portal Login";
            //
            // lblUser
            //
            this.lblUser.AutoSize = true;
            this.lblUser.ForeColor = System.Drawing.Color.White;
            this.lblUser.Location = new System.Drawing.Point(24, 100);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(60, 15);
            this.lblUser.TabIndex = 2;
            this.lblUser.Text = "Username";
            //
            // txtUsername
            //
            this.txtUsername.Location = new System.Drawing.Point(24, 122);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(330, 23);
            this.txtUsername.TabIndex = 3;
            this.txtUsername.Text = "admin";
            //
            // lblPass
            //
            this.lblPass.AutoSize = true;
            this.lblPass.ForeColor = System.Drawing.Color.White;
            this.lblPass.Location = new System.Drawing.Point(24, 165);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(57, 15);
            this.lblPass.TabIndex = 4;
            this.lblPass.Text = "Password";
            //
            // txtPassword
            //
            this.txtPassword.Location = new System.Drawing.Point(24, 187);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(330, 23);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.Text = "Admin@123";
            this.txtPassword.UseSystemPasswordChar = true;
            //
            // btnLogin
            //
            this.btnLogin.Location = new System.Drawing.Point(24, 240);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(330, 40);
            this.btnLogin.TabIndex = 6;
            this.btnLogin.Tag = "primary";
            this.btnLogin.Text = "Sign In";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            //
            // lblStatus
            //
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(147)))), ((int)(((byte)(166)))));
            this.lblStatus.Location = new System.Drawing.Point(24, 290);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(330, 20);
            this.lblStatus.TabIndex = 7;
            //
            // lblHint
            //
            this.lblHint.AutoSize = true;
            this.lblHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(147)))), ((int)(((byte)(166)))));
            this.lblHint.Location = new System.Drawing.Point(40, 380);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(150, 15);
            this.lblHint.TabIndex = 1;
            this.lblHint.Tag = "muted";
            this.lblHint.Text = "Default: admin / Admin@123";
            //
            // frmLogin
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(12)))), ((int)(((byte)(23)))));
            this.ClientSize = new System.Drawing.Size(460, 420);
            this.Controls.Add(this.pnlCard);
            this.Controls.Add(this.lblHint);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sign In - Student Management";
            this.pnlCard.ResumeLayout(false);
            this.pnlCard.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void pnlCard_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(System.Drawing.Color.FromArgb(36, 48, 73)))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, this.pnlCard.Width - 1, this.pnlCard.Height - 1);
            }
        }
    }
}
