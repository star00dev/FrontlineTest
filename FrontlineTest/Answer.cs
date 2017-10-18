using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FrontlineTest
{
    public class Answer
    {
        private string _inputString = String.Empty;
        private string _objPattern = @"([a-zA-Z0-9]*)([(][a-zA-Z0-9, ]*[)])";
        private string _attPattern = @"[a-zA-Z0-9]*";
        private int _treeDepth = 0;
        public Answer(string input)
        {
            this._inputString = input;
        }


        public void ProcessInput()
        {
            var processInput = this._inputString;
        }
        
        public Data BuildTreeFromString()
        {
            var processingString = this._inputString;
            Data currentNode = null, childNode = null;
            while (!string.IsNullOrEmpty(processingString))
            {
                var objMatchString = Regex.Match(processingString, _objPattern).Value;
                currentNode = BuildData(objMatchString, this._treeDepth);
                processingString = processingString.Replace(objMatchString, "").Replace(",,",",");
                if (childNode != null)
                {
                    currentNode.Children.Add(childNode);
                }
                childNode = currentNode;
                this._treeDepth++;
            }

            return currentNode;
        }


        public void PrintTree(Data node)
        {
            Console.WriteLine("{0} {1}", (this._treeDepth - node.Depth), node.Name);

            if (node.Children.Count > 0)
            {
                foreach (Data childNode in node.Children)
                    PrintTree(childNode);
            }
        }

        private Data BuildData(string objString, int depth)
        {
            Data result = new Data();
            result.Name = Regex.Match(objString, _objPattern).Groups[1].Value;
            result.Depth = depth + 1;
            var processingString = Regex.Match(objString, _objPattern).Groups[2].Value;

            var matches = Regex.Matches(processingString, _attPattern);
            foreach(Match match in matches)
            {
                if (!string.IsNullOrEmpty(match.Value))
                {
                    result.Children.Add(new Data
                    {
                        Name = match.Value,
                        Depth = depth
                    });
                }
            }

            return result;
        }

        
        
    }
}
