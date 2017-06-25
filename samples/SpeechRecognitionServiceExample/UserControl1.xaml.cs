using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using SpeechToTextWPFSample;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Collections;
using System.Threading;
using System.Text.RegularExpressions;

namespace SpeechToTextWPFSample
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : MetroWindow
    {
        public UserControl1()
        {
            InitializeComponent();
            initialization();
        }

        private void initialization() {
            //unirest_test();


        }
        //private void WriteLine1(string format, params object[] args)
        //{
        //    var formattedStr = string.Format(format, args);
        //    System.Diagnostics.Trace.WriteLine(formattedStr);
        //    Dispatcher.Invoke(() =>
        //    {
        //        _logText1.Text += (formattedStr + "\n");
        //        _logText.ScrollToEnd();
        //    });
        //}
        private void WriteLine()
        {
            this.WriteLine(string.Empty);
        }
        private void WriteLine(string format, params object[] args)
        {
            var formattedStr = string.Format(format, args);
            System.Diagnostics.Trace.WriteLine(formattedStr);
            Dispatcher.Invoke(() =>
            {
                _logText.Text += (formattedStr + "\n");
                _logText.ScrollToEnd();
            });
        }
        private String Urlfarend(String content) {
            String total = content;
            const string URL = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/keyPhrases";

            HttpWebRequest request = (HttpWebRequest)WebRequest.CreateHttp(URL);

            request.ContentType = "application/json";
            request.Method = "POST";

            request.Headers.Add("Ocp-Apim-Subscription-Key", "4f48e07f41344e4ba825bedfed2f4905");

            using (var streamWriter = new System.IO.StreamWriter(request.GetRequestStream()))
            {
                // string json = "{ \"method\" : \"guru.test\", \"params\" : [ \"Guru\" ], \"id\" : 123 }";
                string json1 = "{ \"documents\": [ { \"language\": \"en\", \"id\" : \"1\", \"text\" : \"" + total + "\"} ] }";

                streamWriter.Write(json1);
                streamWriter.Flush();
            }


            try
            {
                System.Net.WebResponse webResponse = request.GetResponse();
                System.IO.Stream webStream = webResponse.GetResponseStream();
                System.IO.StreamReader responseReader = new System.IO.StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                Newtonsoft.Json.Linq.JObject jo = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                var strings = jo["documents"][0]["keyPhrases"];
                ArrayList results = new ArrayList();
                StringBuilder result = new StringBuilder();
                foreach (var album in strings)
                {
                    result.Append(album.ToString() + ",");


                    // Access album data;
                }
                return result.ToString();

                //responseReader.Close();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(e.Message);
                return String.Empty;
            }

        }
        private  void unirest_test(object ob)
        {
            String total = myContent.getStrings();
            myContent.myContents.Clear();
           
           // total = "Hello! My name's Forrest. Forrest Gump. You wanna Chocolate? I could eat about a million and a half o'these. My momma always said life was like a box o'chocolates. You never know what you gonna get. Now, when I was a baby momma named me after the great civil war hero, General Nathan Bedford Forrest. She said we was related to him in some way. What he did was: he started up this club called the Ku Klux Clan. They'd all dress up in their robes and their bed sheets and act like a bunch of ghosts or spooks or something. They 'd even put bed sheets on their horses and ride around. And anyway, that's how I got my n";
            const string URL = "https://textanalysis-text-summarization.p.mashape.com/text-summarizer-text";
            //string DATA = @"sentnum=4&text=Each of you should have received the below communication from Nick Gunn announcing a tightening of the HPE Global Travel Policy, which introduces a new travel pre-approval process and tool.\n  
            //             In support of this effort, I want to confirm that this is an HPE - wide policy that includes Software.\n  
            //             These changes are effective immediately and apply to all new travel bookings.Trips that were booked prior to the announcement will not be required to go through the pre - approval process.However, given the intended impact of these travel restrictions, all pending trips should be reviewed in line with the guidance and / or canceled as appropriate if they do not meet the higher criteria (e.g.only customer - related, business - critical, etc.).\n
            //                Thank you in advance for your cooperation. For additional information, please refer to the FAQs or contact hpe - next - lmo - questions@hpe.com.\n";
             string DATA = "sentnum=5&text="+total;

           String keyword= Urlfarend(total);
           // WriteLine(keyword);
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = DATA.Length;
            request.Headers["X-Mashape-Key"] = "pl80tf1WWmmshuVLZUCcmLqOM1Gkp1m52x5jsnKzqoMSgldcOF";
            System.IO.StreamWriter requestWriter = new System.IO.StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            requestWriter.Write(DATA);
            requestWriter.Close();

            try
            {
                System.Net.WebResponse webResponse = request.GetResponse();
                System.IO.Stream webStream = webResponse.GetResponseStream();
                System.IO.StreamReader responseReader = new System.IO.StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                JObject jo = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                String strings = jo["sentences"].ToString();
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    panel_txt.Visibility = System.Windows.Visibility.Hidden;

                }));
                WriteLine("Key words:");
                string[] sArray = keyword.Split(',');
                for (int k = 0; k < sArray.Length; k++)
                {
                    WriteLine(sArray[k]);
                    if (k >= 4)
                        break;

                }
               
                WriteLine(keyword);
                WriteLine();
                WriteLine("Summary:");
                Regex regexObj = new Regex("\"(?<result>[^\"\"]+)\"");
                Match matchResult = regexObj.Match(strings);
                String temp = string.Empty;
                // StringCollection resultList = new StringCollection();
                int i = 1;
                while (matchResult.Success)
                {
                    temp = matchResult.Groups["result"].Value.ToString();
                    WriteLine(i++.ToString()+".  "+temp);
                    matchResult = matchResult.NextMatch();
                }
             //   WriteLine(strings);
                WriteLine();
                //WriteLine(total);
                responseReader.Close();
                
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(e.Message);
            }



        }
        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.MediaElement)sender).Position = ((System.Windows.Controls.MediaElement)sender).Position.Add(TimeSpan.FromMilliseconds(1));
        }
        private void MetroWindow_Initialized(object sender, EventArgs e)
        {
            //this.micClient.StartMicAndRecognition();
            ParameterizedThreadStart tStart = new ParameterizedThreadStart(unirest_test);
            Thread thread = new Thread(tStart);
            thread.Start(null);//传递参数

            //
        }
    }
}
