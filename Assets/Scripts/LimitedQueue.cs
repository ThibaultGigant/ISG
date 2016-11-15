using UnityEngine;
using System.Collections.Generic;

public class LimitedQueue<T> : Queue<T>
{


	public int size { get ; set ; }

	public LimitedQueue (int size)
	{
		this.size = size;
	}

	public void Enqueue (T obj)
	{
		base.Enqueue (obj);
		while (Count > size) {
			base.Dequeue ();
		}
	}

	public string ToString ()
	{
		string str = "Limited Queue of type <" + typeof(T).Name + "> size : " + Count + "\n";
		int i = 0;
		foreach (T t in this) {
			str += i + " : " + t.ToString () + "\n";
			i++;
		}
		str += "\n";
		return str;
	}
}
