namespace Dibware.EventDispatcher.UI.Forms
{
    partial class FormB
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                UnwireApplicationEventHandlers();
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DisplayMessageLabel = new System.Windows.Forms.Label();
            this.CreateMessageTextBox = new System.Windows.Forms.TextBox();
            this.SendMessageButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DisplayMessageLabel
            // 
            this.DisplayMessageLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DisplayMessageLabel.Location = new System.Drawing.Point(12, 9);
            this.DisplayMessageLabel.Name = "DisplayMessageLabel";
            this.DisplayMessageLabel.Size = new System.Drawing.Size(260, 97);
            this.DisplayMessageLabel.TabIndex = 0;
            // 
            // CreateMessageTextBox
            // 
            this.CreateMessageTextBox.Location = new System.Drawing.Point(12, 118);
            this.CreateMessageTextBox.Multiline = true;
            this.CreateMessageTextBox.Name = "CreateMessageTextBox";
            this.CreateMessageTextBox.Size = new System.Drawing.Size(260, 102);
            this.CreateMessageTextBox.TabIndex = 1;
            // 
            // SendMessageButton
            // 
            this.SendMessageButton.Location = new System.Drawing.Point(197, 226);
            this.SendMessageButton.Name = "SendMessageButton";
            this.SendMessageButton.Size = new System.Drawing.Size(75, 23);
            this.SendMessageButton.TabIndex = 2;
            this.SendMessageButton.Text = "Send";
            this.SendMessageButton.UseVisualStyleBackColor = true;
            this.SendMessageButton.Click += new System.EventHandler(this.SendMessageButton_Click);
            // 
            // FormB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.SendMessageButton);
            this.Controls.Add(this.CreateMessageTextBox);
            this.Controls.Add(this.DisplayMessageLabel);
            this.Name = "FormB";
            this.Text = "FormB";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DisplayMessageLabel;
        private System.Windows.Forms.TextBox CreateMessageTextBox;
        private System.Windows.Forms.Button SendMessageButton;
    }
}