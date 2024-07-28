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
        /// <summary>
        /// Gets or sets the root node of the B-tree.
        /// </summary>
        public BTreeNode? Root { get; internal set; }
        /// <summary>
        /// Gets the order of the B-tree.
        /// </summary>
        public int Order { get; }

        /// <summary>
        /// Initializes a new instance of the BTree class with the specified order.
        /// </summary>
        /// <param name="order">The order of the B-tree</param>
        public BTree(int order)
        {
            Root = null;
            Order = order;
        }

        /// <summary>
        /// Prints the B-tree structure starting from the root.
        /// </summary>
        public void PrintTree()
        {
            Root?.PrintTree(0);
        }

        /// <summary>
        /// Inserts a key into the B-tree.
        /// </summary>
        /// <param name="key">The key to insert</param>
        public void Insert(int key)
        {
            if (Root == null)
            {
                // If the tree is empty, create a new root
                Root = new BTreeNode(Order, true, this);
                Root.Keys.Add(key);
            }
            else
            {
                // Insert the key into the non-empty tree
                Root.Insert(key);
            }
        }

        /// <summary>
        /// Traverses all nodes in the B-tree.
        /// </summary>
        public void Traverse()
        {
            if (Root != null)
            {
                Root.Traverse();
            }
        }

        /// <summary>
        /// Searches for a key in the B-tree.
        /// </summary>
        /// <param name="key">The key to search for</param>
        /// <returns>The node containing the key, or null if not found</returns>
        public BTreeNode Search(int key)
        {
            return Root?.Search(key);
        }

        /// <summary>
        /// Deletes a key from the B-tree.
        /// </summary>
        /// <param name="key">The key to delete</param>
        public void Delete(int key)
        {
            if (Root == null)
            {
                Console.WriteLine("The tree is empty");
                return;
            }

            Root.Delete(key);

            // If the root node has no keys, update the root
            if (Root.Keys.Count == 0)
            {
                if (Root.IsLeaf)
                    Root = null; // If the root is a leaf, set the root to null
                else
                    Root = Root.Children[0]; // Otherwise, set the root to the first child
            }
        }

        /// <summary>
        /// Validates the structure of a B-tree
        /// </summary>
        /// <returns>True if the node is valid, otherwise false</returns>
        public bool IsValidBTree()
        {
            // Check if the tree is empty
            if (this.Root == null)
            {
                return true; // An empty tree is a valid B-tree
            }

            return this.Root.IsValidBTreeNode();            
        }
    }
}
