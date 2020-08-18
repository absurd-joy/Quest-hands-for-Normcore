using UnityEngine;
using Normal.Realtime;

namespace absurdjoy
{
    public class PlayerAvatar : MonoBehaviour
    {
        public TransformSynchronizer head;
        public TransformSynchronizer leftHand;
        public TransformSynchronizer rightHand;
        
        /// <summary>
        /// Called by the spawning computer (for the local avatar only). This is not called for remote avatars.
        /// </summary>
        public void LinkWithLocal(Transform localHead, Transform localLeftHand, Transform localRightHand)
        {
            // RealtimeTransform requires explicit ownership to update the position.
            RequestOwnershipRecursive(transform);

            head.AssignSourceTransform(localHead, true);
            leftHand.AssignSourceTransform(localLeftHand, true);
            rightHand.AssignSourceTransform(localRightHand, true);

            leftHand.GetComponent<IAssignSkeleton>().AssignLocalSkeleton(localLeftHand.GetComponentInChildren<OVRSkeleton.IOVRSkeletonDataProvider>());
            rightHand.GetComponent<IAssignSkeleton>().AssignLocalSkeleton(localRightHand.GetComponentInChildren<OVRSkeleton.IOVRSkeletonDataProvider>());
        }

        private void RequestOwnershipRecursive(Transform target)
        {
            var rtt = GetComponent<RealtimeTransform>();
            if (rtt != null)
            {
                rtt.RequestOwnership();
            }
            
            for (int i = 0; i < target.childCount; i++)
            {
                RequestOwnershipRecursive(target.GetChild(i));
            }
        }
    }
}