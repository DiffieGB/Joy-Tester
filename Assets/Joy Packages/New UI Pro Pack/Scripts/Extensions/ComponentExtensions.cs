using UnityEngine;
using System.Collections;

namespace nupp
{
    public static class ComponentExtensions
    {
        public static RectTransform rectTransform(this Component instance)
        {
            return instance.transform as RectTransform;
        }
    }
}
