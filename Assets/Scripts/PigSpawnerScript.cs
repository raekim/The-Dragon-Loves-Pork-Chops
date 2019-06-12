using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigSpawnerScript : MonoBehaviour
{
    public int size = 9;    // Max. Pigs in the pool
    public GameObject PigPrefab;    // Object Prefab to instantiate
    [Tooltip("(int)The distance between newly spawned pig and the player")]
    public int SpawnPadding = 5;

    private Queue<GameObject> ObjectQueue = new Queue<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        // Initialize ObjectQueue
        for(int i=0; i<size; ++i)
        {
            var Obj = Instantiate(PigPrefab);
            Obj.SetActive(false);
            ObjectQueue.Enqueue(Obj);
        }

        // Spawn Max. # of pigs
        StartCoroutine("SpawnPig", size);
    }

    public void EnqueuePig(GameObject Pig)
    {
        Pig.SetActive(false);
        ObjectQueue.Enqueue(Pig);
        StartCoroutine("SpawnPig", 1);
    }

    // Spawn num # of pigs
    public IEnumerator SpawnPig(int num)
    {
        for(int i=0; i<num; ++i)
        {
            var pigToSpawn = ObjectQueue.Dequeue();
            if (pigToSpawn != null)
            {
                // Calculate the time to wait
                float ratio = ((size-1) - ObjectQueue.Count) / (size-1);    // The more pigs that are already spawned, the more time the next spawn has to wait
                float timeToWait = Mathf.Lerp(1f, 5f, ratio) + Random.Range(0f, 2f);

                // WAITING...
                yield return new WaitForSeconds(timeToWait);

                // ...and then Spawn the Pig
                // Locate the pig to the right position
                int posX, posY;
                do
                {
                    posX = Random.Range(0, Camera.main.pixelWidth);
                    posY = Random.Range(0, Camera.main.pixelHeight);

                } while (Vector2.Distance(new Vector2(posX, posY), GameManagerScript.GameManager.PlayerObj.transform.position) < (float)SpawnPadding);
                pigToSpawn.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(posX, posY, 1));
                pigToSpawn.GetComponent<PigScript>().StartNewPig();

                pigToSpawn.SetActive(true);
            }
        }
    }
}
