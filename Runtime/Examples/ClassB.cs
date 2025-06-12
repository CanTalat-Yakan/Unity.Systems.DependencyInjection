using UnityEngine;
using UnityEssentials;

namespace Examples.DependencyInjection
{
    public class ClassB : MonoBehaviour
    {
        [Inject]
        private ServiceA _serviceA;

        private ServiceB _serviceB;
        private FactoryA _factoryA;

        [Inject] // Method injection supports multiple dependencies
        public void Initialize(ServiceB serviceB, FactoryA factoryA)
        {
            _serviceB = serviceB;
            _factoryA = factoryA;
            Debug.Log("ClassB initialized with ServiceB and FactoryA");
        }

        public void Start()
        {
            _serviceA.Initialize("ServiceA initialized from ClassB");
            _serviceB.Initialize("ServiceB initialized from ClassB");
            _factoryA.CreateServiceA().Initialize("ServiceA initialized from FactoryA");
        }
    }
}