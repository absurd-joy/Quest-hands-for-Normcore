using System.Reflection;
using Normal.Realtime;
using UnityEditor;
using UnityEngine;

namespace absurdjoy
{
    public class AddTransformSyncUtility
    {
        [MenuItem("GameObject/absurd:joy/TransformSync/Add to all children", false, 0)]
        public static void AddTransformSyncFromSelection()
        {
            Undo.IncrementCurrentGroup();
            var undoIndex = Undo.GetCurrentGroup();
            foreach (var selection in Selection.transforms)
            {
                AddTransformSyncRecursively(selection);
            }
            Undo.CollapseUndoOperations(undoIndex);
        }

        [MenuItem("GameObject/absurd:joy/TransformSync/Remove from all children", false, 0)]
        public static void RemoveTransformSyncFromSelection()
        {
            Undo.IncrementCurrentGroup();
            var undoIndex = Undo.GetCurrentGroup();
            foreach (var selection in Selection.transforms)
            {
                RemoveTransformSyncFromSelectionRecursive(selection);
            }
            Undo.CollapseUndoOperations(undoIndex);
        }

        public static void RemoveTransformSyncFromSelectionRecursive(Transform to)
        {
            var go = to.gameObject;
            if (go.GetComponent<TransformSynchronizer>() != null)
            {
                Undo.DestroyObjectImmediate(go.GetComponent<TransformSynchronizer>());
            }

            EditorUtility.SetDirty(to);
            
            for (int i = 0; i < to.childCount; i++)
            {
                RemoveTransformSyncFromSelectionRecursive(to.GetChild(i));
            }            
        }

        public static void AddTransformSyncRecursively(Transform to)
        {
            var go = to.gameObject;
            if (go.GetComponent<TransformSynchronizer>() == null)
            {
                Undo.AddComponent<TransformSynchronizer>(go);
            }

            EditorUtility.SetDirty(to);
            
            for (int i = 0; i < to.childCount; i++)
            {
                AddTransformSyncRecursively(to.GetChild(i));
            }
        }
    }
}