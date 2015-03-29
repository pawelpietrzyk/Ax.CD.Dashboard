namespace Ax.CD.Dashboard.WindowsForm
{
    partial class CDServiceForm
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
            this.btnStart = new System.Windows.Forms.Button();
            this.btnEnd = new System.Windows.Forms.Button();
            this.txtDays = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtItemOwnerId = new System.Windows.Forms.TextBox();
            this.lblState = new System.Windows.Forms.Label();
            this.customDateTime = new System.Windows.Forms.DateTimePicker();
            this.cbxIsCustomDate = new System.Windows.Forms.CheckBox();
            this.lblCounter = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(194, 38);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnEnd
            // 
            this.btnEnd.Location = new System.Drawing.Point(194, 61);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(75, 23);
            this.btnEnd.TabIndex = 1;
            this.btnEnd.Text = "Stop";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
            // 
            // txtDays
            // 
            this.txtDays.Location = new System.Drawing.Point(275, 85);
            this.txtDays.Name = "txtDays";
            this.txtDays.Size = new System.Drawing.Size(100, 20);
            this.txtDays.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(381, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Days";
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(275, 61);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(100, 20);
            this.txtInterval.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(383, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Interval";
            // 
            // txtUrl
            // 
            this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrl.Location = new System.Drawing.Point(12, 12);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(411, 20);
            this.txtUrl.TabIndex = 6;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(194, 85);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 7;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(381, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Owner";
            // 
            // txtItemOwnerId
            // 
            this.txtItemOwnerId.Enabled = false;
            this.txtItemOwnerId.Location = new System.Drawing.Point(275, 38);
            this.txtItemOwnerId.Name = "txtItemOwnerId";
            this.txtItemOwnerId.Size = new System.Drawing.Size(100, 20);
            this.txtItemOwnerId.TabIndex = 9;
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblState.Location = new System.Drawing.Point(151, 145);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(83, 31);
            this.lblState.TabIndex = 10;
            this.lblState.Text = "State";
            this.lblState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // customDateTime
            // 
            this.customDateTime.CustomFormat = "";
            this.customDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.customDateTime.Location = new System.Drawing.Point(12, 60);
            this.customDateTime.Name = "customDateTime";
            this.customDateTime.Size = new System.Drawing.Size(176, 20);
            this.customDateTime.TabIndex = 11;
            // 
            // cbxIsCustomDate
            // 
            this.cbxIsCustomDate.AutoSize = true;
            this.cbxIsCustomDate.Location = new System.Drawing.Point(12, 37);
            this.cbxIsCustomDate.Name = "cbxIsCustomDate";
            this.cbxIsCustomDate.Size = new System.Drawing.Size(98, 17);
            this.cbxIsCustomDate.TabIndex = 12;
            this.cbxIsCustomDate.Text = "Is Custom Date";
            this.cbxIsCustomDate.UseVisualStyleBackColor = true;
            // 
            // lblCounter
            // 
            this.lblCounter.AutoSize = true;
            this.lblCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblCounter.Location = new System.Drawing.Point(151, 200);
            this.lblCounter.Name = "lblCounter";
            this.lblCounter.Size = new System.Drawing.Size(118, 31);
            this.lblCounter.TabIndex = 13;
            this.lblCounter.Text = "Counter";
            this.lblCounter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CDServiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 269);
            this.Controls.Add(this.lblCounter);
            this.Controls.Add(this.cbxIsCustomDate);
            this.Controls.Add(this.customDateTime);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.txtItemOwnerId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtInterval);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDays);
            this.Controls.Add(this.btnEnd);
            this.Controls.Add(this.btnStart);
            this.Name = "CDServiceForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.TextBox txtDays;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtItemOwnerId;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.DateTimePicker customDateTime;
        private System.Windows.Forms.CheckBox cbxIsCustomDate;
        private System.Windows.Forms.Label lblCounter;
    }
}

