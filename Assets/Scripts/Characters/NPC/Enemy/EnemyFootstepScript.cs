using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFootstepScript : MonoBehaviour
{
    private enum CURRENT_TERRAIN { STREET, DIRT };

    [SerializeField]
    private CURRENT_TERRAIN currentTerrain;

    private FMOD.Studio.EventInstance foosteps;
    public GameObject raycastTest;

    // Update is called once per frame
    void Update()
    {
        DetermineTerrain();
    }

    private void DetermineTerrain()
    {
        RaycastHit2D[] hit;

        hit = Physics2D.RaycastAll(raycastTest.transform.position, Vector2.down, .02f);
        foreach (RaycastHit2D rayhit in hit)
        {
            if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Street"))
            {
                currentTerrain = CURRENT_TERRAIN.STREET;
                Debug.Log(currentTerrain);
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Dirt"))
            {
                currentTerrain = CURRENT_TERRAIN.DIRT;
                Debug.Log(currentTerrain);
            }
        }
    }

    private void PlayFootstep(int terrain)
    {
        foosteps = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Zombie/Footstep");
        foosteps.setParameterByName("Terrain", terrain);
        foosteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        foosteps.start();
        foosteps.release();
    }

    public void SelectAndPlayFootstep()
    {
        switch (currentTerrain)
        {
            case CURRENT_TERRAIN.STREET:
                PlayFootstep(0);
                break;

            case CURRENT_TERRAIN.DIRT:
                PlayFootstep(1);
                break;
        }
    }
}
