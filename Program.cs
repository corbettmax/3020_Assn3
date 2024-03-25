using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointMe
{
    //-------------------------------------------------------------------------

    // Implementation:  2^d branching

    class PointQuadTree
    {

        // node class for a point quadtree

        private class Node
        {
            // Read/write properties

            public int[] Item;
            public Node[] Branches;

            public Node(int[] item, int degree)
            {
                Item = item;
                Branches = new Node[(int)Math.Pow(2, degree)]; // 2^d branches
            }
        }

        private Node root;

        public PointQuadTree()
        {
            root = null;    // Empty tree

        }

        // Add 
        // Insert an item into a point quadtree
        // Duplicate items are not inserted
        // Worst case time complexity:  O(log n) 
        // since the maximum depth of a red-black tree is O(log n)

        public void Add(int[] item)
        {
            Node curr;
            bool inserted = false;
            if (root == null)
            {
                root = new Node(item, item.Length);
                return; //successful insert of first item in an empty tree
            }

            if (item.Length != root.Item.Length)
            {
                throw new ArgumentOutOfRangeException("all items must be of equal degrees");
                return;
            }

            curr = root;
            while(curr != null)
            {
                //Strategy for selecting branch is to mimic the behaviour of a logical decoder
                int branchSelect = 0;
                for(int i = 0; i < (item.Length); i++)
                {
                    //calculate the branch to select by calculating the concatenated binary value of all comparisons to current item
                    if (item[i] > curr.Item[i])
                         branchSelect+=(int)Math.Pow(2, (item.Length-1-i)); // if true, a[i] = 1 so to find the decimal value add 2^i
                }

                if (curr.Branches[branchSelect] == null)
                {
                    curr.Branches[branchSelect] = new Node(item, item.Length); //position found, add node
                    break;
                }
                else
                {
                    curr = curr.Branches[branchSelect]; //follow path to leaf
                }
            }
            return;


        }

        public void Print()
        {
            Print(root, 0, 0, "root ");                // Call private, recursive Print
            Console.WriteLine();
        }

        // Print
        // Inorder traversal of the tree

        private void Print(Node node, int k, int indent, string prefix)
        {
            string s;
            string t = new string(' ', indent);

            if (node != null)
            {
                for (int i = (node.Branches.Length-1); i > (node.Branches.Length/2); i--)
                    Print(node.Branches[i], i, indent + 8, "/");

                Console.Write(t + prefix + "[");
                for (int j = 0; j < (node.Item.Length); j++)
                {
                    Console.Write(node.Item[j].ToString()+",");
                }
                Console.Write("] on branch "+ k +'\n');

                
                for (int i = (node.Branches.Length/2); i > 0; i--)
                    Print(node.Branches[i], i, indent + 8, "\\");
            }
        }

    }

    //-----------------------------------------------------------------------------

    public class Program
    {
        static void Main(string[] args)
        {
            PointQuadTree tree = new PointQuadTree();

            int[][] testValues =
            {
                new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                new int[10] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 },
                new int[10] { 3, 5, 1, 3, 5, 7, 8, 3, 2, 1, },
                new int[10] { 1, 1, 2, 3, 20, 1, 2, 4, 9, 1},
                new int[10] { 4, 7, 1, 2, 4, 8, 9, 2, 1, 6}
            };

            tree.Add(testValues[0]);

            Console.WriteLine("\n\n\n Print #1");
            tree.Print();
            

            tree.Add(testValues[1]);

            Console.WriteLine("\n\n\n Print #2");
            tree.Print();
            

            tree.Add(testValues[2]);

            Console.WriteLine("\n\n\n Print #3");
            tree.Print();
            

            tree.Add(testValues[3]);

            Console.WriteLine("\n\n\n Print #4");
            tree.Print();


            tree.Add(testValues[4]);

            Console.WriteLine("\n\n\n Print #5");
            tree.Print();

            Console.ReadLine();
        }
    }
}