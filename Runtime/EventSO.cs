using UnityEngine;

namespace Marty
{
    [CreateAssetMenu(menuName = "MartyLib/Event SO")]
    public class EventSO : ScriptableObject
    {
        public event System.Action onEventRaised = null;

        public void RaiseEvent()
        {
            onEventRaised?.Invoke();
        }
    }

    public class EventSO<T> : ScriptableObject
    {
        public event System.Action<T> onEventRaised = null;

        public void RaiseEvent(T value)
        {
            onEventRaised?.Invoke(value);
        }
    }

    [CreateAssetMenu(menuName = "MartyLib/Bool Event SO")]
    public class BoolEventSO : EventSO<bool> { };

    [CreateAssetMenu(menuName = "MartyLib/Int Event SO")]
    public class IntEventSO : EventSO<int> { };

    [CreateAssetMenu(menuName = "MartyLib/Float Event SO")]
    public class FloatEventSO : EventSO<float> { };

    [CreateAssetMenu(menuName = "MartyLib/String Event SO")]
    public class StringEventSO : EventSO<string> { };
}
