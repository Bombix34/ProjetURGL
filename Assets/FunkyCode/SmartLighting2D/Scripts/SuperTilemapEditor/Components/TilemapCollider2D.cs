using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if (SUPER_TILEMAP_EDITOR)

    namespace SuperTilemapEditorSupport {

        [System.Serializable]
        public class TilemapCollider2D : Tilemap {
            
            public enum ColliderType {None, Grid, Collider};
            public enum MaskType {None, Grid, Sprite};

            public ColliderType colliderType = ColliderType.Grid;
            public MaskType maskType = MaskType.Sprite;
        }
    }

#else

 namespace SuperTilemapEditorSupport {

        [System.Serializable]
        public class TilemapCollider2D : LightingTilemapCollider.Base {
            
            public enum ColliderType {None, Grid, Collider};
            public enum MaskType {None, Grid, Sprite};

            public ColliderType colliderType = ColliderType.Grid;
            public MaskType maskType = MaskType.Sprite;

            public bool eventsInit;

            public override void Initialize() {}
        }
    }

#endif