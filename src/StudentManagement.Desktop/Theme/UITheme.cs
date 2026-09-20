using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudentManagement.Desktop.Theme
{
    /// <summary>
    /// Ideal High School reference theme — deep navy canvas, sapphire primary,
    /// emerald success, amber warning, rose danger (matches dashboard mockups).
    /// </summary>
    public static class UITheme
    {
        // Surfaces (reference: #090C17 / #070B18 / #0D1430)
        public static readonly Color Canvas = ColorTranslator.FromHtml("#090C17");
        public static readonly Color Sidebar = ColorTranslator.FromHtml("#070B18");
        public static readonly Color Card = ColorTranslator.FromHtml("#0D1430");
        public static readonly Color InputBack = ColorTranslator.FromHtml("#111827");
        public static readonly Color Border = ColorTranslator.FromHtml("#243049");
        public static readonly Color GridAlt = ColorTranslator.FromHtml("#0B1228");
        public static readonly Color HeaderBack = ColorTranslator.FromHtml("#0A1024");

        // Accents
        public static readonly Color Primary = ColorTranslator.FromHtml("#2563EB");
        public static readonly Color PrimaryHover = ColorTranslator.FromHtml("#1D4ED8");
        public static readonly Color PrimaryIndigo = ColorTranslator.FromHtml("#4F46E5");
        public static readonly Color AccentPurple = ColorTranslator.FromHtml("#8B5CF6");
        public static readonly Color AccentCyan = ColorTranslator.FromHtml("#3B82F6");
        public static readonly Color Selection = ColorTranslator.FromHtml("#1D4ED8");

        // Status
        public static readonly Color Success = ColorTranslator.FromHtml("#10B981");
        public static readonly Color Online = ColorTranslator.FromHtml("#22C55E");
        public static readonly Color Warning = ColorTranslator.FromHtml("#FACC15");
        public static readonly Color Partial = ColorTranslator.FromHtml("#F9A03F");
        public static readonly Color Danger = ColorTranslator.FromHtml("#F43F5E");

        // Typography (reference secondary: #8A93A6 / #A0AEC0)
        public static readonly Color TextPrimary = Color.White;
        public static readonly Color TextMuted = ColorTranslator.FromHtml("#8A93A6");
        public static readonly Color TextDim = ColorTranslator.FromHtml("#A0AEC0");

        public static readonly Font FontBody = new Font("Segoe UI", 9F, FontStyle.Regular);
        public static readonly Font FontSubtitle = new Font("Segoe UI", 10F, FontStyle.Bold);
        public static readonly Font FontHeader = new Font("Segoe UI", 14F, FontStyle.Bold);
        public static readonly Font FontTitle = new Font("Segoe UI", 18F, FontStyle.Bold);
        public static readonly Font FontNav = new Font("Segoe UI", 10F, FontStyle.Regular);
        public static readonly Font FontSection = new Font("Segoe UI", 9F, FontStyle.Bold);
        public static readonly Font FontMetric = new Font("Segoe UI", 22F, FontStyle.Bold);

        public static void ApplyForm(Form form)
        {
            form.BackColor = Canvas;
            form.ForeColor = TextPrimary;
            form.Font = FontBody;
            foreach (Control c in form.Controls)
                ApplyControl(c);
        }

        public static void ApplyControl(Control control)
        {
            switch (control)
            {
                case FlowLayoutPanel flow:
                    if (flow.Tag != null && flow.Tag.ToString() == "sidebar")
                        flow.BackColor = Sidebar;
                    break;
                case Panel panel:
                    ApplyPanel(panel);
                    break;
                case Label label:
                    ApplyLabel(label);
                    break;
                case TextBox textBox:
                    StyleTextBox(textBox);
                    break;
                case ComboBox combo:
                    StyleComboBox(combo);
                    break;
                case DateTimePicker dtp:
                    dtp.CalendarMonthBackground = Card;
                    dtp.CalendarForeColor = TextPrimary;
                    dtp.CalendarTitleBackColor = Primary;
                    dtp.CalendarTitleForeColor = Color.White;
                    dtp.Font = FontBody;
                    dtp.Format = DateTimePickerFormat.Custom;
                    dtp.CustomFormat = "dd-MMM-yyyy";
                    break;
                case CheckBox check:
                    check.ForeColor = TextPrimary;
                    check.FlatStyle = FlatStyle.Flat;
                    check.FlatAppearance.CheckedBackColor = Primary;
                    break;
                case Button button:
                    StyleButton(button);
                    break;
                case DataGridView grid:
                    StyleGrid(grid);
                    break;
                case GroupBox group:
                    group.ForeColor = Primary;
                    group.BackColor = Card;
                    group.Font = FontSection;
                    break;
                case StatusStrip strip:
                    strip.BackColor = HeaderBack;
                    strip.ForeColor = TextMuted;
                    break;
            }

            foreach (Control child in control.Controls)
                ApplyControl(child);
        }

        private static void ApplyPanel(Panel panel)
        {
            string tag = panel.Tag != null ? panel.Tag.ToString() : string.Empty;
            if (tag == "card")
            {
                panel.BackColor = Card;
                panel.Padding = new Padding(12);
            }
            else if (tag == "sidebar")
            {
                panel.BackColor = Sidebar;
            }
            else if (tag == "header")
            {
                panel.BackColor = HeaderBack;
            }
            else
            {
                panel.BackColor = panel.Parent != null ? panel.Parent.BackColor : Canvas;
            }
        }

        private static void ApplyLabel(Label label)
        {
            string tag = label.Tag != null ? label.Tag.ToString() : string.Empty;
            // Empty tag preserves designer ForeColor/Font (colored metrics, etc.)
            if (string.IsNullOrEmpty(tag))
                return;

            if (tag.Contains("muted") || tag.Contains("dim"))
                label.ForeColor = TextMuted;
            else if (tag.Contains("section"))
                label.ForeColor = Primary;
            else if (tag.Contains("success"))
                label.ForeColor = Success;
            else if (tag.Contains("danger"))
                label.ForeColor = Danger;
            else if (tag.Contains("warning"))
                label.ForeColor = Warning;
            else if (tag.Contains("primary"))
                label.ForeColor = Primary;
            else if (tag.Contains("cyan"))
                label.ForeColor = AccentCyan;
            else if (tag.Contains("purple"))
                label.ForeColor = AccentPurple;
            else
                label.ForeColor = TextPrimary;

            if (tag.Contains("header"))
                label.Font = FontHeader;
            else if (tag.Contains("subtitle"))
                label.Font = FontSubtitle;
            else if (tag.Contains("metric"))
                label.Font = FontMetric;
            else if (tag.Contains("section"))
                label.Font = FontSection;
        }

        public static void StyleTextBox(TextBox textBox)
        {
            textBox.BackColor = InputBack;
            textBox.ForeColor = TextPrimary;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = FontBody;
        }

        public static void StyleComboBox(ComboBox combo)
        {
            combo.BackColor = InputBack;
            combo.ForeColor = TextPrimary;
            combo.FlatStyle = FlatStyle.Flat;
            combo.Font = FontBody;
        }

        public static void StyleButton(Button button, bool primary = true, bool danger = false, bool success = false)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Cursor = Cursors.Hand;
            button.Font = FontSubtitle;
            button.Height = Math.Max(button.Height, 36);
            button.Padding = new Padding(8, 4, 8, 4);

            string tag = button.Tag != null ? button.Tag.ToString() : string.Empty;

            if (tag == "danger" || danger)
            {
                button.BackColor = Danger;
                button.ForeColor = Color.White;
            }
            else if (tag == "success" || success)
            {
                button.BackColor = Success;
                button.ForeColor = Color.White;
            }
            else if (tag == "ghost")
            {
                button.BackColor = Card;
                button.ForeColor = TextPrimary;
                button.FlatAppearance.BorderSize = 1;
                button.FlatAppearance.BorderColor = Border;
            }
            else if (tag == "indigo")
            {
                button.BackColor = PrimaryIndigo;
                button.ForeColor = Color.White;
            }
            else if (tag == "nav")
            {
                button.BackColor = Color.Transparent;
                button.ForeColor = TextMuted;
                button.TextAlign = ContentAlignment.MiddleLeft;
                button.FlatAppearance.MouseOverBackColor = Card;
                button.Padding = new Padding(16, 8, 8, 8);
                button.Height = 42;
            }
            else if (tag == "nav-active")
            {
                // Reference: solid sapphire pill for active sidebar item
                button.BackColor = Primary;
                button.ForeColor = Color.White;
                button.TextAlign = ContentAlignment.MiddleLeft;
                button.FlatAppearance.MouseOverBackColor = PrimaryHover;
                button.Padding = new Padding(16, 8, 8, 8);
                button.Height = 42;
            }
            else
            {
                button.BackColor = primary ? Primary : PrimaryIndigo;
                button.ForeColor = Color.White;
                button.FlatAppearance.MouseOverBackColor = PrimaryHover;
            }
        }

        public static void SetNavActive(Button button, bool active)
        {
            if (button == null)
                return;
            button.Tag = active ? "nav-active" : "nav";
            StyleButton(button);
        }

        public static void StyleGrid(DataGridView grid)
        {
            grid.BackgroundColor = Card;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = Border;
            grid.RowHeadersVisible = false;
            grid.EnableHeadersVisualStyles = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = HeaderBack,
                ForeColor = TextMuted,
                Font = FontSection,
                SelectionBackColor = HeaderBack,
                SelectionForeColor = TextMuted,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(8, 4, 8, 4)
            };
            grid.ColumnHeadersHeight = 40;
            grid.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Card,
                ForeColor = TextPrimary,
                Font = FontBody,
                SelectionBackColor = Selection,
                SelectionForeColor = Color.White,
                Padding = new Padding(6, 2, 6, 2)
            };
            grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = GridAlt,
                ForeColor = TextPrimary,
                SelectionBackColor = Selection,
                SelectionForeColor = Color.White
            };
            grid.RowTemplate.Height = 36;
        }

        public static Color StatusColor(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return TextMuted;
            string s = status.Trim().ToLowerInvariant();
            if (s == "paid" || s == "active" || s == "cleared")
                return Success;
            if (s == "partial")
                return Partial;
            if (s == "unpaid" || s == "due" || s == "overdue")
                return Danger;
            return TextMuted;
        }

        public static Panel CreateCard(string title, Control content)
        {
            Panel panel = new Panel();
            panel.Tag = "card";
            panel.BackColor = Card;
            panel.Padding = new Padding(16);
            panel.Margin = new Padding(8);
            panel.Paint += delegate (object sender, PaintEventArgs e)
            {
                using (Pen pen = new Pen(Border))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
                }
            };

            Label lbl = new Label();
            lbl.Text = title;
            lbl.Tag = "muted";
            lbl.Font = FontSubtitle;
            lbl.ForeColor = TextMuted;
            lbl.Dock = DockStyle.Top;
            lbl.Height = 28;
            panel.Controls.Add(lbl);

            if (content != null)
            {
                content.Dock = DockStyle.Fill;
                panel.Controls.Add(content);
                content.BringToFront();
            }
            return panel;
        }

        public static Label CreateStatValue(string value, Color color)
        {
            Label label = new Label();
            label.Text = value;
            label.Font = FontMetric;
            label.ForeColor = color;
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleLeft;
            return label;
        }
    }
}
