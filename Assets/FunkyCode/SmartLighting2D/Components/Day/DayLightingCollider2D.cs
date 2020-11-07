using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DayLighting;
using LightingSettings;
 
[ExecuteInEditMode]
public class DayLightingCollider2D : MonoBehaviour {
	public enum ShadowType {None, SpriteCustomPhysicsShape, Collider, Sprite}; 
	public enum MaskType {None, Sprite, BumpedSprite};

	public int shadowLayer = 0;
	public int maskLayer = 0;

	public ShadowType shadowType = ShadowType.SpriteCustomPhysicsShape;
	public MaskType maskType = MaskType.Sprite;

	public float shadowDistance = 1;
	
	public DayLightingColliderShape mainShape = new DayLightingColliderShape();
	public List<DayLightingColliderShape> shapes = new List<DayLightingColliderShape>();

	public DayNormalMapMode normalMapMode = new DayNormalMapMode();
	public SpriteMeshObject spriteMeshObject = new SpriteMeshObject();

	public bool applyToChildren = false;

	public static List<DayLightingCollider2D> list = new List<DayLightingCollider2D>();

	public void OnEnable() {
		list.Add(this);

		LightingManager2D.Get();

		Initialize();
	}

	public void OnDisable() {
		list.Remove(this);
	}

	public bool InAnyCamera() {
		LightingManager2D manager = LightingManager2D.Get();
		CameraSettings[] cameraSettings = manager.cameraSettings;

		for(int i = 0; i < cameraSettings.Length; i++) {
			Camera camera = manager.GetCamera(i);

			if (camera == null) {
				continue;
			}

			float distance = Vector2.Distance(transform.position, camera.transform.position);
			float cameraRadius = CameraHelper.GetRadius(camera);
			float radius = cameraRadius + 5; // 5 = Size

			if (distance < radius) {
				return(true);
			}
		}

		return(false);
	}

	static public List<DayLightingCollider2D> GetList() {
		return(list);
	}

	public static void ForceUpdateAll() {
		foreach(DayLightingCollider2D collider in DayLightingCollider2D.GetList()) {
			collider.ForceUpdate();
		}
	}

	public void ForceUpdate() {
		Initialize();

		foreach(DayLightingColliderShape shape in shapes) {
			shape.transform2D.updateNeeded = true;
		}	
	}

    public void UpdateLoop() {
		foreach(DayLightingColliderShape shape in shapes) {
			shape.height = mainShape.height;
			
			shape.transform2D.Update();

			if (shape.transform2D.updateNeeded) {
				shape.transform2D.updateNeeded = false;
				shape.shadowMesh.Generate(shape);
			}
		}	
    }

	public void Initialize() {
		shapes.Clear();

		mainShape.shadowType = shadowType;
		mainShape.maskType = maskType;
		mainShape.height = shadowDistance;

		mainShape.SetTransform(transform);
		mainShape.ResetLocal();

		mainShape.transform2D.Update();
		
		shapes.Add(mainShape);

		if (applyToChildren) {
			foreach(Transform childTransform in transform) {

				DayLightingColliderShape shape = new DayLightingColliderShape();
				shape.maskType = mainShape.maskType;
				shape.shadowType = mainShape.shadowType;
				shape.height = mainShape.height;

				shape.SetTransform(childTransform);
				shape.ResetLocal();

				shape.transform2D.Update();
		
				shapes.Add(shape);

			}
		}

		foreach(DayLightingColliderShape shape in shapes) {
			shape.shadowMesh.Generate(shape);
		}
	}

	void OnDrawGizmosSelected() {
		if (Lighting2D.ProjectSettings.sceneView.drawGizmos != LightingSettings.EditorView.DrawGizmos.Selected) {
			return;
		}
		
		DrawGizmos();
    }

	private void OnDrawGizmos() {
		if (Lighting2D.ProjectSettings.sceneView.drawGizmos != LightingSettings.EditorView.DrawGizmos.Always) {
			return;
		}
		
		DrawGizmos();
	}

	private void DrawGizmos() {
		Gizmos.color = new Color(1f, 0.5f, 0.25f);
		
		if (mainShape.shadowType != DayLightingCollider2D.ShadowType.None) {
			foreach(DayLightingColliderShape shape in shapes) {
				shape.ResetWorld();
			
				List<Polygon2D> polygons = shape.GetPolygonsWorld();

				if (polygons != null) {
					GizmosHelper.DrawPolygons(polygons, transform.position);
				}
			}
		}

		/*
		switch(Lighting2D.ProjectSettings.sceneView.drawGizmosBounds) {
			case LightingSettings.EditorView.GizmosBounds.Rectangle:
				GizmosHelper.DrawRect(transform.position, mainShape.GetWorldRect());
			break;

			case LightingSettings.EditorView.GizmosBounds.Radius:
				GizmosHelper.DrawCircle(transform.position, 0, 360, mainShape.GetRadiusWorld());
			break;
		}*/
	}
}
