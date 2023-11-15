using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pikachu.Scripts.Common.Miscs;

namespace Pikachu.Scripts.Common.Tasks
{
    public class DrawPairLineTask
    {
        private readonly PairLine _pairLinePrefab;

        public DrawPairLineTask()
        {
            _pairLinePrefab = Resources.Load<PairLine>("Miscs/Pair Line");
        }

        public void DrawLine(params Vector3[] points)
        {
            PairLine pairLine = SimplePool.Spawn(_pairLinePrefab
                                                 , EffectContainer.InstanceTransform
                                                 , Vector3.zero
                                                 , Quaternion.identity);
            pairLine.DrawLine(points);
        }
    }
}
