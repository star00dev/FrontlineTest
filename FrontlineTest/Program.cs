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
            //FrontlineAnswer answer = new FrontlineAnswer(inputString);
            //answer.PrintAttrubutes(answer.ProcessInput());

            Answer answer = new Answer(inputString);
            var node = answer.BuildTreeFromString();
            answer.PrintTree(node);
        }
    }
    public class Attrubute
    {
        public string Name { get; set; }
        public int Depth { get; set; }
        public int Index { get; set; }
        public List<Attrubute> Children { get; set; }

        public Attrubute()
        {
            this.Depth = 0;
            this.Children = new List<Attrubute>();
        }
    }

    public class FrontlineAnswer
    {

        private string _inputString = String.Empty;
        private int maxDepth = 0;

        public FrontlineAnswer(string inputString)
        {
            this._inputString = inputString;
        }

        public Attrubute ProcessInput()
        {
            string processingInput = this._inputString;
            string objPattern = @"([(][a-zA-Z0-9, ()]*[)])";
            Regex regx = new Regex(objPattern);
            int depth = 0;
            Attrubute prevNode = null;
            while (processingInput.Length > 0)
            {

                var matches = Regex.Matches(processingInput, objPattern);
                if (matches.Count > 0)
                {
                    var newNode = new Attrubute();

                    if (prevNode != null)
                        newNode.Children.Add(prevNode);

                    foreach (Match match in matches)
                    {
                        string[] attsArray = match.Groups[1].Value.Replace("(", "").Replace(")", "").Replace(", ", ",").Replace(" ,", ",").Split(",".ToCharArray());
                        foreach (string att in attsArray)
                        {
                            if (!string.IsNullOrEmpty(att))
                            {
                                newNode.Children.Add(new Attrubute
                                {
                                    Depth = depth,
                                    Name = att
                                });
                            }
                        }
                    }
                    prevNode = newNode;
                    depth++;
                    processingInput = regx.Replace(processingInput, "").Replace(",,", ",");
                }
            }
            maxDepth = depth - 1;

            return prevNode;
        }

        public void PrintAttrubutes(Attrubute root, bool bonus = true)
        {
            Console.WriteLine();
            Console.WriteLine("-------Result----------");
            Console.WriteLine("Name:{0} maxDepth: {1} Depth: {2}", root.Name, maxDepth, root.Depth);

            foreach (Attrubute node in root.Children)
            {
                Console.WriteLine("Name:{0} maxDepth: {1} Depth: {2}", node.Name, maxDepth, node.Depth);
            }


            Console.WriteLine("--------------------------");
            PrintNode(root);


            if (bonus)
            {
                Console.WriteLine();
                Console.WriteLine("-------Bonus----------");
            }
        }

        private void PrintNode(Attrubute node)
        {
            if (node.Children.Count > 0)
            {
                Console.WriteLine("Name:{0} maxDepth: {1} Depth: {2}", node.Name, maxDepth, node.Depth);
                foreach (var tmpNode in node.Children)
                {
                    Console.WriteLine("Name:{0} maxDepth: {1} Depth: {2}", node.Name, maxDepth, node.Depth);
                    PrintNode(tmpNode);
                }
            }

            //node.Depth = (maxDepth - node.Depth);

            //Console.WriteLine("{0}{1}", new string('-', maxDepth-node.Depth), node.Name);
            Console.WriteLine("Name:{0} maxDepth: {1} Depth: {2}", node.Name, maxDepth, node.Depth);
        }

        private List<Attrubute> AssignOrginalIndex(List<Attrubute> results)
        {
            int maxDepth = results.OrderByDescending(emp => emp.Depth).First().Depth;
            foreach (var att in results)
            {
                var matches = Regex.Matches(this._inputString, $"{att.Name}");

                //reverse the depth order
                att.Depth = maxDepth - att.Depth;

                //assign the origianl index
                if (att.Depth <= 0 || matches.Count == 1)
                    att.Index = matches[0].Index;
                else
                    att.Index = matches[att.Depth].Index;
            }
            return results;
        }
    }
}
