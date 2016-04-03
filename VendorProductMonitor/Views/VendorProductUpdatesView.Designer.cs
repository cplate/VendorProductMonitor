namespace VendorProductMonitor.Views
{
    partial class VendorProductUpdatesView
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
            this.lblVendors = new System.Windows.Forms.Label();
            this.vendorsLoadingLabel = new System.Windows.Forms.Label();
            this.lblUpdates = new System.Windows.Forms.Label();
            this.waitingForUpdatesLabel = new System.Windows.Forms.Label();
            this.productUpdatesGrid = new System.Windows.Forms.DataGridView();
            this.vendorGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.productUpdatesGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vendorGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // lblVendors
            // 
            this.lblVendors.AutoSize = true;
            this.lblVendors.Location = new System.Drawing.Point(22, 18);
            this.lblVendors.Name = "lblVendors";
            this.lblVendors.Size = new System.Drawing.Size(168, 17);
            this.lblVendors.TabIndex = 1;
            this.lblVendors.Text = "Vendors Being Monitored";
            // 
            // vendorsLoadingLabel
            // 
            this.vendorsLoadingLabel.AutoSize = true;
            this.vendorsLoadingLabel.Location = new System.Drawing.Point(22, 50);
            this.vendorsLoadingLabel.Name = "vendorsLoadingLabel";
            this.vendorsLoadingLabel.Size = new System.Drawing.Size(91, 17);
            this.vendorsLoadingLabel.TabIndex = 2;
            this.vendorsLoadingLabel.Text = "Loading........";
            // 
            // lblUpdates
            // 
            this.lblUpdates.AutoSize = true;
            this.lblUpdates.Location = new System.Drawing.Point(549, 18);
            this.lblUpdates.Name = "lblUpdates";
            this.lblUpdates.Size = new System.Drawing.Size(189, 17);
            this.lblUpdates.TabIndex = 0;
            this.lblUpdates.Text = "Most recent product updates";
            // 
            // waitingForUpdatesLabel
            // 
            this.waitingForUpdatesLabel.AutoSize = true;
            this.waitingForUpdatesLabel.Location = new System.Drawing.Point(549, 50);
            this.waitingForUpdatesLabel.Name = "waitingForUpdatesLabel";
            this.waitingForUpdatesLabel.Size = new System.Drawing.Size(163, 17);
            this.waitingForUpdatesLabel.TabIndex = 3;
            this.waitingForUpdatesLabel.Text = "Waiting for updates........";
            // 
            // productUpdatesGrid
            // 
            this.productUpdatesGrid.AllowUserToAddRows = false;
            this.productUpdatesGrid.AllowUserToDeleteRows = false;
            this.productUpdatesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.productUpdatesGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.productUpdatesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.productUpdatesGrid.Location = new System.Drawing.Point(552, 50);
            this.productUpdatesGrid.MultiSelect = false;
            this.productUpdatesGrid.Name = "productUpdatesGrid";
            this.productUpdatesGrid.ReadOnly = true;
            this.productUpdatesGrid.RowHeadersVisible = false;
            this.productUpdatesGrid.RowTemplate.Height = 24;
            this.productUpdatesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.productUpdatesGrid.Size = new System.Drawing.Size(771, 645);
            this.productUpdatesGrid.TabIndex = 4;
            // 
            // vendorGrid
            // 
            this.vendorGrid.AllowUserToAddRows = false;
            this.vendorGrid.AllowUserToDeleteRows = false;
            this.vendorGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vendorGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.vendorGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.vendorGrid.Location = new System.Drawing.Point(25, 50);
            this.vendorGrid.MultiSelect = false;
            this.vendorGrid.Name = "vendorGrid";
            this.vendorGrid.ReadOnly = true;
            this.vendorGrid.RowHeadersVisible = false;
            this.vendorGrid.RowTemplate.Height = 24;
            this.vendorGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.vendorGrid.Size = new System.Drawing.Size(510, 645);
            this.vendorGrid.TabIndex = 0;
            // 
            // VendorProductUpdatesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1335, 707);
            this.Controls.Add(this.vendorGrid);
            this.Controls.Add(this.productUpdatesGrid);
            this.Controls.Add(this.waitingForUpdatesLabel);
            this.Controls.Add(this.lblVendors);
            this.Controls.Add(this.lblUpdates);
            this.Controls.Add(this.vendorsLoadingLabel);
            this.Name = "VendorProductUpdatesView";
            this.Text = "VendorProductUpdatesView";
            ((System.ComponentModel.ISupportInitialize)(this.productUpdatesGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vendorGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblVendors;
        private System.Windows.Forms.Label vendorsLoadingLabel;
        private System.Windows.Forms.Label waitingForUpdatesLabel;
        private System.Windows.Forms.Label lblUpdates;
        private System.Windows.Forms.DataGridView productUpdatesGrid;
        private System.Windows.Forms.DataGridView vendorGrid;
    }
}