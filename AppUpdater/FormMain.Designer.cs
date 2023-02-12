namespace AppUpdater;

partial class FormMain
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxUri = new System.Windows.Forms.ComboBox();
            this.textBoxTargetDir = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.progressBarStatus = new System.Windows.Forms.ProgressBar();
            this.labelStatus = new System.Windows.Forms.Label();
            this.tableLayoutPanelStart = new System.Windows.Forms.TableLayoutPanel();
            this.buttonUpdateInNoobMode = new System.Windows.Forms.Button();
            this.buttonUpadteInTaskDialogMode = new System.Windows.Forms.Button();
            this.buttonUpdateInLauncherMode = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanelStart.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 7);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(123, 15);
            label1.TabIndex = 0;
            label1.Text = "URI of Updates Server:";
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(3, 37);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(93, 15);
            label2.TabIndex = 0;
            label2.Text = "Target Directory:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxUri, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxTargetDir, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonBrowse, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.progressBarStatus, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelStatus, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanelStart, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(599, 279);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // comboBoxUri
            // 
            this.comboBoxUri.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.comboBoxUri, 2);
            this.comboBoxUri.FormattingEnabled = true;
            this.comboBoxUri.Items.AddRange(new object[] {
            "https://updates1.riuson.com",
            "https://updates2.riuson.com",
            "https://updates3.riuson.com"});
            this.comboBoxUri.Location = new System.Drawing.Point(132, 3);
            this.comboBoxUri.Name = "comboBoxUri";
            this.comboBoxUri.Size = new System.Drawing.Size(464, 23);
            this.comboBoxUri.TabIndex = 1;
            // 
            // textBoxTargetDir
            // 
            this.textBoxTargetDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTargetDir.Location = new System.Drawing.Point(132, 33);
            this.textBoxTargetDir.Name = "textBoxTargetDir";
            this.textBoxTargetDir.Size = new System.Drawing.Size(432, 23);
            this.textBoxTargetDir.TabIndex = 2;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.AutoSize = true;
            this.buttonBrowse.Location = new System.Drawing.Point(570, 32);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(26, 25);
            this.buttonBrowse.TabIndex = 3;
            this.buttonBrowse.Text = "...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // progressBarStatus
            // 
            this.progressBarStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.progressBarStatus, 3);
            this.progressBarStatus.Location = new System.Drawing.Point(3, 98);
            this.progressBarStatus.Name = "progressBarStatus";
            this.progressBarStatus.Size = new System.Drawing.Size(593, 23);
            this.progressBarStatus.TabIndex = 4;
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStatus.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.labelStatus, 3);
            this.labelStatus.Location = new System.Drawing.Point(3, 124);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(593, 15);
            this.labelStatus.TabIndex = 5;
            this.labelStatus.Text = "...";
            // 
            // tableLayoutPanelStart
            // 
            this.tableLayoutPanelStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelStart.AutoSize = true;
            this.tableLayoutPanelStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelStart.ColumnCount = 3;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanelStart, 3);
            this.tableLayoutPanelStart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelStart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanelStart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanelStart.Controls.Add(this.buttonUpdateInNoobMode, 0, 0);
            this.tableLayoutPanelStart.Controls.Add(this.buttonUpadteInTaskDialogMode, 1, 0);
            this.tableLayoutPanelStart.Controls.Add(this.buttonUpdateInLauncherMode, 2, 0);
            this.tableLayoutPanelStart.Location = new System.Drawing.Point(3, 63);
            this.tableLayoutPanelStart.Name = "tableLayoutPanelStart";
            this.tableLayoutPanelStart.RowCount = 1;
            this.tableLayoutPanelStart.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStart.Size = new System.Drawing.Size(593, 29);
            this.tableLayoutPanelStart.TabIndex = 6;
            // 
            // buttonUpdateInNoobMode
            // 
            this.buttonUpdateInNoobMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpdateInNoobMode.Location = new System.Drawing.Point(3, 3);
            this.buttonUpdateInNoobMode.Name = "buttonUpdateInNoobMode";
            this.buttonUpdateInNoobMode.Size = new System.Drawing.Size(191, 23);
            this.buttonUpdateInNoobMode.TabIndex = 0;
            this.buttonUpdateInNoobMode.Text = "Update in Noob Mode";
            this.buttonUpdateInNoobMode.UseVisualStyleBackColor = true;
            this.buttonUpdateInNoobMode.Click += new System.EventHandler(this.buttonUpdateInNoobMode_Click);
            // 
            // buttonUpadteInTaskDialogMode
            // 
            this.buttonUpadteInTaskDialogMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpadteInTaskDialogMode.Location = new System.Drawing.Point(200, 3);
            this.buttonUpadteInTaskDialogMode.Name = "buttonUpadteInTaskDialogMode";
            this.buttonUpadteInTaskDialogMode.Size = new System.Drawing.Size(191, 23);
            this.buttonUpadteInTaskDialogMode.TabIndex = 1;
            this.buttonUpadteInTaskDialogMode.Text = "Update with Task Dialog";
            this.buttonUpadteInTaskDialogMode.UseVisualStyleBackColor = true;
            this.buttonUpadteInTaskDialogMode.Click += new System.EventHandler(this.buttonUpdateWithTaskDialog_Click);
            // 
            // buttonUpdateInLauncherMode
            // 
            this.buttonUpdateInLauncherMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpdateInLauncherMode.Location = new System.Drawing.Point(397, 3);
            this.buttonUpdateInLauncherMode.Name = "buttonUpdateInLauncherMode";
            this.buttonUpdateInLauncherMode.Size = new System.Drawing.Size(193, 23);
            this.buttonUpdateInLauncherMode.TabIndex = 2;
            this.buttonUpdateInLauncherMode.Text = "Update in Launcher Mode";
            this.buttonUpdateInLauncherMode.UseVisualStyleBackColor = true;
            this.buttonUpdateInLauncherMode.Click += new System.EventHandler(this.buttonUpdateInLauncherMode_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanelStart.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    private TableLayoutPanel tableLayoutPanel1;
    private Label label1;
    private Label label2;
    private ComboBox comboBoxUri;
    private TextBox textBoxTargetDir;
    private Button buttonBrowse;
    private ProgressBar progressBarStatus;
    private Label labelStatus;
    private TableLayoutPanel tableLayoutPanelStart;
    private Button buttonUpdateInNoobMode;
    private Button buttonUpadteInTaskDialogMode;
    private Button buttonUpdateInLauncherMode;
}
