using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LightingSettings;

[ExecuteInEditMode]
public class LightingSource2D : LightingMonoBehaviour{
	public enum LightSprite {Default, Custom};
	public enum WhenInsideCollider {DrawInside = 1, DrawAbove = 0}; // Draw Bellow / Do Not Draw
	public enum LitMode {Everything, MaskOnly}

	// Settings
	public int lightPresetId = 0;
	public int nightLayer = 0;

	public Color color = new Color(.5f, .5f, .5f, 1);

	public float size = 5f;
	public float coreSize = 0.5f;
	public float spotAngle = 360;
	public float outerAngle = 15;
	public LitMode litMode = LitMode.Everything;
	
	public bool applyRotation = false;

	public LightingSourceTextureSize textureSize = LightingSourceTextureSize.px2048;

	public MeshMode meshMode = new MeshMode();
	public BumpMap bumpMap = new BumpMap();

	public WhenInsideCollider whenInsideCollider = WhenInsideCollider.DrawInside;

	public LightSprite lightSprite = LightSprite.Default;
	public Sprite sprite;
	public bool spriteFlipX = false;
	public bool spriteFlipY = false;
	
	public LightingSourceTransform transform2D = new LightingSourceTransform();
	public LightEventHandling eventHandling = new LightEventHandling();

	[System.Serializable]
	public class LightEventHandling {
		public bool enable = false;

		public bool useColliders = true;
		public bool useTilemapColliders = false;

		public EventHandling.Object eventHandlingObject = new EventHandling.Object();
	}

	// Internal
	private List<LightingCollider2D> collidersInside = new List<LightingCollider2D>();
	private List<LightingCollider2D> collidersInsideRemove = new List<LightingCollider2D>();

	private static List<LightingSource2D> list = new List<LightingSource2D>();	
	private bool inScreen = false;
	private LightingBuffer2D buffer = null;
	private static Sprite defaultSprite = null;

	public LightingBuffer2D Buffer {
		get => buffer;
		set => buffer = value;
	}

	public void AddCollider(LightingCollider2D id) {
		if (collidersInside.Contains(id)) {
			if (eventHandling.enable) {
				if (id.lightOnEnter != null) {
					id.lightOnEnter.Invoke(this);
				}
			}
			
			collidersInside.Add(id);
		}
	}

	[System.Serializable]
	public class BumpMap {
		public float intensity = 1;
		public float depth = 1;
	}

	public Rect GetWorldRect() {
		return(new Rect(transform2D.position.x - size, transform2D.position.y - size, size * 2, size * 2));
	}

	public LayerSetting[] GetLayerSettings() {
		LightPresetList presetList = Lighting2D.Profile.lightPresets;
		
		if (lightPresetId >= presetList.list.Length) {
			return(null);
		}

		LightPreset lightPreset = presetList.Get()[lightPresetId];

		return(lightPreset.layerSetting.Get());
	}

	static public Sprite GetDefaultSprite() {
		if (defaultSprite == null || defaultSprite.texture == null) {
			defaultSprite = Resources.Load <Sprite> ("Sprites/gfx_light");
		}
		return(defaultSprite);
	}

	public Sprite GetSprite() {
		if (sprite == null || sprite.texture == null) {
			sprite = GetDefaultSprite();
		}
		return(sprite);
	}

	public void ForceUpdate() {
		transform2D.ForceUpdate();
	}

	static public void ForceUpdateAll() {
		foreach(LightingSource2D light in LightingSource2D.GetList()) {
			light.ForceUpdate();
		}
	}

	public void OnEnable() {
		list.Add(this);

		LightingManager2D.Get();

		collidersInside.Clear();

		transform2D.ForceUpdate();

		ForceUpdate();
	}

	public void OnDisable() {
		list.Remove(this);

		Free();
	}

	public void Free() {
		Buffers.FreeBuffer(buffer);

		inScreen = false;
	}

	static public List<LightingSource2D> GetList() {
		return(list);
	}

