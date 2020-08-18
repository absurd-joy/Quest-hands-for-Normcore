using System.Collections.Generic;
using UnityEngine;

namespace absurdjoy
{
    public class FingerLinker : MonoBehaviour, IAssignSkeleton
    {
        public void AssignLocalSkeleton(OVRSkeleton ovrSkeleton)
        {
            List<Transform> bones = new List<Transform>();
            var transformSyncs = GetComponentsInChildren<TransformSynchronizer>();
            foreach (var bone in ovrSkeleton.Bones)
            {
                bool success = false;
                foreach (var transformSync in transformSyncs)
                {
                    if (bone.Transform.name == transformSync.transform.name)
                    {
                        transformSync.AssignSourceTransform(bone.Transform, true);
                        bones.Add(transformSync.transform);
                        success = true;
                        break;
                    }
                }

                if (!success)
                {
                    Debug.LogError("Couldn't find bone with name: "+bone.Transform.name);
                }
            }

            //Initialize the skinnedMeshRender and assign the bones.
            var skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            skinnedMeshRenderer.enabled = true;
            skinnedMeshRenderer.bones = bones.ToArray();
        }
    }
}