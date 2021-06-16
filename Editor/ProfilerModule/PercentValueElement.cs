using UnityEngine.UIElements;

namespace Unity.Profiling.Editor.SystemMetrics.Mali
{
    public class PercentValueElement : VisualElement
    {
        Label m_Label;
        VisualElement m_BarFill;

        public PercentValueElement()
        {
            m_Label = new Label();
            m_Label.AddToClassList("percent-value-label");
            Add(m_Label);

            var bar = new VisualElement();
            bar.AddToClassList("percent-value-bar");
            m_BarFill = new VisualElement();
            m_BarFill.AddToClassList("percent-value-bar__fill");
            bar.Add(m_BarFill);
            Add(bar);
        }

        public void SetPercent(float percent)
        {
            m_Label.text = ProfilerCounterFormatter.FormatPercentage(percent);
            m_BarFill.style.width = new Length(percent, LengthUnit.Percent);
        }

        public new class UxmlFactory : UxmlFactory<PercentValueElement> { }
    }
}