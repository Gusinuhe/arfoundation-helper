using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Angus.ARImageTracker
{
    /// <summary>
    /// 
    /// </summary>
    [DisallowMultipleComponent]
    public class ImageTracker : BaseImageTracker
    {
        [Header("Configuration")]
        
        [SerializeField, Tooltip("")]
        private ImageTrackerManager trackerManager;

        [Header("Public Events")] 
        
        public UnityEvent trackingFound;
        public UnityEvent trackingLost;

        [Header("Debug")] 
        
        [SerializeField, Tooltip("")]
        private bool verbose;

        [SerializeField, HideInInspector] private Texture2D image;

        private float timeSinceLastARFrame;

        /// <inheritdoc/>
        public new Texture2D Image => image;

        protected override void Start()
        {
            ValidateSetup();
        }

        /// <inheritdoc/>
        public override void OnTrackingFound(ARTrackedImage trackedImage)
        {
            Vector3 position = trackedImage.transform.position;
            Quaternion rotation = trackedImage.transform.rotation;
            transform.SetPositionAndRotation(position, rotation);

            trackedImage.transform.localScale = new Vector3(trackedImage.size.x, 1f, trackedImage.size.y);

            trackingFound.Invoke();

            if (verbose)
            {
                Debug.Log($"{name} - Tracking Found.");
            }

            IsTracking = true;
        }

        /// <inheritdoc/>
        public override void OnTrackingLost(ARTrackedImage trackedImage)
        {
            trackingLost.Invoke();

            if (verbose)
            {
                Debug.Log($"{name} - Tracking Lost");
            }

            IsTracking = false;
        }
        
        internal void ValidateSetup()
        {
            if (trackerManager == null)
            {
                trackerManager = FindObjectOfType<ImageTrackerManager>();

                if (trackerManager == null)
                {
                    trackerManager = CreateTrackerManager();
                }
            }
        }
    }
}
