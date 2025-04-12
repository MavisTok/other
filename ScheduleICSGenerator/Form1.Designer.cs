namespace ScheduleICSGenerator;

partial class Form1
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
        this.startDatePicker = new System.Windows.Forms.DateTimePicker();
        this.endDatePicker = new System.Windows.Forms.DateTimePicker();
        this.restDaysCheckedListBox = new System.Windows.Forms.CheckedListBox();
        this.addRestDayButton = new System.Windows.Forms.Button();
        this.generateICSButton = new System.Windows.Forms.Button();
        this.resultLabel = new System.Windows.Forms.Label();
        this.lblStartDate = new System.Windows.Forms.Label();
        this.lblEndDate = new System.Windows.Forms.Label();
        this.lblRestDays = new System.Windows.Forms.Label();
        this.groupBoxShiftType = new System.Windows.Forms.GroupBox();
        this.radioButtonShiftB = new System.Windows.Forms.RadioButton();
        this.radioButtonShiftA = new System.Windows.Forms.RadioButton();
        this.checkBoxUseShiftSystem = new System.Windows.Forms.CheckBox();
        this.addWorkDayButton = new System.Windows.Forms.Button();
        this.lblTip = new System.Windows.Forms.Label();
        this.groupBoxShiftType.SuspendLayout();
        this.SuspendLayout();
        // 
        // startDatePicker
        // 
        this.startDatePicker.Location = new System.Drawing.Point(119, 30);
        this.startDatePicker.Name = "startDatePicker";
        this.startDatePicker.Size = new System.Drawing.Size(200, 23);
        this.startDatePicker.TabIndex = 0;
        // 
        // endDatePicker
        // 
        this.endDatePicker.Location = new System.Drawing.Point(119, 70);
        this.endDatePicker.Name = "endDatePicker";
        this.endDatePicker.Size = new System.Drawing.Size(200, 23);
        this.endDatePicker.TabIndex = 1;
        // 
        // restDaysCheckedListBox
        // 
        this.restDaysCheckedListBox.FormattingEnabled = true;
        this.restDaysCheckedListBox.Location = new System.Drawing.Point(119, 110);
        this.restDaysCheckedListBox.Name = "restDaysCheckedListBox";
        this.restDaysCheckedListBox.Size = new System.Drawing.Size(200, 130);
        this.restDaysCheckedListBox.TabIndex = 2;
        this.restDaysCheckedListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.restDaysCheckedListBox_KeyDown);
        // 
        // addRestDayButton
        // 
        this.addRestDayButton.Location = new System.Drawing.Point(119, 330);
        this.addRestDayButton.Name = "addRestDayButton";
        this.addRestDayButton.Size = new System.Drawing.Size(95, 30);
        this.addRestDayButton.TabIndex = 3;
        this.addRestDayButton.Text = "添加休息日";
        this.addRestDayButton.UseVisualStyleBackColor = true;
        this.addRestDayButton.Click += new System.EventHandler(this.addRestDayButton_Click);
        // 
        // addWorkDayButton
        // 
        this.addWorkDayButton.Location = new System.Drawing.Point(224, 330);
        this.addWorkDayButton.Name = "addWorkDayButton";
        this.addWorkDayButton.Size = new System.Drawing.Size(95, 30);
        this.addWorkDayButton.TabIndex = 11;
        this.addWorkDayButton.Text = "添加工作日";
        this.addWorkDayButton.UseVisualStyleBackColor = true;
        this.addWorkDayButton.Click += new System.EventHandler(this.addWorkDayButton_Click);
        // 
        // generateICSButton
        // 
        this.generateICSButton.Location = new System.Drawing.Point(146, 370);
        this.generateICSButton.Name = "generateICSButton";
        this.generateICSButton.Size = new System.Drawing.Size(143, 30);
        this.generateICSButton.TabIndex = 4;
        this.generateICSButton.Text = "生成ICS文件";
        this.generateICSButton.UseVisualStyleBackColor = true;
        this.generateICSButton.Click += new System.EventHandler(this.generateICSButton_Click);
        // 
        // resultLabel
        // 
        this.resultLabel.AutoSize = true;
        this.resultLabel.Location = new System.Drawing.Point(119, 412);
        this.resultLabel.Name = "resultLabel";
        this.resultLabel.Size = new System.Drawing.Size(67, 15);
        this.resultLabel.TabIndex = 5;
        this.resultLabel.Text = "操作结果";
        // 
        // lblStartDate
        // 
        this.lblStartDate.AutoSize = true;
        this.lblStartDate.Location = new System.Drawing.Point(29, 36);
        this.lblStartDate.Name = "lblStartDate";
        this.lblStartDate.Size = new System.Drawing.Size(67, 15);
        this.lblStartDate.TabIndex = 6;
        this.lblStartDate.Text = "开始日期";
        // 
        // lblEndDate
        // 
        this.lblEndDate.AutoSize = true;
        this.lblEndDate.Location = new System.Drawing.Point(29, 76);
        this.lblEndDate.Name = "lblEndDate";
        this.lblEndDate.Size = new System.Drawing.Size(67, 15);
        this.lblEndDate.TabIndex = 7;
        this.lblEndDate.Text = "结束日期";
        // 
        // lblRestDays
        // 
        this.lblRestDays.AutoSize = true;
        this.lblRestDays.Location = new System.Drawing.Point(29, 110);
        this.lblRestDays.Name = "lblRestDays";
        this.lblRestDays.Size = new System.Drawing.Size(79, 15);
        this.lblRestDays.TabIndex = 8;
        this.lblRestDays.Text = "特殊日期列表";
        // 
        // lblTip
        // 
        this.lblTip.AutoSize = true;
        this.lblTip.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
        this.lblTip.ForeColor = System.Drawing.SystemColors.GrayText;
        this.lblTip.Location = new System.Drawing.Point(119, 240);
        this.lblTip.Name = "lblTip";
        this.lblTip.Size = new System.Drawing.Size(186, 15);
        this.lblTip.TabIndex = 12;
        this.lblTip.Text = "提示：选中日期按Del键可以删除";
        // 
        // checkBoxUseShiftSystem
        // 
        this.checkBoxUseShiftSystem.AutoSize = true;
        this.checkBoxUseShiftSystem.Location = new System.Drawing.Point(119, 260);
        this.checkBoxUseShiftSystem.Name = "checkBoxUseShiftSystem";
        this.checkBoxUseShiftSystem.Size = new System.Drawing.Size(147, 19);
        this.checkBoxUseShiftSystem.TabIndex = 9;
        this.checkBoxUseShiftSystem.Text = "使用大小周排班制度";
        this.checkBoxUseShiftSystem.UseVisualStyleBackColor = true;
        this.checkBoxUseShiftSystem.CheckedChanged += new System.EventHandler(this.checkBoxUseShiftSystem_CheckedChanged);
        // 
        // groupBoxShiftType
        // 
        this.groupBoxShiftType.Controls.Add(this.radioButtonShiftB);
        this.groupBoxShiftType.Controls.Add(this.radioButtonShiftA);
        this.groupBoxShiftType.Location = new System.Drawing.Point(119, 280);
        this.groupBoxShiftType.Name = "groupBoxShiftType";
        this.groupBoxShiftType.Size = new System.Drawing.Size(200, 45);
        this.groupBoxShiftType.TabIndex = 10;
        this.groupBoxShiftType.TabStop = false;
        this.groupBoxShiftType.Text = "排班岗位";
        // 
        // radioButtonShiftA
        // 
        this.radioButtonShiftA.AutoSize = true;
        this.radioButtonShiftA.Checked = true;
        this.radioButtonShiftA.Location = new System.Drawing.Point(20, 22);
        this.radioButtonShiftA.Name = "radioButtonShiftA";
        this.radioButtonShiftA.Size = new System.Drawing.Size(50, 19);
        this.radioButtonShiftA.TabIndex = 0;
        this.radioButtonShiftA.TabStop = true;
        this.radioButtonShiftA.Text = "A岗";
        this.radioButtonShiftA.UseVisualStyleBackColor = true;
        // 
        // radioButtonShiftB
        // 
        this.radioButtonShiftB.AutoSize = true;
        this.radioButtonShiftB.Location = new System.Drawing.Point(120, 22);
        this.radioButtonShiftB.Name = "radioButtonShiftB";
        this.radioButtonShiftB.Size = new System.Drawing.Size(50, 19);
        this.radioButtonShiftB.TabIndex = 1;
        this.radioButtonShiftB.Text = "B岗";
        this.radioButtonShiftB.UseVisualStyleBackColor = true;
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(384, 441);
        this.Controls.Add(this.lblTip);
        this.Controls.Add(this.addWorkDayButton);
        this.Controls.Add(this.groupBoxShiftType);
        this.Controls.Add(this.checkBoxUseShiftSystem);
        this.Controls.Add(this.lblRestDays);
        this.Controls.Add(this.lblEndDate);
        this.Controls.Add(this.lblStartDate);
        this.Controls.Add(this.resultLabel);
        this.Controls.Add(this.generateICSButton);
        this.Controls.Add(this.addRestDayButton);
        this.Controls.Add(this.restDaysCheckedListBox);
        this.Controls.Add(this.endDatePicker);
        this.Controls.Add(this.startDatePicker);
        this.Name = "Form1";
        this.Text = "排班ICS生成器";
        this.groupBoxShiftType.ResumeLayout(false);
        this.groupBoxShiftType.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private DateTimePicker startDatePicker;
    private DateTimePicker endDatePicker;
    private CheckedListBox restDaysCheckedListBox;
    private Button addRestDayButton;
    private Button generateICSButton;
    private Label resultLabel;
    private Label lblStartDate;
    private Label lblEndDate;
    private Label lblRestDays;
    private GroupBox groupBoxShiftType;
    private RadioButton radioButtonShiftB;
    private RadioButton radioButtonShiftA;
    private CheckBox checkBoxUseShiftSystem;
    private Button addWorkDayButton;
    private Label lblTip;
}
