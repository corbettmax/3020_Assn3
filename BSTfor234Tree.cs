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
            root = new Node<T>(); // Empty 2-3-4 tree
            root.leaf = true;
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
                        if (!p.leaf)
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
                                Split(p, index);
                            }
                            p = p.c[index];

                        }

                        else
                        {

                            p.key[p.key.Count()] = k; //insert k into leaf node's array

                            for (int i = 0; i < p.key.Count(); i++) //sort the keys of the leaf node
                            {
                                for (int j = 0; j< (p.key.Count()-1); j++)
                                {
                                    if (p.key[j].CompareTo(p.key[j+1]) == 1)
                                    {
                                        T temp = p.key[j];
                                        p.key[j] = p.key[j+1];
                                        p.key[j+1] = temp;

                                    }
                                }
                            }
                            return true;

                        }

                    }
                    else //root is full so time to make a new root using split
                    {
                        Split(root, -1);
                        p = root;
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
            if(i == -1) // -1 is passed to i when we want to split the root
            {
                Node<T> newRoot = new Node<T>();
                newRoot.key[0] = root.key[1];
                Node<T> newRootChildA = new Node<T>();
                newRootChildA.key[0] = root.key[0];
                Node<T> newRootChildB = new Node<T>();
                newRootChildB.key[0] = root.key[2];


                newRoot.leaf = false;
                newRoot.c[0] = newRootChildA;
                newRoot.c[1] = newRootChildB;



                if (root.leaf) //if the root was a leaf its new children are the new leafs
                {
                    newRootChildA.leaf = true;
                    newRootChildB.leaf = true;
                }
                else
                {
                    for(int j = 0; j<2; j++)
                    {
                        newRootChildA.c[j] = root.c[j];
                        newRootChildB.c[j] = root.c[3-j];
                    }
                }
                root = newRoot;

            }

            else
            {
                Node<T> newRootChildA = new Node<T>();
                newRootChildA.key[0] = x.c[i].key[0];
                Node<T> newRootChildB = new Node<T>();
                newRootChildB.key[0] = x.c[i].key[2];

                if (x.c[i].leaf) //set the two new nodes to leafs if the node being split is a leaf
                {
                    newRootChildA.leaf = true;
                    newRootChildB.leaf = true;
                }
                else
                {
                    for (int j = 0; j < 2; j++) //reattach children of the target node to the newly split nodes
                    {
                        newRootChildA.c[j] = x.c[i].c[j];
                        newRootChildB.c[j] = x.c[i].c[3 - j];
                    }
                }

                if (x.key.Count() == 3)
                {
                    throw new Exception("Split called on a node whose parent is full !");
                }

                T temp = x.key[1];
                x.key[2] = temp;
                x.key[1] = x.c[i].key[1];

                x.c[i] = newRootChildA;
                x.c[i + 1] = newRootChildB;

                

                return;

            }


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
