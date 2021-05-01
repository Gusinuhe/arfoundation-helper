using System;
using UnityEngine;
using UnityEditor;

namespace Angus.ARImageTracker.Editor
{
    [CustomEditor(typeof(ImageTrackerManager))]
    public class ImageTrackerManagerEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            ImageTrackerManager trackerManager = target as ImageTrackerManager;
            trackerManager.ValidateSetup();
        }

        // public void GenerateLibFromSceneSetting()
        // {
        //     ImageTracker[] trackers = GameObject.FindObjectsOfType<ImageTracker>();
        //
        //     Transform[] objs = new Transform[trackers.Length];
        //     XRReferenceImageLibrary newLib = ScriptableObject.CreateInstance<XRReferenceImageLibrary>();
        //
        //     string debugText = "Found " + trackers.Length + " Trackers in Scene:\n";
        //     for (int i = 0; i < trackers.Length; i++)
        //     {
        //         debugText += "  " + trackers[i].gameObject.name + "\n";
        //         objs[i] = trackers[i].transform;
        //
        //         XRReferenceImageLibraryExtensions.Add(newLib);
        //         XRReferenceImageLibraryExtensions.SetName(newLib, i, trackers[i].name);
        //         XRReferenceImageLibraryExtensions.SetTexture(newLib, i, trackers[i].trackerImage, false);
        //         XRReferenceImageLibraryExtensions.SetSpecifySize(newLib, i, true);
        //         XRReferenceImageLibraryExtensions.SetSize(newLib, i, trackers[i].physicalSize);
        //     }
        //
        //     string targetPath = Resources.Load<ARFoundationHelperSettings>("HelperSettings").GeneratedLibrarySavePath;
        //     AssetDatabase.CreateAsset(newLib, targetPath + "generated-lib.asset");
        //
        //     //((ImageTrackerManager)target).targetLib = newLib;
        //     // ((ImageTrackerManager)target).trackerObjs = objs;
        //
        //     Debug.Log(debugText);
        // }
    }
}