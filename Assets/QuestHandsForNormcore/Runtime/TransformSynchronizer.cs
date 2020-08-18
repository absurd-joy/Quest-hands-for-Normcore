using UnityEngine; 
 
namespace absurdjoy 
{ 
	public class TransformSynchronizer : MonoBehaviour 
	{ 
		private Transform source; 
 
		public void AssignSourceTransform(Transform source) 
		{ 
			this.source = source; 
		} 
 
		protected void Update() 
		{ 
			if (source != null) 
			{ 
				SynchronizeTransform(); 
			} 
		} 
 
		protected virtual void SynchronizeTransform() 
		{ 
			MatchTransform(source, transform); 
		} 
 
		protected void MatchTransform(Transform source, Transform target) 
		{ 
			target.localPosition = source.localPosition; 
			target.localRotation = source.localRotation; 
			target.localScale = source.localScale; 
		} 
	} 
}