namespace Game
{
    partial class StartForm
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
            options = new ComboBox();
            button1 = new Button();
            amount = new TextBox();
            SuspendLayout();
            // 
            // options
            // 
            options.FormattingEnabled = true;
            options.Items.AddRange(new object[] { "classic", "fantasy" });
            options.Location = new Point(159, 153);
            options.Name = "options";
            options.Size = new Size(151, 28);
            options.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(186, 200);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 1;
            button1.Text = "Start";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // amount
            // 
            amount.Location = new Point(159, 120);
            amount.Name = "amount";
            amount.Size = new Size(151, 27);
            amount.TabIndex = 2;
            // 
            // StartForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(482, 379);
            Controls.Add(amount);
            Controls.Add(button1);
            Controls.Add(options);
            Name = "StartForm";
            Text = "StartForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox options;
        private Button button1;
        private TextBox amount;
    }
}