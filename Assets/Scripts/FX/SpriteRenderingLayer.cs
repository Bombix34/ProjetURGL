using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRenderingLayer : MonoBehaviour
{
    ObjectManager manager;
	SpriteRenderer spriteRender;
	public float modificator;
	public bool isParentTransform;

	protected virtual void Start () {
		spriteRender = GetComponent<SpriteRenderer> ();
        if (isParentTransform)
            manager = GetComponentInParent<ObjectManager>();
        else
            manager = GetComponent<ObjectManager>();
	}

	protected virtual void Update ()
    {
        if (!manager.isClient)
            return;
		if(isParentTransform)
			spriteRender.sortingOrder = (int)(((1/transform.parent.position.y) * 1000)+modificator);
		else
			spriteRender.sortingOrder = (int)(((1/transform.position.y) * 1000)+modificator);
	}
}