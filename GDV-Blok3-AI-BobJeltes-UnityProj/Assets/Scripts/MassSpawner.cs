using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorBricks
{
    public class MassSpawner : MonoBehaviour
    {
        public GameObject Prefab;
        public GameObject WanderArea;
        public GameObject Player;

        public int Spawns = 750;
        int spawnCount = 0;
        List<GameObject> entities;

        void Start()
        {
            entities = new List<GameObject>();
            entities.Add(Player);
            InvokeRepeating("Spawn", 0f, 1.0f / 1000.0f);
        }

        void Spawn()
        {
            if (spawnCount <= Spawns)
            {
                GameObject instance = 
                    Instantiate(Prefab, GetRandomPosition(), Quaternion.identity)
                    as GameObject;
                BehaviorExecutor component = 
                    instance.GetComponent<BehaviorExecutor>();
                component.SetBehaviorParam("wanderArea", WanderArea);
                component.SetBehaviorParam("player", entities[Random.Range(0, entities.Count)]);

                ++spawnCount;

                entities.Add(instance);
            } else
            {
                CancelInvoke();
            }
        }

        private Vector3 GetRandomPosition()
        {
            Vector3 randomPosition = Vector3.zero;
            BoxCollider boxCollider = WanderArea.GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                randomPosition =
                  new Vector3(
                    Random.Range(
                        WanderArea.transform.position.x -
                        WanderArea.transform.localScale.x *
                        boxCollider.size.x * 0.5f,
                        WanderArea.transform.position.x +
                        WanderArea.transform.localScale.x *
                        boxCollider.size.x * 0.5f),
                    WanderArea.transform.position.y,
                    Random.Range(
                        WanderArea.transform.position.z -
                        WanderArea.transform.localScale.z *
                        boxCollider.size.z * 0.5f,
                        WanderArea.transform.position.z +
                        WanderArea.transform.localScale.z *
                        boxCollider.size.z * 0.5f));
            }
            return randomPosition;
        }
    }
}