namespace Clippy
{
	partial class ClippyForm
	{
		private System.Windows.Forms.ListBox ClipsListBox;
		private System.Windows.Forms.TextBox InputTextBox;
		private System.Windows.Forms.Button AddButton;

		private System.ComponentModel.IContainer components = null;

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClippyForm));
			this.ClipsListBox = new System.Windows.Forms.ListBox();
			this.InputTextBox = new System.Windows.Forms.TextBox();
			this.AddButton = new System.Windows.Forms.Button();
			this.ClearButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// ClipsListBox
			// 
			this.ClipsListBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ClipsListBox.FormattingEnabled = true;
			this.ClipsListBox.HorizontalScrollbar = true;
			this.ClipsListBox.ItemHeight = 21;
			this.ClipsListBox.Location = new System.Drawing.Point(12, 12);
			this.ClipsListBox.Name = "ClipsListBox";
			this.ClipsListBox.Size = new System.Drawing.Size(514, 256);
			this.ClipsListBox.TabIndex = 0;
			this.ClipsListBox.SelectedIndexChanged += new System.EventHandler(this.ClipsListBox_SelectedIndexChanged);
			// 
			// InputTextBox
			// 
			this.InputTextBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.InputTextBox.Location = new System.Drawing.Point(12, 279);
			this.InputTextBox.Name = "InputTextBox";
			this.InputTextBox.Size = new System.Drawing.Size(352, 29);
			this.InputTextBox.TabIndex = 1;
			this.InputTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputTextBox_KeyDown);
			// 
			// AddButton
			// 
			this.AddButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.AddButton.Location = new System.Drawing.Point(370, 279);
			this.AddButton.Name = "AddButton";
			this.AddButton.Size = new System.Drawing.Size(61, 29);
			this.AddButton.TabIndex = 2;
			this.AddButton.Text = "Add";
			this.AddButton.UseVisualStyleBackColor = true;
			this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
			// 
			// ClearButton
			// 
			this.ClearButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ClearButton.Location = new System.Drawing.Point(437, 279);
			this.ClearButton.Name = "ClearButton";
			this.ClearButton.Size = new System.Drawing.Size(89, 29);
			this.ClearButton.TabIndex = 3;
			this.ClearButton.Text = "Clear";
			this.ClearButton.UseVisualStyleBackColor = true;
			this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
			// 
			// ClippyForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(538, 320);
			this.ControlBox = false;
			this.Controls.Add(this.ClearButton);
			this.Controls.Add(this.AddButton);
			this.Controls.Add(this.InputTextBox);
			this.Controls.Add(this.ClipsListBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ClippyForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClipboardForm_Closing);
			this.Load += new System.EventHandler(this.ClipboardForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private System.Windows.Forms.Button ClearButton;
	}
}

