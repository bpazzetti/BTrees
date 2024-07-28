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
        public int Order { get; }
        public List<int> Keys { get; }
        public List<BTreeNode> Children { get; }
        public bool IsLeaf { get; }
        public BTree Tree { get; }
        public int MinKeys { get; private set; }
        public int MaxKeys { get; private set; }

        /// <summary>
        /// Constructor to initialize a BTreeNode
        /// </summary>
        /// <param name="order">The order of the B-tree</param>
        /// <param name="isLeaf">Indicates if the node is a leaf</param>
        /// <param name="tree">Reference to the B-Tree</param>
        public BTreeNode(int order, bool isLeaf, BTree tree)
        {
            Order = order;
            Keys = new List<int>();
            Children = new List<BTreeNode>();
            IsLeaf = isLeaf;
            Tree = tree;
            MinKeys = (int)Math.Ceiling(order / 2.0)-1;
            MaxKeys = order - 1;
        }

        /// <summary>
        /// Prints the B-tree structure starting from this node
        /// </summary>
        /// <param name="level">The current level in the tree (used for indentation)</param>
        public void PrintTree(int level = 0)
        {
            // Print the keys in the current node with indentation
            Console.Write(new string(' ', level * 4));
            Console.WriteLine(string.Join(" ", Keys.Select(item => item.ToString("D2"))));

            // Recursively print the children nodes
            if (!IsLeaf)
            {
                foreach (var child in Children)
                {
                    child.PrintTree(level + 1);
                }
            }
        }

        /// <summary>
        /// Searches for a key in the subtree rooted with this node
        /// </summary>
        /// <param name="key">The key to search for</param>
        /// <returns>The node containing the key, or null if not found</returns>
        public BTreeNode Search(int key)
        {
            int i = 0;
            // Find the first key greater than or equal to the given key
            while (i < Keys.Count && key > Keys[i])
            {
                i++;
            }

            // If the found key is equal to the given key, return this node
            if (i < Keys.Count && Keys[i] == key)
            {
                return this;
            }

            // If the node is a leaf, the key is not present
            if (IsLeaf)
            {
                return null;
            }

            // Recur to the appropriate child
            return Children[i].Search(key);
        }

        /// <summary>
        /// Traverses all nodes in the subtree rooted with this node
        /// </summary>
        public void Traverse()
        {
            int i;
            // Traverse the subtree rooted with each child
            for (i = 0; i < Keys.Count; i++)
            {
                if (!IsLeaf)
                {
                    Children[i].Traverse();
                }
                Console.Write(" " + Keys[i]);
            }

            // Traverse the subtree rooted with the last child
            if (!IsLeaf)
            {
                Children[i].Traverse();
            }
        }

        /// <summary>
        /// Inserts a key into the B-tree node.
        /// </summary>
        /// <param name="key">The key to insert</param>
        public void Insert(int key)
        {
            // If the current node is full, handle the insertion in a full node
            if (Keys.Count == MaxKeys)
            {
                InsertFull(key);
            }
            else
            {
                // Otherwise, handle the insertion in a non-full node
                InsertNonFull(key);
            }
        }

        /// <summary>
        /// Inserts a key into a full node
        /// </summary>
        /// <param name="key">The key to insert</param>
        private void InsertFull(int key)
        {
            int i = Keys.Count - 1;
            if (IsLeaf)
            {
                // Find the location to insert the new key
                while (i >= 0 && Keys[i] > key)
                {
                    i--;
                }
                Keys.Insert(i + 1, key);
            }
            else
            {
                // Find the child which is going to have the new key
                while (i >= 0 && Keys[i] > key)
                {
                    i--;
                }
                i++;
                Children[i].InsertFull(key);
            }

            // If the node is full, split it
            if (Keys.Count == MaxKeys)
            {
                if (Tree.Root == this)
                {
                    // If the node is the root, create a new root
                    BTreeNode newRoot = new BTreeNode(Order, false, this.Tree);
                    newRoot.Children.Add(this);
                    newRoot.SplitChild(0, Tree.Root);
                    Tree.Root = newRoot;
                }
                else
                {
                    // Find the parent and split the child
                    BTreeNode parent = FindParent(Tree.Root, this);
                    parent.SplitChild(parent.Children.IndexOf(this), this);
                }
            }
        }

        /// <summary>
        /// Splits a child node
        /// </summary>
        /// <param name="childIndex">The index of the child to split</param>
        /// <param name="nodeToSplit">The node to split</param>
        private void SplitChild(int childIndex, BTreeNode nodeToSplit)
        {
            int middleIndex = Order / 2;
            BTreeNode newNode = new BTreeNode(nodeToSplit.Order, nodeToSplit.IsLeaf, Tree);
            // Move the second half of keys to the new node
            newNode.Keys.AddRange(nodeToSplit.Keys.GetRange(middleIndex + 1, nodeToSplit.Keys.Count - (middleIndex + 1)));
            nodeToSplit.Keys.RemoveRange(middleIndex + 1, nodeToSplit.Keys.Count - (middleIndex + 1));

            // Move the second half of children to the new node
            if (!nodeToSplit.IsLeaf)
            {
                newNode.Children.AddRange(nodeToSplit.Children.GetRange(middleIndex + 1, nodeToSplit.Children.Count - (middleIndex + 1)));
                nodeToSplit.Children.RemoveRange(middleIndex + 1, nodeToSplit.Children.Count - (middleIndex + 1));
            }

            // Insert the new node as a child
            Children.Insert(childIndex + 1, newNode);
            // Move the middle key up to the parent
            Keys.Insert(childIndex, nodeToSplit.Keys[middleIndex]);
            nodeToSplit.Keys.RemoveAt(middleIndex);
        }

        /// <summary>
        /// Finds the parent of a given node in the B-tree.
        /// </summary>
        /// <param name="node">The current node being checked</param>
        /// <param name="child">The child node for which the parent is being searched</param>
        /// <returns>The parent node if found, otherwise null</returns>
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

        /// <summary>
        /// Inserts a key into a non-full node.
        /// </summary>
        /// <param name="key">The key to insert</param>
        private void InsertNonFull(int key)
        {
            int i = Keys.Count - 1;

            if (IsLeaf)
            {
                // Find the location to insert the new key
                while (i >= 0 && Keys[i] > key)
                {
                    i--;
                }
                Keys.Insert(i + 1, key);
            }
            else
            {
                // Find the child which is going to have the new key
                while (i >= 0 && Keys[i] > key)
                {
                    i--;
                }
                i++;
                // If the found child is full, insert into full node
                if (Children[i].Keys.Count == MaxKeys)
                {
                    Children[i].InsertFull(key);
                }
                else
                {
                    Children[i].InsertNonFull(key);
                }
            }
        }

        /// <summary>
        /// Deletes a key from the B-tree.
        /// </summary>
        /// <param name="key">The key to delete</param>
        public void Delete(int key)
        {
            int idx = FindKey(key);

            if (idx < Keys.Count && Keys[idx] == key)
            {
                if (IsLeaf)
                    RemoveFromLeaf(idx);
                else
                    RemoveFromNonLeaf(idx);
            }
            else
            {
                if (IsLeaf)
                {
                    Console.WriteLine("The key {0} is not present in the tree", key);
                    return;
                }

                bool flag = (idx == Keys.Count);

                // If the child where the key is supposed to exist has less than the minimum number of keys, fill it
                if (Children[idx].Keys.Count < MinKeys)
                    Fill(idx);

                // Recur to the appropriate child
                if (flag && idx > Keys.Count)
                    Children[idx - 1].Delete(key);
                else
                    Children[idx].Delete(key);
            }
        }

        /// <summary>
        /// Finds the index of the first key that is greater than or equal to the given key.
        /// </summary>
        /// <param name="key">The key to find</param>
        /// <returns>The index of the key</returns>
        private int FindKey(int key)
        {
            int idx = 0;
            while (idx < Keys.Count && Keys[idx] < key)
            {
                ++idx;
            }
            return idx;
        }

        /// <summary>
        /// Removes a key from a leaf node.
        /// </summary>
        /// <param name="idx">The index of the key to remove</param>
        private void RemoveFromLeaf(int idx)
        {
            Keys.RemoveAt(idx);

            // Check if the leaf node has fewer than the minimum number of keys
            if (Keys.Count < MinKeys && this != Tree.Root)
            {
                // If it does, borrow or merge to maintain the B-tree properties
                BTreeNode parent = FindParent(Tree.Root, this);
                int parentIdx = parent.Children.IndexOf(this);
                parent.Fill(parentIdx);
            }
        }

        /// <summary>
        /// Removes a key from a non-leaf node.
        /// </summary>
        /// <param name="idx">The index of the key to remove</param>
        private void RemoveFromNonLeaf(int idx)
        {
            int key = Keys[idx];

            // If the child before the key has at least MinKeys, find the predecessor
            if (Children[idx].Keys.Count >= MinKeys)
            {
                int pred = GetPredecessor(idx);
                Keys[idx] = pred;
                Children[idx].Delete(pred);
            }
            // If the child after the key has at least MinKeys, find the successor
            else if (Children[idx + 1].Keys.Count >= MinKeys)
            {
                int succ = GetSuccessor(idx);
                Keys[idx] = succ;
                Children[idx + 1].Delete(succ);
            }
            // If both children have less than MinKeys, merge them
            else
            {
                Merge(idx);
                Children[idx].Delete(key);                
            }
        }

        /// <summary>
        /// Gets the predecessor of the key at the given index.
        /// </summary>
        /// <param name="idx">The index of the key</param>
        /// <returns>The predecessor key</returns>
        private int GetPredecessor(int idx)
        {
            BTreeNode current = Children[idx];
            // Move to the rightmost leaf
            while (!current.IsLeaf)
                current = current.Children[current.Keys.Count];
            return current.Keys[current.Keys.Count - 1];
        }

        /// <summary>
        /// Gets the successor of the key at the given index.
        /// </summary>
        /// <param name="idx">The index of the key</param>
        /// <returns>The successor key</returns>
        private int GetSuccessor(int idx)
        {
            BTreeNode current = Children[idx + 1];
            // Move to the leftmost leaf
            while (!current.IsLeaf)
                current = current.Children[0];
            return current.Keys[0];
        }

        /// <summary>
        /// Fills the child node at the given index if it has less than MinKeys.
        /// </summary>
        /// <param name="idx">The index of the child</param>
        private void Fill(int idx)
        {
            // Borrow from the previous sibling if it has more than MinKeys
            if (idx != 0 && Children[idx - 1].Keys.Count > MinKeys)
                BorrowFromPrev(idx);
            // Borrow from the next sibling if it has more than MinKeys
            else if (idx != Keys.Count && Children[idx + 1].Keys.Count > MinKeys)
                BorrowFromNext(idx);
            else
            {
                // Merge with the previous or next sibling
                if (idx > 0)
                    Merge(idx - 1); //Merge to left sibling
                else
                    Merge(idx); //Merge to right sibling

                // Check if the leaf node has fewer than the minimum number of keys
                if (Keys.Count < MinKeys && this != Tree.Root)
                {
                    BTreeNode parent = FindParent(Tree.Root, this);
                    int parentIdx = parent.Children.IndexOf(this);
                    parent.Fill(parentIdx);
                }
            }
        }

        /// <summary>
        /// Borrows a key from the previous sibling.
        /// </summary>
        /// <param name="idx">The index of the child to borrow for</param>
        private void BorrowFromPrev(int idx)
        {
            BTreeNode child = Children[idx];
            BTreeNode sibling = Children[idx - 1];

            // Move the last key of the sibling to the child
            child.Keys.Insert(0, Keys[idx - 1]);

            // Move the last child of the sibling to the child
            if (!child.IsLeaf)
                child.Children.Insert(0, sibling.Children[sibling.Children.Count-1]);

            // Update the parent's key
            Keys[idx - 1] = sibling.Keys[sibling.Keys.Count - 1];
            sibling.Keys.RemoveAt(sibling.Keys.Count - 1);

            // Remove the last child of the sibling
            if (!sibling.IsLeaf)
                sibling.Children.RemoveAt(sibling.Children.Count-1);

        }

        /// <summary>
        /// Borrows a key from the next sibling.
        /// </summary>
        /// <param name="idx">The index of the child to borrow for</param>
        private void BorrowFromNext(int idx)
        {
            BTreeNode child = Children[idx];
            BTreeNode sibling = Children[idx + 1];

            // Move the first key of the sibling to the child
            child.Keys.Add(Keys[idx]);

            // Move the first child of the sibling to the child
            if (!child.IsLeaf)
                child.Children.Add(sibling.Children[0]);

            // Update the parent's key
            Keys[idx] = sibling.Keys[0];
            sibling.Keys.RemoveAt(0);

            // Remove the first child of the sibling
            if (!sibling.IsLeaf)
                sibling.Children.RemoveAt(0);

        }

        /// <summary>
        /// Merges the child at the given index with its sibling.
        /// </summary>
        /// <param name="idx">The index of the child to merge</param>
        private void Merge(int idx)
        {
            BTreeNode child = Children[idx];
            BTreeNode sibling = Children[idx + 1];

            // Move the key from the parent to the child
            child.Keys.Add(Keys[idx]);

            // Move all keys from the sibling to the child
            for (int i = 0; i < sibling.Keys.Count; ++i)
                child.Keys.Add(sibling.Keys[i]);

            // Move all children from the sibling to the child
            if (!child.IsLeaf)
            {
                for (int i = 0; i <= sibling.Keys.Count; ++i)
                    child.Children.Add(sibling.Children[i]);
            }

            // Remove the key from the parent
            Keys.RemoveAt(idx);
            // Remove the sibling from the children list
            Children.RemoveAt(idx + 1);
        }

        /// <summary>
        /// Validates the structure of a B-tree node.
        /// </summary>
        /// <returns>True if the node is valid, otherwise false</returns>
        public bool IsValidBTreeNode()
        {            
            // Check key count
            if (Keys.Count < (this == Tree.Root ? 1 : MinKeys) || Keys.Count > MaxKeys)
            {
                return false;
            }

            // Check key order
            for (int i = 1; i < Keys.Count; i++)
            {
                if (Keys[i - 1] >= Keys[i])
                {
                    return false;
                }
            }

            // Check children
            if (!IsLeaf)
            {
                //Check Child count
                if (Children.Count > Order)
                {
                    return false;
                }

                // Check child count
                if (Children.Count != Keys.Count + 1)
                {
                    return false;
                }

                // Check the order of keys in children relative to the parent node's keys
                for (int i = 0; i < Children.Count; i++)
                {
                    if (i > 0 && Keys[i - 1] >= Children[i].Keys[0])
                    {
                        return false;
                    }
                    if (i < Keys.Count && Keys[i] <= Children[i].Keys[Children[i].Keys.Count - 1])
                    {
                        return false;
                    }
                }

                // Recursively check all children
                for (int i = 0; i < Children.Count; i++)
                {
                    if (!Children[i].IsValidBTreeNode())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
