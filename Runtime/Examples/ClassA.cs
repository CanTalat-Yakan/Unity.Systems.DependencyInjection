using UnityEngine;
using UnityEssentials;

namespace Examples.DependencyInjection
{
    public class ClassA : MonoBehaviour
    {
        [Inject]
        private ServiceA _serviceA;

        private ServiceB _serviceB;

        [Inject]
        public void Init(ServiceB serviceB)
        {
            _serviceB = serviceB;
            Debug.Log("ClassA initialized with ServiceB");
        }

        [Inject]
        public ServiceC ServiceC { get; private set; }
    }
}
