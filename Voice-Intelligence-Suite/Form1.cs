using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using NAudio.Wave;


namespace Voice_Intelligence_Suite
{
    public partial class Form1 : Form
    {
        // Ojects For Naudio Recording
        private WaveInEvent waveSource;
        private WaveFileWriter waveFile;


        // For Graphical Waveform 
        private Bitmap waveformBitmap;
        private Graphics waveformGraphics;


        int sec = 0, min = 0;

        string modulepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);  // Get the path of the executable 
        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); // Get User Profile path 
        string scriptfullpath;

        string filename = "newrec";
        string extension = ".wav";
        private string currentfilepath;

        private void directory(string folder)
        {
            // Ensure directory exists
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        private string Filename()
        {
            string folder = @$"{userProfile}\rec\";

            // Generate a unique filename
            do
            {
                string timestamp = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
                currentfilepath = Path.Combine(folder, $"{filename}_{timestamp}{extension}");
            } while (File.Exists(currentfilepath));

            return currentfilepath;
        }

        public Form1()
        {
            InitializeComponent();

            string folder = @$"{userProfile}\rec\"; // Set Directory
            directory(folder); // Create directory if it doesn't exist

            SetupUI();
            SetupToggleClickEvents();
        }



        private void Form1_Load(object sender, EventArgs e)
        {

            // Initialize the waveform bitmap and graphics
            waveformBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            waveformGraphics = Graphics.FromImage(waveformBitmap);
            waveformGraphics.Clear(Color.Black);
            pictureBox1.Image = waveformBitmap;
        }




        private void SetupUI()
        {
            HideAllLabels(); // Hide all labels initially
            SetupPanel2Labels(); // Setup labels in panel2

            RoundControlCorners(playbutton, 20);
            RoundControlCorners(stopbutton, 20);
            RoundControlCorners(panel1, 20);
            RoundControlCorners(panel2, 20);
            RoundControlCorners(Transcription_button, 20);
            RoundControlCorners(Translate_button, 20);
            RoundControlCorners(Summarize_button, 20);
            RoundControlCorners(Diarization_button, 20);
            RoundControlCorners(pictureBox1, 20);
        }


        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
        private void RoundControlCorners(Control control, int radius)
        {
            control.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, control.Width, control.Height, radius, radius));
        }


        private void SetupPanel2Labels()
        {
            panel2.AutoScroll = true; // Enable scrolling
            System.Windows.Forms.Label[] labels = { Transcription_label, Translation_label, Summarization_label, Diarization_label };

            foreach (System.Windows.Forms.Label lbl in labels)
            {
                if (lbl.Name != "Timerlabel")
                {
                    panel2.Controls.Add(lbl);  // Add label to panel2

                    lbl.AutoSize = true;  // Allow dynamic height adjustment
                    lbl.MaximumSize = new System.Drawing.Size(panel2.Width - 20, 0);  // Fit width to panel2 with 20px margin
                    lbl.Margin = new Padding(10);  // Add padding for neat appearance
                    lbl.TextAlign = System.Drawing.ContentAlignment.TopLeft;  // Align text to top left
                }
            }
        }

        private void SetupToggleClickEvents()
        {
            Transcription_button.Click += ToggleButton_Click;
            Translate_button.Click += ToggleButton_Click;
            Summarize_button.Click += ToggleButton_Click;
            Diarization_button.Click += ToggleButton_Click;
        }

        private void SetlabelText()
        {
            // Set default text for labels
            Transcription_label.Text = "Recording in progress...";
            Translation_label.Text = "Translating...";
            Summarization_label.Text = "Summarizing...";
            Diarization_label.Text = "Diarizing...";
        }

        private async void playbutton_Click(object sender, EventArgs e)
        {

            SetlabelText();

            ToggleButton_Click(Transcription_button, null);
            Transcription_label.Show();

            Filename(); // Set filename


            waveSource = new WaveInEvent  //  waveSource : get/handles data from the microphone
            {
                WaveFormat = new WaveFormat(44100, 16, 1) // Sample rate, bit depth, mono
            };


            waveFile = new WaveFileWriter(currentfilepath, waveSource.WaveFormat); //Create File and Folder 


            waveSource.DataAvailable += (s, e) =>
            {
                waveFile.Write(e.Buffer, 0, e.BytesRecorded);

                // WAVEFORM drawing
                pictureBox1.Invoke(new Action(() =>
                {
                    waveformGraphics.Clear(Color.Black);

                    int midY = pictureBox1.Height / 2;
                    int width = pictureBox1.Width;
                    int height = pictureBox1.Height;

                    int sampleCount = e.BytesRecorded / 2;
                    Point[] points = new Point[sampleCount];

                    for (int i = 0; i < sampleCount; i++)
                    {
                        short sample = (short)((e.Buffer[i * 2 + 1] << 8) | e.Buffer[i * 2]);
                        float normalized = sample / 32768f;
                        int x = i * width / sampleCount;
                        int y = midY + (int)(normalized * midY);
                        points[i] = new Point(x, y);
                    }

                    // Create a glowing gradient pen
                    using (var gradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                        new Point(0, 0), new Point(pictureBox1.Width, 0),
                        Color.LimeGreen, Color.DeepSkyBlue))
                    using (var gradientPen = new Pen(gradientBrush, 2))
                    {
                        if (points.Length > 1)
                        {
                            waveformGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                            waveformGraphics.DrawLines(gradientPen, points);
                        }
                    }
                    pictureBox1.Invalidate(); // Refresh the PictureBox
                }));
            };


            waveSource.RecordingStopped += (s, e) =>
            {
                waveFile.Dispose();
                waveSource.Dispose();
            };

            waveSource.StartRecording();


            stopbutton.Show();
            playbutton.Hide();
            timer1.Start();
            ResetTimer();
            await RunTranscriptionandTranslationAsync(); // Start transcription with translation
            _ = Task.Run(() => RunDiarizationAsync(currentfilepath)); // Start diarization in the background
        }

        private async Task RunTranscriptionandTranslationAsync()
        {

            scriptfullpath = Path.GetFullPath(Path.Combine(modulepath, @"..\..\..")); // C:\Users\krith\source\repos\Voice-Intelligence-Suite\Voice-Intelligence-Suite\
            string pythonScriptPath = Path.Combine(scriptfullpath, "Transcription&Translation.py"); // C:\Users\krith\source\repos\Voice-Intelligence-Suite\Voice-Intelligence-Suite\Transcription&Translation.py

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"{pythonScriptPath}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            try
            {
                using (Process process = Process.Start(startInfo))  // Start the Python process
                {

                    string output = await process.StandardOutput.ReadToEndAsync();
                    string error = await process.StandardError.ReadToEndAsync();
                    await process.WaitForExitAsync();

                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        MessageBox.Show("Error output from Python:\n" + error, "Python Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Parse JSON output from Python script
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<ProcessResult>(output, options);


                    // Display transcription and translation on the UI thread
                    Invoke(new Action(() =>
                    {
                        Transcription_label.Text = $"Transcription: \r\n{result.Transcription}";
                        Translation_label.Text = $"Translation: \r\n{result.Translation}";

                        RunSummarization(result.Transcription);  // Summarize the transcription

                        stopbutton_Click(null, null);  // Stop recording
                    }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error running Python script:\n" + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void RunSummarization(string transcription)
        {
            scriptfullpath = Path.GetFullPath(Path.Combine(modulepath, @"..\..\..")); // Combines the full path of the script module
            string pythonScriptPath = Path.Combine(scriptfullpath, "Summarization.py");

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"\"{pythonScriptPath}\" \"{transcription}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            try
            {
                using (Process process = Process.Start(startInfo))
                {

                    string summaryOutput = await process.StandardOutput.ReadToEndAsync();
                    await process.WaitForExitAsync();


                    Invoke(new Action(() =>
                    {
                        Summarization_label.Text = $"Summary: \r\n{summaryOutput}";
                    }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in summarization:\n" + ex.Message, "Summarization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Class to hold the transcription and translation results
        public class ProcessResult
        {
            public string Status { get; set; }
            public string Transcription { get; set; }
            public string Translation { get; set; }
        }


        private async Task RunDiarizationAsync(string audioPath)
        {
            scriptfullpath = Path.GetFullPath(Path.Combine(modulepath, @"..\..\..")); // Combines the full path of the script module
            string pythonScriptPath = Path.Combine(scriptfullpath, "Diarization.py");

            var psi = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"\"{pythonScriptPath}\" \"{audioPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(psi))
            {
                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();

                if (!string.IsNullOrWhiteSpace(error))
                {
                    // Show logs instead of calling them errors
                    //MessageBox.Show("Python log output:\n" + error, "Python Logs");
                }


                if (!string.IsNullOrWhiteSpace(output))
                {
                    try
                    {
                        List<DiarizedSegment> segments = JsonSerializer.Deserialize<List<DiarizedSegment>>(output);

                        StringBuilder sb = new StringBuilder();
                        foreach (var seg in segments)
                        {
                            //sb.AppendLine($"Speaker {seg.SpeakerId}: {seg.Text} [{seg.Start:F2}s - {seg.End:F2}s]");
                            sb.AppendLine($"Speaker {seg.SpeakerId}: [{seg.Start:F2}s - {seg.End:F2}s]");
                        }

                        //MessageBox.Show(sb.ToString(), "Diarization Result");
                        Invoke(new Action(() =>
                        {
                            Diarization_label.Text = $"Diarized Transcription:\r\n{sb.ToString()}";

                        }));
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to parse JSON:\n" + ex.Message + "\nRaw output:\n" + output);
                    }
                }
            }
        }

        public class DiarizedSegment
        {
            public int SpeakerId { get; set; }
            public double Start { get; set; }
            public double End { get; set; }
            public string Text { get; set; }
        }



        private void stopbutton_Click(object sender, EventArgs e)
        {
            playbutton.Show();
            stopbutton.Hide();
            timer1.Stop();
            waveSource.StopRecording();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            sec++;
            if (sec == 60) { min++; sec = 0; }
            Timerlabel.Text = $"{min:d2}:{sec:d2}";
        }

        private void ResetTimer()
        {
            min = 0;
            sec = 0;
            Timerlabel.Text = "00:00";
        }


        private void ToggleButton_Click(object sender, EventArgs e)
        {
            Button? clickedButton = sender as Button; // ? means that the sender can be null/nullable parameter pass
            Button[] buttons = { Transcription_button, Translate_button, Summarize_button, Diarization_button };

            foreach (Button btn in buttons)
            {
                btn.BackColor = Color.FromArgb(217, 217, 217);
                btn.ForeColor = SystemColors.ControlText;
            }
            if (clickedButton != null)
            {
                clickedButton.BackColor = Color.FromArgb(218, 145, 145);
                clickedButton.ForeColor = SystemColors.ButtonHighlight;
            }

        }

        private void HideAllLabels()
        {
            // Loop through all controls in panel2 (if labels are inside panel2)
            foreach (Control ctrl in panel2.Controls)
            {
                if (ctrl is System.Windows.Forms.Label lbl)
                {
                    lbl.Hide();
                }
            }
        }

        private void Transcription_button_Click(object sender, EventArgs e)
        {
            HideAllLabels();
            Transcription_label.Show();
        }


        private void Translate_button_Click(object sender, EventArgs e)
        {
            HideAllLabels();
            Translation_label.Show();
        }

        private void Summarize_button_Click(object sender, EventArgs e)

        {
            HideAllLabels();
            Summarization_label.Show();
        }

        private void Diarization_button_Click(object sender, EventArgs e)
        {
            HideAllLabels();
            Diarization_label.Show();
        }
    }
}