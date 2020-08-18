using System.Collections.Generic;
using UnityEngine;

namespace absurdjoy
{
    public class FingerLinker : MonoBehaviour, IAssignSkeleton
    {
        public void AssignLocalSkeleton(OVRSkeleton ovrSkeleton)
        {
            var transformSyncs = new List<TransformSynchronizer>(GetComponentsInChildren<TransformSynchronizer>());
            transformSyncs.Remove(GetComponent<TransformSynchronizer>()); // remove our own, it's set elsewhere
            
            var bones = new List<Transform>();
            
            foreach (var bone in ovrSkeleton.Bones)
            {
                bool success = false;
                foreach (var transformSync in transformSyncs)
                {
                    if (bone.Transform.name == transformSync.transform.name)
                    {
                        transformSync.AssignSourceTransform(bone.Transform);
                        bones.Add(transformSync.transform);
                        success = true;
                        transformSyncs.Remove(transformSync);
                        break;
                    }
                }

                if (!success)
                {
                    Debug.LogError("Couldn't find TransformSync on bone with name: "+bone.Transform.name);
                }
            }

            foreach (var transformSync in transformSyncs)
            {
                Debug.LogError("No bone in skeleton found for TransformSync: "+transformSync.gameObject.name);
            }
        }
    }
}