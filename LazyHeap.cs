using System;
using System.Collections.Generic;

namespace part2
{
    public class LazyBinomialHeap<T> where T : IComparable<T>
    {
        private List<BinomialNode<T>>[] lazyHeap; // Array of lists of binomial lazyHeap
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

        // Add
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

        // Front
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

        //Remove
        public void Remove()
        {
            if (!Empty())
            {
                BinomialNode<T> prev = null;
                BinomialNode<T> curr = highestPrio;

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
            else
            {
                throw new InvalidOperationException("Heap is empty.");
            }
        }

        // Print
        public void Print()
        {
            for (int i = 0; i < lazyHeap.Length; i++)
            {
                Console.WriteLine("Binomial lazyHeap of degree " + i + ":");
                if (lazyHeap[i] != null && lazyHeap[i].Count > 0)
                {
                    foreach (var tree in lazyHeap[i])
                    {
                        BinomialNode<T> current = tree;
                        while (current != null)
                        {
                            Console.WriteLine(current.Item);

                            BinomialNode<T> child = current.LeftMostChild;
                            while (child != null)
                            {
                                Console.WriteLine(child.Item);
                                child = child.RightSibling;
                            }

                            current = current.RightSibling;
                        }
                    }
                }
            }
        }


        // Coalesce
        private void Coalesce()
        {
            List<BinomialNode<T>> newlazyHeap = new List<BinomialNode<T>>();

            // Merge lazyHeap with the same degree until no two lazyHeap have the same degree
            foreach (var lazyHeap in lazyHeap)
            {
                if (lazyHeap != null)
                {
                    foreach (var tree in lazyHeap)
                    {
                        BinomialNode<T> currentTree = tree; // Assign tree to a temporary variable

                        int degree = currentTree.Degree;

                        while (newlazyHeap.Count <= degree)
                        {
                            newlazyHeap.Add(null);
                        }

                        while (newlazyHeap[degree] != null)
                        {
                            BinomialNode<T> other = newlazyHeap[degree];

                            if (currentTree.Item.CompareTo(other.Item) > 0)
                            {
                                // Swap the references
                                BinomialNode<T> temp = currentTree;
                                currentTree = other;
                                other = temp;
                            }

                            other.RightSibling = currentTree.LeftMostChild;
                            currentTree.LeftMostChild = other;
                            currentTree.Degree++;

                            newlazyHeap[degree] = null;

                            degree++;
                        }

                        newlazyHeap[degree] = currentTree;
                    }
                }
            }
        }

        // Empty
        public bool Empty()
        {
            return size == 0;
        }
    }

    
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

    public class Test
    {
        public static void Main(string[] args)
        {
            int i;
            Random r = new Random();

            LazyBinomialHeap<PriorityClass> BH = new LazyBinomialHeap<PriorityClass>();

            for (i = 0; i < 20; i++)
            {
                BH.Add(new PriorityClass(r.Next(50), (char)r.Next('a', 'z' + 1)));
            }
            BH.Print();
            Console.WriteLine("The highest priority item is: " + BH.Front());

            BH.Remove();
            BH.Print();
            Console.ReadLine();
        }
    }
}
