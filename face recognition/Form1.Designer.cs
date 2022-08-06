namespace Face_Recognition_Demo_V1
{
    partial class Form1
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
            this.picDetectedFace = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnUseLBPH = new System.Windows.Forms.Button();
            this.btnUseFisherFace = new System.Windows.Forms.Button();
            this.btnUseEigenFace = new System.Windows.Forms.Button();
            this.btnOpenCamera = new System.Windows.Forms.Button();
            this.picVideoCapture = new System.Windows.Forms.PictureBox();
            this.btnStopCamera = new System.Windows.Forms.Button();
            this.btnDetectFace = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picDetectedFace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVideoCapture)).BeginInit();
            this.SuspendLayout();
            // 
            // picDetectedFace
            // 
            this.picDetectedFace.Location = new System.Drawing.Point(878, 336);
            this.picDetectedFace.Name = "picDetectedFace";
            this.picDetectedFace.Size = new System.Drawing.Size(198, 214);
            this.picDetectedFace.TabIndex = 57;
            this.picDetectedFace.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(482, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 16);
            this.label4.TabIndex = 56;
            this.label4.Text = "label4";
            // 
            // btnUseLBPH
            // 
            this.btnUseLBPH.Location = new System.Drawing.Point(865, 241);
            this.btnUseLBPH.Name = "btnUseLBPH";
            this.btnUseLBPH.Size = new System.Drawing.Size(211, 36);
            this.btnUseLBPH.TabIndex = 52;
            this.btnUseLBPH.Text = "Recognize using LBPH";
            this.btnUseLBPH.UseVisualStyleBackColor = true;
            this.btnUseLBPH.Click += new System.EventHandler(this.btnUseLBPH_Click);
            // 
            // btnUseFisherFace
            // 
            this.btnUseFisherFace.Location = new System.Drawing.Point(865, 203);
            this.btnUseFisherFace.Name = "btnUseFisherFace";
            this.btnUseFisherFace.Size = new System.Drawing.Size(211, 32);
            this.btnUseFisherFace.TabIndex = 51;
            this.btnUseFisherFace.Text = "Recognize using FisherFaces";
            this.btnUseFisherFace.UseVisualStyleBackColor = true;
            this.btnUseFisherFace.Click += new System.EventHandler(this.btnUseFisherFace_Click);
            // 
            // btnUseEigenFace
            // 
            this.btnUseEigenFace.Location = new System.Drawing.Point(865, 165);
            this.btnUseEigenFace.Name = "btnUseEigenFace";
            this.btnUseEigenFace.Size = new System.Drawing.Size(211, 32);
            this.btnUseEigenFace.TabIndex = 50;
            this.btnUseEigenFace.Text = "Recognize using EigenFaces";
            this.btnUseEigenFace.UseVisualStyleBackColor = true;
            this.btnUseEigenFace.Click += new System.EventHandler(this.btnUseEigenFace_Click);
            // 
            // btnOpenCamera
            // 
            this.btnOpenCamera.Location = new System.Drawing.Point(865, 54);
            this.btnOpenCamera.Name = "btnOpenCamera";
            this.btnOpenCamera.Size = new System.Drawing.Size(122, 32);
            this.btnOpenCamera.TabIndex = 49;
            this.btnOpenCamera.Text = "Open Camera";
            this.btnOpenCamera.UseVisualStyleBackColor = true;
            this.btnOpenCamera.Click += new System.EventHandler(this.btnOpenCamera_Click);
            // 
            // picVideoCapture
            // 
            this.picVideoCapture.Location = new System.Drawing.Point(241, 54);
            this.picVideoCapture.Name = "picVideoCapture";
            this.picVideoCapture.Size = new System.Drawing.Size(605, 426);
            this.picVideoCapture.TabIndex = 48;
            this.picVideoCapture.TabStop = false;
            // 
            // btnStopCamera
            // 
            this.btnStopCamera.Location = new System.Drawing.Point(1003, 54);
            this.btnStopCamera.Name = "btnStopCamera";
            this.btnStopCamera.Size = new System.Drawing.Size(122, 32);
            this.btnStopCamera.TabIndex = 58;
            this.btnStopCamera.Text = "Stop Camera";
            this.btnStopCamera.UseVisualStyleBackColor = true;
            this.btnStopCamera.Click += new System.EventHandler(this.btnStopCamera_Click);
            // 
            // btnDetectFace
            // 
            this.btnDetectFace.Location = new System.Drawing.Point(865, 109);
            this.btnDetectFace.Name = "btnDetectFace";
            this.btnDetectFace.Size = new System.Drawing.Size(122, 32);
            this.btnDetectFace.TabIndex = 59;
            this.btnDetectFace.Text = "Detect Face";
            this.btnDetectFace.UseVisualStyleBackColor = true;
            this.btnDetectFace.Click += new System.EventHandler(this.btnDetectFace_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1316, 562);
            this.Controls.Add(this.btnDetectFace);
            this.Controls.Add(this.btnStopCamera);
            this.Controls.Add(this.picDetectedFace);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnUseLBPH);
            this.Controls.Add(this.btnUseFisherFace);
            this.Controls.Add(this.btnUseEigenFace);
            this.Controls.Add(this.btnOpenCamera);
            this.Controls.Add(this.picVideoCapture);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picDetectedFace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVideoCapture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picDetectedFace;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnUseLBPH;
        private System.Windows.Forms.Button btnUseFisherFace;
        private System.Windows.Forms.Button btnUseEigenFace;
        private System.Windows.Forms.Button btnOpenCamera;
        private System.Windows.Forms.PictureBox picVideoCapture;
        private System.Windows.Forms.Button btnStopCamera;
        private System.Windows.Forms.Button btnDetectFace;
    }
}