	public bool InAnyCamera() {
		LightingManager2D manager = LightingManager2D.Get();
		CameraSettings[] cameraSettings = manager.cameraSettings;

		Rect lightRect = GetWorldRect();

		for(int i = 0; i < cameraSettings.Length; i++) {
			Camera camera = manager.GetCamera(i);

			if (camera == null) {
				continue;
			}

			Rect cameraRect = CameraHelper.GetWorldRect(camera);

			if (cameraRect.Overlaps(lightRect)) {
				return(true);
			}
		}

		return(false);
	}

	public LightingBuffer2D GetBuffer() {
		if (buffer == null) { //?
			int textureSizeInt = LightingRender2D.GetTextureSize(textureSize);
			buffer = Buffers.PullBuffer (textureSizeInt, this);
		}
		
		return(buffer);
	}

	public void UpdateLoop() {
		transform2D.Update(this);

		UpdateBuffer();

		DrawMeshMode();

		if (eventHandling.enable) {
			eventHandling.eventHandlingObject.Update(this, eventHandling.useColliders, eventHandling.useTilemapColliders);
		}
	}

	void BufferUpdate() {
		transform2D.UpdateNeeded = false;

		if (Lighting2D.disable == true) {
			return;
		}

		if (buffer == null) {
			return;
		}
		
		buffer.updateNeeded = true;
	}

	void UpdateCollidersInside() {
		foreach(LightingCollider2D collider in collidersInside) {
			if (collider == null) {
				collidersInsideRemove.Add(collider);
				continue;
			}

			if (collider.isActiveAndEnabled == false) {
				collidersInsideRemove.Add(collider);
				continue;
			}

			if (collider.InLightSource(this) == false) {
				collidersInsideRemove.Add(collider);
			}
		}

		foreach(LightingCollider2D collider in collidersInsideRemove) {
			collidersInside.Remove(collider);
			
			transform2D.UpdateNeeded = true;

			if (eventHandling.enable) {
				if (collider != null) {
					if (collider.lightOnExit != null) {
						collider.lightOnExit.Invoke(this);
					}
				}
			}
		}

		collidersInsideRemove.Clear();
	}

	void UpdateBuffer() {
		UpdateCollidersInside();
		
		if (InAnyCamera()) {
			if (GetBuffer() == null) {
				return;
			}

			if (transform2D.UpdateNeeded == true || inScreen == false) {
				BufferUpdate();

				inScreen = true;
			}
			
		} else {
			if (buffer != null) {
				Buffers.FreeBuffer(buffer);
			}
			
			inScreen = false;
		}
		
	}

	public void DrawMeshMode() {
		if (meshMode.enable == false) {
			return;
		}

		if (buffer == null) {
			return;
		}

		if (isActiveAndEnabled == false) {
			return;
		}

		if (InAnyCamera() == false) {
			return;
		}
		
		LightingMeshRenderer lightingMesh = MeshRendererManager.Pull(this);
		
		if (lightingMesh != null) {
			lightingMesh.UpdateLightSource(this, meshMode);
		}	
	}

	void OnDrawGizmosSelected() {
		if (Lighting2D.ProjectSettings.sceneView.drawGizmos != LightingSettings.EditorView.DrawGizmos.Selected) {
			return;
		}
		
		Draw();
    }

	private void OnDrawGizmos() {
		if (Lighting2D.ProjectSettings.sceneView.drawGizmos == LightingSettings.EditorView.DrawGizmos.Disabled) {
			return;
		}
		
		Gizmos.DrawIcon(transform.position, "light", true);

		if (Lighting2D.ProjectSettings.sceneView.drawGizmos != LightingSettings.EditorView.DrawGizmos.Always) {
			return;
		}

		Draw();
	}

	void Draw() {
		if (isActiveAndEnabled == false) {
			return;
		}
		
		Gizmos.color = new Color(1f, 0.5f, 0.25f);

		GizmosHelper.DrawCircle(transform.position, transform2D.rotation, spotAngle, size);
		
		switch(Lighting2D.ProjectSettings.sceneView.drawGizmosBounds) {
			case LightingSettings.EditorView.GizmosBounds.Rectangle:
				GizmosHelper.DrawRect(transform.position, GetWorldRect());
			break;
		}
	}
}