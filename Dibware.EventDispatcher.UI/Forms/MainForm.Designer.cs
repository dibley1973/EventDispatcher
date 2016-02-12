namespace Dibware.EventDispatcher.UI.Forms
{
    partial class MainForm
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
            this.ExitButton = new System.Windows.Forms.Button();
            this.HelloWorldButton = new System.Windows.Forms.Button();
            this.MessagingButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(197, 226);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(75, 23);
            this.ExitButton.TabIndex = 0;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // HelloWorldButton
            // 
            this.HelloWorldButton.Location = new System.Drawing.Point(197, 197);
            this.HelloWorldButton.Name = "HelloWorldButton";
            this.HelloWorldButton.Size = new System.Drawing.Size(75, 23);
            this.HelloWorldButton.TabIndex = 1;
            this.HelloWorldButton.Text = "Hello World!";
            this.HelloWorldButton.UseVisualStyleBackColor = true;
            this.HelloWorldButton.Click += new System.EventHandler(this.HelloWorldButton_Click);
            // 
            // MessagingButton
            // 
            this.MessagingButton.Location = new System.Drawing.Point(197, 168);
            this.MessagingButton.Name = "MessagingButton";
            this.MessagingButton.Size = new System.Drawing.Size(75, 23);
            this.MessagingButton.TabIndex = 2;
            this.MessagingButton.Text = "Messaging";
            this.MessagingButton.UseVisualStyleBackColor = true;
            this.MessagingButton.Click += new System.EventHandler(this.MessagingButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.MessagingButton);
            this.Controls.Add(this.HelloWorldButton);
            this.Controls.Add(this.ExitButton);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button HelloWorldButton;
        private System.Windows.Forms.Button MessagingButton;
    }
}