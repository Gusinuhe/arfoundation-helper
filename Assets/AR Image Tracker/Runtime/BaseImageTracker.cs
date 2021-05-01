using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Angus.ARImageTracker
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseImageTracker : MonoBehaviour, IImageTracker
    {
        /// <inheritdoc/>
        public Texture2D Image => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool IsTracking { get; protected set; }

        protected virtual void Start()
        {
            OnTrackingLost(null);
        }

        /// <inheritdoc/>
        public abstract void OnTrackingFound(ARTrackedImage trackedImage);

        /// <inheritdoc/>
        public abstract void OnTrackingLost(ARTrackedImage trackedImage);

        protected ImageTrackerManager CreateTrackerManager()
        {
            GameObject trackerManagerObject = new GameObject("ImageTrackerManager");
            return trackerManagerObject.AddComponent<ImageTrackerManager>();
        }
    }
}