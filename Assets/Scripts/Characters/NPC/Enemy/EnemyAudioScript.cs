using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioScript : MonoBehaviour
{
    private enum CURRENT_TERRAIN { STREET, DIRT };

    [SerializeField]
    private CURRENT_TERRAIN currentTerrain;

    private FMOD.Studio.EventInstance footsteps;
    private FMOD.Studio.EventInstance idleGrowl;
    private FMOD.Studio.EventInstance attackSound;

    public GameObject raycastTest;

    private float xPosPlayer;
    private float yPosPlayer;
    private Vector3 audioPoint;

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
                //Debug.Log(currentTerrain);
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Dirt"))
            {
                currentTerrain = CURRENT_TERRAIN.DIRT;
                //Debug.Log(currentTerrain);
            }
        }
    }

    private void PlayFootstep(int terrain)
    {
        xPosPlayer = GameManager.Instance.gamePlayer.ActivePlayer.transform.position.x - this.transform.position.x;
        yPosPlayer = GameManager.Instance.gamePlayer.ActivePlayer.transform.position.y - this.transform.position.y;
        audioPoint = new Vector3(xPosPlayer * -1, yPosPlayer * -1, GameManager.Instance.gamePlayer.ActivePlayer.transform.position.z - this.transform.position.z);

        footsteps = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Zombie/Footstep");
        footsteps.setParameterByName("Terrain", terrain);
        footsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(audioPoint));
        footsteps.start();
        footsteps.release();
    }

    private void PlayIdleGrowl()
    {
        xPosPlayer = GameManager.Instance.gamePlayer.ActivePlayer.transform.position.x - this.transform.position.x;
        yPosPlayer = GameManager.Instance.gamePlayer.ActivePlayer.transform.position.y - this.transform.position.y;
        audioPoint = new Vector3(xPosPlayer * -1, yPosPlayer * -1, GameManager.Instance.gamePlayer.ActivePlayer.transform.position.z - this.transform.position.z);

        idleGrowl = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Zombie/Idle");
        idleGrowl.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(audioPoint));
        idleGrowl.start();
        idleGrowl.release();
    }

    private void PlayAttack()
    {
        xPosPlayer = GameManager.Instance.gamePlayer.ActivePlayer.transform.position.x - this.transform.position.x;
        yPosPlayer = GameManager.Instance.gamePlayer.ActivePlayer.transform.position.y - this.transform.position.y;
        audioPoint = new Vector3(xPosPlayer * -1, yPosPlayer * -1, GameManager.Instance.gamePlayer.ActivePlayer.transform.position.z - this.transform.position.z);

        attackSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Zombie/Attack");
        attackSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(audioPoint));
        attackSound.start();
        attackSound.release();
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
