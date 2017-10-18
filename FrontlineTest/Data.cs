using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontlineTest
{
    public class Data {
        public string Name { get; set; }
        public int Depth { get; set; }
        public List<Data> Children { get; set; }

        public Data()
        {
            this.Depth = 0;
            this.Children = new List<Data>();
        }
    }
}
