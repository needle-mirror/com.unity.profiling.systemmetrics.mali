using UnityEngine;

namespace Unity.Profiling.SystemMetrics.Tests
{
    internal class SimpleRotator : MonoBehaviour
    {
        public float Angle = 0;

        void Update()
        {
            Angle += Time.deltaTime * 60;
            transform.rotation = Quaternion.AngleAxis(Angle, Vector3.up);
        }
    }
}