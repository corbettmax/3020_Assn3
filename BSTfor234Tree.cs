using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Part3
{

    //-------------------------------------------------------------------------

    // Implementation:  TwoThreFourTree

    class TwoThreeFourTree<T>
    {

        // Common generic node class for a BSTfor234Tree

        private class Node
        {
            // Read/write properties

            public T Item;
            public Color RB;
            public Node Left;
            public Node Right;

            public Node(T item, Color rb)
            {
                Item = item;
                RB = rb;
                Left = Right = null;
            }
        }

        private Node root;

        public TwoThreeFourTree() //which initializes an empty 2-3-4 tree. (2 marks)
        {
            root = null;    // Empty BSTforRBTree
        }

        bool Insert(T k)// which returns true if key k is successfully inserted; false otherwise. (6 marks)
        bool Delete(T k) //which returns true if key k is successfully deleted; false otherwise. (10 marks)
        bool Search(T k) //which returns true if key k is found; false otherwise(4 marks).
        BSTforRBTree<T> Convert() //which builds and returns the equivalent red-black tree.For this assignment, the red-black tree is represented as an instance of the class BSTforRBTree. The code for (8 marks)
        void Print() //which prints out the keys of the 2-3-4 tree in order. (4 marks)

        
    }

    //-----------------------------------------------------------------------------

    public class Program2
    {
        static void Main(string[] args)
        {
            Random randomValue = new Random();       // Random number
            Color c;

            BSTforRBTree<int> B = new BSTforRBTree<int>();
            for (int i = 0; i < 20; i++)
            {
                c = i % 2 == 0 ? Color.RED : Color.BLACK;
                B.Add(randomValue.Next(90) + 10, c); // Add random integers with alternating colours
            }
            B.Print();                               // In order

            Console.ReadLine();
        }
    }
}
