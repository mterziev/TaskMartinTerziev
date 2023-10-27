using System;
using System.Collections.Generic;
using System.IO;
using HtmlAgilityPack;

class Program
{
    static void Main()
    {
        try
        {
            string filePath = "../../../file.html";
            string htmlContent = File.ReadAllText(filePath);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            List<Dictionary<string, string>> products = new List<Dictionary<string, string>>();

            foreach (var item in doc.DocumentNode.SelectNodes("//div[@class='item']"))
            {
                Dictionary<string, string> product = new Dictionary<string, string>();

                var productNameNode = item.SelectSingleNode(".//h4/a");
                var priceNode = item.SelectSingleNode(".//span[@style='display: none']");
                var rating = item.GetAttributeValue("rating", "");

                if (productNameNode != null && priceNode != null)
                {
                    product["productName"] = System.Net.WebUtility.HtmlDecode(productNameNode.InnerText);
                    product["price"] = priceNode.InnerText.Replace("$", "").Replace(",", "");
                    if (!string.IsNullOrWhiteSpace(rating))
                    {
                        decimal normalizedRating = decimal.Parse(rating);
                        if (normalizedRating > 5)
                        {
                            normalizedRating /= 2;
                        }

                        product["rating"] = normalizedRating.ToString();
                    }
                    else
                    {
                        product["rating"] = "0";
                    }

                    products.Add(product);
                }
            }

            Console.WriteLine("[");
            for (int i = 0; i < products.Count; i++)
            {
                Console.WriteLine("     {");
                Console.WriteLine($"        \"productName\": \"{products[i]["productName"]}\",");
                Console.WriteLine($"        \"price\": \"{products[i]["price"]}\",");
                Console.WriteLine($"        \"rating\": \"{products[i]["rating"]}\"");
                Console.Write("     }");

                if (i < products.Count - 1)
                {
                    Console.WriteLine(",");
                }
                else
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine("]");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
