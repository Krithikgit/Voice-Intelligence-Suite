using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Voice_Intelligence_Suite
{
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
            components = new System.ComponentModel.Container();
            timer1 = new System.Windows.Forms.Timer(components);
            Timerlabel = new Label();
            panel1 = new Panel();
            Translate_button = new Button();
            Transcription_button = new Button();
            panel2 = new Panel();
            Diarization_button = new Button();
            pictureBox1 = new PictureBox();
            Translation_label = new Label();
            Transcription_label = new Label();
            Diarization_label = new Label();
            Summarization_label = new Label();
            Summarize_button = new Button();
            playbutton = new Button();
            stopbutton = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // timer1
            // 
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // Timerlabel
            // 
            Timerlabel.AutoSize = true;
            Timerlabel.Font = new System.Drawing.Font("Segoe Fluent Icons", 64.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Timerlabel.ForeColor = Color.White;
            Timerlabel.Location = new Point(66, 121);
            Timerlabel.Name = "Timerlabel";
            Timerlabel.Size = new Size(255, 109);
            Timerlabel.TabIndex = 0;
            Timerlabel.Text = "00:00";
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(240, 194, 194);
            panel1.Controls.Add(Translate_button);
            panel1.Controls.Add(Transcription_button);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(Summarize_button);
            panel1.Location = new Point(388, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(536, 549);
            panel1.TabIndex = 1;
            // 
            // Translate_button
            // 
            Translate_button.BackColor = Color.FromArgb(217, 217, 217);
            Translate_button.FlatAppearance.BorderSize = 0;
            Translate_button.FlatStyle = FlatStyle.Flat;
            Translate_button.Font = new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
            Translate_button.Location = new Point(231, 26);
            Translate_button.Name = "Translate_button";
            Translate_button.Size = new Size(101, 41);
            Translate_button.TabIndex = 5;
            Translate_button.Text = "Translate";
            Translate_button.UseVisualStyleBackColor = true;
            Translate_button.Click += Translate_button_Click;
            // 
            // Transcription_button
            // 
            Transcription_button.BackColor = Color.FromArgb(217, 217, 217);
            Transcription_button.FlatAppearance.BorderSize = 0;
            Transcription_button.FlatStyle = FlatStyle.Flat;
            Transcription_button.Font = new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Transcription_button.Location = new Point(33, 26);
            Transcription_button.Name = "Transcription_button";
            Transcription_button.Size = new Size(121, 41);
            Transcription_button.TabIndex = 3;
            Transcription_button.Text = "Transcription";
            Transcription_button.UseVisualStyleBackColor = true;
            Transcription_button.Click += Transcription_button_Click;
            // 
            // panel2
            // 
            panel2.AutoScroll = true;
            panel2.BackColor = SystemColors.ButtonHighlight;
            panel2.Controls.Add(Diarization_button);
            panel2.Controls.Add(pictureBox1);
            panel2.Controls.Add(Transcription_label);
            panel2.Controls.Add(Diarization_label);
            panel2.Controls.Add(Summarization_label);
            panel2.Controls.Add(Translation_label);
            panel2.Location = new Point(33, 88);
            panel2.Name = "panel2";
            panel2.Size = new Size(475, 430);
            panel2.TabIndex = 2;
            // 
            // Diarization_button
            // 
            Diarization_button.BackColor = Color.FromArgb(217, 217, 217);
            Diarization_button.FlatAppearance.BorderSize = 0;
            Diarization_button.FlatStyle = FlatStyle.Flat;
            Diarization_button.Font = new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Diarization_button.Location = new Point(349, 384);
            Diarization_button.Name = "Diarization_button";
            Diarization_button.Size = new Size(101, 41);
            Diarization_button.TabIndex = 7;
            Diarization_button.Text = "Diarization";
            Diarization_button.UseVisualStyleBackColor = true;
            Diarization_button.Click += Diarization_button_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Black;
            pictureBox1.Location = new Point(26, 180);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(424, 200);
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // Translation_label
            // 
            Translation_label.AutoSize = true;
            Translation_label.Font = new System.Drawing.Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Translation_label.Location = new Point(26, 21);
            Translation_label.MaximumSize = new Size(200, 0);
            Translation_label.Name = "Translation_label";
            Translation_label.Size = new Size(161, 28);
            Translation_label.TabIndex = 4;
            Translation_label.Text = "translationlabel";
            // 
            // Transcription_label
            // 
            Transcription_label.AutoSize = true;
            Transcription_label.Font = new System.Drawing.Font("Segoe UI", 12F, FontStyle.Bold);
            Transcription_label.Location = new Point(26, 21);
            Transcription_label.MaximumSize = new Size(200, 0);
            Transcription_label.Name = "Transcription_label";
            Transcription_label.Size = new Size(180, 28);
            Transcription_label.TabIndex = 0;
            Transcription_label.Text = "transcriptionlabel";
            // 
            // Diarization_label
            // 
            Diarization_label.AutoSize = true;
            Diarization_label.Font = new System.Drawing.Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Diarization_label.Location = new Point(26, 21);
            Diarization_label.MaximumSize = new Size(200, 0);
            Diarization_label.Name = "Diarization_label";
            Diarization_label.Size = new Size(160, 28);
            Diarization_label.TabIndex = 8;
            Diarization_label.Text = "diarizationlabel";
            // 
            // Summarization_label
            // 
            Summarization_label.AutoSize = true;
            Summarization_label.Font = new System.Drawing.Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Summarization_label.Location = new Point(26, 21);
            Summarization_label.MaximumSize = new Size(200, 0);
            Summarization_label.Name = "Summarization_label";
            Summarization_label.Size = new Size(199, 28);
            Summarization_label.TabIndex = 5;
            Summarization_label.Text = "summarizationlabel";
            // 
            // Summarize_button
            // 
            Summarize_button.BackColor = Color.FromArgb(217, 217, 217);
            Summarize_button.FlatAppearance.BorderSize = 0;
            Summarize_button.FlatStyle = FlatStyle.Flat;
            Summarize_button.Font = new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
            Summarize_button.Location = new Point(407, 26);
            Summarize_button.Name = "Summarize_button";
            Summarize_button.Size = new Size(101, 41);
            Summarize_button.TabIndex = 4;
            Summarize_button.Text = "Summarize";
            Summarize_button.UseVisualStyleBackColor = true;
            Summarize_button.Click += Summarize_button_Click;
            // 
            // playbutton
            // 
            playbutton.BackColor = Color.FromArgb(240, 194, 194);
            playbutton.FlatAppearance.BorderSize = 0;
            playbutton.FlatStyle = FlatStyle.Flat;
            playbutton.Image = Properties.Resources.PlayButton;
            playbutton.Location = new Point(94, 268);
            playbutton.Name = "playbutton";
            playbutton.Size = new Size(190, 80);
            playbutton.TabIndex = 2;
            playbutton.UseVisualStyleBackColor = false;
            playbutton.Click += playbutton_Click;
            // 
            // stopbutton
            // 
            stopbutton.BackColor = Color.FromArgb(255, 95, 120);
            stopbutton.FlatAppearance.BorderSize = 0;
            stopbutton.FlatStyle = FlatStyle.Flat;
            stopbutton.Image = Properties.Resources.StopButton;
            stopbutton.Location = new Point(94, 268);
            stopbutton.Name = "stopbutton";
            stopbutton.Size = new Size(190, 80);
            stopbutton.TabIndex = 3;
            stopbutton.UseVisualStyleBackColor = false;
            stopbutton.Click += stopbutton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(136, 159, 129);
            ClientSize = new Size(936, 573);
            Controls.Add(panel1);
            Controls.Add(Timerlabel);
            Controls.Add(playbutton);
            Controls.Add(stopbutton);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private Label Timerlabel;
        private Panel panel1;
        private Panel panel2;
        private Button playbutton;
        private Button Translate_button;
        private Button Summarize_button;
        private Button Transcription_button;
        private Button stopbutton;
        private Label Transcription_label;
        private Label Translation_label;
        private Label Summarization_label;
        private PictureBox pictureBox1;
        private Button Diarization_button;
        private Label Diarization_label;
    }
}