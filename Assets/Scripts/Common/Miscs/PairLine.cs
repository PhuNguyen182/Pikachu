using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pikachu.Scripts.Common.Miscs
{
    public class PairLine : MonoBehaviour
    {
        [SerializeField] private LineRenderer line;

        public void DrawLine(Vector3[] points)
        {
            line.enabled = false;
            line.positionCount = points.Length;
            line.SetPositions(points);
            line.enabled = true;
        }
    }
}