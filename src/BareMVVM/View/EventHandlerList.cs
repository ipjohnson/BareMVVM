using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BareMVVM.View
{
	public sealed class EventHandlerList : IList<EventHandlerInstance>
	{
		private readonly List<EventHandlerInstance> list = new List<EventHandlerInstance>();

		public IEnumerator<EventHandlerInstance> GetEnumerator()
		{
			return list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)list).GetEnumerator();
		}

		public void Add(EventHandlerInstance item)
		{
			list.Add(item);
		}

		public void Clear()
		{
			list.Clear();
		}

		public bool Contains(EventHandlerInstance item)
		{
			return list.Contains(item);
		}

		public void CopyTo(EventHandlerInstance[] array, int arrayIndex)
		{
			list.CopyTo(array, arrayIndex);
		}

		public bool Remove(EventHandlerInstance item)
		{
			return list.Remove(item);
		}

		public int Count
		{
			get { return list.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public int IndexOf(EventHandlerInstance item)
		{
			return list.IndexOf(item);
		}

		public void Insert(int index, EventHandlerInstance item)
		{
			list.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			list.RemoveAt(index);
		}

		public EventHandlerInstance this[int index]
		{
			get { return list[index]; }
			set { list[index] = value; }
		}
	}
}