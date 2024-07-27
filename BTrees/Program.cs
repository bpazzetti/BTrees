namespace BTrees
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BTree tree = new BTree(5);

            int[] keysToInsert = { 10, 20, 5, 6, 12, 30, 7, 17, 3, 8, 25, 15, 1, 19, 4, 21, 23, 11, 18, 13, 14, 2, 22, 16, 9, 24, 26, 27, 28, 29, 31, 32, 33, 34, 35 };
            foreach (int key in keysToInsert)
            {
                Console.WriteLine($"Inserting {key}...");
                tree.Insert(key);
                Console.WriteLine("Tree structure after insertion:");
                tree.PrintTree();
            }

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

            Console.ReadLine();
        }
    }
}
