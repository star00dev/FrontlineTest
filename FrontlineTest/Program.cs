using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace FrontlineTest
{
    public class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Program started!");

            string inputString = @"(id,created,employee(id,firstname,employeeType(id), lastname),location)";
            FrontlineAnswer answer = new FrontlineAnswer(inputString);
            answer.PrintOrgChart(answer.GetOrgChart());
        }
    }
    public class Attrubute {
        public string Name { get; set; }
        public int Depth { get; set; }

        public Attrubute() {
            this.Depth = 0;
        }
    }

    public class FrontlineAnswer {

        private string _inputString = String.Empty;

        public FrontlineAnswer(string inputString)
        {
            this._inputString = inputString;
        }

        public List<Attrubute> GetOrgChart() {
            List<Attrubute> results = new List<Attrubute>();
            string processingInput = this._inputString;
            string objPattern = @"([(][a-zA-Z0-9, ]*[)])";
            Regex regx = new Regex(objPattern);
            int depth = 1;
            while (processingInput.Length > 0) {
                foreach (Match match in Regex.Matches(processingInput, objPattern))
                {
                    string[] attsArray = match.Groups[1].Value.Replace("(", "").Replace(")", "").Replace(", ", ",").Replace(" ,", ",").Split(",".ToCharArray());
                    foreach (string att in attsArray)
                    {
                        results.Add(new Attrubute
                        {
                            Depth = depth,
                            Name = att
                        });
                    }
                }
                depth++;
                processingInput = regx.Replace(processingInput, "").Replace(",,", ",");
            }
            return results;
        }

        public void PrintOrgChart(List<Attrubute> attList) {
            int maxDepth = attList.OrderByDescending(emp => emp.Depth).First().Depth;


            var displayList = attList.OrderByDescending(tmp => tmp.Depth).ThenBy(tmp => tmp.Name).ToList();
            foreach (Attrubute att in displayList)
            {
                Console.WriteLine("{0}{1}", new String('-', maxDepth - att.Depth), att.Name);
            }
        }
    }
}
