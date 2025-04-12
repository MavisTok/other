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
    ///  Required method for Designer support - do not modify the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        startDatePicker = new DateTimePicker();
        endDatePicker = new DateTimePicker();
        restDaysCheckedListBox = new CheckedListBox();
        addRestDayButton = new Button();
        generateICSButton = new Button();
        resultLabel = new Label();
        lblStartDate = new Label();
        lblEndDate = new Label();
        lblRestDays = new Label();
        groupBoxShiftType = new GroupBox();
        radioButtonShiftB = new RadioButton();
        radioButtonShiftA = new RadioButton();
        checkBoxUseShiftSystem = new CheckBox();
        addWorkDayButton = new Button();
        lblTip = new Label();
        checkBoxAutoUpload = new CheckBox();
        checkBoxUploadWebDav = new CheckBox();
        lblWebDavUrl = new Label();
        textBoxWebDavUrl = new TextBox();
        lblWebDavUser = new Label();
        textBoxWebDavUser = new TextBox();
        lblWebDavPassword = new Label();
        textBoxWebDavPassword = new TextBox();
        loadICSButton = new Button();
        updateHolidaysButton = new Button();
        groupBoxShiftType.SuspendLayout();
        SuspendLayout();
        // 
        // startDatePicker
        // 
        startDatePicker.Location = new Point(187, 48);
        startDatePicker.Margin = new Padding(5);
        startDatePicker.Name = "startDatePicker";
        startDatePicker.Size = new Size(312, 30);
        startDatePicker.TabIndex = 0;
        // 
        // endDatePicker
        // 
        endDatePicker.Location = new Point(187, 112);
        endDatePicker.Margin = new Padding(5);
        endDatePicker.Name = "endDatePicker";
        endDatePicker.Size = new Size(312, 30);
        endDatePicker.TabIndex = 1;
        // 
        // restDaysCheckedListBox
        // 
        restDaysCheckedListBox.FormattingEnabled = true;
        restDaysCheckedListBox.Location = new Point(187, 176);
        restDaysCheckedListBox.Margin = new Padding(5);
        restDaysCheckedListBox.Name = "restDaysCheckedListBox";
        restDaysCheckedListBox.Size = new Size(312, 193);
        restDaysCheckedListBox.TabIndex = 2;
        restDaysCheckedListBox.KeyDown += restDaysCheckedListBox_KeyDown;
        // 
        // addRestDayButton
        // 
        addRestDayButton.Location = new Point(187, 528);
        addRestDayButton.Margin = new Padding(5);
        addRestDayButton.Name = "addRestDayButton";
        addRestDayButton.Size = new Size(149, 48);
        addRestDayButton.TabIndex = 3;
        addRestDayButton.Text = "添加休息日";
        addRestDayButton.UseVisualStyleBackColor = true;
        addRestDayButton.Click += addRestDayButton_Click;
        // 
        // generateICSButton
        // 
        generateICSButton.Location = new Point(47, 824);
        generateICSButton.Margin = new Padding(5);
        generateICSButton.Name = "generateICSButton";
        generateICSButton.Size = new Size(212, 48);
        generateICSButton.TabIndex = 21;
        generateICSButton.Text = "生成ICS文件";
        generateICSButton.UseVisualStyleBackColor = true;
        generateICSButton.Click += generateICSButton_Click;
        // 
        // resultLabel
        // 
        resultLabel.AutoSize = true;
        resultLabel.Location = new Point(55, 891);
        resultLabel.Margin = new Padding(5, 0, 5, 0);
        resultLabel.Name = "resultLabel";
        resultLabel.Size = new Size(82, 24);
        resultLabel.TabIndex = 22;
        resultLabel.Text = "操作结果";
        // 
        // lblStartDate
        // 
        lblStartDate.AutoSize = true;
        lblStartDate.Location = new Point(46, 58);
        lblStartDate.Margin = new Padding(5, 0, 5, 0);
        lblStartDate.Name = "lblStartDate";
        lblStartDate.Size = new Size(82, 24);
        lblStartDate.TabIndex = 6;
        lblStartDate.Text = "开始日期";
        // 
        // lblEndDate
        // 
        lblEndDate.AutoSize = true;
        lblEndDate.Location = new Point(46, 122);
        lblEndDate.Margin = new Padding(5, 0, 5, 0);
        lblEndDate.Name = "lblEndDate";
        lblEndDate.Size = new Size(82, 24);
        lblEndDate.TabIndex = 7;
        lblEndDate.Text = "结束日期";
        // 
        // lblRestDays
        // 
        lblRestDays.AutoSize = true;
        lblRestDays.Location = new Point(46, 176);
        lblRestDays.Margin = new Padding(5, 0, 5, 0);
        lblRestDays.Name = "lblRestDays";
        lblRestDays.Size = new Size(118, 24);
        lblRestDays.TabIndex = 8;
        lblRestDays.Text = "特殊日期列表";
        // 
        // groupBoxShiftType
        // 
        groupBoxShiftType.Controls.Add(radioButtonShiftB);
        groupBoxShiftType.Controls.Add(radioButtonShiftA);
        groupBoxShiftType.Location = new Point(187, 448);
        groupBoxShiftType.Margin = new Padding(5);
        groupBoxShiftType.Name = "groupBoxShiftType";
        groupBoxShiftType.Padding = new Padding(5);
        groupBoxShiftType.Size = new Size(314, 72);
        groupBoxShiftType.TabIndex = 10;
        groupBoxShiftType.TabStop = false;
        groupBoxShiftType.Text = "排班岗位";
        // 
        // radioButtonShiftB
        // 
        radioButtonShiftB.AutoSize = true;
        radioButtonShiftB.Location = new Point(189, 35);
        radioButtonShiftB.Margin = new Padding(5);
        radioButtonShiftB.Name = "radioButtonShiftB";
        radioButtonShiftB.Size = new Size(64, 28);
        radioButtonShiftB.TabIndex = 1;
        radioButtonShiftB.Text = "B岗";
        radioButtonShiftB.UseVisualStyleBackColor = true;
        // 
        // radioButtonShiftA
        // 
        radioButtonShiftA.AutoSize = true;
        radioButtonShiftA.Checked = true;
        radioButtonShiftA.Location = new Point(31, 35);
        radioButtonShiftA.Margin = new Padding(5);
        radioButtonShiftA.Name = "radioButtonShiftA";
        radioButtonShiftA.Size = new Size(66, 28);
        radioButtonShiftA.TabIndex = 0;
        radioButtonShiftA.TabStop = true;
        radioButtonShiftA.Text = "A岗";
        radioButtonShiftA.UseVisualStyleBackColor = true;
        // 
        // checkBoxUseShiftSystem
        // 
        checkBoxUseShiftSystem.AutoSize = true;
        checkBoxUseShiftSystem.Location = new Point(187, 416);
        checkBoxUseShiftSystem.Margin = new Padding(5);
        checkBoxUseShiftSystem.Name = "checkBoxUseShiftSystem";
        checkBoxUseShiftSystem.Size = new Size(198, 28);
        checkBoxUseShiftSystem.TabIndex = 9;
        checkBoxUseShiftSystem.Text = "使用大小周排班制度";
        checkBoxUseShiftSystem.UseVisualStyleBackColor = true;
        checkBoxUseShiftSystem.CheckedChanged += checkBoxUseShiftSystem_CheckedChanged;
        // 
        // addWorkDayButton
        // 
        addWorkDayButton.Location = new Point(352, 528);
        addWorkDayButton.Margin = new Padding(5);
        addWorkDayButton.Name = "addWorkDayButton";
        addWorkDayButton.Size = new Size(149, 48);
        addWorkDayButton.TabIndex = 11;
        addWorkDayButton.Text = "添加工作日";
        addWorkDayButton.UseVisualStyleBackColor = true;
        addWorkDayButton.Click += addWorkDayButton_Click;
        // 
        // lblTip
        // 
        lblTip.AutoSize = true;
        lblTip.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Italic);
        lblTip.ForeColor = SystemColors.GrayText;
        lblTip.Location = new Point(187, 384);
        lblTip.Margin = new Padding(5, 0, 5, 0);
        lblTip.Name = "lblTip";
        lblTip.Size = new Size(273, 24);
        lblTip.TabIndex = 12;
        lblTip.Text = "提示：选中日期按Del键可以删除";
        // 
        // checkBoxAutoUpload
        // 
        checkBoxAutoUpload.AutoSize = true;
        checkBoxAutoUpload.Location = new Point(187, 592);
        checkBoxAutoUpload.Margin = new Padding(5);
        checkBoxAutoUpload.Name = "checkBoxAutoUpload";
        checkBoxAutoUpload.Size = new Size(188, 28);
        checkBoxAutoUpload.TabIndex = 13;
        checkBoxAutoUpload.Text = "自动上传到GitHub";
        checkBoxAutoUpload.UseVisualStyleBackColor = true;
        // 
        // checkBoxUploadWebDav
        // 
        checkBoxUploadWebDav.AutoSize = true;
        checkBoxUploadWebDav.Location = new Point(187, 632);
        checkBoxUploadWebDav.Margin = new Padding(5);
        checkBoxUploadWebDav.Name = "checkBoxUploadWebDav";
        checkBoxUploadWebDav.Size = new Size(174, 28);
        checkBoxUploadWebDav.TabIndex = 14;
        checkBoxUploadWebDav.Text = "上传到 WebDAV";
        checkBoxUploadWebDav.UseVisualStyleBackColor = true;
        checkBoxUploadWebDav.CheckedChanged += checkBoxUploadWebDav_CheckedChanged;
        // 
        // lblWebDavUrl
        // 
        lblWebDavUrl.AutoSize = true;
        lblWebDavUrl.Location = new Point(46, 677);
        lblWebDavUrl.Margin = new Padding(5, 0, 5, 0);
        lblWebDavUrl.Name = "lblWebDavUrl";
        lblWebDavUrl.Size = new Size(128, 24);
        lblWebDavUrl.TabIndex = 15;
        lblWebDavUrl.Text = "WebDAV URL";
        // 
        // textBoxWebDavUrl
        // 
        textBoxWebDavUrl.Enabled = false;
        textBoxWebDavUrl.Location = new Point(187, 672);
        textBoxWebDavUrl.Margin = new Padding(5);
        textBoxWebDavUrl.Name = "textBoxWebDavUrl";
        textBoxWebDavUrl.Size = new Size(312, 30);
        textBoxWebDavUrl.TabIndex = 16;
        // 
        // lblWebDavUser
        // 
        lblWebDavUser.AutoSize = true;
        lblWebDavUser.Location = new Point(46, 725);
        lblWebDavUser.Margin = new Padding(5, 0, 5, 0);
        lblWebDavUser.Name = "lblWebDavUser";
        lblWebDavUser.Size = new Size(64, 24);
        lblWebDavUser.TabIndex = 17;
        lblWebDavUser.Text = "用户名";
        // 
        // textBoxWebDavUser
        // 
        textBoxWebDavUser.Enabled = false;
        textBoxWebDavUser.Location = new Point(187, 720);
        textBoxWebDavUser.Margin = new Padding(5);
        textBoxWebDavUser.Name = "textBoxWebDavUser";
        textBoxWebDavUser.Size = new Size(312, 30);
        textBoxWebDavUser.TabIndex = 18;
        // 
        // lblWebDavPassword
        // 
        lblWebDavPassword.AutoSize = true;
        lblWebDavPassword.Location = new Point(46, 773);
        lblWebDavPassword.Margin = new Padding(5, 0, 5, 0);
        lblWebDavPassword.Name = "lblWebDavPassword";
        lblWebDavPassword.Size = new Size(46, 24);
        lblWebDavPassword.TabIndex = 19;
        lblWebDavPassword.Text = "密码";
        // 
        // textBoxWebDavPassword
        // 
        textBoxWebDavPassword.Enabled = false;
        textBoxWebDavPassword.Location = new Point(187, 768);
        textBoxWebDavPassword.Margin = new Padding(5);
        textBoxWebDavPassword.Name = "textBoxWebDavPassword";
        textBoxWebDavPassword.Size = new Size(312, 30);
        textBoxWebDavPassword.TabIndex = 20;
        textBoxWebDavPassword.UseSystemPasswordChar = true;
        // 
        // loadICSButton
        // 
        loadICSButton.Location = new Point(267, 824);
        loadICSButton.Margin = new Padding(5);
        loadICSButton.Name = "loadICSButton";
        loadICSButton.Size = new Size(189, 48);
        loadICSButton.TabIndex = 23;
        loadICSButton.Text = "读取ICS文件";
        loadICSButton.UseVisualStyleBackColor = true;
        loadICSButton.Click += loadICSButton_Click;
        // 
        // updateHolidaysButton
        // 
        updateHolidaysButton.Location = new Point(471, 824);
        updateHolidaysButton.Margin = new Padding(5);
        updateHolidaysButton.Name = "updateHolidaysButton";
        updateHolidaysButton.Size = new Size(189, 48);
        updateHolidaysButton.TabIndex = 24;
        updateHolidaysButton.Text = "更新节假日";
        updateHolidaysButton.UseVisualStyleBackColor = true;
        updateHolidaysButton.Click += updateHolidaysButton_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(685, 946);
        Controls.Add(textBoxWebDavPassword);
        Controls.Add(lblWebDavPassword);
        Controls.Add(textBoxWebDavUser);
        Controls.Add(lblWebDavUser);
        Controls.Add(textBoxWebDavUrl);
        Controls.Add(lblWebDavUrl);
        Controls.Add(checkBoxUploadWebDav);
        Controls.Add(checkBoxAutoUpload);
        Controls.Add(loadICSButton);
        Controls.Add(updateHolidaysButton);
        Controls.Add(lblTip);
        Controls.Add(addWorkDayButton);
        Controls.Add(groupBoxShiftType);
        Controls.Add(checkBoxUseShiftSystem);
        Controls.Add(lblRestDays);
        Controls.Add(lblEndDate);
        Controls.Add(lblStartDate);
        Controls.Add(resultLabel);
        Controls.Add(generateICSButton);
        Controls.Add(addRestDayButton);
        Controls.Add(restDaysCheckedListBox);
        Controls.Add(endDatePicker);
        Controls.Add(startDatePicker);
        FormBorderStyle = FormBorderStyle.FixedToolWindow;
        Margin = new Padding(5);
        Name = "Form1";
        Text = "排班ICS生成器";
        FormClosing += Form1_FormClosing;
        groupBoxShiftType.ResumeLayout(false);
        groupBoxShiftType.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
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
    private CheckBox checkBoxAutoUpload; 
    private CheckBox checkBoxUploadWebDav;
    private Label lblWebDavUrl;
    private TextBox textBoxWebDavUrl;
    private Label lblWebDavUser;
    private TextBox textBoxWebDavUser;
    private Label lblWebDavPassword;
    private TextBox textBoxWebDavPassword;
    private Button loadICSButton;
    private Button updateHolidaysButton;
}
