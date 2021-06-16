using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Unity.Profiling.Editor.SystemMetrics.Mali
{
    public class ValueLabel : Label
    {
        public ProfilerMarkerDataUnit Unit { get; private set; }

        public void SetValue(long value)
        {
            text = ProfilerCounterFormatter.FormatValue(value, Unit);
        }

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
    }
}