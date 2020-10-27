using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using LightingSettings;
using EventHandling;
using UnityEngine.Events;

[ExecuteInEditMode]
public class LightingCollider2D : MonoBehaviour {
	public enum ShadowType {None, SpriteCustomPhysicsShape, Collider2D, CompositeCollider2D, MeshRenderer, SkinnedMeshRenderer};
	public enum MaskType {None, Sprite, BumpedSprite,  SpriteCustomPhysicsShape, Collider2D, CompositeCollider2D, MeshRenderer, SkinnedMeshRenderer};

	public ShadowType shadowType = LightingCollider2D.ShadowType.SpriteCustomPhysicsShape;
	public int shadowLayer = 0;
	public int shadowEffectLayer = 0;
	public float shadowDistance = 0;

	public MaskType maskType = LightingCollider2D.MaskType.Sprite;
	public int maskLayer = 0;
	public MaskEffect maskEffect = MaskEffect.Lit;

	public LightingColliderShape mainShape = new LightingColliderShape();
	public List<LightingColliderShape> shapes = new List<LightingColliderShape>();

	public SpriteMeshObject spriteMeshObject = new SpriteMeshObject();

	public BumpMapMode bumpMapMode = new BumpMapMode();

	public bool applyToChildren = false;

	public event CollisionEvent2D collisionEvents;
	public bool usingEvents = false;

	public LightEvent lightOnEnter;
	public LightEvent lightOnExit;

	// List Manager 
	public static List<LightingCollider2D> list = new List<LightingCollider2D>();
	public static List<LightingCollider2D> listEventReceivers = new List<LightingCollider2D>();
	public static LayerManager<LightingCollider2D> layerManagerMask = new LayerManager<LightingCollider2D>();
	public static LayerManager<LightingCollider2D> layerManagerCollision = new LayerManager<LightingCollider2D>();
	public static LayerManager<LightingCollider2D> layerManagerEffect = new LayerManager<LightingCollider2D>();
	
	private int listMaskLayer = -1;
	private int listCollisionLayer = -1;
	private int listEffectLayer = -1;

	public void AddEventOnEnter(UnityAction<LightingSource2D> call) {
		 if (lightOnEnter == null) {
			 lightOnEnter = new LightEvent();
		 }

        lightOnEnter.AddListener(call);
	}

	public void AddEventOnExit(UnityAction<LightingSource2D> call) {
		 if (lightOnExit == null) {
			 lightOnExit = new LightEvent();
		 }

        lightOnExit.AddListener(call);
	}

	public void AddEvent(CollisionEvent2D collisionEvent) {
		collisionEvents += collisionEvent;

		listEventReceivers.Add(this);

		usingEvents = true;
	}

	public static void ForceUpdateAll() {
		foreach (LightingCollider2D lightCollider2D in LightingCollider2D.GetList()) {
			lightCollider2D.Initialize();
		}
	}
	
	private void OnEnable() {
		list.Add(this);
		UpdateLayerList();

		LightingManager2D.Get();

		Initialize();

		UpdateNearbyLights();
	}

	private void OnDisable() {
		list.Remove(this);
		ClearLayerList();
		
		UpdateNearbyLights();
	}

	private void OnDestroy() {
		if (listEventReceivers.Contains(this)) {
			listEventReceivers.Remove(this);
		}
	}

	public void Update() {
		UpdateLayerList();
	}





	// Layer List
	void ClearLayerList() {
		layerManagerMask.Remove(listMaskLayer, this);
		layerManagerCollision.Remove(listCollisionLayer, this);
		layerManagerEffect.Remove(listEffectLayer, this);

		listMaskLayer = -1;
		listCollisionLayer = -1;
		listEffectLayer = -1;
	}

	void UpdateLayerList() {
		listMaskLayer = layerManagerMask.Update(listMaskLayer, maskLayer, this);
		listCollisionLayer = layerManagerCollision.Update(listCollisionLayer, shadowLayer, this);
		listEffectLayer = layerManagerEffect.Update(listEffectLayer, shadowEffectLayer, this);
	}

	static public List<LightingCollider2D> GetMaskList(int layer) {
		return(layerManagerMask.layerList[layer]);
	}

	static public List<LightingCollider2D> GetCollisionList(int layer) {
		return(layerManagerCollision.layerList[layer]);
	}
		static public List<LightingCollider2D> GetEffectList(int layer) {
		return(layerManagerEffect.layerList[layer]);
	}
	
