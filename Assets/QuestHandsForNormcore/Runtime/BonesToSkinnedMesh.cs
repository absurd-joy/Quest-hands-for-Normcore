using System.Collections.Generic;
using UnityEngine;

namespace absurdjoy
{
    public class BonesToSkinnedMesh : MonoBehaviour
    {
        public Transform boneRoot;
        
        [Tooltip("This is required for Oculus' implementation of the CustomSkeleton, but not the default Skeleton.")]
        public bool putTipsAtEnd = false;

        // Recreate hand structure to replicate Oculus
        // This orders all the bones within the list, setting the finger tips last.
        public void OnEnable()
        {
            var listOfChildren = AddRecursiveChildren(boneRoot);
            List<Transform> allBones = new List<Transform>();
            
            // We need bones to be in the same order as oculus
            // So we add all the bones and keep a reference to 5 finger tips. (OVRSkeleton sets these bone id's last)
            // We then add finger tips back to bones so they are last.
            List<Transform> fingerTips = new List<Transform>();
            foreach (var bone in listOfChildren)
            {
                if (putTipsAtEnd && bone.name.Contains("Tip"))
                {
                    fingerTips.Add(bone); //Keep reference to finger tips
                }
                else
                {
                    allBones.Add(bone);
                }
            }

            if (putTipsAtEnd)
            {
                //And finger tips back to bones
                foreach (var bone in fingerTips)
                {
                    allBones.Add(bone);
                }
            }

            //Initialize the skinnedMeshRender and assign the bones.
            var skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            skinnedMeshRenderer.enabled = true;
            skinnedMeshRenderer.bones = allBones.ToArray();
        }
        
        private List<Transform> AddRecursiveChildren(Transform obj, List<Transform> set = null)
        {
            if (set == null)
            {
                set = new List<Transform>();
            }

            if (obj == null)
            {
                return set;
            }

            for (int i = 0; i < obj.childCount; i++)
            {
                var child = obj.GetChild(i);
                if (child == null)
                {
                    continue;
                }

                if (child != obj)
                {
                    set.Add(child);
                }

                AddRecursiveChildren(child, set);
            }

            return set;
        }
    }
}