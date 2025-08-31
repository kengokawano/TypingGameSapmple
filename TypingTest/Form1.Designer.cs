
namespace TypingTest
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxInput = new System.Windows.Forms.TextBox();
            this.buttonConvert = new System.Windows.Forms.Button();
            this.labelOutput = new System.Windows.Forms.Label();
            this.laKana = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxInput
            // 
            this.textBoxInput.Location = new System.Drawing.Point(12, 12);
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.Size = new System.Drawing.Size(260, 19);
            this.textBoxInput.TabIndex = 0;
            this.textBoxInput.Text = "こんにちは";
            // 
            // buttonConvert
            // 
            this.buttonConvert.Location = new System.Drawing.Point(278, 12);
            this.buttonConvert.Name = "buttonConvert";
            this.buttonConvert.Size = new System.Drawing.Size(75, 23);
            this.buttonConvert.TabIndex = 1;
            this.buttonConvert.Text = "変換";
            this.buttonConvert.UseVisualStyleBackColor = true;
            this.buttonConvert.Click += new System.EventHandler(this.buttonConvert_Click);
            // 
            // labelOutput
            // 
            this.labelOutput.AutoSize = true;
            this.labelOutput.Location = new System.Drawing.Point(12, 48);
            this.labelOutput.Name = "labelOutput";
            this.labelOutput.Size = new System.Drawing.Size(29, 12);
            this.labelOutput.TabIndex = 2;
            this.labelOutput.Text = "結果";
            // 
            // laKana
            // 
            this.laKana.AutoSize = true;
            this.laKana.Location = new System.Drawing.Point(397, 61);
            this.laKana.Name = "laKana";
            this.laKana.Size = new System.Drawing.Size(29, 12);
            this.laKana.TabIndex = 3;
            this.laKana.Text = "結果";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.laKana);
            this.Controls.Add(this.labelOutput);
            this.Controls.Add(this.buttonConvert);
            this.Controls.Add(this.textBoxInput);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Roman Typing Converter";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.Button buttonConvert;
        private System.Windows.Forms.Label labelOutput;
        private System.Windows.Forms.Label laKana;
    }
}

