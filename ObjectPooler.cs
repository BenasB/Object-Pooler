using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    public static ObjectPooler Instance;

    Dictionary<int, Queue<PoolObject>> dictionary = new Dictionary<int, Queue<PoolObject>>();

    void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Reuses an object from a pool
    /// </summary>
    public Transform ReuseObject(Transform prefab, Vector2 position, Quaternion rotation)
    {
        int key = prefab.GetInstanceID();

        if (!dictionary.ContainsKey(key))
        {
            Debug.LogError("Trying to reuse an object from a pool that doesn't exist");
            return null;
        }

        PoolObject objectToReuse = dictionary[key].Dequeue();

        dictionary[key].Enqueue(objectToReuse);

        objectToReuse.ResetObject();

        // Set position and rotation
        Transform objectTransform = objectToReuse.ReuseObject(position, rotation);

        return objectTransform;
    }

    /// <summary>
    /// Creates a pool with desired size
    /// </summary>
    public void CreatePool (Transform prefab, int size)
    {
        int key = prefab.GetInstanceID();

        if (dictionary.ContainsKey(key))
        {
            Debug.LogError("Trying to create a pool with a key that already exists in the dictionary");
            return;
        }

        Queue<PoolObject> pool = new Queue<PoolObject>();

        // Populate the queue
        for (int i = 0; i < size; i++)
        {
            Transform spawnedObject = Instantiate(prefab, transform);

            spawnedObject.gameObject.SetActive(false);

            pool.Enqueue(new PoolObject(spawnedObject));
        }

        // Add the new queue to the dictionary
        dictionary.Add(key, pool);
    }

    /// <summary>
    /// Expands the pool by desired amount
    /// </summary>
    public void ExpandPool (Transform prefab, int size)
    {
        int key = prefab.GetInstanceID();

        if (!dictionary.ContainsKey(key))
        {
            Debug.LogError("Trying to expand a pool with a key that doesn't exist");
            return;
        }

        // Add more objects to the queue
        for (int i = 0; i < size; i++)
        {
            Transform spawnedObject = Instantiate(prefab, transform);

            spawnedObject.gameObject.SetActive(false);

            dictionary[key].Enqueue(new PoolObject(spawnedObject));
        }
    }

    /// <summary>
    /// Checks if a pool is present
    /// </summary>
    public bool PoolExists(Transform prefab)
    {
        int key = prefab.GetInstanceID();

        return dictionary.ContainsKey(key);
    }

    /// <summary>
    /// Keeps an object and handles it's resetting
    /// </summary>
    class PoolObject
    {
        Transform objectTransform;

        bool isPoolable;
        IPoolable poolable;

        public PoolObject(Transform spawnedObject)
        {
            objectTransform = spawnedObject;

            poolable = objectTransform.GetComponent<IPoolable>();
            if (poolable != null)
                isPoolable = true;
        }

        /// <summary>
        /// Resets the object for reusability
        /// </summary>
        public void ResetObject()
        {
            if (isPoolable)
                poolable.ResetPoolObject();
        }

        /// <summary>
        /// Places an object to desired position with desired rotation
        /// </summary>
        public Transform ReuseObject(Vector2 position, Quaternion rotation)
        {
            objectTransform.gameObject.SetActive(true);
            objectTransform.position = position;
            objectTransform.rotation = rotation;

            return objectTransform;
        }
    }
}
