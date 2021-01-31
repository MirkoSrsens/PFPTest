using Assets.Scripts.Core;
using Assets.Scripts.Data.NetworkMessages;
using Assets.Scripts.Network;
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

        /// <summary>
        /// Defines network ids used to find pooled objects.
        /// </summary>
        public Dictionary<int, NetworkObject> NetworkRegisteredObjects { get; set; }

        [SerializeField]
        private List<NetworkObject> poolList;

        private void Awake()
        {
            NetworkRegisteredObjects = new Dictionary<int, NetworkObject>();
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

        public int NetworkSendSpawn<T>(T spawnObject, Vector2 position, Quaternion rotation)
            where T : NetworkObject
        {
            if (!pooledObjects.ContainsKey(spawnObject.name))
            {
                pooledObjects.Add(spawnObject.name, new Queue<Component>());
            }

            var obj = default(NetworkObject);

            if (pooledObjects[spawnObject.name].Count <= 0)
            {
                obj = Instantiate<NetworkObject>(spawnObject,position, rotation, null);
            }
            else
            {
                obj = pooledObjects[spawnObject.name].Dequeue().GetComponent<NetworkObject>();
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
            }

            var id = NetworkRegisteredObjects.Count;
            NetworkRegisteredObjects.Add(id, obj);
            obj.ID = id;

            var message = new SpawnMessage(id, spawnObject.name, position.x, position.y, rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z);

            NetworkMessageHandler.Inst.Send(message);

            return obj.ID;
        }

        public void NetworkRecieveSpawn(SpawnMessage msg)
        {
            if (!pooledObjects.ContainsKey(msg.PrefabName))
            {
                pooledObjects.Add(msg.PrefabName, new Queue<Component>());
            }

            var position = new Vector2(msg.PositionX, msg.PositionY);
            var rotation = Quaternion.Euler(msg.RotationX, msg.RotationY, msg.RotationZ);

            var obj = default(NetworkObject);

            if (pooledObjects[msg.PrefabName].Count <= 0)
            {
                foreach (var item in poolList)
                {
                    if (item.name == msg.PrefabName)
                    {
                        obj = Instantiate(item.GetComponent<NetworkObject>(), position, rotation, null);
                        obj.ID = msg.NetworkId;
                    }
                }
            }
            else
            {
                obj = pooledObjects[msg.PrefabName].Dequeue().GetComponent<NetworkObject>();
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
                obj.ID = msg.NetworkId;
            }

            if (!NetworkRegisteredObjects.ContainsKey(obj.ID))
            {
                NetworkRegisteredObjects.Add(NetworkRegisteredObjects.Count, obj);
            }
            else
            {
                NetworkRegisteredObjects[obj.ID] = obj;
            }
        }
    }
}
