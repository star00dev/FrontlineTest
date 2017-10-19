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
        private int _treeDepth = 1;
        private List<Data> _nodeList = new List<Data>();




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
                
                var match = Regex.Match(processingString, _objPattern);
                currentNode = BuildData(match.Value, this._treeDepth);
                processingString = processingString.Replace(match.Value, "").Replace(",,",",");


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
            var reverDepthORder = (this._treeDepth - node.Depth) - 1;
            var depthLabel = new string('-',reverDepthORder<=0?0:reverDepthORder);
            Console.WriteLine("{0}{1}", depthLabel, node.Name);

            if (node.Children.Count > 0)
            {
                foreach (Data childNode in node.Children.OrderBy(n=>n.Index)) {
                    PrintTree(childNode);
                }
                    
            }
        }

        public void PrintBonusTree(){
            var depth0children = this._nodeList.Where(n => n.Depth >= 2 && n.Depth <= 4).OrderBy(n => n.Name).ToList();
            foreach(Data node in depth0children){
                var reverDepthORder = (this._treeDepth - node.Depth) - 1;
                var depthLabel = new string('-', reverDepthORder <= 0 ? 0 : reverDepthORder);
                Console.WriteLine("{0}{1}", depthLabel, node.Name);
                if (node.Children.Count > 0 && node.Depth <= 2) {
                    foreach (Data child in node.Children)
                        PrintTree((child));
                }
            }

        }

        private Data BuildData(string objString, int depth)
        {
            Data result = new Data();
            this._nodeList.Add(result);
            result.Name = Regex.Match(objString, _objPattern).Groups[1].Value;
            result.Depth = depth + 1;
            result.Index = this._inputString.IndexOf(result.Name);
            var processingString = Regex.Match(objString, _objPattern).Groups[2].Value;

            var matches = Regex.Matches(processingString, _attPattern);
            foreach(Match match in matches)
            {
                if (!string.IsNullOrEmpty(match.Value))
                {
                    var newNode = new Data
                    {
                        Name = match.Value,
                        Depth = depth,
                        Index = this._inputString.IndexOf(match.Value)
                    };
                    this._nodeList.Add(newNode);
                    result.Children.Add(newNode);
                }
            }

            return result;
        }

        
        
    }
}
