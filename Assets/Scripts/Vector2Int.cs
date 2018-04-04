using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Vector2Int {

	private int x;
	private int y;

	public int X {
		get {
			return this.x;
		}
	}

	public int Y {
		get {
			return this.y;
		}
	}

	public Vector2Int (int x, int y)
	{
		this.x = x;
		this.y = y;
	}
	
}
