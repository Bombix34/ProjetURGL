using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DayLighting;
using LightingShape;

[System.Serializable]
public class DayLightingColliderShape {
	public DayLightingCollider2D.ShadowType shadowType = DayLightingCollider2D.ShadowType.SpriteCustomPhysicsShape;
    
	public DayLightingCollider2D.MaskType maskType = DayLightingCollider2D.MaskType.Sprite;
    
	public Transform transform;
   
    public DayLightingColliderTransform transform2D = new DayLightingColliderTransform();

    public SpriteShape spriteShape = new SpriteShape();
    public SpriteCustomPhysicsShape spriteCustomPhysicsShape = new SpriteCustomPhysicsShape();
	public ColliderShape colliderShape = new ColliderShape();

    public float height = 1;
    public ShadowMesh shadowMesh = new ShadowMesh();

    public void SetTransform(Transform t) {
        transform = t;

        transform2D.SetShape(this);

        spriteShape.SetTransform(t);
        spriteCustomPhysicsShape.SetTransform(t);
		
		colliderShape.SetTransform(t);
    }

    public void ResetLocal() {
		spriteShape.ResetLocal();
		spriteCustomPhysicsShape.ResetLocal();

		colliderShape.ResetLocal();
	}

    public void ResetWorld() {
		spriteCustomPhysicsShape.ResetWorld();

		colliderShape.ResetWorld();
	}

	public List<Polygon2D> GetPolygonsLocal() {
		switch(shadowType) {
			case DayLightingCollider2D.ShadowType.SpriteCustomPhysicsShape:
				return(spriteCustomPhysicsShape.GetPolygonsLocal());

			case DayLightingCollider2D.ShadowType.Collider:
				return(colliderShape.GetPolygonsLocal());

		}

		return(null);
	}

    public List<Polygon2D> GetPolygonsWorld() {
		switch(shadowType) {
			case DayLightingCollider2D.ShadowType.SpriteCustomPhysicsShape:
				return(spriteCustomPhysicsShape.GetPolygonsWorld());

			case DayLightingCollider2D.ShadowType.Collider:
				return(colliderShape.GetPolygonsWorld());
		}

		return(null);
	}
}
