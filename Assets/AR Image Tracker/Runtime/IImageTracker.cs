using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Angus.ARImageTracker
{
    /// <summary>
    /// 
    /// </summary>
    public interface IImageTracker
    {
        /// <summary>
        /// 
        /// </summary>
        Texture2D Image { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IsTracking { get; }

        /// <summary>
        /// 
        /// </summary>
        void OnTrackingFound(ARTrackedImage trackedImage);

        /// <summary>
        /// 
        /// </summary>
        void OnTrackingLost(ARTrackedImage trackedImage);
    }
}