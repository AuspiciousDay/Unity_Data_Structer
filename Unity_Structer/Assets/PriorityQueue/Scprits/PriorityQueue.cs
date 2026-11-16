using System;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<TElement, TPriority>
{
    private List<(TElement Element, TPriority Priority)> heap;
    private readonly IComparer<TPriority> comparer;

    public int Count => heap.Count;

    public PriorityQueue()
    {
        heap = new List<(TElement Element, TPriority Priority)>();
        comparer = Comparer<TPriority>.Default;
    }

    public PriorityQueue(IComparer<TPriority> comparer)
    {
        heap = new List<(TElement Element, TPriority Priority)>();
        this.comparer = comparer;
    }

    public PriorityQueue(IEnumerable<(TElement Element, TPriority Priority)> items)
    {
        if(items == null)
        {
            throw new ArgumentNullException();
        }
        heap = new List<(TElement Element, TPriority Priority)>();
        comparer = Comparer<TPriority>.Default;
        foreach (var item in items)
        {
            Enqueue(item.Element, item.Priority);
        }
    }

    public PriorityQueue(IEnumerable<(TElement Element, TPriority Priority)> items, IComparer<TPriority> comparer)
    {
        if(items == null)
        {
            throw new ArgumentNullException();
        }
        // heap = new List<(TElement Element, TPriority Priority)>();
        // this.comparer = comparer;
        // foreach (var item in items)
        // {
        //     Enqueue(item.Element, item.Priority);
        // }

        this.comparer = comparer;
        heap = new List<(TElement Element, TPriority Priority)>(items);
        for(int i = heap.Count/2 - 1 ; i >= 0; i--)
        {
            HeapifyDown(i);
        }
    }

    public void Clear()
    {
        heap.Clear();
    }

    public TElement Peek()
    {
        if (heap.Count == 0)
        {
            throw new InvalidOperationException("큐가 비어 있습니다.");
        }
        return heap[0].Element;
    }

    private void Swap(int i, int j)
    {
        var temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;
    }

    public void Enqueue(TElement element, TPriority priority)
    {
        heap.Add((element, priority));

        int index = heap.Count - 1;
        HeapifyUp(index);
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;

            if (comparer.Compare(heap[index].Priority, heap[parentIndex].Priority) >= 0)
            {
                break;
            }
            Swap(index, parentIndex);
            index = parentIndex;
        }
    }

    public TElement Dequeue()
    {
        if (heap.Count == 0)
        {
            throw new InvalidOperationException("큐가 비어 있습니다.");
        }

        TElement result = heap[0].Element;
        int lastIndex = heap.Count - 1;
        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);

        int index = 0;
        HeapifyDown(index);

        return result;
    }

    private void HeapifyDown(int index)
    {
        int lastIndex = heap.Count - 1;
        while (true)
        {
            int leftChildIndex = 2 * index + 1;
            int rightChildIndex = 2 * index + 2;

            int smallsetIndex = index;
            if (lastIndex >= leftChildIndex && comparer.Compare(heap[leftChildIndex].Priority, heap[smallsetIndex].Priority) < 0)
            {
                smallsetIndex = leftChildIndex;
            }

            if (lastIndex >= rightChildIndex && comparer.Compare(heap[rightChildIndex].Priority, heap[smallsetIndex].Priority) < 0)
            {
                smallsetIndex = rightChildIndex;
            }

            if (index == smallsetIndex)
            {
                break;
            }

            Swap(index, smallsetIndex);
            index = smallsetIndex;
        }
    }
}
