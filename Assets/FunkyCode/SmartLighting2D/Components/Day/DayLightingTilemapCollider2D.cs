using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingTilemapCollider;

public class DayLightingTile {
  public List<Polygon2D> polygons;

  public DayLighting.TilemapShadowMesh shadowMesh;

  public float height = 1;
}

[ExecuteInEditMode]
public class DayLightingTilemapCollider2D : MonoBehaviour {

    public MapType tilemapType = MapType.UnityRectangle;

    public LightingLayer shadowLayer = LightingLayer.Layer1;

	public ShadowTileType shadowTileType = ShadowTileType.AllTiles;

	public DayLightingTilemapColliderTransform transform2D = new DayLightingTilemapColliderTransform();

	public float height = 1;

    public LightingLayer maskLayer = LightingLayer.Layer1;

    public static List<DayLightingTilemapCollider2D> list = new List<DayLightingTilemapCollider2D>();

    public Rectangle rectangle = new Rectangle();
	public Isometric isometric = new Isometric();
	public Hexagon hexagon = new Hexagon();

    public SuperTilemapEditorSupport.TilemapCollider2D superTilemapEditor = new SuperTilemapEditorSupport.TilemapCollider2D();

    public List<DayLightingTile> dayTiles = new List<DayLightingTile>();

	static public List<DayLightingTilemapCollider2D> GetList() {
		return(list);
	}

    public void OnEnable() {
		list.Add(this);

		rectangle.SetGameObject(gameObject);
		isometric.SetGameObject(gameObject);
		hexagon.SetGameObject(gameObject);

		superTilemapEditor.eventsInit = false;
		superTilemapEditor.SetGameObject(gameObject);

		LightingManager2D.Get();

		Initialize();
    }

	public void OnDisable() {
		list.Remove(this);
	}

	void Update() {
		transform2D.Update(this);

		if (transform2D.moved) {
			transform2D.moved = false;

			foreach(DayLightingTile dayTile in dayTiles) {
				dayTile.height = height;
				dayTile.shadowMesh.Generate(dayTile);
			}
		}
	}

	public LightingTilemapCollider.Base GetCurrentTilemap() {
		switch(tilemapType) {
			case MapType.SuperTilemapEditor:
				return(superTilemapEditor);
			case MapType.UnityRectangle:
				return(rectangle);
			case MapType.UnityIsometric:
				return(isometric);
			case MapType.UnityHexagon:
				return(hexagon);
		}
		return(null);
	}

	public void Initialize() {
		TilemapEvents.Initialize();

		GetCurrentTilemap().Initialize();

		dayTiles.Clear();

		switch(tilemapType) {
			case MapType.UnityRectangle:

				switch(rectangle.colliderType) {
					case LightingTilemapCollider.Rectangle.ColliderType.Grid:
						foreach(LightingTile tile in GetTileList()) {
							DayLightingTile dayTile = new DayLightingTile();
							dayTile.shadowMesh = new DayLighting.TilemapShadowMesh();
							dayTile.height = height;

							dayTile.polygons = tile.GetWorldPolygons(GetCurrentTilemap());

							dayTiles.Add(dayTile);

							dayTile.shadowMesh.Generate(dayTile);
						}

					break;

					case LightingTilemapCollider.Rectangle.ColliderType.CompositeCollider:
						foreach(Polygon2D polygon in rectangle.compositeColliders) {
							DayLightingTile dayTile = new DayLightingTile();
							dayTile.shadowMesh = new DayLighting.TilemapShadowMesh();
							dayTile.height = height;

							dayTile.polygons = new List<Polygon2D>();
							
							Polygon2D poly = polygon.Copy();
							poly.ToOffsetItself(transform.position);

							dayTile.polygons.Add(poly);
						
							dayTiles.Add(dayTile);

							dayTile.shadowMesh.Generate(dayTile);
						}
					break;
				}
				
			break;
		}

		
	}

    public List<LightingTile> GetTileList() {
		return(GetCurrentTilemap().mapTiles);
	}

    public TilemapProperties GetTilemapProperties() {
		return(GetCurrentTilemap().Properties);
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

		TilemapProperties properties = GetTilemapProperties();

		LightingTilemapCollider.Base tilemap = GetCurrentTilemap();

		// Debug.Log(GetTileList().Count);

		foreach(DayLightingTile dayTile in dayTiles) {
			GizmosHelper.DrawPolygons(dayTile.polygons, transform.position);
		}

		switch(Lighting2D.ProjectSettings.sceneView.drawGizmosBounds) {
			case LightingSettings.EditorView.GizmosBounds.Rectangle:
				GizmosHelper.DrawRect(transform.position, GetCurrentTilemap().GetRect());
			break;

			case LightingSettings.EditorView.GizmosBounds.Radius:
				float radius = GetCurrentTilemap().GetRadius();
				GizmosHelper.DrawCircle(transform.position, 0, 360, radius);
			break;
		}
	}
}
