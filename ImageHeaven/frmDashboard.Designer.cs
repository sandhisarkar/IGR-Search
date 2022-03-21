namespace ImageHeaven
{
    partial class frmDashboard
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDashboard));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.deCheckBox2 = new nControls.deCheckBox();
            this.deCheckBox1 = new nControls.deCheckBox();
            this.deTextBox1 = new nControls.deTextBox();
            this.deLabel1 = new nControls.deLabel();
            this.deButton1 = new nControls.deButton();
            this.deTextBox2 = new nControls.deTextBox();
            this.deLabel2 = new nControls.deLabel();
            this.deTextBox3 = new nControls.deTextBox();
            this.deLabel3 = new nControls.deLabel();
            this.cmsDeeds = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.updateDeedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvbatch = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView6 = new System.Windows.Forms.DataGridView();
            this.dataGridView5 = new System.Windows.Forms.DataGridView();
            this.dataGridView4 = new System.Windows.Forms.DataGridView();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.sfdUAT = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.cmsDeeds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvbatch)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Controls.Add(this.deCheckBox2);
            this.groupBox1.Controls.Add(this.deCheckBox1);
            this.groupBox1.Controls.Add(this.deTextBox1);
            this.groupBox1.Controls.Add(this.deLabel1);
            this.groupBox1.Controls.Add(this.deButton1);
            this.groupBox1.Controls.Add(this.deTextBox2);
            this.groupBox1.Controls.Add(this.deLabel2);
            this.groupBox1.Controls.Add(this.deTextBox3);
            this.groupBox1.Controls.Add(this.deLabel3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(632, 158);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Criteria :";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(1126, 103);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(68, 41);
            this.dataGridView1.TabIndex = 13;
            // 
            // deCheckBox2
            // 
            this.deCheckBox2.AutoSize = true;
            this.deCheckBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deCheckBox2.Location = new System.Drawing.Point(157, 132);
            this.deCheckBox2.Name = "deCheckBox2";
            this.deCheckBox2.Size = new System.Drawing.Size(99, 21);
            this.deCheckBox2.TabIndex = 12;
            this.deCheckBox2.Text = "Select None";
            this.deCheckBox2.UseVisualStyleBackColor = true;
            this.deCheckBox2.CheckedChanged += new System.EventHandler(this.deCheckBox2_CheckedChanged);
            // 
            // deCheckBox1
            // 
            this.deCheckBox1.AutoSize = true;
            this.deCheckBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deCheckBox1.Location = new System.Drawing.Point(18, 133);
            this.deCheckBox1.Name = "deCheckBox1";
            this.deCheckBox1.Size = new System.Drawing.Size(81, 21);
            this.deCheckBox1.TabIndex = 11;
            this.deCheckBox1.Text = "Select All";
            this.deCheckBox1.UseVisualStyleBackColor = true;
            this.deCheckBox1.CheckedChanged += new System.EventHandler(this.deCheckBox1_CheckedChanged);
            // 
            // deTextBox1
            // 
            this.deTextBox1.BackColor = System.Drawing.Color.White;
            this.deTextBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deTextBox1.ForeColor = System.Drawing.Color.Black;
            this.deTextBox1.Location = new System.Drawing.Point(242, 26);
            this.deTextBox1.Mandatory = true;
            this.deTextBox1.Name = "deTextBox1";
            this.deTextBox1.Size = new System.Drawing.Size(237, 23);
            this.deTextBox1.TabIndex = 1;
            // 
            // deLabel1
            // 
            this.deLabel1.AutoSize = true;
            this.deLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel1.Location = new System.Drawing.Point(153, 29);
            this.deLabel1.Name = "deLabel1";
            this.deLabel1.Size = new System.Drawing.Size(85, 15);
            this.deLabel1.TabIndex = 0;
            this.deLabel1.Text = "District Name :";
            // 
            // deButton1
            // 
            this.deButton1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.deButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deButton1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deButton1.Image = ((System.Drawing.Image)(resources.GetObject("deButton1.Image")));
            this.deButton1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.deButton1.Location = new System.Drawing.Point(389, 92);
            this.deButton1.Name = "deButton1";
            this.deButton1.Size = new System.Drawing.Size(90, 36);
            this.deButton1.TabIndex = 10;
            this.deButton1.Text = "&Search";
            this.deButton1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deButton1.UseCompatibleTextRendering = true;
            this.deButton1.UseVisualStyleBackColor = true;
            this.deButton1.Click += new System.EventHandler(this.deButton1_Click);
            // 
            // deTextBox2
            // 
            this.deTextBox2.BackColor = System.Drawing.Color.White;
            this.deTextBox2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deTextBox2.ForeColor = System.Drawing.Color.Black;
            this.deTextBox2.Location = new System.Drawing.Point(242, 60);
            this.deTextBox2.Mandatory = true;
            this.deTextBox2.Name = "deTextBox2";
            this.deTextBox2.Size = new System.Drawing.Size(237, 23);
            this.deTextBox2.TabIndex = 3;
            // 
            // deLabel2
            // 
            this.deLabel2.AutoSize = true;
            this.deLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel2.Location = new System.Drawing.Point(112, 64);
            this.deLabel2.Name = "deLabel2";
            this.deLabel2.Size = new System.Drawing.Size(125, 15);
            this.deLabel2.TabIndex = 2;
            this.deLabel2.Text = "Registry Office Name :";
            // 
            // deTextBox3
            // 
            this.deTextBox3.BackColor = System.Drawing.Color.White;
            this.deTextBox3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deTextBox3.ForeColor = System.Drawing.Color.Black;
            this.deTextBox3.Location = new System.Drawing.Point(243, 97);
            this.deTextBox3.Mandatory = true;
            this.deTextBox3.Name = "deTextBox3";
            this.deTextBox3.Size = new System.Drawing.Size(84, 23);
            this.deTextBox3.TabIndex = 5;
            // 
            // deLabel3
            // 
            this.deLabel3.AutoSize = true;
            this.deLabel3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel3.Location = new System.Drawing.Point(170, 100);
            this.deLabel3.Name = "deLabel3";
            this.deLabel3.Size = new System.Drawing.Size(67, 15);
            this.deLabel3.TabIndex = 4;
            this.deLabel3.Text = "Book Type :";
            // 
            // cmsDeeds
            // 
            this.cmsDeeds.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateDeedToolStripMenuItem});
            this.cmsDeeds.Name = "cmsDeeds";
            this.cmsDeeds.Size = new System.Drawing.Size(159, 26);
            this.cmsDeeds.Text = "Download CSV Only";
            // 
            // updateDeedToolStripMenuItem
            // 
            this.updateDeedToolStripMenuItem.Name = "updateDeedToolStripMenuItem";
            this.updateDeedToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.updateDeedToolStripMenuItem.Text = "Download Excel";
            this.updateDeedToolStripMenuItem.Click += new System.EventHandler(this.updateDeedToolStripMenuItem_Click);
            // 
            // Column1
            // 
            this.Column1.FalseValue = "false";
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.TrueValue = "true";
            // 
            // dgvbatch
            // 
            this.dgvbatch.AllowUserToAddRows = false;
            this.dgvbatch.AllowUserToDeleteRows = false;
            this.dgvbatch.AllowUserToResizeColumns = false;
            this.dgvbatch.AllowUserToResizeRows = false;
            this.dgvbatch.BackgroundColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.dgvbatch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvbatch.ColumnHeadersVisible = false;
            this.dgvbatch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.dgvbatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvbatch.Location = new System.Drawing.Point(0, 0);
            this.dgvbatch.Name = "dgvbatch";
            this.dgvbatch.RowHeadersVisible = false;
            this.dgvbatch.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvbatch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvbatch.Size = new System.Drawing.Size(632, 420);
            this.dgvbatch.TabIndex = 13;
            this.dgvbatch.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvbatch_CellContentClick);
            this.dgvbatch.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvbatch_CellMouseClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView6);
            this.panel2.Controls.Add(this.dataGridView5);
            this.panel2.Controls.Add(this.dataGridView4);
            this.panel2.Controls.Add(this.dataGridView3);
            this.panel2.Controls.Add(this.dataGridView2);
            this.panel2.Controls.Add(this.dgvbatch);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 158);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(632, 420);
            this.panel2.TabIndex = 15;
            // 
            // dataGridView6
            // 
            this.dataGridView6.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView6.Location = new System.Drawing.Point(1125, 100);
            this.dataGridView6.Name = "dataGridView6";
            this.dataGridView6.Size = new System.Drawing.Size(174, 116);
            this.dataGridView6.TabIndex = 18;
            // 
            // dataGridView5
            // 
            this.dataGridView5.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView5.Location = new System.Drawing.Point(1121, 192);
            this.dataGridView5.Name = "dataGridView5";
            this.dataGridView5.Size = new System.Drawing.Size(174, 116);
            this.dataGridView5.TabIndex = 17;
            // 
            // dataGridView4
            // 
            this.dataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView4.Location = new System.Drawing.Point(1120, 162);
            this.dataGridView4.Name = "dataGridView4";
            this.dataGridView4.Size = new System.Drawing.Size(174, 116);
            this.dataGridView4.TabIndex = 16;
            // 
            // dataGridView3
            // 
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Location = new System.Drawing.Point(1118, 102);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.Size = new System.Drawing.Size(174, 116);
            this.dataGridView3.TabIndex = 15;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(1126, 40);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(72, 105);
            this.dataGridView2.TabIndex = 14;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(632, 158);
            this.panel1.TabIndex = 14;
            // 
            // frmDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 578);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.frmDashboard_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.cmsDeeds.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvbatch)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private nControls.deCheckBox deCheckBox2;
        private nControls.deCheckBox deCheckBox1;
        private nControls.deTextBox deTextBox1;
        private nControls.deLabel deLabel1;
        private nControls.deButton deButton1;
        private nControls.deTextBox deTextBox2;
        private nControls.deLabel deLabel2;
        private nControls.deTextBox deTextBox3;
        private nControls.deLabel deLabel3;
        private System.Windows.Forms.ContextMenuStrip cmsDeeds;
        private System.Windows.Forms.ToolStripMenuItem updateDeedToolStripMenuItem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridView dgvbatch;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView6;
        private System.Windows.Forms.DataGridView dataGridView5;
        private System.Windows.Forms.DataGridView dataGridView4;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SaveFileDialog sfdUAT;
    }
}