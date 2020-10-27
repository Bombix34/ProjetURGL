using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingSettings;

#if UNITY_EDITOR
	using UnityEditor;
#endif

[ExecuteInEditMode] 
public class LightingManager2D : LightingMonoBehaviour {
	private static LightingManager2D instance;

	public CameraSettings[] cameraSettings = new CameraSettings[1];

	public bool debug = false;
	public int version = 0;

	public LightingSettings.Profile setProfile;
    public LightingSettings.Profile profile;

	// Sets Lighting Main Profile Settings for Lighting2D at the start of the scene
	private static bool initialized = false; 

	public Camera GetCamera(int id) {
		if (cameraSettings.Length <= id) {
			return(null);
		}

		return(cameraSettings[id].GetCamera());
	}

	public int GetCameraBufferID(int id) {
		if (cameraSettings.Length <= id) {
			return(0);
		}

		return(cameraSettings[id].bufferID);
	}

	public static void ForceUpdate() {
		// LightingManager2D manager = Get();

		// if (manager != null) {
			// manager.gameObject.SetActive(false);
			// manager.gameObject.SetActive(true);
		// }
	}
	
	static public LightingManager2D Get() {
		if (instance != null) {
			return(instance);
		}

		foreach(LightingManager2D manager in Object.FindObjectsOfType(typeof(LightingManager2D))) {
			instance = manager;
			return(instance);
		}

		// Create New Light Manager
		GameObject gameObject = new GameObject();
		gameObject.name = "Lighting Manager 2D";

		instance = gameObject.AddComponent<LightingManager2D>();
		instance.Initialize();

		return(instance);
	}

	public void Initialize () {
		instance = this;

		transform.position = Vector3.zero;

		version = Lighting2D.VERSION;

		if (cameraSettings == null) {
			cameraSettings = new CameraSettings[1];
			cameraSettings[0] = new CameraSettings();
		}
	}

	public void Awake() {

		if (instance != null && instance != this) {

			switch(Lighting2D.Profile.qualitySettings.managerInstance) {
				case LightingSettings.QualitySettings.ManagerInstance.Static:
				case LightingSettings.QualitySettings.ManagerInstance.DontDestroyOnLoad:
					
					Debug.LogWarning("Smart Lighting2D: Lighting Manager duplicate was found, new instance destroyed.", gameObject);

					foreach(LightingManager2D manager in Object.FindObjectsOfType(typeof(LightingManager2D))) {
						if (manager != instance) {
							manager.DestroySelf();
						}
					}

					return; // Cancel Initialization

				case LightingSettings.QualitySettings.ManagerInstance.Dynamic:
					instance = this;
					
					Debug.LogWarning("Smart Lighting2D: Lighting Manager duplicate was found, old instance destroyed.", gameObject);

					foreach(LightingManager2D manager in Object.FindObjectsOfType(typeof(LightingManager2D))) {
						if (manager != instance) {
							manager.DestroySelf();
						}
					}
				break;
			}
			

		}

		LightingManager2D.initialized = false;
		SetupProfile();

		if (Application.isPlaying) {
			if (Lighting2D.Profile.qualitySettings.managerInstance == LightingSettings.QualitySettings.ManagerInstance.DontDestroyOnLoad) {
				DontDestroyOnLoad(instance.gameObject);
			}
		}
		
		Buffers.Get();
	}

