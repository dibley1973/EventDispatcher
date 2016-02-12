namespace Dibware.EventDispatcher.UI.Forms
{
    partial class FormA
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
            this.SendMessageButton = new System.Windows.Forms.Button();
            this.CreateMessageTextBox = new System.Windows.Forms.TextBox();
            this.DisplayMessageLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SendMessageButton
            // 
            this.SendMessageButton.Location = new System.Drawing.Point(199, 227);
            this.SendMessageButton.Name = "SendMessageButton";
            this.SendMessageButton.Size = new System.Drawing.Size(75, 23);
            this.SendMessageButton.TabIndex = 5;
            this.SendMessageButton.Text = "Send";
            this.SendMessageButton.UseVisualStyleBackColor = true;
            this.SendMessageButton.Click += new System.EventHandler(this.SendMessageButton_Click);
            // 
            // CreateMessageTextBox
            // 
            this.CreateMessageTextBox.Location = new System.Drawing.Point(12, 119);
            this.CreateMessageTextBox.Multiline = true;
            this.CreateMessageTextBox.Name = "CreateMessageTextBox";
            this.CreateMessageTextBox.Size = new System.Drawing.Size(260, 102);
            this.CreateMessageTextBox.TabIndex = 4;
            // 
            // DisplayMessageLabel
            // 
            this.DisplayMessageLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DisplayMessageLabel.Location = new System.Drawing.Point(12, 10);
            this.DisplayMessageLabel.Name = "DisplayMessageLabel";
            this.DisplayMessageLabel.Size = new System.Drawing.Size(260, 97);
            this.DisplayMessageLabel.TabIndex = 3;
            // 
            // FormA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.SendMessageButton);
            this.Controls.Add(this.CreateMessageTextBox);
            this.Controls.Add(this.DisplayMessageLabel);
            this.Name = "FormA";
            this.Text = "FormA";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SendMessageButton;
        private System.Windows.Forms.TextBox CreateMessageTextBox;
        private System.Windows.Forms.Label DisplayMessageLabel;
    }
}