using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Angus.ARImageTracker.Editor
{
    [CustomEditor(typeof(ImageTracker))]
    public class ImageTrackerEditor : UnityEditor.Editor
    {
        [SerializeField] 
        private bool advanceSettings;
        
        [SerializeField]
        private int lastSelection;
        
        private const int PreviewImageSize = 256;

        private int imageIndex;
        private ImageTracker imageTracker;
        private bool isImageLibraryMissing;
        private IReferenceImageLibrary library;
        private bool isARTrackedImageManagerMissing;
        
        private string[] trackerNames;

        private readonly GUIContent imageLibraryContent = new GUIContent("Open Reference Image Library", "The library of images which will be detected and/or tracked in the physical environment.");
        
        
        private void OnEnable()
        {
            imageTracker = target as ImageTracker;
            imageTracker.ValidateSetup();

            ARTrackedImageManager trackerManager = FindObjectOfType<ARTrackedImageManager>();

            isARTrackedImageManagerMissing = trackerManager == null;
            isImageLibraryMissing = trackerManager?.referenceLibrary == null;

            if (!isARTrackedImageManagerMissing && !isImageLibraryMissing)
            {
                List<string> imagesNames = new List<string>();
                library = trackerManager.referenceLibrary;
                
                for (int index = 0; index < library.count; index++)
                {
                    string imageName = library[index].name;
                    imagesNames.Add(imageName);
                }
                
                Texture2D texture = GetImageAtIndex(imageIndex);
                SetTexture(texture);

                trackerNames = imagesNames.ToArray();
                lastSelection = imageIndex;
            }
            else
            {
                advanceSettings = false;
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField(EditorGUIUtility.TrTempContent("Advance Settings"));
            
            DrawDefaultInspector();
            
            EditorGUILayout.Space(10f);

            ValidateLibraryReachability();

            advanceSettings = EditorGUILayout.Foldout(advanceSettings, EditorGUIUtility.TrTempContent("Advance Settings"), true);
            
            if (advanceSettings && !isARTrackedImageManagerMissing && !isImageLibraryMissing)
            {
                EditorGUILayout.Space(4f);
                imageIndex = EditorGUILayout.Popup(EditorGUIUtility.TrTempContent("Tracked Image"), imageIndex, trackerNames);
                EditorGUILayout.Space(4f);

                Texture2D texture = GetImageAtIndex(imageIndex);
                    
                GUI.enabled = false;
                EditorGUILayout.ObjectField(texture, typeof(Texture2D), true, GUILayout.Width(PreviewImageSize), GUILayout.Height(PreviewImageSize));
                GUI.enabled = true;

                if (lastSelection != imageIndex)
                {
                    SetTexture(texture);
                }

                lastSelection = imageIndex;
                
                EditorGUILayout.Space(4f);
                
                if (GUILayout.Button(imageLibraryContent))
                {
                    XRReferenceImageLibrary imageLibrary = library as XRReferenceImageLibrary;
                    Selection.activeObject = imageLibrary;
                }
            }
        }

        private Texture2D GetImageAtIndex(int index)
        {
            XRReferenceImage referenceImage = library[index];
            string texturePath = AssetDatabase.GUIDToAssetPath(referenceImage.textureGuid.ToString("N"));
            return AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
        }

        private void SetTexture(Texture2D texture)
        {
            MeshRenderer meshRenderer = imageTracker.GetComponent<MeshRenderer>();
            meshRenderer.sharedMaterial.SetTexture("_MainTex", texture);
        }

        private void ValidateLibraryReachability()
        {
            if (isARTrackedImageManagerMissing)
            {
                EditorGUILayout.HelpBox("ARTrackedImageManager could not be found, please add it to the scene.", MessageType.Error, true);
                GUI.enabled = false;
            }
            else if (isImageLibraryMissing)
            {
                EditorGUILayout.HelpBox("ARTrackedImageManager does not have a Serialized Library referenced, please create one and reference it to ARTrackedImageManager.", MessageType.Error, true);
                GUI.enabled = false;
            }
        }
    }
}