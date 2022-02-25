using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using HtmlAgilityPack;
using System.Net;
using System.Text;

public class Prod {
    IWebDriver driver = new ChromeDriver();
    driver.Url = @"https://www.olx.pl/d/oferta/super-gra-ojciec-chrzestny-CID99-IDMIbvI.html#506dd58b3b";

//driver.FindElement(By.XPath(@".//div[@id='search-3']/form/input[@id='s']")).SendKeys("c#");

//driver.FindElement(By.XPath(@".//input[@id='searchsubmit']")).Click();
//Thread.Sleep(3000);

var links = driver.FindElements(By.XPath("/html/body/div[1]/div[1]/div[3]/div[3]/div[1]/div[2]/div[3]/h3"));
foreach (IWebElement link in links)
Console.WriteLine("{0}", link.Text);
}

//Console.WriteLine("Введите адрес сайта:");

//string pageLink = Console.ReadLine();


/*
string pageLink = @"https://www.olx.pl/d/oferta/sacred-2-fallen-angel-gra-na-pc-CID99-IDK9upe.html#a144e37909";

var pageContent = LoadPage(@$"{pageLink}");

Thread.Sleep(1403);

var document = new HtmlDocument();
document.LoadHtml(pageContent);

Thread.Sleep(1033);

var d = document.DocumentNode.SelectSingleNode("/html/body/div[1]/div[1]/div[3]/div[3]/div[2]/div[1]/div[1]").Attributes;

//var d = document.DocumentNode.SelectNodes(".//div[@class='css-1eowuel']");

foreach (var v in d)
{
    Console.WriteLine(v.Value);
}

//{
Console.WriteLine($"{d}");
return;
    //return;
//}

int count = 0;
HtmlNodeCollection links = document.DocumentNode.SelectNodes(".//h3[@class='lheight22 margintop5']/a");

string[] mass = new string[links.Count];

foreach (HtmlNode link in links)
{
    string linkDef = link.GetAttributeValue("href", "");

    mass[count] = linkDef;

    count++;
}

foreach (var a in mass)
{
    if (a.Contains(';'))
    {
        string v = a.Remove(a.IndexOf(";"));
        Console.WriteLine(v);
    }
    else Console.WriteLine(a);

    var pageProductContent = LoadPage(@$"{a}");
    var documentProduct = new HtmlDocument();
    documentProduct.LoadHtml(pageProductContent);

    var nameProduct = documentProduct.DocumentNode.SelectSingleNode(".//h1[@class='css-r9zjja-Text eu5v0x0']").InnerText;

    var cost = documentProduct.DocumentNode.SelectSingleNode(".//h3[@class='css-okktvh-Text eu5v0x0']").InnerText;

    var description = documentProduct.DocumentNode.SelectSingleNode(".//div[@class='css-g5mtbi-Text']").InnerText;

    var nameSeller = documentProduct.DocumentNode.SelectSingleNode(".//h2[@class='css-u8mbra-Text eu5v0x0']").InnerText;

    var regDate = documentProduct.DocumentNode.SelectSingleNode(".//div[@class='css-1bafgv4-Text eu5v0x0']").InnerText;

    var publishTime = documentProduct.DocumentNode.SelectSingleNode(".//span[@class='css-19yf5ek']").InnerText;

    Console.WriteLine(
        $"Название - {nameProduct}\n\n" +
        $"Стоимость - {cost}\n\n" +
        $"Описание - {description}\n\n" +
        $"Имя продавца - {nameSeller}\n\n" +
        $"Зареган - {regDate}\n\n" +
        $"Опубликовано - {publishTime}\n\n" +
        $"************\n");

    Thread.Sleep(1000);
}


static string LoadPage(string url)
{
    var result = "";

    var request = WebRequest.CreateHttp(url);

    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36";
    request.Headers.Add("Accept-Langauge", "pl");


    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
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
                result = readStream.ReadToEnd();
                readStream.Close();
            }
        }
        return result;
    }
}*/