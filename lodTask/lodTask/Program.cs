using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the ingredients separated by spaces:");
            string ingredients = Console.ReadLine().Replace(" ", ",");
            int recipe = 0;
            int minIngr = 0;
            int a = 0;
            string title = "";
            string href = "";
            string ingredient = "";
            Console.WriteLine("Searching recipe for you");
            for (int i = 1; i <= 10; i++)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.recipepuppy.com/api/?i=" +
                       ingredients + "&p=" + i);
                    HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
                    Stream dataStream = resp.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    RootObject newObj = JsonConvert.DeserializeObject<RootObject>(responseFromServer);
                    foreach (Result item in newObj.results)
                    {
                        int ingrLength = item.ingredients.Split(new Char[] { ',' }).Length;
                        if (recipe == 0)
                        {
                            minIngr = ingrLength;
                            a = recipe;
                            title = item.title;
                            href = item.href;
                            ingredient = item.ingredients;
                        }
                        else
                        {
                            if (ingrLength < minIngr)
                            {
                                minIngr = ingrLength;
                                a = recipe;
                                title = item.title;
                                href = item.href;
                                ingredient = item.ingredients;
                            }
                        }
                        recipe++;
                    }

                }
                catch { }
            }
            if (title != "")
            {
                Console.WriteLine("Simpliest recipe for requested ingredients is '{0}'", title);
                Console.WriteLine(" It contains only this ingredients '{0}'", ingredient);
                Console.WriteLine(" Link: {0}", href);
            }
            else Console.WriteLine("Nothing is found.");
        }
    }

    public class RootObject
    {
        public List<Result> results { get; set; }
    }

    public class Result
    {
        public string title { get; set; }
        public string href { get; set; }
        public string ingredients { get; set; }

    }

}
