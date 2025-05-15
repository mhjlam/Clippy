using Clippy.Win32;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Clippy;

public class ClippyContext : ApplicationContext
{
    public NotifyIcon NotifyIcon { get; set; }
    private readonly ClippyForm clipboardForm;
    private readonly IContainer components;
    private bool isFormVisible = false;

    // Hotkey constants
    private const int WM_HOTKEY = 0x0312;
    private const int MOD_ALT = 0x1;
    private const int MOD_SHIFT = 0x4;
    private const int VK_C = 0x43;
    private const int HOTKEY_ID = 0xC1C1;

    public ClippyContext()
    {
        components = new Container();

        NotifyIcon = new NotifyIcon(components)
        {
            ContextMenuStrip = new ContextMenuStrip(),
			Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath),
            Text = "Clippy",
            Visible = true
        };

        NotifyIcon.Click += NotifyIcon_Click;

        var contextMenuStrip = new ContextMenuStrip();
        var toolStripMenuItemClear = new ToolStripMenuItem("Clear", null, Clear);
        var toolStripMenuItemExit = new ToolStripMenuItem("Exit", null, Exit);
        contextMenuStrip.Items.AddRange([toolStripMenuItemClear, toolStripMenuItemExit]);
        NotifyIcon.ContextMenuStrip = contextMenuStrip;

        clipboardForm = new ClippyForm(this)
        {
            TopMost = true,
            KeyPreview = true,
            Opacity = 0
        };
        clipboardForm.KeyDown += ClipboardForm_KeyDown;

        RegisterGlobalHotkey();
        Application.AddMessageFilter(new HotkeyMessageFilter(this));
    }

    private void RegisterGlobalHotkey()
    {
        var handle = clipboardForm.Handle;
        clipboardForm.Hide();
        clipboardForm.Opacity = 1;
		User32.RegisterHotKey(handle, HOTKEY_ID, MOD_ALT | MOD_SHIFT, VK_C);
    }

    private void UnregisterGlobalHotkey()
    {
        if (clipboardForm != null && clipboardForm.IsHandleCreated)
        {
			User32.UnregisterHotKey(clipboardForm.Handle, HOTKEY_ID);
        }
    }

    internal void OnGlobalHotkeyPressed()
    {
        if (isFormVisible)
        {
            var foreground = User32.GetForegroundWindow();
            if (clipboardForm.Handle != foreground)
            {
                clipboardForm.TopMost = true;
                clipboardForm.BringToFront();
                clipboardForm.Activate();
                clipboardForm.Focus();
            }
            else
            {
                clipboardForm.Hide();
                isFormVisible = false;
            }
        }
        else
        {
            ToggleClipboardForm(null, EventArgs.Empty);
        }
    }

    protected override void Dispose(bool disposing)
    {
        UnregisterGlobalHotkey();
        base.Dispose(disposing);
    }

    private void ClipboardForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            clipboardForm.Hide();
            isFormVisible = false;
        }
    }

    private void NotifyIcon_Click(object? sender, EventArgs e)
    {
        if (e is MouseEventArgs mouseEvent && mouseEvent.Button == MouseButtons.Left)
        {
            ToggleClipboardForm(sender, EventArgs.Empty);
            return;
        }
        else if (e is KeyEventArgs keyEvent && keyEvent.KeyCode == Keys.Enter)
        {
            ToggleClipboardForm(sender, EventArgs.Empty);
            return;
        }
    }

    private void ToggleClipboardForm(object? sender, EventArgs args)
    {
        if (isFormVisible)
        {
            clipboardForm.Hide();
            isFormVisible = false;
        }
        else
        {
            clipboardForm.TopMost = true;
            clipboardForm.Show();
            clipboardForm.Activate();
            isFormVisible = true;
        }
    }

    private void Clear(object? sender, EventArgs e)
    {
        clipboardForm.ClearClips();
    }

    private void Exit(object? sender, EventArgs e)
    {
        // Properly clean up resources before exiting
        UnregisterGlobalHotkey();
        NotifyIcon.Visible = false;
        NotifyIcon.Dispose();
        clipboardForm.Dispose();
        components.Dispose();
        Application.Exit();
    }

    private class HotkeyMessageFilter(ClippyContext ctx) : IMessageFilter
    {
        private readonly ClippyContext context = ctx;

		public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_HOTKEY && (int)m.WParam == HOTKEY_ID)
            {
                context.OnGlobalHotkeyPressed();
                return true;
            }
            return false;
        }
    }
}
