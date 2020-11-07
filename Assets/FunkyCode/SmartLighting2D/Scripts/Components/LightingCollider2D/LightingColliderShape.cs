﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingShape;

[System.Serializable]
public class LightingColliderShape {
	public LightingCollider2D.ShadowType shadowType = LightingCollider2D.ShadowType.SpriteCustomPhysicsShape;
	public LightingCollider2D.MaskType maskType = LightingCollider2D.MaskType.Sprite;
	public float shadowDistance = 0;

	public ColliderShape colliderShape = new ColliderShape();
	public CompositeShape compositeShape = new CompositeShape();

	public SpriteShape spriteShape = new SpriteShape();
	public SpriteCustomPhysicsShape spriteCustomPhysicsShape = new SpriteCustomPhysicsShape();
	
	public MeshShape meshShape = new MeshShape();
	public SkinnedMeshShape skinnedMeshShape = new SkinnedMeshShape();
	
	public LightingColliderTransform transform2D = new LightingColliderTransform();
	public Transform transform;
	
	public void SetTransform(Transform setTransform) {
		transform = setTransform;

		transform2D.SetShape(this);

		spriteShape.SetTransform(transform);
		spriteCustomPhysicsShape.SetTransform(transform);

		colliderShape.SetTransform(transform);
		compositeShape.SetTransform(transform);

		meshShape.SetTransform(transform);
		skinnedMeshShape.SetTransform(transform);
	}

	public void ResetLocal() {
		spriteShape.ResetLocal();
		spriteCustomPhysicsShape.ResetLocal();

		colliderShape.ResetLocal();
		compositeShape.ResetLocal();

		meshShape.ResetLocal(); 
		skinnedMeshShape.ResetLocal(); 

		ResetWorld();
	}

	public void ResetWorld() {
		spriteShape.ResetWorld();
		spriteCustomPhysicsShape.ResetWorld();

		colliderShape.ResetWorld();
		compositeShape.ResetWorld();

		meshShape.ResetWorld();
		skinnedMeshShape.ResetWorld();
	}

	public bool IsEdgeCollider() {
		switch(shadowType) {
			case LightingCollider2D.ShadowType.Collider2D:
				return(colliderShape.edgeCollider2D);
		}
		
		return(false);
	}

	public Rect GetWorldRect() {
		switch(shadowType) {
			case LightingCollider2D.ShadowType.SpriteCustomPhysicsShape:
				return(spriteCustomPhysicsShape.GetWorldRect());
				
			case LightingCollider2D.ShadowType.Collider2D:
				return(colliderShape.GetWorldRect());

			case LightingCollider2D.ShadowType.CompositeCollider2D:
				return(compositeShape.GetWorldRect());

			// case LightingCollider2D.shadowType.Mesh:
            // case LightingCollider2D.shadowType.SkinnedMesh:
		}

		switch(maskType) {
			case LightingCollider2D.MaskType.Sprite:
			case LightingCollider2D.MaskType.BumpedSprite:
				return(spriteShape.GetWorldRect());
		}

		return(new Rect());
	}

	public float GetRadiusWorld() {
		float sx = Mathf.Abs(transform.lossyScale.x);
		float sy = Mathf.Abs(transform.lossyScale.y);

		float multiplier = Mathf.Max(sx, sy);

		switch(shadowType) {
			case LightingCollider2D.ShadowType.SpriteCustomPhysicsShape:
				return(spriteCustomPhysicsShape.GetRadius() * multiplier);
				
			case LightingCollider2D.ShadowType.Collider2D:
				return(colliderShape.GetRadius() * multiplier);

			case LightingCollider2D.ShadowType.CompositeCollider2D:
				return(compositeShape.GetRadius() * multiplier);

			// case LightingCollider2D.shadowType.Mesh:
            // case LightingCollider2D.shadowType.SkinnedMesh:
		}

		switch (maskType) {
			case LightingCollider2D.MaskType.BumpedSprite:
            case LightingCollider2D.MaskType.Sprite:
				return(spriteShape.GetRadius() * multiplier);

			case LightingCollider2D.MaskType.SpriteCustomPhysicsShape:
                return (spriteCustomPhysicsShape.GetRadius() * multiplier);

			case LightingCollider2D.MaskType.Collider2D:
				return(colliderShape.GetRadius() * multiplier);

			//case LightingCollider2D.MaskType.CompositeCollider:
			//	return(compositeShape.GetRadius() * multiplier);
        }

		return(1000f);
	}
	
	public List<MeshObject> GetMeshes() {
		switch(maskType) {
			case LightingCollider2D.MaskType.SpriteCustomPhysicsShape:
				return(spriteCustomPhysicsShape.GetMeshes());

			case LightingCollider2D.MaskType.Collider2D:
				return(colliderShape.GetMeshes());
		
			case LightingCollider2D.MaskType.CompositeCollider2D:
				return(compositeShape.GetMeshes());
				
			case LightingCollider2D.MaskType.MeshRenderer:
				return(meshShape.GetMeshes());

			case LightingCollider2D.MaskType.SkinnedMeshRenderer:
				return(skinnedMeshShape.GetMeshes());

		}
	
		return(null);
	}

	public List<Polygon2D> GetPolygonsLocal() {
		switch(shadowType) {
			case LightingCollider2D.ShadowType.SpriteCustomPhysicsShape:
				return(spriteCustomPhysicsShape.GetPolygonsLocal());

			case LightingCollider2D.ShadowType.Collider2D:
				return(colliderShape.GetPolygonsLocal());

			case LightingCollider2D.ShadowType.CompositeCollider2D:
				return(compositeShape.GetPolygonsLocal());

			case LightingCollider2D.ShadowType.MeshRenderer:
				return(null);

			case LightingCollider2D.ShadowType.SkinnedMeshRenderer:
				return(null);
		}

		return(null);
	}

	public List<Polygon2D> GetPolygonsWorld() {
		switch(shadowType) {
			case LightingCollider2D.ShadowType.SpriteCustomPhysicsShape:
				return(spriteCustomPhysicsShape.GetPolygonsWorld());

			case LightingCollider2D.ShadowType.Collider2D:
				return(colliderShape.GetPolygonsWorld());

			case LightingCollider2D.ShadowType.CompositeCollider2D:
				return(compositeShape.GetPolygonsWorld());

			case LightingCollider2D.ShadowType.MeshRenderer:
				return(meshShape.GetPolygonsWorld());
				
			case LightingCollider2D.ShadowType.SkinnedMeshRenderer:
				return(skinnedMeshShape.GetPolygonsWorld());
		}

		return(null);
	}
}