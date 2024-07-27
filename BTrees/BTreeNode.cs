using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BTrees
{
    public class BTreeNode
    {
        public int Degree { get; }
        public List<int> Keys { get; }
        public List<BTreeNode> Children { get; }
        public bool IsLeaf { get; }
        public BTree Tree { get; }

        public BTreeNode(int degree, bool isLeaf, BTree tree)
        {
            Degree = degree;
            Keys = new List<int>();
            Children = new List<BTreeNode>();
            IsLeaf = isLeaf;
            Tree = tree;
        }

        public void PrintTree(int level = 0)
        {
            Console.Write(new string(' ', level * 4));
            Console.WriteLine(string.Join(" ", Keys.Select(item => item.ToString("D2"))));

            if (!IsLeaf)
            {
                foreach (var child in Children)
                {
                    child.PrintTree(level + 1);
                }
            }
        }

        public BTreeNode Search(int key)
        {
            int i = 0;
            while (i < Keys.Count && key > Keys[i])
            {
                i++;
            }

            if (i < Keys.Count && Keys[i] == key)
            {
                return this;
            }

            if (IsLeaf)
            {
                return null;
            }

            return Children[i].Search(key);
        }

        public void Traverse()
        {
            int i;
            for (i = 0; i < Keys.Count; i++)
            {
                if (!IsLeaf)
                {
                    Children[i].Traverse();
                }
                Console.Write(" " + Keys[i]);
            }

            if (!IsLeaf)
            {
                Children[i].Traverse();
            }
        }

        internal void InsertFull(int key)
        {
            int i = Keys.Count - 1;
            if (IsLeaf)
            {
                while (i >= 0 && Keys[i] > key)
                {
                    i--;
                }
                Keys.Insert(i + 1, key);
            }
            else
            {
                while (i >= 0 && Keys[i] > key)
                {
                    i--;
                }
                i++;
                Children[i].InsertFull(key);
            }

            if (Keys.Count == Degree)
            {
                if (Tree.Root == this)
                {
                    BTreeNode newRoot = new BTreeNode(Degree, false, this.Tree);
                    newRoot.Children.Add(this);
                    newRoot.SplitChild(0, Tree.Root);
                    Tree.Root = newRoot;
                }
                else
                {
                    BTreeNode parent = FindParent(Tree.Root, this);
                    parent.SplitChild(parent.Children.IndexOf(this), this);
                }
            }
        }

        private void SplitChild(int childIndex, BTreeNode nodeToSplit)
        {
            int middleIndex = Degree / 2;
            BTreeNode newNode = new BTreeNode(nodeToSplit.Degree, nodeToSplit.IsLeaf, Tree);
            newNode.Keys.AddRange(nodeToSplit.Keys.GetRange(middleIndex + 1, nodeToSplit.Keys.Count - (middleIndex + 1)));
            nodeToSplit.Keys.RemoveRange(middleIndex + 1, nodeToSplit.Keys.Count - (middleIndex + 1));

            if (!nodeToSplit.IsLeaf)
            {
                newNode.Children.AddRange(nodeToSplit.Children.GetRange(middleIndex + 1, nodeToSplit.Children.Count - (middleIndex + 1)));
                nodeToSplit.Children.RemoveRange(middleIndex + 1, nodeToSplit.Children.Count - (middleIndex + 1));
            }

            Children.Insert(childIndex + 1, newNode);
            Keys.Insert(childIndex, nodeToSplit.Keys[middleIndex]);
            nodeToSplit.Keys.RemoveAt(middleIndex);
        }

        private BTreeNode FindParent(BTreeNode node, BTreeNode child)
        {
            if (node == null || node.IsLeaf)
            {
                return null;
            }
            foreach (BTreeNode n in node.Children)
            {
                if (n == child)
                    return node;
                else
                {
                    BTreeNode res = FindParent(n, child);
                    if (res != null)
                        return res;
                }
            }
            return null;
        }

        internal void InsertNonFull(int key)
        {
            int i = Keys.Count - 1;

            if (IsLeaf)
            {
                while (i >= 0 && Keys[i] > key)
                {
                    i--;
                }
                Keys.Insert(i + 1, key);
            }
            else
            {
                while (i >= 0 && Keys[i] > key)
                {
                    i--;
                }
                i++;
                if (Children[i].Keys.Count == Degree - 1)
                {
                    Children[i].InsertFull(key);
                }
                else
                {
                    Children[i].InsertNonFull(key);
                }
            }
        }
    }
}
