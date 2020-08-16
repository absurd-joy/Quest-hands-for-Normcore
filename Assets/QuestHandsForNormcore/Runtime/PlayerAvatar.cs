using UnityEngine;
using Normal.Realtime;

namespace absurdjoy
{
    public class PlayerAvatar : MonoBehaviour
    {
        public RealtimeTransform head;
        public QuestSkeletonSerializer leftHand;
        public QuestSkeletonSerializer rightHand;

        private Transform localHead;
        private Transform localLeftHand;
        private Transform localRightHand;

        private RealtimeView realtimeView;
        
        private void OnEnable()
        {
            realtimeView = GetComponent<RealtimeView>();
        }
        
        /// <summary>
        /// Called by the spawning computer (for the local avatar only). This is not called for remote avatars.
        /// </summary>
        public void LinkWithLocal(Transform localHead, OVRCustomSkeleton localLeftSkeleton, OVRCustomSkeleton localRightSkeleton)
        {
            // RealtimeTransform requires explicit ownership to update the position.
            head.RequestOwnership();
            leftHand.GetComponent<RealtimeTransform>().RequestOwnership();
            rightHand.GetComponent<RealtimeTransform>().RequestOwnership();
            
            this.localHead = localHead;
            this.localLeftHand = localLeftSkeleton.transform.parent;
            this.localRightHand = localRightSkeleton.transform.parent;
            
            leftHand.AssignLocalSkeleton(localLeftSkeleton);
            rightHand.AssignLocalSkeleton(localRightSkeleton);
        }

        private void Update()
        {
            if (realtimeView.isOwnedLocallySelf)
            {
                // We need to manually set our avatar positions to match head/hand positions for the local stuff:
                MatchTransform(localHead, head.transform);
                MatchTransform(localLeftHand, leftHand.transform);
                MatchTransform(localRightHand, rightHand.transform);
            }
        }

        private void MatchTransform(Transform source, Transform target)
        {
            target.localPosition = source.localPosition;
            target.localRotation = source.localRotation;
            target.localScale = source.localScale;
        }
    }
}