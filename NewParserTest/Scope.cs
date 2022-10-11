using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewParserTest.Node.Interfaces;

namespace NewParserTest
{
    public class Scope
    {
        public List<INode> Nodes { get; set; }

        public Scope()
        {
            Nodes = new();
        }

        public bool AddNode(INode node)
        {
            try
            {
                Nodes.Add(node);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            
        }
    }
}