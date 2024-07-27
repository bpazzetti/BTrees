using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BTrees
{
    public class BTree
    {
        public BTreeNode? Root { get; internal set; }
        public int Degree { get; }

        public BTree(int degree)
        {
            Root = null;
            Degree = degree;
        }

        public void PrintTree()
        {
            Root?.PrintTree(0);
        }

        public void Insert(int key)
        {
            if (Root == null)
            {
                Root = new BTreeNode(Degree, true, this);
                Root.Keys.Add(key);
            }
            else
            {
                if (Root.Keys.Count == Degree - 1)
                {
                    Root.InsertFull(key);
                }
                else
                {
                    Root.InsertNonFull(key);
                }
            }
        }

        public void Traverse()
        {
            if (Root != null)
            {
                Root.Traverse();
            }
        }

        public BTreeNode Search(int key)
        {
            return Root?.Search(key);
        }
    }
}
