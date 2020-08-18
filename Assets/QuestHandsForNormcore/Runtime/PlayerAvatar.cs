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

            head.AssignSourceTransform(localHead);
            leftHand.AssignSourceTransform(localLeftHand);
            rightHand.AssignSourceTransform(localRightHand);

            foreach (var assignable in leftHand.GetComponents<IAssignSkeleton>())
            {
                assignable.AssignLocalSkeleton(localLeftHand.GetComponentInChildren<OVRSkeleton>());
            }
            foreach (var assignable in rightHand.GetComponents<IAssignSkeleton>())
            {
                assignable.AssignLocalSkeleton(localRightHand.GetComponentInChildren<OVRSkeleton>());
            }
        }

        private void RequestOwnershipRecursive(Transform target)
        {
            var rtt = target.GetComponent<RealtimeTransform>();
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