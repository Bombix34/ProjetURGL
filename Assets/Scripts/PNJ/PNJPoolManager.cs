using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class PNJPoolManager : NetworkBehaviour
{
    public int TOTAL_PNJ;

    [SerializeField]
    [NotNull]
    private GameObject pnjPrefab;

    private GameObject[] pnjsInScene;

    [SerializeField]
    private Vector2 spawnPosition;

    [SerializeField]
    private RangedFloat randomChronoSpawn;
    private float curChrono=10000f;


    [ServerCallback]
    private void Update()
    {
        curChrono -= Time.deltaTime;
        if(curChrono<=0f)
        {
            SpawnPNJ();
            curChrono = Random.Range(randomChronoSpawn.minValue, randomChronoSpawn.maxValue) * 100f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.4f, 0.2f, 1f);
        Gizmos.DrawSphere(spawnPosition, 0.3f);
    }

    public void InitPNJ()
    {
        pnjsInScene = new GameObject[TOTAL_PNJ];
        for(int i = 0; i < TOTAL_PNJ; ++i)
        {
            pnjsInScene[i] = InstantiatePNJ();
        }
    }

    public void LaunchGame()
    {
        int randPNJ = Random.Range(TOTAL_PNJ/6, TOTAL_PNJ/3);
        int nbToSpawn = TOTAL_PNJ - randPNJ;
        for (int i = 0; i < nbToSpawn; ++i)
        {
            SpawnPNJ();
        }
        curChrono = Random.Range(randomChronoSpawn.minValue, randomChronoSpawn.maxValue) * 100f;
    }

    public void StopAllPNJ()
    {
        foreach(var pnj in pnjsInScene)
        {
            PNJManager pnjManager = pnj.GetComponent<PNJManager>();
            if(!pnjManager.IsDead)
            {
                pnjManager.ChangeState(new PNJWaitState(pnjManager));
            }
        }
    }

    private GameObject InstantiatePNJ()
    {
        GameObject instantiatePNJ = Instantiate(pnjPrefab);
        instantiatePNJ.transform.position = spawnPosition;
        instantiatePNJ.GetComponent<PNJManager>().Init();
        NetworkServer.Spawn(instantiatePNJ);
        return instantiatePNJ;
    }

    public void SpawnPNJ()
    {
        PNJManager pnjToSpawn = AvailablePNJ;
        if (pnjToSpawn == null)
            return;
        pnjToSpawn.ChangeState(new PNJSpawnState(pnjToSpawn, spawnPosition));
    }

    private PNJManager AvailablePNJ
    {
        get
        {
            return pnjsInScene
                .FirstOrDefault(pnj => pnj.GetComponent<PNJManager>().IsDead)
                ?.GetComponent<PNJManager>();
        }
    }
}
