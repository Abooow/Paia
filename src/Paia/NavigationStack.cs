using System.Collections.Generic;
using System.Linq;

namespace Paia
{
    public class NavigationStack<T>
    {
        public T Current => backStack.Peek();

        public IEnumerable<T> BackStack => backStack.AsEnumerable();
        public IEnumerable<T> FrontStack => frontStack.AsEnumerable();

        private Stack<T> backStack;
        private Stack<T> frontStack;

        public NavigationStack()
        {
            backStack = new Stack<T>();
            frontStack = new Stack<T>();
        }

        public void Push(T item)
        {
            backStack.Push(item);
            frontStack.Clear();
        }

        public void Forward()
        {
            backStack.Push(frontStack.Pop());
        }

        public void Back()
        {
            frontStack.Push(backStack.Pop());
        }
    }
}
