namespace EveRouteOptimizer
{
    partial class FormMain
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvRoutes;
        private System.Windows.Forms.Button btnNewRoute;
        private System.Windows.Forms.Button btnSendSelectedRoute;
        private System.Windows.Forms.PictureBox pictureAvatar;
        private System.Windows.Forms.ComboBox cmbCharacters;
        private System.Windows.Forms.Button btnAddCharacter;
        private System.Windows.Forms.Button btnRemoveCharacter;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvRoutes = new System.Windows.Forms.DataGridView();
            this.btnNewRoute = new System.Windows.Forms.Button();
            this.btnSendSelectedRoute = new System.Windows.Forms.Button();
            this.pictureAvatar = new System.Windows.Forms.PictureBox();
            this.cmbCharacters = new System.Windows.Forms.ComboBox();
            this.btnAddCharacter = new System.Windows.Forms.Button();
            this.btnRemoveCharacter = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureAvatar)).BeginInit();
            this.SuspendLayout();

            // pictureAvatar
            this.pictureAvatar.Location = new System.Drawing.Point(20, 10);
            this.pictureAvatar.Name = "pictureAvatar";
            this.pictureAvatar.Size = new System.Drawing.Size(64, 64);
            this.pictureAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureAvatar.TabIndex = 3;
            this.pictureAvatar.TabStop = false;

            // cmbCharacters
            this.cmbCharacters.FormattingEnabled = true;
            this.cmbCharacters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCharacters.Location = new System.Drawing.Point(230, 29);
            this.cmbCharacters.Name = "cmbCharacters";          
            this.cmbCharacters.Size = new System.Drawing.Size(180, 28);
            this.cmbCharacters.TabIndex = 4;            

            // btnAddCharacter
            this.btnAddCharacter.Location = new System.Drawing.Point(420, 29);
            this.btnAddCharacter.Name = "btnAddCharacter";
            this.btnAddCharacter.Size = new System.Drawing.Size(30, 28);
            this.btnAddCharacter.TabIndex = 5;
            this.btnAddCharacter.Text = "+";
            this.btnAddCharacter.UseVisualStyleBackColor = true;
            this.btnAddCharacter.Click += new System.EventHandler(this.btnAddCharacter_Click);

            // btnRemoveCharacter
            this.btnRemoveCharacter.Location = new System.Drawing.Point(455, 29);
            this.btnRemoveCharacter.Name = "btnRemoveCharacter";
            this.btnRemoveCharacter.Size = new System.Drawing.Size(30, 28);
            this.btnRemoveCharacter.TabIndex = 6;
            this.btnRemoveCharacter.Text = "−";
            this.btnRemoveCharacter.UseVisualStyleBackColor = true;
            this.btnRemoveCharacter.Click += new System.EventHandler(this.btnRemoveCharacter_Click);

            // dgvRoutes
            this.dgvRoutes.AllowUserToAddRows = false;
            this.dgvRoutes.AllowUserToDeleteRows = false;
            this.dgvRoutes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRoutes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRoutes.Location = new System.Drawing.Point(20, 84);
            this.dgvRoutes.MultiSelect = false;
            this.dgvRoutes.Name = "dgvRoutes";
            this.dgvRoutes.ReadOnly = true;
            this.dgvRoutes.RowHeadersVisible = false;
            this.dgvRoutes.RowTemplate.Height = 24;
            this.dgvRoutes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRoutes.Size = new System.Drawing.Size(460, 300);
            this.dgvRoutes.TabIndex = 0;            

            // btnNewRoute
            this.btnNewRoute.Location = new System.Drawing.Point(20, 390);
            this.btnNewRoute.Name = "btnNewRoute";
            this.btnNewRoute.Size = new System.Drawing.Size(150, 40);
            this.btnNewRoute.TabIndex = 1;
            this.btnNewRoute.Text = "Рассчитать маршрут";
            this.btnNewRoute.UseVisualStyleBackColor = true;
            this.btnNewRoute.Click += new System.EventHandler(this.btnNewRoute_Click);

            // btnSendSelectedRoute
            this.btnSendSelectedRoute.Location = new System.Drawing.Point(180, 390);
            this.btnSendSelectedRoute.Name = "btnSendSelectedRoute";
            this.btnSendSelectedRoute.Size = new System.Drawing.Size(150, 40);
            this.btnSendSelectedRoute.TabIndex = 2;
            this.btnSendSelectedRoute.Text = "В автопилот";
            this.btnSendSelectedRoute.UseVisualStyleBackColor = true;
            this.btnSendSelectedRoute.Click += new System.EventHandler(this.btnSendSelectedRoute_Click);

            // FormMain
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 465);
            this.Controls.Add(this.btnRemoveCharacter);
            this.Controls.Add(this.btnAddCharacter);
            this.Controls.Add(this.cmbCharacters);
            this.Controls.Add(this.pictureAvatar);
            this.Controls.Add(this.btnSendSelectedRoute);
            this.Controls.Add(this.btnNewRoute);
            this.Controls.Add(this.dgvRoutes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EVE Route Optimizer";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureAvatar)).EndInit();
            this.ResumeLayout(false);

            this.statusStrip = new StatusStrip();
            this.statusLabel = new ToolStripStatusLabel();

            this.statusStrip.Items.Add(this.statusLabel);
            this.statusStrip.Dock = DockStyle.Bottom;

            this.Controls.Add(this.statusStrip);
        }
    }
}