using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Normal.Realtime;
using UnityEngine;
using static OVRSkeleton;
    
/*
    This version of this file inspired by the GitHub project SpeakGeek-Normcore-Quest-Hand-Tracking
    https://github.com/dylanholshausen/SpeakGeek-Normcore-Quest-Hand-Tracking
    Thanks, Dylan Holshausen! Especially for the Skeletal Serialization, which I cribbed heavily from. Click that link and buy Dylan a beer.
*/

namespace absurdjoy
{
    public class QuestSkeletonSerializer : MonoBehaviour
    {
        // TODO: Implement this.
        // [Tooltip("If true, will disable the visualization of your local hand once the networked hand is in place.")]
        // public bool disableLocalVisualization = true;
        
        [Tooltip("The root bone structure within this prefab.")]
        public Transform boneRoot;

        private RealtimeView realtimeView;
        private HandPoseSync handPoseSync;
        private SkinnedMeshRenderer skinnedMeshRenderer;
        private List<Transform> allBones = new List<Transform>();

        private OVRCustomSkeleton ovrSkeleton;
        private IOVRSkeletonDataProvider ovrSkeletonDataProvider;

        private StringBuilder stringBuilder;

        private bool isInitialized = false;

        private void OnEnable()
        {
            realtimeView = GetComponent<RealtimeView>();
            handPoseSync = GetComponent<HandPoseSync>();
            skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            
            stringBuilder = new StringBuilder();

            transform.eulerAngles = Vector3.zero;
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
        }

        /// <summary>
        /// (only called on locally controlled avatars) Assign a skeleton to this script to harvest data from.  
        /// </summary>
        public void AssignLocalSkeleton(OVRCustomSkeleton skeleton)
        {
            ovrSkeleton = skeleton;
            ovrSkeletonDataProvider = ovrSkeleton.GetComponent<IOVRSkeletonDataProvider>();
        }

        private bool IsOnline()
        {
            return realtimeView != null && realtimeView.realtime != null && realtimeView.realtime.connected;
        }
        
        // Recreate hand structure to replicate Oculus
        // This orders all the bones within the list, setting the finger tips last.
        public void Initialize()
        {
            allBones.Clear();
            var listOfChildren = AddRecursiveChildren(boneRoot.transform);

            // We need bones to be in the same order as oculus
            // So we add all the bones and keep a reference to 5 finger tips. (OVRSkeleton sets these bone id's last)
            // We then add finger tips back to bones so they are last.
            List<Transform> fingerTips = new List<Transform>();
            foreach (var bone in listOfChildren)
            {
                if (bone.name.Contains("Tip"))
                {
                    fingerTips.Add(bone); //Keep reference to finger tips
                }
                else
                {
                    allBones.Add(bone);
                }
            }

            //And finger tips back to bones
            foreach (var bone in fingerTips)
            {
                allBones.Add(bone);
            }

            //Initialize the skinnedMeshRender and assign the bones.
            skinnedMeshRenderer.enabled = true;
            skinnedMeshRenderer.bones = allBones.ToArray();

            isInitialized = true;
        }        

        private void Update()
        {
            if (!IsOnline())
            {
                return;
            }

            if (!isInitialized)
            {
                Initialize();
            }

            if (realtimeView.isOwnedLocallyInHierarchy)
            {
                LocalUpdate();
                // RemoteUpdate is called by changes to the model; see HandPoseSync.cs
            }
        }
        
        /// <summary>
        /// Only called on the avatar representing your local hand.
        /// </summary>
        private void LocalUpdate()
        {
            handPoseSync.SendData( SerializeSkeletalData() );
        }

        private string SerializeSkeletalData()
        { 
            var data = ovrSkeletonDataProvider.GetSkeletonPoseData();
            stringBuilder.Clear();

            if (!data.IsDataValid || !data.IsDataHighConfidence)
            {
                // Data is invalid or low confidence; we don't want to transmit garbage data to the remote machine
                // Hide the renderer.
                skinnedMeshRenderer.enabled = false;
                stringBuilder.Append("0|");
            }
            else
            {
                // Data is valid.
                // Show the renderer.
                skinnedMeshRenderer.enabled = true;

                stringBuilder.Append("1|");

                //Set bone transform from SkeletonPoseData
                for (var i = 0; i < allBones.Count; ++i)
                {
                    allBones[i].transform.localRotation = data.BoneRotations[i].FromFlippedZQuatf();

                    stringBuilder.Append(allBones[i].transform.localEulerAngles.x + "|" + allBones[i].transform.localEulerAngles.y + "|" + allBones[i].transform.localEulerAngles.z + "|");
                }
            }

            return stringBuilder.ToString();
        }
        
        /// <summary>
        /// Called from HandPoseSync when new network data arrives.
        /// </summary>
        public void DeserializeSkeletalData(string netHandData)
        {
            if (string.IsNullOrEmpty(netHandData) || !IsOnline() || realtimeView.isOwnedLocallyInHierarchy)
            {
                // Invalid data or we ar the local player. No need to update.
                return;
            }

            string[] dataArray = netHandData.Split('|');

            if (dataArray[0] == "0")
            {
                // Hand was turned off.
                skinnedMeshRenderer.enabled = false;

                return;
            }
            else if (dataArray[0] == "1")
            {
                // Hand was turned on.
                skinnedMeshRenderer.enabled = true;
            }

            int startIndex = 1;
            for (var i = 0; i < allBones.Count; ++i)
            {
                int tmpBoneCount = i * 3;

                allBones[i].transform.localEulerAngles = new Vector3(
                    float.Parse(dataArray[startIndex + tmpBoneCount], CultureInfo.InvariantCulture),
                    float.Parse(dataArray[startIndex + 1 + tmpBoneCount], CultureInfo.InvariantCulture), 
                    float.Parse(dataArray[startIndex + 2 + tmpBoneCount], CultureInfo.InvariantCulture));
            }
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