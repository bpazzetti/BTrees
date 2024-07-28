using System.Xml.Linq;

namespace BTrees
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BTree tree = new BTree(5);
            int[] keysToInsert = { 10, 20, 5, 6, 12, 30, 7, 17, 3, 8, 25, 15, 1, 19, 4, 21, 23, 11, 18, 13, 14, 2, 22, 16, 9, 24, 26, 27, 28, 29, 31, 32, 33, 34, 35 };
            if(InsertTree(tree, keysToInsert))
            {
                Console.WriteLine("Traversal of the constructed tree is:");
                tree.Traverse();
                Console.WriteLine();

                // Search Test
                int[] keysToSearch = { 6, 13, 30, 50 };
                foreach (int key in keysToSearch)
                {
                    Console.WriteLine($"\nSearching for key {key}:");
                    var node = tree.Search(key);
                    Console.WriteLine(node != null ? $"Key {key} found in tree." : $"Key {key} not found in tree.");
                }

                int[] keysToDelete = { 6, 13, 7, 4, 2, 16, 10, 5, 17, 20, 12, 30, 3, 8, 25, 15, 1, 19, 21, 23, 11, 18, 14, 22, 9, 24, 26, 27, 28, 29, 31, 32, 33, 34, 35 };
                if(DeleteTree(tree, keysToDelete))
                {
                    Console.WriteLine("BTree Test Succesfull");
                }
            }
                                    
            Console.ReadLine();
        }    
        
        private static bool InsertTree(BTree tree, int[] keys)
        {
            foreach (int key in keys)
            {
                Console.WriteLine($"Inserting {key}...");
                tree.Insert(key);
                Console.WriteLine("Tree structure after insertion:");
                tree.PrintTree();
                if (!tree.IsValidBTree())
                {
                    Console.WriteLine($"After insert Key {key} tree is invalid.");
                    return false;
                }                                
            }
            return true;
        }

        private static bool DeleteTree(BTree tree, int[] keys)
        {
            foreach (int key in keys)
            {
                Console.WriteLine($"Deleting {key}...");
                tree.Delete(key);
                Console.WriteLine("Tree structure after deletion:");
                tree.PrintTree();
                if (!tree.IsValidBTree())
                {
                    Console.WriteLine($"After deleting Key {key} tree is invalid.");
                    return false;
                }
            }
            return true;
        }
    }
}
