using System.Collections;
using Normal.Realtime;
using UnityEngine;

namespace absurdjoy
{
    public class ConnectAfterBonesPopulate : MonoBehaviour
    {
        public string roomName;

        IEnumerator Start()
        {
            var skeleton = GetComponent<SpawnPlayerAvatar>().localLeftHandReference.GetComponentInChildren<OVRSkeleton>();
            while (skeleton.Bones.Count == 0)
            {
                yield return null;
            }

            skeleton = GetComponent<SpawnPlayerAvatar>().localRightHandReference.GetComponentInChildren<OVRSkeleton>();
            while (skeleton.Bones.Count == 0)
            {
                yield return null;
            }

            var realtime = GetComponent<Realtime>();
            realtime.Connect(roomName);
        }
    }
}