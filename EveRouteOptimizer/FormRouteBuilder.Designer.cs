namespace EveRouteOptimizer
{
    partial class FormRouteBuilder
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtSystems;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.Label lblInfo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtSystems = new System.Windows.Forms.TextBox();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // 
            // txtSystems
            // 
            this.txtSystems.Location = new System.Drawing.Point(20, 20);
            this.txtSystems.Multiline = true;
            this.txtSystems.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSystems.Name = "txtSystems";
            this.txtSystems.Size = new System.Drawing.Size(440, 200);
            this.txtSystems.TabIndex = 0;

            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point(20, 230);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(120, 40);
            this.btnCalculate.TabIndex = 1;
            this.btnCalculate.Text = "Рассчитать";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);

            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(160, 237);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(0, 20);
            this.lblInfo.TabIndex = 2;

            // 
            // FormRouteBuilder
            // 
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 280);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnCalculate);
            this.Controls.Add(this.txtSystems);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRouteBuilder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Маршрут";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
