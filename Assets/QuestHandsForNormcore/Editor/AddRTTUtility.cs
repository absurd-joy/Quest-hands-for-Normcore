using System.Reflection;
using Normal.Realtime;
using Normal.Utility;
using UnityEditor;
using UnityEngine;

namespace absurdjoy
{
    public class AddRTTUtility
    {
        public static bool trackPosition = false;
        public static bool trackRotation = true;
        public static bool trackScale = false;
        public static bool trackVelocity = false;
        public static bool interpolate = true;

        [MenuItem("GameObject/absurd:joy/Add RealtimeTransform to all children", false, 0)]
        public static void AddRTTComponentsToSelection()
        {
            Undo.IncrementCurrentGroup();
            var undoIndex = Undo.GetCurrentGroup();
            foreach (var selection in Selection.transforms)
            {
                AddRTTComponentsRecursively(selection);
            }
            Undo.CollapseUndoOperations(undoIndex);
        }

        public static void AddRTTComponentsRecursively(Transform to)
        {
            var go = to.gameObject;
            if (go.GetComponent<RealtimeView>() == null)
            {
                Undo.AddComponent<RealtimeView>(go);
            }

            if (go.GetComponent<RealtimeTransform>() == null)
            {
                Undo.AddComponent<RealtimeTransform>(go);
            }

            Undo.RecordObject(go, "Change RealtimeTransform settings");
            var rtt = go.GetComponent<RealtimeTransform>();
            SetPrivateVariable(typeof(RealtimeTransform), "_syncPosition", trackPosition, rtt);
            SetPrivateVariable(typeof(RealtimeTransform), "_syncRotation", trackRotation, rtt);
            SetPrivateVariable(typeof(RealtimeTransform), "_syncScale", trackScale, rtt);
            SetPrivateVariable(typeof(RealtimeTransform), "_syncVelocity", trackVelocity, rtt);
            SetPrivateVariable(typeof(RealtimeTransform), "_interpolate", interpolate, rtt);

            EditorUtility.SetDirty(to);
            
            for (int i = 0; i < to.childCount; i++)
            {
                AddRTTComponentsRecursively(to.GetChild(i));
            }
        }

        public static void SetPrivateVariable(System.Type t, string varName, object varValue, object objInstance)
        {
            BindingFlags eFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            FieldInfo fieldInfo = t.GetField(varName, eFlags);
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(objInstance, varValue);
            }
            else
            {
                Debug.LogError("Property `"+varName+"` not found");
            }
        }
    }
}