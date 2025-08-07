using System;
using System.Reflection;
using AudioVisualization;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Utilities
{
    public static class Helpers
    {
        public static void LogObjectDetails(object obj, string label = "")
        {
            if (obj == null)
            {
                Debug.Log($"{label} [null object]");
                return;
            }

            Type type = obj.GetType();
            string log = $"--- {label} ({type.Name}) ---\n";

            // Log all public properties
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                try
                {
                    object value = prop.GetValue(obj, null);
                    log += $"{prop.Name}: {value}\n";
                }
                catch
                { /* ignore inaccessible properties */
                }
            }

            // Log all public fields
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                try
                {
                    object value = field.GetValue(obj);
                    log += $"{field.Name}: {value}\n";
                }
                catch
                { /* ignore */
                }
            }

            log += "-----------------------------";
            Debug.Log(log);
        }
    }
}
