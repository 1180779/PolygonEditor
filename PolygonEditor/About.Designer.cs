namespace PolygonEditor
{
    partial class About
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
            button_ok = new Button();
            label_about = new Label();
            SuspendLayout();
            // 
            // button_ok
            // 
            button_ok.Location = new Point(97, 89);
            button_ok.Name = "button_ok";
            button_ok.Size = new Size(75, 20);
            button_ok.TabIndex = 0;
            button_ok.Text = "OK";
            button_ok.UseVisualStyleBackColor = true;
            button_ok.Click += button_ok_Click;
            // 
            // label_about
            // 
            label_about.Location = new Point(12, 9);
            label_about.MaximumSize = new Size(400, 400);
            label_about.Name = "label_about";
            label_about.Size = new Size(160, 69);
            label_about.TabIndex = 2;
            label_about.Text = "PolygonEditor Version 0.0.1. Radosław Glasek";
            // 
            // About
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(184, 121);
            ControlBox = false;
            Controls.Add(label_about);
            Controls.Add(button_ok);
            Name = "About";
            StartPosition = FormStartPosition.CenterParent;
            Text = "About";
            ResumeLayout(false);
        }

        #endregion

        private Button button_ok;
        private Label label_about;
    }
}