using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnObject : MonoBehaviour
{
    public GameObject objectToSpawn;
    public GameObject parent;
    public int numberToSpawn;
    public int limit = 20;
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
        if (parent.transform.childCount < limit)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                for (int i = 0; i < numberToSpawn; i++)
                {
                   // new Vector3(this.transform.position.x + GetModifier(true), 1, this.transform.position.z + GetModifier(false)
                    Vector3 randomPoint = this.transform.position + Random.insideUnitSphere * 10;
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(randomPoint, out hit, 20.0f, NavMesh.AllAreas))
                    {
                      GameObject enemys = Instantiate(objectToSpawn,new Vector3 (hit.position.x,1, hit.position.z), Quaternion.identity);
                        enemys.transform.parent = parent.gameObject.transform;
                    }

                }
                spawnTimer = rate;
            }
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
