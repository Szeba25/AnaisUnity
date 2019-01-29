using System;
using UnityEngine;

namespace Anais {

    public class MinHeap {

        private const int INITIAL_CAPACITY = 5;

        private Node[] queue;
        private int size;

        public MinHeap() {
            queue = new Node[INITIAL_CAPACITY];
            size = 0;
        }

        public void Clear() {
            for (int i = 1; i <= size; i++) {
                queue[i] = null;
            }
            size = 0;
        }

        public void Add(Node node) {
            size++;
            Grow(size + 1);
            queue[size] = node;
            node.Index = size;
            BubbleUp(size);
        }

        public Node Peek() {
            return queue[1];
        }

        public Node Poll() {
            Node minNode = Peek();
            queue[1] = queue[size];
            queue[1].Index = 1;
            queue[size] = null;
            size--;
            BubbleDown(1);
            return minNode;
        }

        public void Decrease(int i) {
            BubbleUp(i);
        }

        public bool Empty() {
            return size == 0;
        }

        private int Parent(int i) {
            return i / 2;
        }

        private int LeftChild(int i) {
            return i * 2;
        }

        private int RightChild(int i) {
            return (i * 2) + 1;
        }

        private bool HasLeftChild(int i) {
            return LeftChild(i) <= size;
        }

        private bool HasRightChild(int i) {
            return RightChild(i) <= size;
        }

        private void BubbleUp(int i) {
            while (i > 1 && queue[Parent(i)].TotalCost > queue[i].TotalCost) {
                Swap(i, Parent(i));
                i = Parent(i);
            }
        }

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

        private void Swap(int i, int j) {
            Node temp1 = queue[i];
            Node temp2 = queue[j];
            queue[j] = temp1;
            temp1.Index = j;
            queue[i] = temp2;
            temp2.Index = i;
        }

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