	static public List<LightingCollider2D> GetList() {
		return(list);
	}
	











	public void CollisionEvent(LightCollision2D collision) {
		if (collisionEvents != null) {
			collisionEvents (collision);
		}
	}

	public bool InLightSource(LightingSource2D light) {
		float radius = mainShape.GetRadiusWorld();

		if (Vector2.Distance (light.transform2D.position, mainShape.transform2D.position) > radius + light.size) {
			return(false);
		}

		Rect lightRect = light.GetWorldRect();
		Rect colliderRect = mainShape.GetWorldRect();

		return(lightRect.Overlaps(colliderRect));
	}

	public bool InLightMesh(LightMesh2D light) {
		Rect lightRect = light.GetWorldRect();
		Rect colliderRect = mainShape.GetWorldRect();

		return(lightRect.Overlaps(colliderRect));
	}


	public void UpdateNearbyLights() {
		foreach (LightingSource2D id in LightingSource2D.GetList()) {
			if (DrawOrNot(id) == false) {
				continue;
			}

			if (InLightSource(id)) {
				id.ForceUpdate();
			}
		}

		foreach (LightMesh2D id in LightMesh2D.GetList()) {
			if (shadowLayer != (int)id.lightLayer) {
				continue;
			}
	
			if (InLightMesh(id)) {
				id.ForceUpdate();
			}
		}
	}

	 private void AddChildShapes(Transform parent) {
        foreach (Transform child in parent) {
			LightingColliderShape shape = new LightingColliderShape();
			shape.maskType = mainShape.maskType;
			shape.shadowType = mainShape.shadowType;
			shape.shadowDistance = mainShape.shadowDistance;
			
			shape.SetTransform(child);
			shape.transform2D.Update();
			
			shapes.Add(shape);

			AddChildShapes(child);
        }
    }


	public void Initialize() {
		shapes.Clear();

		mainShape.maskType = maskType;
		mainShape.shadowType = shadowType;
		mainShape.shadowDistance = shadowDistance;

		mainShape.SetTransform(transform);
		mainShape.transform2D.Reset();
		mainShape.transform2D.Update();
		mainShape.transform2D.UpdateNeeded = true;

		shapes.Add(mainShape);

		if (applyToChildren) {
			AddChildShapes(transform);
		}

		foreach(LightingColliderShape shape in shapes) {
			shape.ResetLocal();
		}
	}

	public bool DrawOrNot(LightingSource2D id) {
		LayerSetting[] layerSetting = id.GetLayerSettings();

		if (layerSetting == null) {
			return(false);
		}

		for(int i = 0; i < layerSetting.Length; i++) {
			if (layerSetting[i] == null) {
				continue;
			}

			int layerID = (int)layerSetting[i].layerID;
			
			switch(layerSetting[i].type) {
				case LightingLayerType.ShadowAndMask:
					if (layerID == shadowLayer || layerID == maskLayer) {
						return(true);
					}
				break;

				case LightingLayerType.MaskOnly:
					if (layerID == maskLayer) {
						return(true);
					}
				break;

				case LightingLayerType.ShadowOnly:
					if (layerID  == shadowLayer) {
						return(true);
					}
				break;
			}
		}

		return(false);
	}

	public void UpdateLoop() {
		bool updateLights = false;

		foreach(LightingColliderShape shape in shapes) {
			shape.transform2D.Update();

			if (shape.transform2D.UpdateNeeded) {
				shape.transform2D.UpdateNeeded = false;
				
				shape.ResetWorld();

				updateLights = true;
			}
		}

		if (updateLights) {
			UpdateNearbyLights();
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
		if (isActiveAndEnabled == false) {
			return;
		}

		Gizmos.color = new Color(1f, 0.5f, 0.25f);
		
		if (mainShape.shadowType != LightingCollider2D.ShadowType.None) {
			foreach(LightingColliderShape shape in shapes) {
				List<Polygon2D> polygons = shape.GetPolygonsWorld();
				
				GizmosHelper.DrawPolygons(polygons, transform.position);
			}
		}

		switch(Lighting2D.ProjectSettings.sceneView.drawGizmosBounds) {
			case LightingSettings.EditorView.GizmosBounds.Rectangle:
				GizmosHelper.DrawRect(transform.position, mainShape.GetWorldRect());
			break;

			case LightingSettings.EditorView.GizmosBounds.Radius:
				GizmosHelper.DrawCircle(transform.position, 0, 360, mainShape.GetRadiusWorld());
			break;
		}
	}
}