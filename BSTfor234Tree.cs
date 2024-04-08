
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Linq;

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

        // Insert(T k)
        // Purpose: Inserts the key, k, into the tree according to the 2-3-4
        // Tree Insertion algorithm. If successfully inserted, returns true;
        // otherwise, returns false.
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
                    if (p.n != 3) //make sure the root is not full
                    {
                        if (!p.leaf)
                        {
                            index = 0;
                            for (int i = 0; i < p.n; i++) //find where k would be in the tree
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

                            if (p.c[index].n == 3) //if proper child is full don't descend
                            {
                                Split(p, index);
                            }
                            else
                            {
                                p = p.c[index];
                            }

                        }

                        else
                        {

                            p.key[p.n] = k; //insert k into leaf node's array
                            p.n++;

                            for (int i = 0; i < p.n; i++) //sort the keys of the leaf node
                            {
                                for (int j = 0; j < (p.n - 1); j++)
                                {
                                    if (p.key[j].CompareTo(p.key[j + 1]) == 1)
                                    {
                                        T temp = p.key[j];
                                        p.key[j] = p.key[j + 1];
                                        p.key[j + 1] = temp;

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

        // Delete(T k)
        // Purpose: Searches through the tree and deletes the key, k, according
        // to the 2-3-4 Tree deletion algorithm. If key, k, is successfully
        // deleted returns true; otherwise, returns false.
        public bool Delete(T k) //which returns true if key k is successfully deleted; false otherwise. (10 marks)
        {
            if (Search(k) == false)
            {
                return false;
            }
            Node<T> p = root;
            Node<T> curr;
            while (p != null)
            {
                if (p.key.Contains(k))
                {
                    if (p.leaf == false) // The Node containing k is an internal node
                    {
                        T successor = default(T);
                        int index = 0;
                        for (int i = 0; i < p.key.Count(); i++) // find the index of the node we want to delete
                            if (k.CompareTo(p.key[i]) == 0)
                                index = i;

                        Node<T> temp = p.c[index];
                        // Find the Successor value that will be replacing the deleted value
                        while (temp != null)
                        {
                            if (temp.leaf == true)
                            {
                                for (int i = temp.key.Count() - 1; i >= 0; i--)
                                    if (k.CompareTo(temp.key[i]) > 0)
                                    {
                                        successor = temp.key[i];
                                        temp.key[i] = default(T);
                                        temp.n--;
                                        break;
                                    }
                                break;
                            }
                            curr = temp.c[0];
                            for (int i = 0; i < temp.key.Count(); i++)
                            {
                                if (k.CompareTo(temp.key[i]) == 1)
                                    curr = temp.c[i + 1];
                                else
                                    break;
                            }
                            temp = curr;
                        }
                        p.key[index] = successor;
                        return true;
                    }
                    else // The Node containing k is a leaf node
                    {
                        int index = 0;
                        for (int i = 0; i < p.key.Count(); i++) // find the index of the node we want to delete
                            if (k.CompareTo(p.key[i]) == 0)
                                index = i;
                        p.key[index] = default(T);
                        p.n--;
                        return true;
                    }
                }
                else
                {
                    int currentChildIndex = 0;
                    curr = p.c[0];
                    for (int i = 0; i < p.n + 1; i++) //find where k would be in the tree
                    {
                        if (k.CompareTo(p.key[i]) == 1)
                        {
                            curr = p.c[i + 1]; //move p along while k is greater than the key being compared
                        }
                        else
                        {
                            i = currentChildIndex;
                            break; //break if it's found its spot
                        }
                    }
                    // Due to overflow, if we encounter a 2-Node we have to go through the cases.
                    if (p.n == 1)
                    {
                        // Case 1: Either of the Siblings are 3-Node / 4-Node
                        if (p.c[currentChildIndex + 1].n > 1 || p.c[currentChildIndex + 1].n > 1)
                        {
                            // Rotate on adjacent sibling
                            T parentKey = p.key[0];
                            p.key[0] = p.c[currentChildIndex + 1].key[0];
                            curr.key[1] = parentKey;
                            curr.n++;
                            curr.c[1] = p.c[currentChildIndex + 1].c[0];

                            p = curr;
                        }
                        // Case 2: Parent is 2-Node and the Sibling is 2-Node.
                        else if (p.n == 1 && p.c[currentChildIndex + 1].n == 1)
                        {
                            // Merge the Parent Node, the Next Node, and the Sibling of the Next Node together.
                            Node<T> sibling = p.c[currentChildIndex + 1];

                            p.n = 3;
                            // Add the Parent Node
                            p.key[1] = p.key[0];
                            // Add the Node we are entering
                            p.key[0] = curr.key[0];
                            p.c[0] = curr.c[0];
                            p.c[1] = curr.c[1];
                            // Add the Sibling to the right of the leaf node
                            p.key[2] = sibling.key[0];
                            p.c[2] = sibling.c[0];
                            p.c[3] = sibling.c[1];
                        }
                        // Case 3: Siblings are 2-Node and Parent is 3-Node / 4-Node
                        else if ((p.n == 2 || p.n == 3)
                            && ((p.c[currentChildIndex + 1].n == 1) && (p.c[currentChildIndex + 2].n == 1)))
                        {
                            // Merge the adjacent sibling nodes and the parent key
                            p.c[currentChildIndex + 1].n = 3;
                            // Add the parent key overlooking the siblings to the New 4-Node
                            p.c[currentChildIndex + 1].key[1] = p.key[1];
                            // Merge the adjacent sibling with the other adjacent sibling
                            p.c[currentChildIndex + 1].key[2] = p.c[currentChildIndex + 2].key[0];
                            p.c[currentChildIndex + 1].c[2] = p.c[currentChildIndex + 2].c[0];
                            p.c[currentChildIndex + 1].c[3] = p.c[currentChildIndex + 2].c[1];

                            p = curr;
                        }

                    }
                    else
                        p = curr;

                }
            }
            return true;
        }

        // Search(T k)
        // Purpose: Searches through the tree for key, k, and returns true if
        // found; otherwise, returns false.
        //O(log n)
        public bool Search(T k)
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
                        for (int i = 0; i < p.n; i++) //find where k would be in the tree
                        {
                            if (k.CompareTo(p.key[i]) == 1)
                            {
                                curr = p.c[i + 1]; //move p along while k is greater than the key being compared
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
        // Convert()
        // Purpose: Wrapper function which builds and returns the equivalent
        // Red-Black Tree reperesentation of the 2-3-4 Tree.
        public BSTforRBTree<T> Convert()
        {
            BSTforRBTree<T> redBlack = new BSTforRBTree<T>();
            Convert(ref redBlack, root);
            return redBlack;

        }

        // Convert(ref BSTforRBTree<T> tree, Node<T> node)
        // Purpose: Private method that recursively converts each node into the
        // form of a Red-Black Tree.
        private void Convert(ref BSTforRBTree<T> tree, Node<T> node)
        {
            if (node != null)
            {
                if (node.n == 1)
                {
                    tree.Add(node.key[0], Color.BLACK);
                }
                else if (node.n == 2)
                {
                    tree.Add(node.key[0], Color.BLACK); //insert black first
                    tree.Add(node.key[1], Color.RED);

                }
                else
                {
                    tree.Add(node.key[1], Color.BLACK); //insert black first
                    tree.Add(node.key[0], Color.RED);
                    tree.Add(node.key[2], Color.RED);
                }

                for (int i = 0; i < 4; i++)
                {
                    Convert(ref tree, node.c[i]);
                }
            }

        }


        // Print()
        // Purpose: Wrapper function that prints out the keys of the 2-3-4 Tree
        // in order.
        public void Print()
        {
            PrintNode(root, 0); // call private, recursive Print
            Console.WriteLine();

        }

        // Print(Node<T> node, int indent)
        // Purpose: Recursive function that prints a Node with a certain amount
        // of indent space depending on where in the tree the node is
        private void PrintNode(Node<T> node, int indent) //private recursive print method
        {
            string s;
            string t = new string(' ', indent);

            if (node != null)
            {
                for (int j = 0; j < (node.n); j++)
                {
                    PrintNode(node.c[j + 1], indent + 8);
                    Console.WriteLine(t + node.key[j].ToString() + ",");
                    PrintNode(node.c[j], indent + 8);
                }
            }
        }

        // Split(Node<T> x, int i)
        // Purpose: Support method that splits the ith full child of x into 2 nodes
        private void Split(Node<T> x, int i)
        {
            if (i == -1) // -1 is passed to i when we want to split the root
            {
                Node<T> newRoot = new Node<T>();
                newRoot.key[0] = root.key[1];
                newRoot.n++;
                Node<T> newRootChildA = new Node<T>();
                newRootChildA.key[0] = root.key[0];
                newRootChildA.n++;
                Node<T> newRootChildB = new Node<T>();
                newRootChildB.key[0] = root.key[2];
                newRootChildB.n++;


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
                    newRootChildA.leaf = false;
                    newRootChildB.leaf = false;
                    for (int j = 0; j < 2; j++)
                    {
                        newRootChildA.c[j] = root.c[j];
                        newRootChildB.c[j] = root.c[j + 2];
                    }
                }
                root = newRoot;

            }

            else
            {
                Node<T> newRootChildA = new Node<T>();
                newRootChildA.key[0] = x.c[i].key[0];
                newRootChildA.n++;
                Node<T> newRootChildB = new Node<T>();
                newRootChildB.key[0] = x.c[i].key[2];
                newRootChildB.n++;

                if (x.c[i].leaf) //set the two new nodes to leafs if the node being split is a leaf
                {
                    newRootChildA.leaf = true;
                    newRootChildB.leaf = true;
                }
                else
                {
                    newRootChildA.leaf = false;
                    newRootChildB.leaf = false;
                    for (int j = 0; j < 2; j++) //reattach children of the target node to the newly split nodes
                    {
                        newRootChildA.c[j] = x.c[i].c[j];
                        newRootChildB.c[j] = x.c[i].c[3 - j];
                    }
                }

                if (x.n == 3)
                {
                    throw new Exception("Split called on a node whose parent is full !");
                }

                if (i >= 2)
                {
                    x.key[2] = x.c[i].key[1];
                    x.n++;

                }
                else if (i == 1)
                {
                    T temp = x.key[1];
                    x.key[2] = temp;
                    x.key[1] = x.c[i].key[1];
                    x.n++;
                }
                else
                {
                    T temp = x.key[1];
                    x.key[2] = temp;
                    x.key[1] = x.key[0];
                    x.key[0] = x.c[i].key[1];
                    x.n++;

                }

                x.c[i] = newRootChildA;
                x.c[i + 1] = newRootChildB;

                return;
            }
        }
    }

    //-----------------------------------------------------------------------------

    public class Program
    {
        static void Main(string[] args)
        {
            TwoThreeFourTree<int> tree = new TwoThreeFourTree<int>();


            tree.Insert(6);

            //Console.WriteLine("\n\n\n Print #1");
            //tree.Print();


            tree.Insert(7);
            //Console.WriteLine("\n\n\n Print #2");
            //tree.Print();
            tree.Insert(2);
            //Console.WriteLine("\n\n\n Print #3");
            //tree.Print();
            tree.Insert(4);
            //Console.WriteLine("\n\n\n Print #4");
            //tree.Print();
            tree.Insert(8);
            //Console.WriteLine("\n\n\n Print #5");
            //tree.Print();
            tree.Insert(10);
            //Console.WriteLine("\n\n\n Print #6");
            //tree.Print();
            tree.Insert(17);
            //Console.WriteLine("\n\n\n Print #7");
            //tree.Print();
            tree.Insert(20);
            //Console.WriteLine("\n\n\n Print #8");
            //tree.Print();
            tree.Insert(1);
            //Console.WriteLine("\n\n\n Print #9");
            //tree.Print();
            tree.Insert(9);
            //Console.WriteLine("\n\n\n Print #10");
            //tree.Print();
            tree.Insert(12);

            Console.WriteLine("\n\n\n Print #1:");
            tree.Print();

            Console.WriteLine("\n\n\n Post Delete:");
            tree.Delete(6);
            tree.Print();

            BSTforRBTree<int> rbTree = tree.Convert();
            Console.WriteLine("\n\n\n Print after conversion");
            rbTree.Print();


            Console.ReadLine();
        }
    }
}
