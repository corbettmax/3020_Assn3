using System;
using System.Collections.Generic;

namespace part2
{
    public class LazyBinomialHeap<T> where T : IComparable<T>
    {
        private List<BinomialNode<T>>[] lazyHeap; // Array of lists of binomial trees
        private BinomialNode<T> highestPrio; // Reference to the item with highest priority
        private int size; // Size of the heap

        public class BinomialNode<T>
        {
            public T Item { get; set; }
            public int Degree { get; set; }
            public BinomialNode<T> LeftMostChild { get; set; }
            public BinomialNode<T> RightSibling { get; set; }

            public BinomialNode(T item)
            {
                Item = item;
                Degree = 0;
                LeftMostChild = null;
                RightSibling = null;
            }
        }

        // Constructor
        public LazyBinomialHeap()
        {
            // Creating a new lazyHeap that supports up to 2^16 elements (we weren't sure what degree to support up to so we just chose 16).
            lazyHeap = new List<BinomialNode<T>>[17];
            highestPrio = null;
            size = 0;
        }

        // Add(T item)
        // Creates a new binomial tree of degree 0 with the new item in it, then adds it to the List at lazyHeap[0].
        public void Add(T item)
        {
            BinomialNode<T> newNode = new BinomialNode<T>(item);
            if (highestPrio == null || item.CompareTo(highestPrio.Item) > 0)
            {
                highestPrio = newNode;
            }

            if (lazyHeap[0] == null)
            {
                lazyHeap[0] = new List<BinomialNode<T>>();
            }

            lazyHeap[0].Add(newNode);

            size++;
        }

        // Front()
        // Returns the item at the front of the queue (the item with the highest priority in the heap right now).
        public T Front()
        {
            if (highestPrio != null)
            {
                return highestPrio.Item;
            }
            else
            {
                Console.WriteLine("The heap is empty.");
                return default(T);
            }
        }

