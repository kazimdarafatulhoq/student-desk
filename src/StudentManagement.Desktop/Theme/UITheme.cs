using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace StudentManagement.Desktop.Theme;

/// <summary>
/// Tailwind Slate-900 / modern dark-indigo design system for WinForms.
/// </summary>
public static class UITheme
{
    public static readonly Color Canvas = ColorTranslator.FromHtml("#0F172A");
    public static readonly Color Card = ColorTranslator.FromHtml("#1E293B");
    public static readonly Color Border = ColorTranslator.FromHtml("#334155");
    public static readonly Color Primary = ColorTranslator.FromHtml("#2563EB");
    public static readonly Color PrimaryIndigo = ColorTranslator.FromHtml("#4F46E5");
    public static readonly Color Success = ColorTranslator.FromHtml("#10B981");
    public static readonly Color Danger = ColorTranslator.FromHtml("#F43F5E");
    public static readonly Color Warning = ColorTranslator.FromHtml("#F59E0B");
    public static readonly Color TextPrimary = ColorTranslator.FromHtml("#F8FAFC");
    public static readonly Color TextMuted = ColorTranslator.FromHtml("#94A3B8");
    public static readonly Color GridAlt = ColorTranslator.FromHtml("#162032");
    public static readonly Color Selection = ColorTranslator.FromHtml("#1D4ED8");
    public static readonly Color Sidebar = ColorTranslator.FromHtml("#020617");
    public static readonly Color InputBack = ColorTranslator.FromHtml("#0B1220");

    public static readonly Font FontBody = new("Segoe UI", 9F, FontStyle.Regular);
    public static readonly Font FontSubtitle = new("Segoe UI", 10F, FontStyle.Bold);
    public static readonly Font FontHeader = new("Segoe UI", 14F, FontStyle.Bold);
    public static readonly Font FontTitle = new("Segoe UI", 18F, FontStyle.Bold);
    public static readonly Font FontNav = new("Segoe UI", 10F, FontStyle.Regular);

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
            case Panel panel:
                if (panel.Tag?.ToString() == "card")
                {
                    panel.BackColor = Card;
                    panel.Padding = new Padding(12);
                }
                else if (panel.Tag?.ToString() == "sidebar")
                {
                    panel.BackColor = Sidebar;
                }
                else
                {
                    panel.BackColor = panel.Parent?.BackColor ?? Canvas;
                }
                break;
            case Label label:
                label.ForeColor = label.Tag?.ToString() == "muted" ? TextMuted : TextPrimary;
                if (label.Tag?.ToString() == "header")
                    label.Font = FontHeader;
                else if (label.Tag?.ToString() == "subtitle")
                    label.Font = FontSubtitle;
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
                dtp.CalendarTitleForeColor = TextPrimary;
                dtp.Font = FontBody;
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = "dd-MMM-yyyy";
                break;
            case CheckBox check:
                check.ForeColor = TextPrimary;
                check.FlatStyle = FlatStyle.Flat;
                break;
            case Button button:
                StyleButton(button);
                break;
            case DataGridView grid:
                StyleGrid(grid);
                break;
            case GroupBox group:
                group.ForeColor = TextPrimary;
                group.BackColor = Card;
                group.Font = FontSubtitle;
                break;
            case StatusStrip strip:
                strip.BackColor = Card;
                strip.ForeColor = TextMuted;
                break;
        }

        foreach (Control child in control.Controls)
            ApplyControl(child);
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

        if (button.Tag?.ToString() == "danger" || danger)
        {
            button.BackColor = Danger;
            button.ForeColor = Color.White;
        }
        else if (button.Tag?.ToString() == "success" || success)
        {
            button.BackColor = Success;
            button.ForeColor = Color.White;
        }
        else if (button.Tag?.ToString() == "ghost")
        {
            button.BackColor = Card;
            button.ForeColor = TextPrimary;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = Border;
        }
        else if (button.Tag?.ToString() == "nav")
        {
            button.BackColor = Color.Transparent;
            button.ForeColor = TextMuted;
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.FlatAppearance.MouseOverBackColor = Card;
            button.Padding = new Padding(16, 8, 8, 8);
            button.Height = 44;
        }
        else
        {
            button.BackColor = primary ? Primary : PrimaryIndigo;
            button.ForeColor = Color.White;
        }
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
            BackColor = ColorTranslator.FromHtml("#0B1220"),
            ForeColor = TextPrimary,
            Font = FontSubtitle,
            SelectionBackColor = ColorTranslator.FromHtml("#0B1220"),
            SelectionForeColor = TextPrimary,
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

    public static Panel CreateCard(string title, Control? content = null)
    {
        var panel = new Panel
        {
            Tag = "card",
            BackColor = Card,
            Padding = new Padding(16),
            Margin = new Padding(8)
        };
        panel.Paint += (_, e) =>
        {
            using var pen = new Pen(Border);
            e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
        };

        var lbl = new Label
        {
            Text = title,
            Tag = "subtitle",
            Font = FontSubtitle,
            ForeColor = TextMuted,
            Dock = DockStyle.Top,
            Height = 28
        };
        panel.Controls.Add(lbl);
        if (content is not null)
        {
            content.Dock = DockStyle.Fill;
            panel.Controls.Add(content);
            content.BringToFront();
        }
        return panel;
    }

    public static Label CreateStatValue(string value, Color color)
    {
        return new Label
        {
            Text = value,
            Font = FontTitle,
            ForeColor = color,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };
    }
}
