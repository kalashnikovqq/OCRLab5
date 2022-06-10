using Newtonsoft.Json.Linq;
using RestSharp;
namespace OCR
{
    public partial class Form1 : Form
    {
        private string path = "";
        private int Lang = -1;
        private readonly string Link = "https://api.ocr.space/parse/image";
        readonly string[] apiLang = {
            "rus",
            "eng"
        };

        public Form1()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = 0;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDlg = new();
            fileDlg.Filter = "jpeg and png files|*.png;*.jpg;*.JPG";
            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                FileInfo fileInfo = new(fileDlg.FileName);
                pictureBox1.Image = Image.FromFile(fileDlg.FileName);
                path = fileDlg.FileName;
                label1.Text = "Загружено изображение: " + fileInfo.Name;
                label1.BackColor = Color.LightGreen;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (path == "")
            {
                MessageBox.Show("Не выбрано изображение", "", MessageBoxButtons.OK);
            }
            else
            {
                var client = new RestClient(Link);
                var request = new RestRequest(Method.POST);
                request.AddHeader("apikey", "helloworld");
                request.AlwaysMultipartFormData = true;
                request.AddFile("file", path);
                request.AddParameter("url", path);
                if (Lang >= 0)
                    request.AddParameter("language", apiLang[Lang]);
                IRestResponse response = client.Execute(request);
                JObject j = JObject.Parse(response.Content);
                JToken item = j.SelectToken(".ParsedResults")[0].SelectToken(".ParsedText");
                textBox.Text = item.ToString();
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Lang = comboBox1.SelectedIndex;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BackColor = Color.FromArgb(40, 40, 40);
        }
    }
}