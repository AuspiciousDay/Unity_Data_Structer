using System;
using System.Collections;
using System.Collections.Generic;

public class CircularDoublyLinkedListNode<T>
{
    public T Value { get; set; }

    public CircularDoublyLinkedListNode<T> Next { get; set; }
    public CircularDoublyLinkedListNode<T> Previous { get; set; }
    public CircularDoublyLinkedList<T> List { get; set; }

    public CircularDoublyLinkedListNode<T> next;
    public CircularDoublyLinkedListNode<T> prev;

    public CircularDoublyLinkedListNode(T value)
    {
        Value = value;
    }
}

public class CircularDoublyLinkedList<T> : IEnumerable<T>
{
    private CircularDoublyLinkedListNode<T> head;

    public int Count { get; private set; }

    public CircularDoublyLinkedListNode<T> First => head;

    public CircularDoublyLinkedListNode<T> Last => head.Previous;

    public IEnumerator<T> GetEnumerator()
    {
        CircularDoublyLinkedListNode<T> current = head;
        while (current != null)
        {
            yield return current.Value;
            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public CircularDoublyLinkedListNode<T> AddFirst(T value)
    {
        var node = new CircularDoublyLinkedListNode<T>(value);
        node.List = this;

        if (head.Previous == null)
        {
            head.Previous = node;
        }
        
        if (head == null)
        {
            head = node;
        }
        else
        {
            node.Next = head;
            head.Previous = node;
            head = node;
        }

        Count++;
        return node;
    }

    public CircularDoublyLinkedListNode<T> AddLast(T value)
    {
        var node = new CircularDoublyLinkedListNode<T>(value);
        node.List = this;

        if (head.Previous == null)
        {
            head = node;
            head.Previous = node;
        }
        else
        {
            head.Previous.Next = node;
            node.Previous = head.Previous;
            head.Previous = node;
        }
        
        Count++;
        return node;
    }

    public CircularDoublyLinkedListNode<T> AddAfter(CircularDoublyLinkedListNode<T> node, T value)
    {
        ValidataNode(node);

        if (node == head.Previous)
        {
            return AddLast(value);
        }
        var newNode = new CircularDoublyLinkedListNode<T>(value);
        newNode.List = this;

        newNode.Next = node.Next;
        newNode.Previous =node;

        node.Next.Previous = newNode;
        node.Next = newNode;

        Count++;
        return newNode;
    }

    public CircularDoublyLinkedListNode<T> AddBefore(CircularDoublyLinkedListNode<T> node, T value)
    {
        ValidataNode(node);

        if (node == head)
        {
            return AddFirst(value);
        }

        var previous = node.Previous;
        var newNode = new CircularDoublyLinkedListNode<T>(value);
        newNode.List = this;

        newNode.Next = node;
        newNode.Previous = previous;

        previous.Next = newNode;
        previous.Previous = newNode;

        Count++;
        return newNode;
    }

    public void RemoveFirst()
    {
        if (head == null)
        {
            throw new InvalidOperationException("리스트가 비어있습니다.");
        }

        Remove(head);
    }

    public void RemoveLast()
    {
        if (head.Previous == null)
        {
            throw new InvalidOperationException("리스트가 비어있습니다.");
        }

        Remove(head.Previous);
    }

    public bool Remove(T value)
    {
        var node = Find(value);
        if (node == null)
        {
            return false;
        }
        Remove(node);
        return true;
    }

    public void Remove(CircularDoublyLinkedListNode<T> node)
    {
        ValidataNode(node);

        if(node.Previous == null)
        {
            head = node.Next;
        }
        else
        {
            node.Previous.Next = node.Next;
        }

        if(node.Next == null)
        {
            head.Previous = node.Previous;
        }
        else
        {
            node.Next.Previous = node.Previous;
        }

        node.Next = null;
        node.Previous = null;
        node.List = null;

        Count--;
    }

    public CircularDoublyLinkedListNode<T> Find(T value)
    {
        var comparer = EqualityComparer<T>.Default;
        CircularDoublyLinkedListNode<T> current = head;

        while (current != null)
        {
            if (comparer.Equals(current.Value, value))
            {
                return current;
            }
            current = current.Next;
        }
        return null;
    }

    public bool Contains(T value)
    {
        return Find(value) != null;
    }

    public void Clear()
    {
        var current = head;
        while (current != null)
        {
            var Next = current.Next;

            current.Next = null;
            if (current.Previous != null)
            {
                current.Previous = null;
            }
            current.List = null;

            current = Next;
        }

        head = null;
        head.Previous = null;
        Count = 0;
    }

    private void ValidataNode(CircularDoublyLinkedListNode<T> node)
    {
        if (node == null)
        {
            throw new ArgumentNullException(nameof(node));
        }
        if (node.List != this)
        {
            throw new InvalidOperationException("이 리스트에 속한 노드가 아닙니다.");
        }
    }

    public IEnumerable<T> Backward()
    {
        CircularDoublyLinkedListNode<T> current = head.Previous;
        while (current != null)
        {
            yield return current.Value;
            current = current.Previous;
        }
    }

    private void InsertBefore(CircularDoublyLinkedListNode<T> target, CircularDoublyLinkedListNode<T> newNode)
    {
        
    }
}
