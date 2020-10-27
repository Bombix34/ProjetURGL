using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LightCollision2D {

	public LightingSource2D light;
	public LightingCollider2D collider;

	public List<Vector2> points;
	public LightingEventState state;

	public LightCollision2D(bool _active) {
		light = null;

		collider = null;
		
		points = null;

		state = LightingEventState.None;
	}
}