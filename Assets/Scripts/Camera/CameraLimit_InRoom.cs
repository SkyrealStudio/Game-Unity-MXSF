using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Scripts.Helper
{
    public class CameraLimit_InRoom : MonoBehaviour
    {
        public Vector2Int[] LimitPoints;
        public void SetLimit(Vector2Int[] points)
        {
            if(points.Length==4)
            {
                LimitPoints = points;
            }
            else
            {
                throw new System.Exception("Incorrect Point Length");
            }
        }

    }
}
