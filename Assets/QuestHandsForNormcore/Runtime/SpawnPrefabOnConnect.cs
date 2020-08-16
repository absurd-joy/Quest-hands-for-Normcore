using UnityEngine;
using Normal.Realtime;

namespace absurdjoy
{
    public class SpawnPrefabOnConnect : MonoBehaviour
    {
        public GameObject prefab;
        public bool ownedByClient = true;
        public bool preventOwnershipTakeover = true;
        public bool destroyWhenOwnerOrLastClientLeaves = true;

        private void OnEnable()
        {
            var rt = GetComponent<Realtime>();
            if (rt.connected)
            {
                RealtimeConnected(rt);
            }
            else
            {
                GetComponent<Realtime>().didConnectToRoom += RealtimeConnected;
            }
        }

        private void OnDisable()
        {
            GetComponent<Realtime>().didConnectToRoom -= RealtimeConnected;
        }

        protected virtual void RealtimeConnected(Realtime realtime)
        {
            Realtime.Instantiate(prefab.name, ownedByClient, preventOwnershipTakeover, destroyWhenOwnerOrLastClientLeaves);
        }
    }
}