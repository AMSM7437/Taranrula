namespace Tarantula.Interface
{
    partial class frmMainTInterface
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
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            dgvCrawledUrls = new DataGridView();
            clmUrl = new DataGridViewTextBoxColumn();
            clmStatus = new DataGridViewTextBoxColumn();
            clmWordCount = new DataGridViewTextBoxColumn();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCrawledUrls).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28.5714283F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 71.42857F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(dgvCrawledUrls, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(1057, 536);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 490F));
            tableLayoutPanel2.Size = new Size(296, 530);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // dataGridView1
            // 
            dgvCrawledUrls.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCrawledUrls.Columns.AddRange(new DataGridViewColumn[] { clmUrl, clmStatus, clmWordCount });
            dgvCrawledUrls.Dock = DockStyle.Fill;
            dgvCrawledUrls.Location = new Point(302, 0);
            dgvCrawledUrls.Margin = new Padding(0);
            dgvCrawledUrls.Name = "dataGridView1";
            dgvCrawledUrls.Size = new Size(755, 536);
            dgvCrawledUrls.TabIndex = 1;
            // 
            // clmUrl
            // 
            clmUrl.HeaderText = "Url";
            clmUrl.Name = "clmUrl";
            // 
            // clmStatus
            // 
            clmStatus.HeaderText = "Status";
            clmStatus.Name = "clmStatus";
            // 
            // clmWordCount
            // 
            clmWordCount.HeaderText = "Word Count";
            clmWordCount.Name = "clmWordCount";
            // 
            // frmMainTInterface
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1057, 536);
            Controls.Add(tableLayoutPanel1);
            Name = "frmMainTInterface";
            Text = "Tarantula Interface";
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCrawledUrls).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private DataGridView dgvCrawledUrls;
        private DataGridViewTextBoxColumn clmUrl;
        private DataGridViewTextBoxColumn clmStatus;
        private DataGridViewTextBoxColumn clmWordCount;
    }
}