using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence.ObjectPooling
{
    public class ObjectPooler : SingletonMonoBehaviour<ObjectPooler>
    {
        [field: SerializeField]
        private ObjectPoolingData PoolingData { get; set; }

        private Dictionary<string, Queue<Transform>> PoolDictionary { get; set; }

        public Transform GetObjectFromPool(string objectID, Vector3 positionToSet, Quaternion rotationToSet)
        {
            if (PoolDictionary.ContainsKey(objectID) == false)
            {
                Debug.LogWarning($"Pool with object ID {objectID} doesn't exist.");
                return null;
            }

            Transform objectToUse = PoolDictionary[objectID].Dequeue();

            objectToUse.gameObject.SetActive(true);
            objectToUse.position = positionToSet;
            objectToUse.rotation = rotationToSet;

            return objectToUse;
        }

        public Transform ReturnObjectToPool(string objectID, Transform objectToDisable)
        {
            if (PoolDictionary.ContainsKey(objectID) == false)
            {
                Debug.LogWarning($"Pool with object ID {objectID} doesn't exist.");
                return null;
            }

            PoolDictionary[objectID].Enqueue(objectToDisable);

            objectToDisable.position = transform.position;
            objectToDisable.rotation = transform.rotation;
            objectToDisable.gameObject.SetActive(false);

            return objectToDisable;
        }

        protected virtual void Start()
        {
            InitializePool();
        }

        private void InitializePool()
        {
            PoolDictionary = new Dictionary<string, Queue<Transform>>();

            for (int poolIndex = 0; poolIndex < PoolingData.PoolCollection.Count; poolIndex++)
            {
                Queue<Transform> objectPool = new Queue<Transform>();

                for (int index = 0; index < PoolingData.PoolCollection[poolIndex].PoolSize; index++)
                {
                    Transform spawnedObject = Instantiate(PoolingData.PoolCollection[poolIndex].Prefab);
                    spawnedObject.gameObject.SetActive(false);
                    spawnedObject.parent = transform;
                    objectPool.Enqueue(spawnedObject);
                }

                PoolDictionary.Add(PoolingData.PoolCollection[poolIndex].ObjectID, objectPool);
            }
        }
    }
}
