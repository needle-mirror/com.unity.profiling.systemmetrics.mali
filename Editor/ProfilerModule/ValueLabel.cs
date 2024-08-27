using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Unity.Profiling.Editor.SystemMetrics.Mali
{
    #if UNITY_6000_0_OR_NEWER
    [UxmlElement]
    #endif
    internal partial class ValueLabel : Label
    {
        #if UNITY_6000_0_OR_NEWER
        [UxmlAttribute]
        #endif
        public ProfilerMarkerDataUnit Unit { get; private set; }

        public void SetValue(long value)
        {
            text = ProfilerCounterFormatter.FormatValue(value, Unit);
        }

        #if !UNITY_6000_0_OR_NEWER
        public new class UxmlFactory : UxmlFactory<ValueLabel, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlEnumAttributeDescription<ProfilerMarkerDataUnit> m_Unit = new UxmlEnumAttributeDescription<ProfilerMarkerDataUnit> { name = "unit" };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ((ValueLabel)ve).Unit = m_Unit.GetValueFromBag(bag, cc);
            }
        }
        #endif
    }
}
