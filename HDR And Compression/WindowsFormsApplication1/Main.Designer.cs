namespace WindowsFormsApplication1
{
    partial class Main
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
            this.btnOpenFile1 = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnOpenFile2 = new System.Windows.Forms.Button();
            this.btnOpenFile3 = new System.Windows.Forms.Button();
            this.labelInstruction = new System.Windows.Forms.Label();
            this.btnHDR = new System.Windows.Forms.Button();
            this.btnCompress = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnHuffmanCoding = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOpenFile1
            // 
            this.btnOpenFile1.Location = new System.Drawing.Point(12, 75);
            this.btnOpenFile1.Name = "btnOpenFile1";
            this.btnOpenFile1.Size = new System.Drawing.Size(235, 70);
            this.btnOpenFile1.TabIndex = 0;
            this.btnOpenFile1.Text = "1. Open First File";
            this.btnOpenFile1.UseVisualStyleBackColor = true;
            this.btnOpenFile1.Click += new System.EventHandler(this.btnOpenFile1_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(12, 531);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(235, 70);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnOpenFile2
            // 
            this.btnOpenFile2.Location = new System.Drawing.Point(12, 151);
            this.btnOpenFile2.Name = "btnOpenFile2";
            this.btnOpenFile2.Size = new System.Drawing.Size(235, 70);
            this.btnOpenFile2.TabIndex = 8;
            this.btnOpenFile2.Text = "2. Open Second File";
            this.btnOpenFile2.UseVisualStyleBackColor = true;
            this.btnOpenFile2.Click += new System.EventHandler(this.btnOpenFile2_Click);
            // 
            // btnOpenFile3
            // 
            this.btnOpenFile3.Location = new System.Drawing.Point(12, 227);
            this.btnOpenFile3.Name = "btnOpenFile3";
            this.btnOpenFile3.Size = new System.Drawing.Size(235, 70);
            this.btnOpenFile3.TabIndex = 9;
            this.btnOpenFile3.Text = "3. Open Third File";
            this.btnOpenFile3.UseVisualStyleBackColor = true;
            this.btnOpenFile3.Click += new System.EventHandler(this.btnOpenFile3_Click);
            // 
            // labelInstruction
            // 
            this.labelInstruction.Location = new System.Drawing.Point(7, 9);
            this.labelInstruction.Name = "labelInstruction";
            this.labelInstruction.Size = new System.Drawing.Size(257, 50);
            this.labelInstruction.TabIndex = 13;
            this.labelInstruction.Text = "Instruction";
            this.labelInstruction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnHDR
            // 
            this.btnHDR.Location = new System.Drawing.Point(12, 303);
            this.btnHDR.Name = "btnHDR";
            this.btnHDR.Size = new System.Drawing.Size(235, 70);
            this.btnHDR.TabIndex = 14;
            this.btnHDR.Text = "4. Display HDR";
            this.btnHDR.UseVisualStyleBackColor = true;
            this.btnHDR.Click += new System.EventHandler(this.btnHDR_Click);
            // 
            // btnCompress
            // 
            this.btnCompress.Location = new System.Drawing.Point(12, 379);
            this.btnCompress.Name = "btnCompress";
            this.btnCompress.Size = new System.Drawing.Size(235, 70);
            this.btnCompress.TabIndex = 15;
            this.btnCompress.Text = "5. Compress All Files";
            this.btnCompress.UseVisualStyleBackColor = true;
            this.btnCompress.Click += new System.EventHandler(this.btnCompress_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 617);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(268, 25);
            this.label1.TabIndex = 16;
            this.label1.Text = "Compressed File Location:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnHuffmanCoding
            // 
            this.btnHuffmanCoding.Location = new System.Drawing.Point(12, 455);
            this.btnHuffmanCoding.Name = "btnHuffmanCoding";
            this.btnHuffmanCoding.Size = new System.Drawing.Size(235, 70);
            this.btnHuffmanCoding.TabIndex = 17;
            this.btnHuffmanCoding.Text = "6. Huffman Coding";
            this.btnHuffmanCoding.UseVisualStyleBackColor = true;
            this.btnHuffmanCoding.Click += new System.EventHandler(this.btnHuffmanCoding_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1107, 841);
            this.Controls.Add(this.btnHuffmanCoding);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCompress);
            this.Controls.Add(this.btnHDR);
            this.Controls.Add(this.labelInstruction);
            this.Controls.Add(this.btnOpenFile3);
            this.Controls.Add(this.btnOpenFile2);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnOpenFile1);
            this.Name = "Form1";
            this.Text = "TIFF Image Loader";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenFile1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnOpenFile2;
        private System.Windows.Forms.Button btnOpenFile3;
        private System.Windows.Forms.Label labelInstruction;
        private System.Windows.Forms.Button btnHDR;
        private System.Windows.Forms.Button btnCompress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnHuffmanCoding;
    }
}

