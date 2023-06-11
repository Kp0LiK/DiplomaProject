using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;


// ReSharper disable InconsistentNaming

namespace Client
{
    public static class FloatExtensions
    {
        private const float EPSILON = 0.00001f;
        
        public static TweenerCore<float, float, FloatOptions> DOValue(this float target, float endValue, float duration,
            bool snapping = false)
        {
            var t = DOTween.To(() => target, x => target = x, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }
        

        public static float Remap(this float value, float from1, float to1, float from2, float to2) =>
            (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        
    }
}