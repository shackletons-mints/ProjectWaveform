using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using AudioVisualization;


namespace Utilities

{
    public static class Helpers
	{

		public static Texture2D DebugRenderTexture(RenderTexture rt, string name = "DebugRT")
		{
			RenderTexture currentRT = RenderTexture.active;
			RenderTexture.active = rt;

			Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false, false);
			tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
			tex.Apply();

			RenderTexture.active = currentRT;

			byte[] bytes = tex.EncodeToPNG();
			string path = Application.dataPath + $"/../{name}.png";
			System.IO.File.WriteAllBytes(path, bytes);
			Debug.Log($"RenderTexture saved to: {path}");

			return tex;
		}

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
				catch { /* ignore inaccessible properties */ }
			}

			// Log all public fields
			foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
			{
				try
				{
					object value = field.GetValue(obj);
					log += $"{field.Name}: {value}\n";
				}
				catch { /* ignore */ }
			}

			log += "-----------------------------";
			Debug.Log(log);
		}
	}
}

