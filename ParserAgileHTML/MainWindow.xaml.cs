using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace ParserAgileHTML
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            filterAccountSellerComboBox.Items.Add("личный");
            filterAccountSellerComboBox.Items.Add("фирма");
            filterAccountSellerComboBox.Items.Add("без разницы");

            //IEnumerable<int> numbers = Enumerable.Range(1, 5);

            //foreach (var number in numbers)
            //    CountAdsSellerComboBox.Items.Add(number);
        }

        private async void ParsBtn_Click(object sender, RoutedEventArgs e)
        {
            string pageLink = @"https://www.olx.pl/elektronika/gry-konsole/?search%5Bprivate_business%5D=private";

            var pageContent = await LoadPage($@"{pageLink}");

            var document = new HtmlDocument();

            document.LoadHtml(pageContent);

            int count = 0;

            HtmlNodeCollection links = document.DocumentNode.SelectNodes(".//h3[@class='lheight22 margintop5']/a");

            string[] linksMass = new string[links.Count];

            foreach (HtmlNode link in links)
            {
                string linkDef = link.GetAttributeValue("href", "");

                linksMass[count] = linkDef;

                count++;
            }

            try
            {
                int countAd = Convert.ToInt32(CountAdsForParsTextBox.Text);

                for (int i = 0; i < countAd; i++)
                {
                    Random random = new Random();

                    int taskDelay = random.Next(1104, 2434);
                    
                    await ParsePageProduct(linksMass[i], Convert.ToInt32(CountAdsSellerTextBox.Text), ListParser);
                    
                    await Task.Delay(taskDelay);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        static async Task<string> LoadPage(string url)
        {
            var result = "";

            var request = WebRequest.CreateHttp(url);

            WebProxy proxyObject = new WebProxy("http://proxyserver:80/", true);

            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36";
            request.Headers.Add("Accept-Langauge", "pl");
            
            //request.Proxy

            //request.CookieContainer.SetCookies(url, ""); //= new CookieContainer();

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var receiveStream = response.GetResponseStream();
                    if (receiveStream != null)
                    {
                        StreamReader readStream;
                        if (response.CharacterSet == null)
                            readStream = new StreamReader(receiveStream);
                        else
                            readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                        
                        result = await readStream.ReadToEndAsync();
                        
                        readStream.Close();
                    }
                }
                return result;
            }
        }

        static async Task ParsePageProduct(string link, int CountAdsSellerTextBox, ListBox ListParser)
        {
            var pageProductContent = await LoadPage($@"{link}");
            var documentProduct = new HtmlDocument();

            documentProduct.LoadHtml(pageProductContent);

            /*if (documentProduct.DocumentNode.SelectNodes(".//button[@data-cy='ad-contact-phone']") == null)
            {
                MessageBox.Show("no btb");
                return;
            }*/

            int countAds = await ParsePageSeller(documentProduct);

            if (countAds > CountAdsSellerTextBox)
            {
                ListParser.Items.Add($"У продавца больше объявлений - {countAds}\n*******************");
                return;
            }

            string nameProduct = documentProduct.DocumentNode.SelectSingleNode(".//h1[@class='css-r9zjja-Text eu5v0x0']").InnerText;
            string costProduct = documentProduct.DocumentNode.SelectSingleNode(".//h3[@class='css-okktvh-Text eu5v0x0']").InnerText;
            string descProduct = documentProduct.DocumentNode.SelectSingleNode(".//div[@class='css-g5mtbi-Text']").InnerText;
            string nameSeller = documentProduct.DocumentNode.SelectSingleNode(".//h2[@class='css-u8mbra-Text eu5v0x0']").InnerText;
            string regDate = documentProduct.DocumentNode.SelectSingleNode(".//div[@class='css-1bafgv4-Text eu5v0x0']").InnerText;
            string publishDate = documentProduct.DocumentNode.SelectSingleNode(".//span[@class='css-19yf5ek']").InnerText;

            ListParser.Items.Add(
                        $"Название - {nameProduct}\n\n" +
                        $"Стоимость - {costProduct}\n\n" +
                        $"Описание - {descProduct}\n\n" +
                        $"Имя продавца - {nameSeller}\n\n" +
                        $"Зарегестрирован - {regDate}\n\n" +
                        $"Количество объявлений - {countAds}\n\n" +
                        $"Опубликовано - {publishDate}\n\n" +
                        $"Ссылка - {link}\n\n" +
                        $"************\n");
        }

        static async Task<int> ParsePageSeller(HtmlDocument pageSellerDoc)
        {
            if (pageSellerDoc.DocumentNode.SelectSingleNode(".//section[@class='css-ecq9m8']/a").GetAttributeValue("href", "") == null)
                return 0;

            string urlSeller = pageSellerDoc.DocumentNode.SelectSingleNode(".//section[@class='css-ecq9m8']/a").GetAttributeValue("href", "");

            var pageSeller = await LoadPage($@"https://www.olx.pl/{urlSeller}");

            var documentPageSeller = new HtmlDocument();

            documentPageSeller.LoadHtml(pageSeller);

            var countAds = documentPageSeller.DocumentNode.SelectNodes(".//tr[@class='wrap']").Count;

            return countAds;
        }

        private void SelectCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowSelectCategory window = new WindowSelectCategory();

            window.Show();

            //MainWindowParser.IsEnabled = false;
        }
    }
}
