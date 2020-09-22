using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spire.Pdf;
using Spire.Pdf.Exporting.Text;
using Spire.Pdf.General.Find;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                this.statusBox.Text += Environment.NewLine + "Selecting file..." + Environment.NewLine;
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.messageBox.Text = " ";
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }

            this.statusBox.Text += "Processing PDF file..." + Environment.NewLine;
            this.fileName.Text = filePath;

            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(filePath);
            PdfPageBase page = doc.Pages[0];
            SimpleTextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
            string text = page.ExtractText(strategy);

            //PdfTextFind[] results = null;

            //results = page.FindText("MS61").Finds;

            //foreach (PdfTextFind texts in results)
            //{
            //    PointF p = texts.Position;
            //    float x = texts.Position.X;
            //    float y = texts.Position.Y;

            //   // this.displayText.Text += p + Environment.NewLine;
            //    string textArea = page.ExtractText(new RectangleF(x-30, y, 80, 180));
            //  //  this.displayText.Text += Environment.NewLine + textArea + Environment.NewLine;
            //}



            //MessageBox.Show(text, " ", MessageBoxButtons.OK);

            //Document pdfDocument = new Document(filePath);

            //TextAbsorber textAbsorber = new TextAbsorber();

            //pdfDocument.Pages.Accept(textAbsorber);

            //string extractedText = textAbsorber.Text;

            //Console.WriteLine(extractedText);

            //MessageBox.Show(extractedText, "Upstate Gold Sheet", MessageBoxButtons.OK);

            //PdfDocument PDF = PdfDocument.FromFile(filePath);
            //string AllText = PDF.ExtractAllText();


            string[] stringArray = text.Split(Environment.NewLine);

            //string[] stringArray = text.Split("$");
            this.statusBox.Text += "API Request/Response..." + Environment.NewLine;

            foreach (string line in stringArray)
            {
                 bool stringExists = line.Contains("$");
                if (stringExists)
                {
                    string[] lineSplit = line.Split(' ');
                //  foreach ( string word in lineSplit)
                //                    {
                //   this.displayText.Text += word + Environment.NewLine;
                //                    }
                //                   this.displayText.Text += line + Environment.NewLine;
                //  this.displayText.Text += " = " + lineSplit[3] + Environment.NewLine;
 
                    string requestString = "method=test&requestString=" + line;
                    this.displayText.Text += "--> " + lineSplit[0] + " " + lineSplit[1];
                    string responseString = APIRequest(requestString);
                    string[] responseArray = responseString.Split(':');
                    responseArray[1] = responseArray[1].Replace("}", string.Empty);
                   // MessageBox.Show(responseArray[1], "api response");
                    if(responseArray[1] == "0") {
                        this.displayText.Text += ": Success" + Environment.NewLine;
                    }
                    else
                    {
                     //   this.displayText.ForeColor
                        this.displayText.Text += ": Failure" + Environment.NewLine;
                    }
//                    this.displayText.Text += responseString + Environment.NewLine;
                }
            }


            this.messageBox.Text = "Click the 'Exit' button to end the application.";
            this.statusBox.Text += "Job End...";
           // MessageBox.Show(fileContent, "Unconverted Content " + filePath, MessageBoxButtons.OK);

          // MessageBox.Show(AllText, "Upstate Gold Sheet", MessageBoxButtons.OK);
           // MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);

            // Show the dialog and get result.
            //DialogResult result = openFileDialog1.ShowDialog();
            //if (result == DialogResult.OK) // Test result.
            //{
            //}
            // Console.WriteLine(result); // <-- For debugging use.
        }

        public string APIRequest(string requestString)
        {
            //
            // format rest_server/REST/price.class.php?method=updatePrice&product=10Liberty&price=1874
            //
            string requestUrl = "http://localhost/rest_server/REST/price.class.php";
            HttpWebRequest request = HttpWebRequest.CreateHttp(requestUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            using ( var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(requestString);
            }

            string responseFromRemoteServer;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using( StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    responseFromRemoteServer = reader.ReadToEnd();
                }
            }

            return responseFromRemoteServer;
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        public class ExtractTextAll
        {
            public static void Run()
            {

            }
        }

        private void displayText_TextChanged(object sender, EventArgs e)
        {

        }

        private void statusBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
