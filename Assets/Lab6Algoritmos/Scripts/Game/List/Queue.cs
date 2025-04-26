using UnityEngine;

public class Queue<T>
{
    public class QueueNode
    {
        #region Properties
        private T value;
        private QueueNode next;
        #endregion

        #region Getters
        public T Value => value;
        public QueueNode Next => next;
        #endregion

        #region Constructors
        public QueueNode(T value)
        {
            this.value = value;
            this.next = null;
        }

        public void SetNext(QueueNode _next)
        {
            this.next = _next;
        }
        #endregion
    }

    private QueueNode head = null;
    private QueueNode last = null;
    private int count = 0;

    public QueueNode Head => head;
    public QueueNode Last => last;
    public int Count => count;

    public virtual void Enqueue(T item)
    {
        QueueNode newNode = new QueueNode(item);
        count++;

        if (head == null)
        {
            head = newNode;
            last = newNode;
        }
        else
        {
            last.SetNext(newNode);
            last = newNode;
        }
    }

    public virtual T Dequeue()
    {
        if (count <= 0 || head == null)
        {
            count = 0;
            return default;
        }

        T value = head.Value;
        head = head.Next;
        count--;

        if (head == null)
        {
            last = null;
        }

        return value;
    }

    public virtual T Peek()
    {
        if (head == null)
            return default;
        return head.Value;
    }

    public virtual void Clear()
    {
        head = null;
        last = null;
        count = 0;
    }
     
    public void ReadQueue(QueueNode _current = null, int depth = 0)
    {
        if (_current == null)
            _current = head;

        if (_current == null)
        {
            Debug.Log("Cola vacía");
            return;
        }

        Debug.Log($"Nodo en posición {depth}: {_current.Value.ToString()}");
        if (_current.Next != null)
            ReadQueue(_current.Next, depth + 1);
    }
}