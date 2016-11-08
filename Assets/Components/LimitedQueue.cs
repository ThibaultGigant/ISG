using UnityEngine;
using System.Collections.Generic;

public class LimitedQueue<T> : Queue<T> {


	public int size { get ; set ;}

	public LimitedQueue (int size){
		this.size = size;
	}

	public void Enqueue(T obj){
		base.Enqueue (obj);
		while (Count > size){
			base.Dequeue ();
		}
	}
}
