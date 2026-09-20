using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudentManagement.Desktop.Forms
{
    partial class frmDashboard
    {
        private Label lblTitle;
        private Label lblSubtitle;
        private Button btnQuickFee;
        private Panel cardStudents;
        private Panel cardCollection;
        private Panel cardDues;
        private Panel cardEligible;
        private Panel cardBreakdown;
        private Label lblStudents;
        private Label lblCollection;
        private Label lblDues;
        private Label lblEligible;
        private Label lblPaidHint;
        private Label lblCollectionHint;
        private Label lblDueHint;
        private Label lblEligibleHint;
        private Label lblStudentsTitle;
        private Label lblCollectionTitle;
        private Label lblDuesTitle;
        private Label lblEligibleTitle;
        private Label lblBreakdownTitle;
        private ProgressBar barTuition;
        private ProgressBar barIct;
        private ProgressBar barExam;
        private Label lblBarTuition;
        private Label lblBarIct;
        private Label lblBarExam;
        private Label lblPctTuition;
        private Label lblPctIct;
        private Label lblPctExam;

        private void InitializeComponent()
        {
            SuspendLayout();

            lblTitle = new Label
            {
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Location = new Point(16, 16),
                Text = "Main Dashboard & Real-Time Analytics"
            };
            lblSubtitle = new Label
            {
                AutoSize = true,
                ForeColor = System.Drawing.Color.FromArgb(138, 147, 166),
                Tag = "muted",
                Location = new Point(16, 48),
                Text = "Institutional overview for Ideal High School & College."
            };
            btnQuickFee = new Button
            {
                Text = "⚡  Quick Fee Collection",
                Size = new Size(200, 36),
                Location = new Point(680, 24),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnQuickFee.Click += btnQuickFee_Click;

            cardStudents = MetricCard(16, 90, "Total Active Students", out lblStudentsTitle, out lblStudents, out lblPaidHint);
            lblStudents.ForeColor = System.Drawing.Color.White;
            lblPaidHint.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            lblPaidHint.Tag = "success";
            cardStudents.Paint += cardStudents_Paint;

            cardCollection = MetricCard(300, 90, "Today's Collection", out lblCollectionTitle, out lblCollection, out lblCollectionHint);
            lblCollection.ForeColor = System.Drawing.Color.FromArgb(37, 99, 235);
            lblCollection.Tag = "primary,metric";
            lblCollectionHint.ForeColor = System.Drawing.Color.FromArgb(138, 147, 166);
            lblCollectionHint.Tag = "muted";
            cardCollection.Paint += cardCollection_Paint;

            cardDues = MetricCard(584, 90, "Current Month Due", out lblDuesTitle, out lblDues, out lblDueHint);
            lblDues.ForeColor = System.Drawing.Color.FromArgb(250, 204, 21);
            lblDues.Tag = "warning,metric";
            lblDueHint.ForeColor = System.Drawing.Color.FromArgb(244, 63, 94);
            lblDueHint.Tag = "danger";
            cardDues.Paint += cardDues_Paint;

            cardEligible = MetricCard(868, 90, "Exam Admit Eligible", out lblEligibleTitle, out lblEligible, out lblEligibleHint);
            lblEligible.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            lblEligible.Tag = "success,metric";
            lblEligibleHint.ForeColor = System.Drawing.Color.FromArgb(244, 63, 94);
            lblEligibleHint.Tag = "danger";
            cardEligible.Paint += cardEligible_Paint;

            cardBreakdown = new Panel
            {
                Tag = "card",
                BackColor = System.Drawing.Color.FromArgb(13, 20, 48),
                Location = new Point(16, 260),
                Size = new Size(1132, 220),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            cardBreakdown.Paint += cardBreakdown_Paint;
            lblBreakdownTitle = new Label
            {
                Text = "Monthly Collection Breakdown",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Location = new Point(20, 16),
                AutoSize = true
            };

            lblBarTuition = BarLabel("Tuition Fees", 20, 56);
            barTuition = ThemedBar(20, 80, System.Drawing.Color.FromArgb(16, 185, 129));
            lblPctTuition = PctLabel(System.Drawing.Color.FromArgb(16, 185, 129), 1060, 78);

            lblBarIct = BarLabel("ICT & Computer Lab Fees", 20, 112);
            barIct = ThemedBar(20, 136, System.Drawing.Color.FromArgb(59, 130, 246));
            lblPctIct = PctLabel(System.Drawing.Color.FromArgb(59, 130, 246), 1060, 134);

            lblBarExam = BarLabel("Term Examination Fees", 20, 168);
            barExam = ThemedBar(20, 192, System.Drawing.Color.FromArgb(139, 92, 246));
            lblPctExam = PctLabel(System.Drawing.Color.FromArgb(139, 92, 246), 1060, 190);

            cardBreakdown.Controls.AddRange(new Control[]
            {
                lblBreakdownTitle,
                lblBarTuition, barTuition, lblPctTuition,
                lblBarIct, barIct, lblPctIct,
                lblBarExam, barExam, lblPctExam
            });

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(9, 12, 23);
            ClientSize = new Size(1180, 520);
            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);
            Controls.Add(btnQuickFee);
            Controls.Add(cardStudents);
            Controls.Add(cardCollection);
            Controls.Add(cardDues);
            Controls.Add(cardEligible);
            Controls.Add(cardBreakdown);
            Name = "frmDashboard";
            Text = "Dashboard";
            Resize += frmDashboard_Resize;
            ResumeLayout(false);
            PerformLayout();
        }

        private static Panel MetricCard(int x, int y, string title, out Label titleLabel, out Label valueLabel, out Label hintLabel)
        {
            Panel card = new Panel
            {
                Tag = "card",
                BackColor = System.Drawing.Color.FromArgb(13, 20, 48),
                Location = new Point(x, y),
                Size = new Size(260, 140)
            };
            titleLabel = new Label
            {
                Text = title,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(138, 147, 166),
                Tag = "muted",
                Location = new Point(20, 18),
                AutoSize = true
            };
            valueLabel = new Label
            {
                Text = "...",
                Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Location = new Point(20, 50),
                AutoSize = true
            };
            hintLabel = new Label
            {
                Text = string.Empty,
                Location = new Point(20, 100),
                AutoSize = true,
                ForeColor = System.Drawing.Color.FromArgb(138, 147, 166)
            };
            card.Controls.Add(titleLabel);
            card.Controls.Add(valueLabel);
            card.Controls.Add(hintLabel);
            return card;
        }

        private static Label BarLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                ForeColor = System.Drawing.Color.FromArgb(138, 147, 166),
                Tag = "muted",
                Location = new Point(x, y),
                AutoSize = true
            };
        }

        private static ProgressBar ThemedBar(int x, int y, Color fill)
        {
            return new ProgressBar
            {
                Location = new Point(x, y),
                Size = new Size(1000, 14),
                Style = ProgressBarStyle.Continuous,
                Minimum = 0,
                Maximum = 100,
                Value = 0,
                ForeColor = fill,
                BackColor = System.Drawing.Color.FromArgb(17, 24, 39)
            };
        }

        private static Label PctLabel(Color color, int x, int y)
        {
            return new Label
            {
                Text = "0%",
                ForeColor = color,
                Location = new Point(x, y),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
        }

        private void cardStudents_Paint(object sender, PaintEventArgs e)
        {
            PaintCard(e, cardStudents, Color.FromArgb(37, 99, 235));
        }

        private void cardCollection_Paint(object sender, PaintEventArgs e)
        {
            PaintCard(e, cardCollection, Color.FromArgb(37, 99, 235));
        }

        private void cardDues_Paint(object sender, PaintEventArgs e)
        {
            PaintCard(e, cardDues, Color.FromArgb(250, 204, 21));
        }

        private void cardEligible_Paint(object sender, PaintEventArgs e)
        {
            PaintCard(e, cardEligible, Color.FromArgb(16, 185, 129));
        }

        private void cardBreakdown_Paint(object sender, PaintEventArgs e)
        {
            PaintCard(e, cardBreakdown, Color.FromArgb(139, 92, 246));
        }

        private static void PaintCard(PaintEventArgs e, Panel card, Color accent)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (Pen border = new Pen(Color.FromArgb(36, 48, 73)))
                e.Graphics.DrawRectangle(border, 0, 0, card.Width - 1, card.Height - 1);
            using (Pen accentPen = new Pen(accent, 3))
                e.Graphics.DrawLine(accentPen, 0, 8, 0, card.Height - 8);
        }

        private void frmDashboard_Resize(object sender, System.EventArgs e)
        {
            btnQuickFee.Left = Width - btnQuickFee.Width - 24;
            int gap = 16;
            int cardW = Math.Max(200, (Width - 48 - gap * 3) / 4);
            cardStudents.Width = cardW;
            cardCollection.Width = cardW;
            cardCollection.Left = cardStudents.Right + gap;
            cardDues.Width = cardW;
            cardDues.Left = cardCollection.Right + gap;
            cardEligible.Width = cardW;
            cardEligible.Left = cardDues.Right + gap;
            cardBreakdown.Width = Math.Max(400, Width - 40);
            int barW = Math.Max(200, cardBreakdown.Width - 120);
            barTuition.Width = barW;
            barIct.Width = barW;
            barExam.Width = barW;
            lblPctTuition.Left = cardBreakdown.Width - 60;
            lblPctIct.Left = cardBreakdown.Width - 60;
            lblPctExam.Left = cardBreakdown.Width - 60;
        }

        private void btnQuickFee_Click(object sender, System.EventArgs e)
        {
            Control host = Parent;
            while (host != null)
            {
                if (host is frmMainMenu menu)
                {
                    menu.OpenFeeCollectionFromDashboard();
                    return;
                }
                host = host.Parent;
            }
        }
    }
}