	private void Update() {
		if (Lighting2D.disable) {
			return;
		}

		ForceUpdate(); // For Late Update Method?

		if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.L)) {
			debug = !debug;
		}

		if (profile != null) {
			if (Lighting2D.Profile != profile) {
				Lighting2D.UpdateByProfile(profile);
			}
        }
	}

	void LateUpdate() {
		if (Lighting2D.disable) {
			return;
		}

		Camera camera = Buffers.Get().GetCamera();

		UpdateLoop();
		
		if (Lighting2D.Profile.qualitySettings.updateMethod == LightingSettings.QualitySettings.UpdateMethod.LateUpdate) {
			RenderLoop();
			
			camera.enabled = false;
		} else {
			camera.enabled = true;
		}
	}

	public void SetupProfile() {
		if (LightingManager2D.initialized) {
			return;
		}

		LightingManager2D.initialized = true;

		LightingSettings.Profile profile = Lighting2D.Profile;
		Lighting2D.UpdateByProfile(profile);
		
		AtlasSystem.Manager.Initialize();
		Lighting2D.materials.Reset();
	}

	public void UpdateLoop() {
		if (Lighting2D.disable) {
			return;
		}

		SetupProfile();

		UpdateMaterials();

		UpdateBuffers();

		UpdateMainBuffers();

		AtlasSystem.Manager.Update();
		// Colliders should be updated before the light sources
	}

	public void RenderLoop() {
		if (Lighting2D.disable) {
			return;
		}

		foreach(DayLightingCollider2D collider in DayLightingCollider2D.GetList()) {
			collider.UpdateLoop();
		}
		
		foreach(LightingCollider2D collider in LightingCollider2D.GetList()) {
			collider.UpdateLoop();
		}

		foreach(LightingSpriteRenderer2D source in LightingSpriteRenderer2D.GetList()) {
			source.UpdateLoop();
		}

		foreach(LightingSource2D source in LightingSource2D.GetList()) {
			source.UpdateLoop();
		}

		foreach(LightMesh2D source in LightMesh2D.GetList()) {
			source.UpdateLoop();
		}
		
		for(int i = 0; i < FogOfWarBuffer2D.list.Count; i++) {
			FogOfWarBuffer2D buffer = FogOfWarBuffer2D.list[i];

			buffer.UpdateLoop();
		}

		foreach(LightingBuffer2D buffer in LightingBuffer2D.GetList()) {
			buffer.Render();
		}

		foreach(FogOfWarBuffer2D buffer in FogOfWarBuffer2D.list) {
			buffer.Render();
		}

		foreach(LightingMainBuffer2D buffer in LightingMainBuffer2D.list) {
			buffer.Render();
		}
	}

	void UpdateMainBuffers() {
		foreach(LightingMainBuffer2D buffer in LightingMainBuffer2D.list) {

			if (Lighting2D.disable) {
				buffer.updateNeeded = false;	
				return;
			}

			CameraSettings cameraSettings = buffer.cameraSettings;
			bool render = cameraSettings.renderMode != CameraSettings.RenderMode.Disabled;

			if (render && cameraSettings.GetCamera() != null) {
				buffer.updateNeeded = true;
			
			} else {
				buffer.updateNeeded = false;
			}
		}
	}

	public void UpdateBuffers() {
	
		for(int i = 0; i < cameraSettings.Length; i++) {
			CameraSettings cameraSetting = cameraSettings[i];

			if (cameraSetting.renderMode == CameraSettings.RenderMode.Disabled) {
				continue;
			}
			
			LightingMainBuffer2D buffer = LightingMainBuffer2D.Get(cameraSetting);

			if (buffer != null) {
				buffer.cameraSettings.renderMode = cameraSetting.renderMode;

				if (buffer.cameraSettings.customMaterial != cameraSetting.customMaterial) {
					buffer.cameraSettings.customMaterial = cameraSetting.customMaterial;

					buffer.ClearMaterial();
				}

				if (buffer.cameraSettings.renderShader != cameraSetting.renderShader) {
					buffer.cameraSettings.renderShader = cameraSetting.renderShader;

					buffer.ClearMaterial();
				}
			}

			if (Lighting2D.fogOfWar.enabled && cameraSetting.bufferID == Lighting2D.fogOfWar.bufferID) {
				if (cameraSetting.cameraType != CameraSettings.CameraType.SceneView) {
					FogOfWarBuffer2D.Get(cameraSetting);
				}
			}
		}


		for(int i = 0; i < LightingMainBuffer2D.list.Count; i++) {
			LightingMainBuffer2D buffer = LightingMainBuffer2D.list[i];

			if (buffer != null) {
				buffer.Update();
			}
			
		}
	}
	
	public void UpdateMaterials() {
		if (Lighting2D.materials.Initialize(Lighting2D.commonSettings.HDR)) {
			LightingMainBuffer2D.Clear();
			LightingBuffer2D.Clear();

			LightingSource2D.ForceUpdateAll();
		}
	}

	void OnGUI() {
		if (debug) {
			LightingDebug.OnGUI();
		}
	}

	public bool IsSceneView() {
		for(int i = 0; i < cameraSettings.Length; i++) {
			CameraSettings cameraSetting = cameraSettings[i];
			
			if (cameraSetting.cameraType == CameraSettings.CameraType.SceneView) {
				if (cameraSetting.renderMode == CameraSettings.RenderMode.Draw) {
					return(true);
				}
			}
		}
		
		return(false);
	}

	private void OnDisable() {
		if (profile != null) {
			if (Application.isPlaying) {
				if (setProfile != profile) {
					if (Lighting2D.Profile == profile) {
						Lighting2D.RemoveProfile();
					}
				}
			}
		}

		#if UNITY_EDITOR
			#if UNITY_2019_1_OR_NEWER
				SceneView.beforeSceneGui -= OnSceneView;
				//SceneView.duringSceneGui -= OnSceneView;
			#else
				SceneView.onSceneGUIDelegate -= OnSceneView;
			#endif
		#endif
	}

	public void UpdateProfile() {
		if (setProfile == null) {
            setProfile = Lighting2D.ProjectSettings.Profile;
        } 

		if (Application.isPlaying == true) {
			profile = Object.Instantiate(setProfile);
		} else {
			profile = setProfile;
		}
	}

	private void OnEnable() {
		FogOfWarBuffer2D.Clear();

		UpdateProfile();
	
		Update();
		LateUpdate();
	
		#if UNITY_EDITOR
			#if UNITY_2019_1_OR_NEWER
				SceneView.beforeSceneGui += OnSceneView;
				//SceneView.duringSceneGui += OnSceneView;
			#else
				SceneView.onSceneGUIDelegate += OnSceneView;
			#endif	
		#endif	
	}

	public void OnRenderObject() {
		if (Lighting2D.renderingMode != RenderingMode.OnPostRender) {
			return;
		}
		
		foreach(LightingMainBuffer2D buffer in LightingMainBuffer2D.list) {
			Rendering.LightingMainBuffer.DrawPost(buffer);
		}
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

		if (Lighting2D.ProjectSettings.sceneView.drawGizmosBounds == LightingSettings.EditorView.GizmosBounds.Rectangle) {
			for(int i = 0; i < cameraSettings.Length; i++) {
				CameraSettings cameraSetting = cameraSettings[i];

				Camera camera = cameraSetting.GetCamera();

				if (camera != null) {
					GizmosHelper.DrawRect(transform.position, CameraHelper.GetWorldRect(camera));
				}
			}
		}
	}

	#if UNITY_EDITOR
		static public void OnSceneView(SceneView sceneView) {
			LightingManager2D manager = LightingManager2D.Get();
	
			if (manager.IsSceneView() == false) {
				return;
			}

			ForceUpdate();

			manager.UpdateLoop();
			manager.RenderLoop();

			Buffers lightingCamera = Buffers.Get();;

			if (lightingCamera != null) {
				lightingCamera.enabled = false;
				lightingCamera.enabled = true;
			}
		}
	#endif
}