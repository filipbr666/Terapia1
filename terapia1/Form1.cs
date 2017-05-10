using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NAudio;
using NAudio.CoreAudioApi;

namespace terapia1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active);
            comboBox1.Items.AddRange(devices.ToArray());
        }

        string[] Letters = { "A", "B", "C", "D", "E", "F", "G", "H" };
        
        int thirdcolor = 200;
        int secondcolor;
        Random rnd = new Random();
        bool deviceIsNull = true;
        private NAudio.Wave.WaveFileReader wave = null;
        private NAudio.Wave.DirectSoundOut output = null;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Wave File (*.wav)|*.wav;";
            if (open.ShowDialog() != DialogResult.OK) return;

            wave = new NAudio.Wave.WaveFileReader(open.FileName);
            output = new NAudio.Wave.DirectSoundOut();
            output.Init(new NAudio.Wave.WaveChannel32(wave));
            output.Play();
            if(deviceIsNull==false)
            {
                comboBox1.Hide();
                button1.Hide();
                textBox1.Visible = true;
                this.BackColor = Color.Black;
                timer2.Enabled = true;
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                var device = (MMDevice)comboBox1.SelectedItem;
                deviceIsNull = false;
                button1.Enabled = true;
                progressBar1.Value = (int)(Math.Round(device.AudioMeterInformation.MasterPeakValue * 100));
                if (thirdcolor<240)
                {
                    thirdcolor = thirdcolor + 1;
                }
                else
                {
                    thirdcolor = 0;
                    
                }
                if (progressBar1.Value > 60)
                    secondcolor = secondcolor + 5;
                if (secondcolor >= 250 || secondcolor < 10)
                    secondcolor = 100;

                textBox1.ForeColor = Color.FromArgb(250*progressBar1.Value/100, secondcolor, thirdcolor);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            textBox1.Text = Letters[rnd.Next(Letters.Length)];
        }
    }
}