        // Remove()
        // Removes the highest priority item from the binomial heap, then calls coalesce to merge all trees that have the same degree and update
        // the highestPrio so that Front() can maintain O(1).
        public void Remove()
        {
            if (!Empty())
            {
                BinomialNode<T> prev = null;
                BinomialNode<T> curr = highestPrio;

                if (highestPrio.RightSibling == null)
                {
                    for (int i = 0; i < lazyHeap[0].Count; i++)
                    {
                        if (lazyHeap[0][i] == highestPrio)
                        {
                            lazyHeap[0].RemoveAt(i);
                            size--;
                        }
                    }
                    Coalesce();
                }
                else
                {
                    for (int i = 0; i < lazyHeap.Length; i++)
                    {
                        if (lazyHeap[i] != null)
                        {
                            foreach (var node in lazyHeap[i])
                            {
                                if (node.RightSibling == curr)
                                {
                                    prev = node;
                                    break;
                                }
                            }
                        }
                    }
                    if (prev != null && curr != null)
                    {
                        prev.RightSibling = curr.RightSibling;
                        size--;

                        // Coalesce to maintain heap properties
                        Coalesce();
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Heap is empty.");
            }
        }

        // Print()
        // Traverses every tree and prints out each node, organized by overall tree degree and further by individual node degree.
        public void Print()
        {
            for (int i = 0; i < lazyHeap.Length; i++)
            {
                if (lazyHeap[i] != null && lazyHeap[i].Count > 0)
                {
                    Console.WriteLine("BINOMIAL TREE(S) OF DEGREE " + i + ":");
                    foreach (var tree in lazyHeap[i])
                    {
                        int degCount = 0;
                        BinomialNode<T> leftNode = lazyHeap[i][0];
                        BinomialNode<T> currNode = lazyHeap[i][0];
                        while (leftNode != null)
                        {
                            Console.WriteLine("NODES OF DEGREE " + degCount);
                            while (currNode != null)
                            {
                                Console.WriteLine(" - " + currNode.Item);
                                currNode = currNode.RightSibling;
                            }
                            leftNode = leftNode.LeftMostChild;
                            currNode = leftNode;
                            degCount++;
                            Console.Write("\n"); 
                        }
                    }
                }
            }
        }



        // Coalesce()
        // Loops through each tree in each index of lazyHeap[], combining trees that exist in the same degree
        // and moving them to the appropriate index of lazyHeap[] after they are combined. Continues until
        // there is a maximum of one tree at each Degree index.
        // Then, loops through each node and updates the highestPrio node accordingly so that Front can maintain O(1).
        private void Coalesce()
        {
            BinomialNode<T> newPrio = null;

            for (int i = 0; i < lazyHeap.Length; i++)
            {
                if (lazyHeap[i] != null)
                {
                    while (lazyHeap[i].Count > 1)
                    {
                        BinomialNode<T> root1 = lazyHeap[i][0];
                        BinomialNode<T> root2 = lazyHeap[i][1];
                        if (root1.Item.CompareTo(root2.Item) < 0)
                        {
                            BinomialNode<T> newTree = root2;
                            root1.RightSibling = newTree.LeftMostChild;
                            newTree.LeftMostChild = root1;

                            newTree.Degree++;
                            if (lazyHeap[newTree.Degree] == null)
                            {
                                lazyHeap[newTree.Degree] = new List<BinomialNode<T>>();
                            }
                            lazyHeap[newTree.Degree].Add(newTree);
                        }
                        else
                        {
                            BinomialNode<T> newTree = root1;
                            root2.RightSibling = newTree.LeftMostChild;
                            newTree.LeftMostChild = root2;

                            newTree.Degree++;
                            if (lazyHeap[newTree.Degree] == null)
                            {
                                lazyHeap[newTree.Degree] = new List<BinomialNode<T>>();
                            }
                            lazyHeap[newTree.Degree].Add(newTree);
                        }
                        lazyHeap[i].RemoveAt(1);
                        lazyHeap[i].RemoveAt(0);
                    }
                }
            }

            // Loops through every root node of each tree and re-assigns the value of the highest priority item
            // to highestPrio
            for (int i = 0; i < lazyHeap.Length; i++)
            {
                if (lazyHeap[i] != null)
                {
                    for (int j = 0; j < lazyHeap[i].Count; j++)
                    {
                        if (newPrio == null || lazyHeap[i][j].Item.CompareTo(newPrio.Item) > 0)
                        {
                            newPrio = lazyHeap[i][j];
                        }
 
                    }
                }
            }

            highestPrio = newPrio;
        }


        // Empty()
        // Returns whether the heap is empty or not.
        public bool Empty()
        {
            return size == 0;
        }
    }

    // PriorityClass
    // Class implemented so that the characters and integers in the 'item' of each Node can be compared and sorted correctly.
    public class PriorityClass : IComparable<PriorityClass>
    {
        private int priorityValue;
        private char letter;

        public PriorityClass(int priority, char letter)
        {
            this.letter = letter;
            priorityValue = priority;
        }

        public int CompareTo(PriorityClass other)
        {
            return priorityValue - other.priorityValue;
        }

        public override string ToString()
        {
            return letter.ToString() + " with priority " + priorityValue;
        }
    }

    //public class Test
    //{
    //    public static void Main(string[] args)
    //    {
    //        int i;
    //        Random r = new Random();

    //        LazyBinomialHeap<PriorityClass> BH = new LazyBinomialHeap<PriorityClass>();
    //        //for (i = 0; i < 65; i++)
    //        //{
    //        //    BH.Add(new PriorityClass(r.Next(50), (char)r.Next('a', 'z' + 1)));
    //        //}
    //        //BH.Remove();
    //        //Console.WriteLine("First item removed.");
    //        //BH.Print();
    //        //Console.WriteLine("Item at the front of the queue is " + BH.Front());
    //        BH.Front();
    //        Console.ReadLine();
    //    }
    //}
}
