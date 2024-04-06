using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Part3
{

    //-------------------------------------------------------------------------

    // Implementation:  TwoThreFourTree

    class TwoThreeFourTree<T> where T : IComparable
    {

        // Common generic node class for a BSTfor234Tree

        private class Node<T>
        {
            // Read/write properties

            public int n; //num of keys
            public bool leaf; //true if leaf node, otherwise false
            public T[] key; //array of keys
            public Node<T>[] c; //array of child references

            public Node()
            {
                n = 0;
                leaf = true;
                key = new T[3];
                c = new Node<T>[4];

            }
        }

        private Node<T> root; //root node

        public TwoThreeFourTree() //which initializes an empty 2-3-4 tree. (2 marks)
        {
            root = new Node<T>();    // Empty 2-3-4 tree
        }

        public bool Insert(T k) // which returns true if key k is successfully inserted; false otherwise. (6 marks)
        {
            Node<T> p = root;
            int index;
            while (p != null)
            {
                if (p.key.Contains(k))
                {
                    return false;
                }
                else
                {
                    if (root.key.Count() != 3) //make sure the root is not full
                    {
                        index = 0;
                        for (int i = 0; i < p.key.Count(); i++) //find where k would be in the tree
                        {
                            if (k.CompareTo(p.key[i]) == 1)
                            {

                                index = i + 1; //move p along while k is greater than the key being compared
                            }
                            else
                            {
                                break; //break if it's found its spot
                            }
                        }
                        if (p.c[index].key.Count() == 3) //if proper child is full don't descend
                        {


                        }
                        else
                        {
                            p = p.c[index]; //descend to the proper child
                        }
                    }
                    else //root is full so time to make a new root using split
                    {
                        

                    }
                }
            }
            return false;


        }
        public bool Delete(T k) //which returns true if key k is successfully deleted; false otherwise. (10 marks)
        {

        }

        //O(log n)
        public bool Search(T k) //which returns true if key k is found; false otherwise(4 marks).
        {
            Node<T> p = root;
            Node<T> curr;
            while (p != null)
            {
                if (p.key.Contains(k))
                {
                    return true;
                }
                else
                {
                    if (p.leaf == true)
                    {
                        return false;
                    }
                    else
                    {
                        curr = p.c[0];
                        for(int i=0; i<p.key.Count(); i++) //find where k would be in the tree
                        {
                            if (k.CompareTo(p.key[i]) == 1) 
                            {
                                curr=p.c[i+1]; //move p along while k is greater than the key being compared
                            }
                            else
                            {
                                break; //break if it's found its spot
                            }
                        }
                        p = curr;

                    }
                }
            }
            return false;

        }
        public BSTforRBTree<T> Convert() //which builds and returns the equivalent red-black tree.For this assignment, the red-black tree is represented as an instance of the class BSTforRBTree. The code for (8 marks)
        {

        }
        public void Print() //which prints out the keys of the 2-3-4 tree in order. (4 marks)
        {

        }

        private void Split(Node<T> x, int i) //support method that splits the ith full child of x into 2 nodes
        {

        }

        
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
