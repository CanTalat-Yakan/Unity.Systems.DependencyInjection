using UnityEngine;
using UnityEssentials;

namespace Examples.DependencyInjection
{
    public class DependencyInjectionProvider : MonoBehaviour, IDependencyProvider
    {
        [Provide]
        public ServiceA ProvideServiceA() =>
            new ServiceA();

        [Provide]
        public ServiceB ProvideServiceB() =>
            new ServiceB();

        [Provide]
        public ServiceC ProvideServiceC() =>
            new ServiceC();

        [Provide]
        public FactoryA ProvideFactoryA() =>
            new FactoryA();
    }

    public class ServiceA
    {
        public void Initialize(string message = null) =>
            Debug.Log($"ServiceA.Initialize({message})");
    }

    public class ServiceB
    {
        public void Initialize(string message = null) =>
            Debug.Log($"ServiceB.Initialize({message})");
    }

    public class ServiceC
    {
        public void Initialize(string message = null) =>
            Debug.Log($"ServiceC.Initialize({message})");
    }

    public class FactoryA
    {
        ServiceA _cachedServiceA;

        public ServiceA CreateServiceA()
        {
            if (_cachedServiceA == null)
                _cachedServiceA = new ServiceA();

            return _cachedServiceA;
        }
    }
}