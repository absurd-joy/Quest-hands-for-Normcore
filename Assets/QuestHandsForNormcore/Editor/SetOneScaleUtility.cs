using UnityEditor;
using UnityEngine;

namespace absurdjoy
{
    public class SetOneScaleUtility : MonoBehaviour
    {
        [MenuItem("GameObject/absurd:joy/Set all child scales to Vector3.One", false, 0)]
        public static void SetAllChildScalesToOne()
        {
            Undo.IncrementCurrentGroup();
            var undoIndex = Undo.GetCurrentGroup();
            foreach (var selection in Selection.transforms)
            {
                SetChildScalesToOneRecursively(selection);
            }

            Undo.CollapseUndoOperations(undoIndex);
        }

        public static void SetChildScalesToOneRecursively(Transform to)
        {
            Undo.RecordObject(to, "scale");
            to.localScale = Vector3.one;
            EditorUtility.SetDirty(to);

            for (int i = 0; i < to.childCount; i++)
            {
                SetChildScalesToOneRecursively(to.GetChild(i));
            }
        }
    }
}