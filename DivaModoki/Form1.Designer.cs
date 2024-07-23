namespace DivaModoki
{
    partial class Form_Launch
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button_Launch = new Button();
            checkBox_VSync = new CheckBox();
            checkBox_Borderless = new CheckBox();
            SuspendLayout();
            // 
            // button_Launch
            // 
            button_Launch.Location = new Point(12, 226);
            button_Launch.Name = "button_Launch";
            button_Launch.Size = new Size(260, 23);
            button_Launch.TabIndex = 0;
            button_Launch.Text = "Launch";
            button_Launch.UseVisualStyleBackColor = true;
            button_Launch.Click += button_Launch_Click;
            // 
            // checkBox_VSync
            // 
            checkBox_VSync.AutoSize = true;
            checkBox_VSync.Location = new Point(12, 12);
            checkBox_VSync.Name = "checkBox_VSync";
            checkBox_VSync.Size = new Size(96, 19);
            checkBox_VSync.TabIndex = 1;
            checkBox_VSync.Text = "Enable VSync";
            checkBox_VSync.UseVisualStyleBackColor = true;
            // 
            // checkBox_Borderless
            // 
            checkBox_Borderless.AutoSize = true;
            checkBox_Borderless.Location = new Point(12, 37);
            checkBox_Borderless.Name = "checkBox_Borderless";
            checkBox_Borderless.Size = new Size(127, 19);
            checkBox_Borderless.TabIndex = 2;
            checkBox_Borderless.Text = "Borderless Window";
            checkBox_Borderless.UseVisualStyleBackColor = true;
            // 
            // Form_Launch
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 261);
            Controls.Add(checkBox_Borderless);
            Controls.Add(checkBox_VSync);
            Controls.Add(button_Launch);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form_Launch";
            Text = "Launcher";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_Launch;
        private CheckBox checkBox_VSync;
        private CheckBox checkBox_Borderless;
    }
}
