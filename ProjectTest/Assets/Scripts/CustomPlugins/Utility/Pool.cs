using Assets.Scripts.Core;
using Assets.Scripts.Data.NetworkMessages;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CustomPlugins.Utility
{
    /// <summary>
    /// Creates and manages objects on scene, using caching rather than initializing every time.
    /// </summary>
    public class Pool : SingletonBehaviour<Pool>
    {
        /// <summary>
        /// Gets or sets dictionary of pooled objects.
        /// </summary>
        private Dictionary<string, Queue<Component>> pooledObjects { get; set; }

        [SerializeField]
        private List<GameObject> poolList;

        private void Awake()
        {
            pooledObjects = new Dictionary<string, Queue<Component>>();
        }

        /// <summary>
        /// Spawns new object from pool or creates new one.
        /// </summary>
        /// <typeparam name="T">The generic parameter of object type.</typeparam>
        /// <param name="spawnObject">The object to spawn.</param>
        /// <param name="parent">The parent of object.</param>
        /// <returns></returns>
        public T Spawn<T>(T spawnObject, Transform parent)
            where T: Component
        {
            if(!pooledObjects.ContainsKey(spawnObject.name))
            {
                pooledObjects.Add(spawnObject.name, new Queue<Component>());
            }

            if(pooledObjects[spawnObject.name].Count <= 0)
            {
                return Instantiate(spawnObject, parent);
            }
            else
            {
                var obj = pooledObjects[spawnObject.name].Dequeue();
                obj.transform.SetParent(parent);
                obj.gameObject.SetActive(true);
                return obj.GetComponent<T>();
            }
        }

        /// <summary>
        /// Despawns the object. Queuing it for later use.
        /// </summary>
        /// <typeparam name="T">Type of object to despawn.</typeparam>
        /// <param name="despawnObject">The object to despawn.</param>
        public void Despawn<T>(T despawnObject)
            where T: Component
        {
            despawnObject.transform.SetParent(this.transform);
            despawnObject.gameObject.SetActive(false);
            var nameWithoutClone = despawnObject.name.Remove(despawnObject.name.Length - Const.SUFIX_CLONE.Length, Const.SUFIX_CLONE.Length);

            if (!pooledObjects.ContainsKey(nameWithoutClone))
            {
                pooledObjects.Add(nameWithoutClone, new Queue<Component>());
            }

            pooledObjects[nameWithoutClone].Enqueue(despawnObject);
        }

        public void NetworkSendSpawn<T>(T spawnObject, Vector2 position, Quaternion rotation)
            where T : Component
        {
            if (!pooledObjects.ContainsKey(spawnObject.name))
            {
                pooledObjects.Add(spawnObject.name, new Queue<Component>());
            }

            if (pooledObjects[spawnObject.name].Count <= 0)
            {
                Instantiate(spawnObject,position, rotation, null);
            }
            else
            {
                var obj = pooledObjects[spawnObject.name].Dequeue();
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
                obj.GetComponent<T>();
            }

            var message = new SpawnMessage(spawnObject.name, position.x, position.y, rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z);

            NetworkingAdapter.Inst.playfabManager.SendDataMessageToAllPlayers(message.ObjectToByteArray());
        }

        public void NetworkRecieveSpawn(SpawnMessage msg)
        {
            if (!pooledObjects.ContainsKey(msg.PrefabName))
            {
                pooledObjects.Add(msg.PrefabName, new Queue<Component>());
            }

            var position = new Vector2(msg.PositionX, msg.PositionY);
            var rotation = Quaternion.Euler(msg.RotationX, msg.RotationY, msg.RotationZ);

            if (pooledObjects[msg.PrefabName].Count <= 0)
            {
                foreach (var item in poolList)
                {
                    if (item.name == msg.PrefabName)
                    {
                        Instantiate(item, position, rotation, null);
                    }
                }
            }
            else
            {
                var obj = pooledObjects[msg.PrefabName].Dequeue();
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
            }
        }
    }
}
