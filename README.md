# Clippy

A simple clipboard history manager for Windows 11.

Written in C# 12.0, using .NET 8.0.

## User's Guide

Clippy starts minimized to the notification area. Clicking the notification icon with the left mouse button shows Clippy's main window. It is always displayed on top of everything else.

The main window can also be shown or hidden by using the global shortcut key combination `Ctrl + Shift + C`.

Clippy's clipboard is automatically populated when the user copies some text to the standard clipboard, and can store up to 12 items.

Additional items can also be added from the input field at the bottom. Press `Enter` or the "Add" button to add a new item.

Items can be removed by selecting them and pressing `Delete`. Item navigation (`arrow up/down`) and deletion is also possible when the input field is selected.

Pressing the "Clear" button in the main window or icon context menu will remove all the saved clipboard items.

Right click the notification icon and choose "Exit" to exit Clippy.

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.
