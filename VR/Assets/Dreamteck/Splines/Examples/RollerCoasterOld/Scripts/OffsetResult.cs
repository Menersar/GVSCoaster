using UnityEngine;
using System.Collections;

namespace Dreamteck.Splines.Examples
{
    public class OffsetResult : MonoBehaviour
    {
        public SplineFollower follower;
        public float distanceOffset = 0f;

        // Update is called once per frame
        void LateUpdate()
        {
          //  SplineResult result = follower.Evaluate(follower.Travel(follower.result.percent, distanceOffset, follower.direction));
        //    transform.position = result.position;
       //     transform.rotation = result.rotation;
        }
    }
}
