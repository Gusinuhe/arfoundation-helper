using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Angus.ARImageTracker
{
    public class ImageTrackerManager : MonoBehaviour
    {
        [SerializeField] 
        private ARSession session;

        [SerializeField] 
        private ARTrackedImageManager trackerManager;

        private bool initialized = false;

        /// <summary>
        /// 
        /// </summary>
        public bool Initialized => initialized;


        private List<IImageTracker> trackers = new List<IImageTracker>();

        private IEnumerator Start()
        {
            ValidateSetup();
            CheckForSerializedImageLibrary();

            if (ARSession.state == ARSessionState.None || ARSession.state == ARSessionState.CheckingAvailability)
            {
                yield return ARSession.CheckAvailability();
            }

            if (ARSession.state == ARSessionState.Unsupported)
            {
                // Start some fallback experience for unsupported devices
            }
            else
            {
                // Start the AR session
                initialized = true;
            }
        }

        private void OnEnable()
        {
            trackerManager.trackedImagesChanged += OnTrackedImagesChanged;
        }

        private void OnDisable()
        {
            trackerManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }

        public bool RegisterTracker(IImageTracker tracker)
        {
            if (trackers.Contains(tracker))
            {
                trackers.Add(tracker);
                return true;
            }

            return false;
        }

        public bool UnregisterTracker(IImageTracker tracker)
        {
            if (trackers.Contains(tracker))
            {
                trackers.Remove(tracker);
                return true;
            }

            return false;
        }

        private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            foreach (ARTrackedImage trackedImage in eventArgs.added)
                UpdateTrackers(trackedImage, true);

            foreach (ARTrackedImage trackedImage in eventArgs.updated)
                UpdateTrackers(trackedImage, true);

            foreach (ARTrackedImage trackedImage in eventArgs.removed)
                UpdateTrackers(trackedImage, false);
        }

        private void UpdateTrackers(ARTrackedImage trackedImage, bool detected)
        {
            if (initialized)
            {
                foreach (IImageTracker tracker in trackers)
                {
                    if (trackedImage.referenceImage.texture == tracker.Image)
                    {
                        if (detected)
                        {
                            tracker.OnTrackingFound(trackedImage);
                        }
                        else
                        {
                            tracker.OnTrackingLost(trackedImage);
                        }
                    }
                }
            }
        }

        private void CheckForSerializedImageLibrary()
        {
            if (trackerManager.referenceLibrary == null)
            {
                throw new NullReferenceException("The AR Tracked Image Manager does not have a Serialized Library.");

#if UNITY_EDITOR
                UnityEditor.Selection.activeObject = trackerManager;
#endif
            }

            if (trackerManager.referenceLibrary.count == 0)
            {
                throw new NullReferenceException(
                    "The Serialized Library in the AR Tracked Image Manager does not have any referenced image.");

#if UNITY_EDITOR
                UnityEditor.Selection.activeObject = trackerManager;
#endif
            }
        }
        
        internal void ValidateSetup()
        {
            if (session == null)
            {
                session = FindObjectOfType<ARSession>();
        
                if (session == null)
                {
                    throw new NullReferenceException("ARSession could not be found, please add it to the scene.");
                }
            }
        
            if (FindObjectOfType<ARSessionOrigin>() == null)
            {
                throw new NullReferenceException("ARSessionOrigin could not be found, please add it to the scene.");
            }
        
            if (trackerManager == null)
            {
                trackerManager = FindObjectOfType<ARTrackedImageManager>();
        
                if (trackerManager == null)
                {
                    trackerManager = session.gameObject.AddComponent<ARTrackedImageManager>();
                }
            }
        }
    }
}
