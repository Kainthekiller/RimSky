using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnerBehavior : MonoBehaviour
{
    public GameObject objectToSpawn;
    public GameObject parent;
    public GameObject MiniBoss;
    public int numberToSpawn;
    public int limit = 10;
    public float rate;
    public Vector2 range;
    int xPos;
    int zPos;
    float spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = rate;
    }

    // Update is called once per frame
    void Update()
    {

        //check to see if the enemies have reached the spawn limit
        if (parent.transform.childCount < limit)
        {   
            //increment timer
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                for (int i = 0; i < numberToSpawn; i++)
                {
                    Vector3 randomPoint = this.transform.position + Random.insideUnitSphere * 10;
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(randomPoint, out hit, 10.0f, NavMesh.AllAreas))
                    {
                        Transform enemy = (Transform) Instantiate(objectToSpawn.transform, new Vector3(hit.position.x, 1, hit.position.z), Quaternion.identity);
                        enemy.transform.parent = parent.transform;

                        //Debug.Log("Hit " + hit.position);
                    }
                }
                spawnTimer = rate;
            }
        }

        if (!MiniBoss)
        {
            Destroy(gameObject);
        }
    }

    float GetModifier(bool X)
    {
        float modifier;
        if (X)
        {
            modifier = Random.Range(0f, range.x);
        }
        else
        {
            modifier = Random.Range(0f, range.y);
        }

        if (Random.Range(0, 2) > 0)
            return -modifier;
        else
            return modifier;
    }

    
}
