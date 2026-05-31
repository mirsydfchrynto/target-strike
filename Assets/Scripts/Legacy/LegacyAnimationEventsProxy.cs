using UnityEngine;

namespace TargetStrike.Legacy
{
    /// <summary>
    /// This script exists purely to catch and silence legacy animation events 
    /// from the Infima Low Poly Shooter Pack animations.
    /// </summary>
    public class LegacyAnimationEventsProxy : MonoBehaviour
    {
        // Add empty methods for all known Infima Animation Events
        public void OnAnimationEndedHolster() {}
        public void OnAnimationEndedUnholster() {}
        public void OnAnimationEndedReload() {}
        public void OnAnimationEndedReloadEmpty() {}
        public void OnAnimationEndedFire() {}
        public void OnAnimationEndedBolt() {}
        public void OnAnimationEndedGrenade() {}
        public void OnAnimationEndedMelee() {}
        public void OnAnimationEndedInspect() {}
        
        // Audio events
        public void PlaySound(Object audioClip) {}
    }
}
