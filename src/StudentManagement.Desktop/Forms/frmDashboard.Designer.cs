using System;
using System.Drawing;
using System.Windows.Forms;
using StudentManagement.Desktop.Theme;

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
                Font = UITheme.FontHeader,
                ForeColor = UITheme.TextPrimary,
                Location = new Point(16, 16),
                Text = "Main Dashboard & Real-Time Analytics"
            };
            lblSubtitle = new Label
            {
                AutoSize = true,
                ForeColor = UITheme.TextMuted,
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
            lblStudents.ForeColor = UITheme.TextPrimary;
            lblPaidHint.ForeColor = UITheme.Success;
            lblPaidHint.Tag = "success";
            cardStudents.Paint += (s, e) => PaintCard(e, cardStudents, UITheme.Primary);

            cardCollection = MetricCard(300, 90, "Today's Collection", out lblCollectionTitle, out lblCollection, out lblCollectionHint);
            lblCollection.ForeColor = UITheme.Primary;
            lblCollection.Tag = "primary,metric";
            lblCollectionHint.ForeColor = UITheme.TextMuted;
            lblCollectionHint.Tag = "muted";
            cardCollection.Paint += (s, e) => PaintCard(e, cardCollection, UITheme.Primary);

            cardDues = MetricCard(584, 90, "Current Month Due", out lblDuesTitle, out lblDues, out lblDueHint);
            lblDues.ForeColor = UITheme.Warning;
            lblDues.Tag = "warning,metric";
            lblDueHint.ForeColor = UITheme.Danger;
            lblDueHint.Tag = "danger";
            cardDues.Paint += (s, e) => PaintCard(e, cardDues, UITheme.Warning);

            cardEligible = MetricCard(868, 90, "Exam Admit Eligible", out lblEligibleTitle, out lblEligible, out lblEligibleHint);
            lblEligible.ForeColor = UITheme.Success;
            lblEligible.Tag = "success,metric";
            lblEligibleHint.ForeColor = UITheme.Danger;
            lblEligibleHint.Tag = "danger";
            cardEligible.Paint += (s, e) => PaintCard(e, cardEligible, UITheme.Success);

            cardBreakdown = new Panel
            {
                Tag = "card",
                BackColor = UITheme.Card,
                Location = new Point(16, 260),
                Size = new Size(1132, 220),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            cardBreakdown.Paint += (s, e) => PaintCard(e, cardBreakdown, UITheme.AccentPurple);
            lblBreakdownTitle = new Label
            {
                Text = "Monthly Collection Breakdown",
                Font = UITheme.FontSubtitle,
                ForeColor = UITheme.TextPrimary,
                Location = new Point(20, 16),
                AutoSize = true
            };

            lblBarTuition = BarLabel("Tuition Fees", 20, 56);
            barTuition = ThemedBar(20, 80, UITheme.Success);
            lblPctTuition = PctLabel(UITheme.Success, 1060, 78);

            lblBarIct = BarLabel("ICT & Computer Lab Fees", 20, 112);
            barIct = ThemedBar(20, 136, UITheme.AccentCyan);
            lblPctIct = PctLabel(UITheme.AccentCyan, 1060, 134);

            lblBarExam = BarLabel("Term Examination Fees", 20, 168);
            barExam = ThemedBar(20, 192, UITheme.AccentPurple);
            lblPctExam = PctLabel(UITheme.AccentPurple, 1060, 190);

            cardBreakdown.Controls.AddRange(new Control[]
            {
                lblBreakdownTitle,
                lblBarTuition, barTuition, lblPctTuition,
                lblBarIct, barIct, lblPctIct,
                lblBarExam, barExam, lblPctExam
            });

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = UITheme.Canvas;
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
                BackColor = UITheme.Card,
                Location = new Point(x, y),
                Size = new Size(260, 140)
            };
            titleLabel = new Label
            {
                Text = title,
                Font = UITheme.FontSubtitle,
                ForeColor = UITheme.TextMuted,
                Tag = "muted",
                Location = new Point(20, 18),
                AutoSize = true
            };
            valueLabel = new Label
            {
                Text = "...",
                Font = UITheme.FontMetric,
                ForeColor = UITheme.TextPrimary,
                Location = new Point(20, 50),
                AutoSize = true
            };
            hintLabel = new Label
            {
                Text = string.Empty,
                Location = new Point(20, 100),
                AutoSize = true,
                ForeColor = UITheme.TextMuted
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
                ForeColor = UITheme.TextMuted,
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
                BackColor = UITheme.InputBack
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

        private static void PaintCard(PaintEventArgs e, Panel card, Color accent)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
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
