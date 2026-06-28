namespace Guryflix.Forms
{
    partial class SignUP
    {
        
        
        
        private System.ComponentModel.IContainer components = null;

        
        
        
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        
        
        
        
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignUP));
            this.resetBtn = new System.Windows.Forms.Button();
            this.signUpBtn = new System.Windows.Forms.Button();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.Label();
            this.userIDBox = new System.Windows.Forms.TextBox();
            this.userID = new System.Windows.Forms.Label();
            this.confirmPasswordLabel = new System.Windows.Forms.Label();
            this.confirmPasswordBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.statusSymbolID = new System.Windows.Forms.PictureBox();
            this.statusSymbolPassword = new System.Windows.Forms.PictureBox();
            this.statusSymbolConfirmPassword = new System.Windows.Forms.PictureBox();
            this.statusID = new System.Windows.Forms.Label();
            this.statusPassword = new System.Windows.Forms.Label();
            this.statusConfirmPassword = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Close = new System.Windows.Forms.PictureBox();
            this.sidePanel = new System.Windows.Forms.Panel();
            this.sideTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusSymbolID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusSymbolPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusSymbolConfirmPassword)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Close)).BeginInit();
            this.sidePanel.SuspendLayout();
            this.SuspendLayout();
            
            
            
            this.resetBtn.BackColor = System.Drawing.Color.Chocolate;
            this.resetBtn.FlatAppearance.BorderSize = 0;
            this.resetBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.resetBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetBtn.ForeColor = System.Drawing.Color.White;
            this.resetBtn.Location = new System.Drawing.Point(535, 290);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(115, 48);
            this.resetBtn.TabIndex = 11;
            this.resetBtn.Text = "LIMPAR";
            this.resetBtn.UseVisualStyleBackColor = false;
            this.resetBtn.Click += new System.EventHandler(this.resetBtn_Click);
            this.resetBtn.MouseLeave += new System.EventHandler(this.resetBtn_MouseLeave);
            this.resetBtn.MouseHover += new System.EventHandler(this.resetBtn_MouseHover);
            
            
            
            this.signUpBtn.BackColor = System.Drawing.Color.Chocolate;
            this.signUpBtn.FlatAppearance.BorderSize = 0;
            this.signUpBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.signUpBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.signUpBtn.ForeColor = System.Drawing.Color.White;
            this.signUpBtn.Location = new System.Drawing.Point(400, 290);
            this.signUpBtn.Name = "signUpBtn";
            this.signUpBtn.Size = new System.Drawing.Size(129, 48);
            this.signUpBtn.TabIndex = 10;
            this.signUpBtn.Text = "REGISTAR";
            this.signUpBtn.UseVisualStyleBackColor = false;
            this.signUpBtn.Click += new System.EventHandler(this.signUpBtn_Click);
            this.signUpBtn.MouseLeave += new System.EventHandler(this.signUpBtn_MouseLeave);
            this.signUpBtn.MouseHover += new System.EventHandler(this.signUpBtn_MouseHover);
            
            
            
            this.passwordBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(35)))), ((int)(((byte)(45)))));
            this.passwordBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.passwordBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordBox.ForeColor = System.Drawing.Color.White;
            this.passwordBox.Location = new System.Drawing.Point(440, 170);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(210, 16);
            this.passwordBox.TabIndex = 9;
            this.passwordBox.UseSystemPasswordChar = true;
            this.passwordBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.passwordBox_MouseClick);
            
            
            
            this.password.AutoSize = true;
            this.password.BackColor = System.Drawing.Color.Transparent;
            this.password.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password.ForeColor = System.Drawing.Color.White;
            this.password.Location = new System.Drawing.Point(270, 170);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(132, 24);
            this.password.TabIndex = 8;
            this.password.Text = "Palavra-passe:";
            
            
            
            this.userIDBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(35)))), ((int)(((byte)(45)))));
            this.userIDBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.userIDBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userIDBox.ForeColor = System.Drawing.Color.White;
            this.userIDBox.Location = new System.Drawing.Point(440, 120);
            this.userIDBox.Name = "userIDBox";
            this.userIDBox.Size = new System.Drawing.Size(210, 16);
            this.userIDBox.TabIndex = 7;
            this.userIDBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.userIDBox_MouseClick);
            
            
            
            this.userID.AutoSize = true;
            this.userID.BackColor = System.Drawing.Color.Transparent;
            this.userID.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userID.ForeColor = System.Drawing.Color.White;
            this.userID.Location = new System.Drawing.Point(311, 120);
            this.userID.Name = "userID";
            this.userID.Size = new System.Drawing.Size(91, 24);
            this.userID.TabIndex = 6;
            this.userID.Text = "Utilizador:";
            
            
            
            this.confirmPasswordLabel.AutoSize = true;
            this.confirmPasswordLabel.BackColor = System.Drawing.Color.Transparent;
            this.confirmPasswordLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.confirmPasswordLabel.ForeColor = System.Drawing.Color.White;
            this.confirmPasswordLabel.Location = new System.Drawing.Point(306, 220);
            this.confirmPasswordLabel.Name = "confirmPasswordLabel";
            this.confirmPasswordLabel.Size = new System.Drawing.Size(96, 24);
            this.confirmPasswordLabel.TabIndex = 12;
            this.confirmPasswordLabel.Text = "Confirmar:";
            
            
            
            this.confirmPasswordBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(35)))), ((int)(((byte)(45)))));
            this.confirmPasswordBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.confirmPasswordBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.confirmPasswordBox.ForeColor = System.Drawing.Color.White;
            this.confirmPasswordBox.Location = new System.Drawing.Point(440, 220);
            this.confirmPasswordBox.Name = "confirmPasswordBox";
            this.confirmPasswordBox.Size = new System.Drawing.Size(210, 16);
            this.confirmPasswordBox.TabIndex = 13;
            this.confirmPasswordBox.UseSystemPasswordChar = true;
            this.confirmPasswordBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.confirmPasswordBox_MouseClick);
            this.confirmPasswordBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.confirmPasswordBox_KeyPress_1);
            
            
            
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(440, 138);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 3);
            this.label1.TabIndex = 17;
            
            
            
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(440, 188);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(210, 3);
            this.label2.TabIndex = 18;
            
            
            
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(440, 238);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(210, 3);
            this.label3.TabIndex = 19;
            
            
            
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(270, 15);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(40, 32);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 20;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            
            
            
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(15, 120);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(230, 90);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            
            
            
            this.statusSymbolID.Location = new System.Drawing.Point(660, 112);
            this.statusSymbolID.Name = "statusSymbolID";
            this.statusSymbolID.Size = new System.Drawing.Size(30, 30);
            this.statusSymbolID.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.statusSymbolID.TabIndex = 22;
            this.statusSymbolID.TabStop = false;
            
            
            
            this.statusSymbolPassword.Location = new System.Drawing.Point(660, 162);
            this.statusSymbolPassword.Name = "statusSymbolPassword";
            this.statusSymbolPassword.Size = new System.Drawing.Size(30, 30);
            this.statusSymbolPassword.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.statusSymbolPassword.TabIndex = 23;
            this.statusSymbolPassword.TabStop = false;
            
            
            
            this.statusSymbolConfirmPassword.Location = new System.Drawing.Point(660, 212);
            this.statusSymbolConfirmPassword.Name = "statusSymbolConfirmPassword";
            this.statusSymbolConfirmPassword.Size = new System.Drawing.Size(30, 30);
            this.statusSymbolConfirmPassword.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.statusSymbolConfirmPassword.TabIndex = 24;
            this.statusSymbolConfirmPassword.TabStop = false;
            
            
            
            this.statusID.AutoSize = true;
            this.statusID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.249999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusID.ForeColor = System.Drawing.Color.White;
            this.statusID.Location = new System.Drawing.Point(670, 125);
            this.statusID.Name = "statusID";
            this.statusID.Size = new System.Drawing.Size(0, 13);
            this.statusID.TabIndex = 25;
            
            
            
            this.statusPassword.AutoSize = true;
            this.statusPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.249999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusPassword.ForeColor = System.Drawing.Color.White;
            this.statusPassword.Location = new System.Drawing.Point(670, 175);
            this.statusPassword.Name = "statusPassword";
            this.statusPassword.Size = new System.Drawing.Size(0, 13);
            this.statusPassword.TabIndex = 26;
            
            
            
            this.statusConfirmPassword.AutoSize = true;
            this.statusConfirmPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.249999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusConfirmPassword.ForeColor = System.Drawing.Color.White;
            this.statusConfirmPassword.Location = new System.Drawing.Point(670, 225);
            this.statusConfirmPassword.Name = "statusConfirmPassword";
            this.statusConfirmPassword.Size = new System.Drawing.Size(0, 13);
            this.statusConfirmPassword.TabIndex = 27;
            
            
            
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.Close);
            this.panel1.Location = new System.Drawing.Point(659, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(141, 26);
            this.panel1.TabIndex = 35;
            
            
            
            this.Close.Image = ((System.Drawing.Image)(resources.GetObject("Close.Image")));
            this.Close.Location = new System.Drawing.Point(96, 0);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(45, 23);
            this.Close.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Close.TabIndex = 26;
            this.Close.TabStop = false;
            this.Close.Click += new System.EventHandler(this.closebtn_Click);
            this.Close.MouseLeave += new System.EventHandler(this.Close_MouseLeave);
            this.Close.MouseHover += new System.EventHandler(this.Close_MouseHover);
            
            
            
            this.sidePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(22)))), ((int)(((byte)(28)))));
            this.sidePanel.Controls.Add(this.pictureBox1);
            this.sidePanel.Controls.Add(this.sideTitle);
            this.sidePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidePanel.Location = new System.Drawing.Point(0, 0);
            this.sidePanel.Name = "sidePanel";
            this.sidePanel.Size = new System.Drawing.Size(260, 450);
            this.sidePanel.TabIndex = 36;
            
            
            
            this.sideTitle.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sideTitle.ForeColor = System.Drawing.Color.White;
            this.sideTitle.Location = new System.Drawing.Point(15, 230);
            this.sideTitle.Name = "sideTitle";
            this.sideTitle.Size = new System.Drawing.Size(230, 60);
            this.sideTitle.TabIndex = 0;
            this.sideTitle.Text = "Crie a sua conta Na Guryflix";
            this.sideTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(35)))), ((int)(((byte)(45)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.sidePanel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusConfirmPassword);
            this.Controls.Add(this.statusPassword);
            this.Controls.Add(this.statusID);
            this.Controls.Add(this.statusSymbolConfirmPassword);
            this.Controls.Add(this.statusSymbolPassword);
            this.Controls.Add(this.statusSymbolID);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.confirmPasswordBox);
            this.Controls.Add(this.confirmPasswordLabel);
            this.Controls.Add(this.resetBtn);
            this.Controls.Add(this.signUpBtn);
            this.Controls.Add(this.passwordBox);
            this.Controls.Add(this.password);
            this.Controls.Add(this.userIDBox);
            this.Controls.Add(this.userID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SignUP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Guryflix";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusSymbolID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusSymbolPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusSymbolConfirmPassword)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Close)).EndInit();
            this.sidePanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button resetBtn;
        private System.Windows.Forms.Button signUpBtn;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Label password;
        private System.Windows.Forms.TextBox userIDBox;
        private System.Windows.Forms.Label userID;
        private System.Windows.Forms.Label confirmPasswordLabel;
        private System.Windows.Forms.TextBox confirmPasswordBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox statusSymbolID;
        private System.Windows.Forms.PictureBox statusSymbolPassword;
        private System.Windows.Forms.PictureBox statusSymbolConfirmPassword;
        private System.Windows.Forms.Label statusID;
        private System.Windows.Forms.Label statusPassword;
        private System.Windows.Forms.Label statusConfirmPassword;
        private System.Windows.Forms.Panel panel1;
        private new System.Windows.Forms.PictureBox Close;
        private System.Windows.Forms.Panel sidePanel;
        private System.Windows.Forms.Label sideTitle;
    }
}
