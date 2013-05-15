using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace Sunset.Data.Integration
{
    public static class ParallelCommon
    {
        public static ConcurrentStack<T> ToConcurrentStack<T>(this IEnumerable<T> Content)
        {
            ConcurrentStack<T> Stack = new ConcurrentStack<T>();

            foreach (T Value in Content)
                Stack.Push(Value);

            return Stack;
        }

        public static ConcurrentQueue<T> ToConcurrentQueue<T>(this IEnumerable<T> Content)
        {
            ConcurrentQueue<T> Queue = new ConcurrentQueue<T>();

            foreach (T Value in Content)
                Queue.Enqueue(Value);

            return Queue;
        }
    }
}