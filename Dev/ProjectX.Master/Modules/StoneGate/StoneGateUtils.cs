using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectX.Master.Modules.StoneGate
{
    public static class StoneGateUtils
    {
        // Helper to avoid MissingMethodException on GetComponentsInChildren
        public static List<T> GetComponentsInChildrenRecursive<T>(Transform t) where T : Component
        {
            var list = new List<T>();
            if (t == null) return list;

            ProcessRecursive(t, list);
            return list;
        }

        private static void ProcessRecursive<T>(Transform t, List<T> list) where T : Component
        {
            var comp = t.GetComponent<T>();
            if (comp != null)
            {
                list.Add(comp);
            }

            for (int i = 0; i < t.childCount; i++)
            {
                ProcessRecursive(t.GetChild(i), list);
            }
        }
    }
}
