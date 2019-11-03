using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Ultimate_Predictor
{
    public partial class Form1 : Form
    {
        private const string APP_NAME = "Ultimate Predictor";
        private readonly string PREDICTIONS_CONFIG_PATH = $"{Environment.CurrentDirectory}\\PredictionsConfig.json";
        private string[] predictions;
        private Random rand = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private async void buttonPredictClick(object sender, EventArgs e)
        {
            buttonPredict.Enabled = false;
            await Task.Run(() =>
            {
                for (int i = 1; i < 100; i++)
                {
                    this.Invoke(new Action(() => 
                    {
                        UpdateProgressBar(i);
                        this.Text = $"{i}%";
                    }));
                    Thread.Sleep(20);
                }
            });

            var index = rand.Next(predictions.Length);
            var prediction = predictions[index];

            MessageBox.Show($"{prediction}");

            progressBar.Value = 0;
            this.Text = APP_NAME;
            buttonPredict.Enabled = true;
        }

        private void UpdateProgressBar(int i)
        {
            // To get around the proggresive animation, we need to move the progress bar backwards.
            if (i == progressBar.Maximum)
            {
                //Special case as value can't be set greater than Maximum.
                progressBar.Maximum = i + 1; //Temporarily Increase Maximum.
                progressBar.Value = i + 1; //Move past.
                progressBar.Maximum = i; //Reset maximum.
            }
            else progressBar.Value = i + 1; //Move past.
            progressBar.Value = i; //Move to correct value.
        }

        private void Form_Load(object sender, EventArgs e)
        {
            this.Text = APP_NAME;

            try
            {
                var data = File.ReadAllText(PREDICTIONS_CONFIG_PATH);
                predictions = JsonConvert.DeserializeObject<string[]>(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (predictions == null) Close();
                else if(predictions.Length == 0)
                {
                    MessageBox.Show("OOPS! Predictions are over. =)");
                    Close();
                }
            }
        }
    }
}
