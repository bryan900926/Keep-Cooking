using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    private readonly List<(T item, float priority)> heap = new List<(T, float)>();
    private readonly Func<T, bool> inValid;

    public int Count
    {
        get
        {
            CleanInvalidTop();
            return heap.Count;
        }
    }
    public PriorityQueue(Func<T, bool> inValid = null)
    {
        this.inValid = inValid ?? (_ => false);
    }

    public void Enqueue(T item, float priority)
    {
        heap.Add((item, priority));
        HeapifyUp(heap.Count - 1);
    }

    public T Peek()
    {
        CleanInvalidTop();
        if (heap.Count == 0)
            throw new InvalidOperationException("PriorityQueue is empty.");
        return heap[0].item;
    }

    public T Dequeue()
    {
        CleanInvalidTop();
        if (heap.Count == 0)
            return default;

        T root = heap[0].item;
        heap[0] = heap[^1];
        heap.RemoveAt(heap.Count - 1);
        if (heap.Count > 0)
            HeapifyDown(0);

        return root;
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parent = (index - 1) / 2;
            if (heap[index].priority >= heap[parent].priority)
                break;

            (heap[index], heap[parent]) = (heap[parent], heap[index]);
            index = parent;
        }
    }

    private void HeapifyDown(int index)
    {
        int lastIndex = heap.Count - 1;
        while (true)
        {
            int left = 2 * index + 1;
            int right = 2 * index + 2;
            int smallest = index;

            if (left <= lastIndex && heap[left].priority < heap[smallest].priority)
                smallest = left;
            if (right <= lastIndex && heap[right].priority < heap[smallest].priority)
                smallest = right;

            if (smallest == index)
                break;

            (heap[index], heap[smallest]) = (heap[smallest], heap[index]);
            index = smallest;
        }
    }

    // ⚡ Lazy cleanup: removes invalid (destroyed) objects from the top
    private void CleanInvalidTop()
    {
        while (heap.Count > 0 && inValid(heap[0].item))
        {
            heap[0] = heap[^1];
            heap.RemoveAt(heap.Count - 1);
            if (heap.Count > 0)
                HeapifyDown(0);
        }
    }
}
