using System;
using UnityEngine;

namespace Anais {

    public class MinHeap {

        private const int INITIAL_CAPACITY = 5;

        private Node[] queue;
        private int size;

        /// <summary>
        /// Constructs a new empty minimum heap.
        /// </summary>
        public MinHeap() {
            queue = new Node[INITIAL_CAPACITY];
            size = 0;
        }

        /// <summary>
        /// Clears the heap for later reuse, but does not reset the capacity.
        /// </summary>
        public void Clear() {
            for (int i = 1; i <= size; i++) {
                queue[i] = null;
            }
            size = 0;
        }

        /// <summary>
        /// Add a new node to the heap.
        /// </summary>
        /// <param name="node"></param>
        public void Add(Node node) {
            size++;
            Grow(size + 1);
            queue[size] = node;
            node.Index = size;
            BubbleUp(size);
        }

        /// <summary>
        /// Peek at the first (lowest cost node) at the heap.
        /// </summary>
        /// <returns>The lowest cost node</returns>
        public Node Peek() {
            return queue[1];
        }

        /// <summary>
        /// Peek, and remove the lowest cost node.
        /// </summary>
        /// <returns>The lowest cost node</returns>
        public Node Poll() {
            Node minNode = Peek();
            queue[1] = queue[size];
            queue[1].Index = 1;
            queue[size] = null;
            size--;
            BubbleDown(1);
            return minNode;
        }

        /// <summary>
        /// Try to move the node at this index "up" as much as possible.
        /// </summary>
        /// <param name="i"></param>
        public void Decrease(int i) {
            BubbleUp(i);
        }

        /// <summary>
        /// Check if the heap is empty.
        /// </summary>
        /// <returns>True, if the heap is empty</returns>
        public bool Empty() {
            return size == 0;
        }

        /// <summary>
        /// Gets the parent index of i.
        /// </summary>
        /// <param name="i"></param>
        /// <returns>The parent index of i</returns>
        private int Parent(int i) {
            return i / 2;
        }

        /// <summary>
        /// Gets the left child index of i.
        /// </summary>
        /// <param name="i"></param>
        /// <returns>The left child index of i</returns>
        private int LeftChild(int i) {
            return i * 2;
        }

        /// <summary>
        /// Gets the right child index of i.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int RightChild(int i) {
            return (i * 2) + 1;
        }

        /// <summary>
        /// Check if i has a left child in the heap.
        /// </summary>
        /// <param name="i"></param>
        /// <returns>True if i has a left child</returns>
        private bool HasLeftChild(int i) {
            return LeftChild(i) <= size;
        }

        /// <summary>
        /// Check if i has a right child in the heap.
        /// </summary>
        /// <param name="i"></param>
        /// <returns>True if i has a right child</returns>
        private bool HasRightChild(int i) {
            return RightChild(i) <= size;
        }

        /// <summary>
        /// Try to move i up as much as possible (bubbling up the node).
        /// </summary>
        /// <param name="i"></param>
        private void BubbleUp(int i) {
            while (i > 1 && queue[Parent(i)].TotalCost > queue[i].TotalCost) {
                Swap(i, Parent(i));
                i = Parent(i);
            }
        }

        /// <summary>
        /// Try to move i down as much as possible (bubbling down the node).
        /// </summary>
        /// <param name="i"></param>
        private void BubbleDown(int i) {
            int swapWith = i;
            int current = 0;
            while (current != swapWith) {
                current = swapWith;
                if (HasLeftChild(current) && 
                    queue[LeftChild(current)].TotalCost < queue[current].TotalCost) {

                    swapWith = LeftChild(current);
                }
                if (HasRightChild(current) && 
                    queue[RightChild(current)].TotalCost < queue[swapWith].TotalCost) {

                    swapWith = RightChild(current);
                }
                if (current != swapWith) {
                    Swap(current, swapWith);
                }
            }
        }

        /// <summary>
        /// Swap two nodes at i and j indices.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private void Swap(int i, int j) {
            Node temp1 = queue[i];
            Node temp2 = queue[j];
            queue[j] = temp1;
            temp1.Index = j;
            queue[i] = temp2;
            temp2.Index = i;
        }

        /// <summary>
        /// Grow the heap, if its capacity is too low.
        /// </summary>
        /// <param name="minCapacity"></param>
        /// <returns></returns>
        public bool Grow(int minCapacity) {
            if (queue.Length < minCapacity) {
                int newCapacity = queue.Length * 2;
                Node[] newQueue = new Node[newCapacity];
                Array.Copy(queue, newQueue, size);
                queue = newQueue;
                Debug.Log("MinHeap: growth occoured to " + newCapacity);
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Check for heap errors. Useful if "something" funky happens.
        /// </summary>
        public void CheckForErrors() {
            for (int i = 1; i <= size; i++) {
                if (queue[i].Index != i) {
                    Debug.Log("MinHeap: index error at " + i);
                }
            }
            for (int i = 1; i <= size; i++) {
                if (HasLeftChild(i) && queue[i].TotalCost > queue[LeftChild(i)].TotalCost) {
                    Debug.Log("MinHeap: invalid structure at " + i + " (left child)");
                }
                if (HasRightChild(i) && queue[i].TotalCost > queue[RightChild(i)].TotalCost) {
                    Debug.Log("MinHeap: invalid structure at " + i + " (right child)");
                }
            }
        }

    }

}
