namespace EveRouteOptimizer
{
    partial class FormOAuthLogin
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnClose;

        /// <summary>
        /// Освобождение ресурсов.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Инициализация компонентов формы.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblStatus.Location = new System.Drawing.Point(30, 20);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(200, 23);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Ожидание авторизации...";
            // 
            // btnClose
            // 
            this.btnClose.Enabled = false;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnClose.Location = new System.Drawing.Point(30, 60);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 32);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FormOAuthLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 120);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormOAuthLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Авторизация через EVE";
            this.Load += new System.EventHandler(this.FormOAuthLogin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
