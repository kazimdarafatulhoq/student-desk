using System.Drawing;
using System.Windows.Forms;
using StudentManagement.Desktop.Theme;

namespace StudentManagement.Desktop.Forms
{
    partial class frmDashboard
    {
        private Label lblTitle;
        private Label lblSubtitle;
        private Panel cardStudents;
        private Panel cardDues;
        private Label lblStudents;
        private Label lblDues;
        private Label lblPaidHint;
        private Label lblDueHint;
        private Label lblStudentsTitle;
        private Label lblDuesTitle;

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.cardStudents = new System.Windows.Forms.Panel();
            this.lblStudentsTitle = new System.Windows.Forms.Label();
            this.lblStudents = new System.Windows.Forms.Label();
            this.lblPaidHint = new System.Windows.Forms.Label();
            this.cardDues = new System.Windows.Forms.Panel();
            this.lblDuesTitle = new System.Windows.Forms.Label();
            this.lblDues = new System.Windows.Forms.Label();
            this.lblDueHint = new System.Windows.Forms.Label();
            this.cardStudents.SuspendLayout();
            this.cardDues.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = UITheme.FontHeader;
            this.lblTitle.ForeColor = UITheme.TextPrimary;
            this.lblTitle.Location = new System.Drawing.Point(16, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Operations Dashboard";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.ForeColor = UITheme.TextMuted;
            this.lblSubtitle.Location = new System.Drawing.Point(16, 48);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(260, 15);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Live snapshot of admissions and receivables.";
            // 
            // cardStudents
            // 
            this.cardStudents.BackColor = UITheme.Card;
            this.cardStudents.Controls.Add(this.lblStudentsTitle);
            this.cardStudents.Controls.Add(this.lblStudents);
            this.cardStudents.Controls.Add(this.lblPaidHint);
            this.cardStudents.Location = new System.Drawing.Point(16, 90);
            this.cardStudents.Name = "cardStudents";
            this.cardStudents.Size = new System.Drawing.Size(260, 140);
            this.cardStudents.TabIndex = 2;
            this.cardStudents.Tag = "card";
            this.cardStudents.Paint += new System.Windows.Forms.PaintEventHandler(this.cardStudents_Paint);
            // 
            // lblStudentsTitle
            // 
            this.lblStudentsTitle.AutoSize = true;
            this.lblStudentsTitle.Font = UITheme.FontSubtitle;
            this.lblStudentsTitle.ForeColor = UITheme.TextMuted;
            this.lblStudentsTitle.Location = new System.Drawing.Point(20, 18);
            this.lblStudentsTitle.Name = "lblStudentsTitle";
            this.lblStudentsTitle.Size = new System.Drawing.Size(100, 19);
            this.lblStudentsTitle.TabIndex = 0;
            this.lblStudentsTitle.Text = "Active Students";
            // 
            // lblStudents
            // 
            this.lblStudents.AutoSize = true;
            this.lblStudents.Font = UITheme.FontTitle;
            this.lblStudents.ForeColor = UITheme.Primary;
            this.lblStudents.Location = new System.Drawing.Point(20, 50);
            this.lblStudents.Name = "lblStudents";
            this.lblStudents.Size = new System.Drawing.Size(30, 32);
            this.lblStudents.TabIndex = 1;
            this.lblStudents.Text = "...";
            // 
            // lblPaidHint
            // 
            this.lblPaidHint.AutoSize = true;
            this.lblPaidHint.ForeColor = UITheme.TextMuted;
            this.lblPaidHint.Location = new System.Drawing.Point(20, 100);
            this.lblPaidHint.Name = "lblPaidHint";
            this.lblPaidHint.Size = new System.Drawing.Size(0, 15);
            this.lblPaidHint.TabIndex = 2;
            // 
            // cardDues
            // 
            this.cardDues.BackColor = UITheme.Card;
            this.cardDues.Controls.Add(this.lblDuesTitle);
            this.cardDues.Controls.Add(this.lblDues);
            this.cardDues.Controls.Add(this.lblDueHint);
            this.cardDues.Location = new System.Drawing.Point(300, 90);
            this.cardDues.Name = "cardDues";
            this.cardDues.Size = new System.Drawing.Size(260, 140);
            this.cardDues.TabIndex = 3;
            this.cardDues.Tag = "card";
            this.cardDues.Paint += new System.Windows.Forms.PaintEventHandler(this.cardDues_Paint);
            // 
            // lblDuesTitle
            // 
            this.lblDuesTitle.AutoSize = true;
            this.lblDuesTitle.Font = UITheme.FontSubtitle;
            this.lblDuesTitle.ForeColor = UITheme.TextMuted;
            this.lblDuesTitle.Location = new System.Drawing.Point(20, 18);
            this.lblDuesTitle.Name = "lblDuesTitle";
            this.lblDuesTitle.Size = new System.Drawing.Size(110, 19);
            this.lblDuesTitle.TabIndex = 0;
            this.lblDuesTitle.Text = "Net Receivables";
            // 
            // lblDues
            // 
            this.lblDues.AutoSize = true;
            this.lblDues.Font = UITheme.FontTitle;
            this.lblDues.ForeColor = UITheme.Danger;
            this.lblDues.Location = new System.Drawing.Point(20, 50);
            this.lblDues.Name = "lblDues";
            this.lblDues.Size = new System.Drawing.Size(30, 32);
            this.lblDues.TabIndex = 1;
            this.lblDues.Text = "...";
            // 
            // lblDueHint
            // 
            this.lblDueHint.AutoSize = true;
            this.lblDueHint.ForeColor = UITheme.TextMuted;
            this.lblDueHint.Location = new System.Drawing.Point(20, 100);
            this.lblDueHint.Name = "lblDueHint";
            this.lblDueHint.Size = new System.Drawing.Size(0, 15);
            this.lblDueHint.TabIndex = 2;
            // 
            // frmDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = UITheme.Canvas;
            this.ClientSize = new System.Drawing.Size(900, 500);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.cardStudents);
            this.Controls.Add(this.cardDues);
            this.Name = "frmDashboard";
            this.Text = "Dashboard";
            this.cardStudents.ResumeLayout(false);
            this.cardStudents.PerformLayout();
            this.cardDues.ResumeLayout(false);
            this.cardDues.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void cardStudents_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(UITheme.Border))
                e.Graphics.DrawRectangle(pen, 0, 0, this.cardStudents.Width - 1, this.cardStudents.Height - 1);
            using (Pen accent = new Pen(UITheme.Primary, 3))
                e.Graphics.DrawLine(accent, 0, 0, 0, this.cardStudents.Height);
        }

        private void cardDues_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(UITheme.Border))
                e.Graphics.DrawRectangle(pen, 0, 0, this.cardDues.Width - 1, this.cardDues.Height - 1);
            using (Pen accent = new Pen(UITheme.Danger, 3))
                e.Graphics.DrawLine(accent, 0, 0, 0, this.cardDues.Height);
        }
    }
}
