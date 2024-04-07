using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace part2
{
    public class BinomialNode<T>
    {
        public T Item { get; set; }
        public int Degree { get; set; }
        public BinomialNode<T> LeftMostChild { get; set; }
        public BinomialNode<T> RightSibling { get; set; }

        // Constructor

        public BinomialNode(T item)
        {
            Item = item;
            Degree = 0;
            LeftMostChild = null;
            RightSibling = null;
        }
    }

    //--------------------------------------------------------------------------------------

    // Common interface for all non-linear data structures

    public interface IContainer<T>
    {
        void MakeEmpty();  // Reset an instance to empty
        bool Empty();      // Test if an instance is empty
        int Size();        // Return the number of items in an instance
    }

    //--------------------------------------------------------------------------------------

    public interface ILazyHeap<T> : IContainer<T> where T : IComparable
    {
        void Add(T item);               // Add an item to a binomial heap
        void Remove();                  // Remove the item with the highest priority
        T Front();                      // Return the item with the highest priority
        void Merge(LazyHeap<T> H);  // Merge H with the current binomial heap
    }

    //--------------------------------------------------------------------------------------

    // Binomial Heap
    // Implementation:  Leftmost-child, right-sibling

    public class LazyHeap<T> where T : IComparable
    {
        private BinomialNode<T> head;  // Head of the root list
        private BinomialNode<T> highestPriorityNode;
        private int size;              // Size of the binomial heap

        // Contructor
        // Time complexity:  O(1)
        public LazyHeap()
        {
            head = new BinomialNode<T>(default(T));   // Header node
            size = 0;
        }

        // Add
        // Inserts an item into the binomial heap
        // Time complexity:  O(1)
        public void Add(T item)
        {
            BinomialNode<T> H = new BinomialNode<T>(item);
            if (highestPriorityNode == null || item.CompareTo(highestPriorityNode.Item) > 0)
            {
                highestPriorityNode = H;
            }

            H.RightSibling = head.RightSibling;
            head.RightSibling = H;

            size++;
        }

        // Front
        // Returns the item with the highest priority
        // Time complexity:  O(1)
        public T Front()
        {
            if (highestPriorityNode != null)
            {
                return highestPriorityNode.Item;
            }
            else
            {
                Console.WriteLine("The heap is empty.");
                return default(T);
            }
        }


        // Remove
        // Removes the item with the highest priority from the binomial heap
        // Time complexity:  O(log n)
        public void Remove()
        {
            if (!Empty())
            {
                BinomialNode<T> prev = head;
                BinomialNode<T> curr = head.RightSibling;

                while (curr != null && curr != highestPriorityNode)
                {
                    prev = curr;
                    curr = curr.RightSibling;
                }

                if (curr == highestPriorityNode)
                {
                    prev.RightSibling = curr.RightSibling;
                    highestPriorityNode = null;

                    BinomialNode<T> temp = head.RightSibling;
                    while (temp != null)
                    {
                        if (highestPriorityNode == null || temp.Item.CompareTo(highestPriorityNode.Item) > 0)
                        {
                            highestPriorityNode = temp;
                        }
                        temp = temp.RightSibling;
                    }

                    size--;

                    Coalesce();
                }
            }
            else
            {
                throw new InvalidOperationException("Heap is empty.");
            }
        }

        // Print
        // Prints the items in the binomial heap
        public void Print()
        {
            BinomialNode<T> curr = head.RightSibling;

            while (curr != null)
            {
                Console.WriteLine(curr.Item);
                curr = curr.RightSibling;
            }
        }

        private void Coalesce()
        {
            List<BinomialNode<T>> binomialTrees = new List<BinomialNode<T>>();

            BinomialNode<T> prev = head;
            BinomialNode<T> curr = head.RightSibling;
            while (curr != null)
            {
                int degree = curr.Degree;

                while (binomialTrees.Count <= degree)
                {
                    binomialTrees.Add(null);
                }

                while (binomialTrees[degree] != null)
                {
                    BinomialNode<T> other = binomialTrees[degree];

                    if (curr.Item.CompareTo(other.Item) > 0)
                    {
                        BinomialNode<T> temp = curr;
                        curr = other;
                        other = temp;
                    }

                    other.RightSibling = curr.LeftMostChild;
                    curr.LeftMostChild = other;
                    curr.Degree++;

                    prev.RightSibling = other.RightSibling;
                    other.RightSibling = null;

                    if (prev.RightSibling == null)
                    {
                        break;
                    }
                    else
                    {
                        prev = prev.RightSibling;
                    }
                }

                binomialTrees[degree] = curr;
                curr = curr.RightSibling;
            }

            head.RightSibling = null;
            foreach (BinomialNode<T> tree in binomialTrees)
            {
                if (tree != null)
                {
                    tree.RightSibling = head.RightSibling;
                    head.RightSibling = tree;
                }
            }
        }

        // Degrees
        // Prints the degree for each binomial tree in the root list
        // Time complexity:  O(log n)
        public void Degrees()
        {
            BinomialNode<T> p = head.RightSibling;

            while (p != null)
            {
                Console.WriteLine(p.Degree);
                p = p.RightSibling;
            }
        }

        // MakeEmpty
        // Creates an empty binomial heap
        // Time complexity:  O(1)
        public void MakeEmpty()
        {
            head.RightSibling = null;
            size = 0;
        }

        // Empty
        // Returns true is the binomial heap is empty; false otherwise
        // Time complexity:  O(1)
        public bool Empty()
        {
            return size == 0;
        }

        // Size
        // Returns the number of items in the binomial heap
        // Time complexity:  O(1)
        public int Size()
        {
            return size;
        }
    }

    public class PriorityClass : IComparable
    {
        private int priorityValue;
        private char letter;

        public PriorityClass(int priority, char letter)
        {
            this.letter = letter;
            priorityValue = priority;
        }

        public int CompareTo(Object obj)
        {
            PriorityClass other = (PriorityClass)obj;
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

            LazyHeap<PriorityClass> BH = new LazyHeap<PriorityClass>();

            for (i = 0; i < 20; i++)
            {
                BH.Add(new PriorityClass(r.Next(50), (char)r.Next('a', 'z' + 1)));
            }

            Console.WriteLine("Size of heap: " + BH.Size());
            BH.Print();
            Console.ReadLine();
        }
    }
}

