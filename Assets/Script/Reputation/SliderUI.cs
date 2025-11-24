using UnityEngine;
using UnityEngine.UI;

namespace Reputation
{
    [RequireComponent(typeof(Slider))]
    public class SliderUI : MonoBehaviour
    {
        private Slider slider;
        void Start()
        {
            slider = GetComponent<Slider>();
            UpdatedUI();
        }

        private void UpdatedUI()
        {
            float ratio = ReputationSystem.Instance.GetReputationRatio();
            slider.value = ratio;
        }

        private void OnEnable()
        {
            ReputationSystem.OnReputationChanged += UpdatedUI;
        }

        private void OnDisable()
        {
            ReputationSystem.OnReputationChanged -= UpdatedUI;
        }
        private void OnDestroy()
        {
            ReputationSystem.OnReputationChanged -= UpdatedUI;
        }

    }
}

