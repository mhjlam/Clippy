using System.Drawing;
using System.Windows.Forms;

namespace Clippy;

public partial class ClippyForm : Form
{
    private IntPtr nextClipboardViewer;
    private bool suppressClipboardEvent = false;
    private const int MaxClips = 12;

    public ClippyForm(ClippyContext trayAppContext)
    {
        InitializeComponent();

        ShowInTaskbar = false;
        Text = string.Empty;
        ControlBox = false;
        FormBorderStyle = FormBorderStyle.FixedDialog;

        ClipsListBox.KeyDown += ClipsListBox_KeyDown;
        Load += (_, _) => Hide();
    }

    public void ClearClips()
    {
        ClipsListBox.Items.Clear();
        InputTextBox.Clear();
    }

    private void ClipsListBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Delete && ClipsListBox.SelectedIndex != -1)
        {
            int selectedIndex = ClipsListBox.SelectedIndex;
            ClipsListBox.Items.RemoveAt(selectedIndex);
            ClipsListBox.ClearSelected();
            suppressClipboardEvent = true;
            Clipboard.Clear();
            suppressClipboardEvent = false;
            e.Handled = true;
        }
    }

    protected override void WndProc(ref Message m)
    {
        switch ((Win32.Msgs)m.Msg)
        {
            case Win32.Msgs.WM_DRAWCLIPBOARD:
                try
                {
                    if (!suppressClipboardEvent)
                    {
                        var dataObject = Clipboard.GetDataObject();
                        if (dataObject?.GetDataPresent(DataFormats.Text) == true)
                        {
                            AddItemToClipboard((string)dataObject.GetData(DataFormats.Text)!);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
                Win32.User32.SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                break;

            case Win32.Msgs.WM_CHANGECBCHAIN:
                if (m.WParam == nextClipboardViewer)
                {
                    nextClipboardViewer = m.LParam;
                }
                else
                {
                    Win32.User32.SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                }
                break;

            default:
                base.WndProc(ref m);
                break;
        }
    }

    protected override void OnActivated(EventArgs e)
    {
        var workingArea = Screen.GetWorkingArea(this);
        Location = new Point(workingArea.Right - Size.Width - 10, workingArea.Bottom - Size.Height - 10);

        Show();
        Activate();
        InputTextBox.Focus();
        base.OnActivated(e);
    }

    private void ClipboardForm_Load(object? sender, EventArgs e)
    {
        nextClipboardViewer = Win32.User32.SetClipboardViewer(Handle);
    }

    private void ClipboardForm_Closing(object? sender, FormClosingEventArgs e)
    {
        Win32.User32.ChangeClipboardChain(Handle, nextClipboardViewer);
    }

    private void ClipsListBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        try
        {
            if (ClipsListBox.SelectedItem is not string item)
                return;

            item = item.Trim();
            suppressClipboardEvent = true;
            Clipboard.SetText(item);
            suppressClipboardEvent = false;
        }
        catch
        {
            suppressClipboardEvent = false;
        }
    }

    private void InputTextBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            AddItemToClipboard(InputTextBox.Text);
        }
        else if (e.KeyCode == Keys.Delete && string.IsNullOrWhiteSpace(InputTextBox.Text) && ClipsListBox.SelectedIndex != -1)
        {
            int selectedIndex = ClipsListBox.SelectedIndex;
            ClipsListBox.Items.RemoveAt(selectedIndex);
            ClipsListBox.ClearSelected();
            suppressClipboardEvent = true;
            Clipboard.Clear();
            suppressClipboardEvent = false;
            e.Handled = true;
        }
        else if (e.Control && e.KeyCode == Keys.Back)
        {
            int selStart = InputTextBox.SelectionStart;
            int selLength = InputTextBox.SelectionLength;
            if (selStart > 0 && selLength == 0)
            {
                int prev = selStart - 1;
                while (prev >= 0 && char.IsWhiteSpace(InputTextBox.Text[prev]))
                    prev--;
                while (prev >= 0 && !char.IsWhiteSpace(InputTextBox.Text[prev]))
                    prev--;
                int wordStart = prev + 1;
                InputTextBox.Text = InputTextBox.Text.Remove(wordStart, selStart - wordStart);
                InputTextBox.SelectionStart = wordStart;
                e.SuppressKeyPress = true;
            }
            else if (selLength > 0)
            {
                int start = InputTextBox.SelectionStart;
                InputTextBox.Text = InputTextBox.Text.Remove(start, selLength);
                InputTextBox.SelectionStart = start;
                e.SuppressKeyPress = true;
            }
            e.Handled = true;
        }
        else if (e.Control && e.KeyCode == Keys.Delete)
        {
            int selStart = InputTextBox.SelectionStart;
            int selLength = InputTextBox.SelectionLength;
            int textLen = InputTextBox.Text.Length;
            if (selStart < textLen && selLength == 0)
            {
                int next = selStart;
                while (next < textLen && char.IsWhiteSpace(InputTextBox.Text[next]))
                    next++;
                while (next < textLen && !char.IsWhiteSpace(InputTextBox.Text[next]))
                    next++;
                int wordEnd = next;
                InputTextBox.Text = InputTextBox.Text.Remove(selStart, wordEnd - selStart);
                InputTextBox.SelectionStart = selStart;
                e.SuppressKeyPress = true;
            }
            else if (selLength > 0)
            {
                int start = InputTextBox.SelectionStart;
                InputTextBox.Text = InputTextBox.Text.Remove(start, selLength);
                InputTextBox.SelectionStart = start;
                e.SuppressKeyPress = true;
            }
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Up && ClipsListBox.Items.Count > 0)
        {
            if (ClipsListBox.SelectedIndex == -1)
            {
                ClipsListBox.SelectedIndex = ClipsListBox.Items.Count - 1;
            }
            else
            {
                ClipsListBox.SelectedIndex = Math.Max(0, ClipsListBox.SelectedIndex - 1);
            }
        }
        else if (e.KeyCode == Keys.Down && ClipsListBox.Items.Count > 0)
        {
            if (ClipsListBox.SelectedIndex == -1)
            {
                ClipsListBox.SelectedIndex = 0;
            }
            else
            {
                ClipsListBox.SelectedIndex = Math.Min(ClipsListBox.Items.Count - 1, ClipsListBox.SelectedIndex + 1);
            }
        }
        else if (e.KeyCode == Keys.Escape)
        {
            Hide();
            e.Handled = true;
        }
    }

    private void AddButton_Click(object? sender, EventArgs e)
    {
        AddItemToClipboard(InputTextBox.Text);
    }

    private void AddItemToClipboard(string item)
    {
        item = item.Trim();
        if (string.IsNullOrWhiteSpace(item))
        {
            InputTextBox.Clear();
            return;
        }

        int existingIndex = ClipsListBox.Items.IndexOf(item);
        if (existingIndex != -1)
        {
            ClipsListBox.SelectedIndex = existingIndex;
            FlashListBoxItem(existingIndex);
        }
        else
        {
            if (ClipsListBox.Items.Count >= MaxClips)
            {
                ClipsListBox.Items.RemoveAt(0);
            }
            ClipsListBox.Items.Add(item);
            ClipsListBox.SelectedIndex = ClipsListBox.Items.Count - 1;
        }
        InputTextBox.Clear();
    }

    private async void FlashListBoxItem(int index)
    {
        var originalDrawMode = ClipsListBox.DrawMode;
        DrawItemEventHandler? drawHandler = null;
        var highlightColor = Color.LightGoldenrodYellow;
        var flashIndex = index;

        drawHandler = (s, e) =>
        {
            if (e.Index == flashIndex)
            {
                e.Graphics.FillRectangle(new SolidBrush(highlightColor), e.Bounds);
            }
            else
            {
                e.DrawBackground();
            }
            string text = ClipsListBox.Items[e.Index]?.ToString() ?? string.Empty;
            var font = e.Font ?? ClipsListBox.Font ?? SystemFonts.DefaultFont;
            using var brush = new SolidBrush(ClipsListBox.ForeColor);
            e.Graphics.DrawString(text, font, brush, e.Bounds);
            e.DrawFocusRectangle();
        };

        ClipsListBox.DrawMode = DrawMode.OwnerDrawFixed;
        ClipsListBox.DrawItem += drawHandler;

        ClipsListBox.Invalidate(ClipsListBox.GetItemRectangle(index));
        await Task.Delay(200);

        ClipsListBox.DrawItem -= drawHandler;
        ClipsListBox.DrawMode = originalDrawMode;
        ClipsListBox.Invalidate(ClipsListBox.GetItemRectangle(index));
    }

    private void ClearButton_Click(object? sender, EventArgs e)
    {
        ClearClips();
    }
}
