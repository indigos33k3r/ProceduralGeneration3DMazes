#if !ZEN_NOT_UNITY3D

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    public class MonoBehaviourSingletonProvider : ProviderBase
    {
        Component _instance;
        DiContainer _container;
        Type _componentType;
        GameObject _gameObject;

        public MonoBehaviourSingletonProvider(
            Type componentType, DiContainer container, GameObject gameObject)
        {
            Assert.That(componentType.DerivesFrom<Component>());

            _gameObject = gameObject;
            _componentType = componentType;
            _container = container;
        }

        public override Type GetInstanceType()
        {
            return _componentType;
        }

        public override object GetInstance(InjectContext context)
        {
            Assert.That(_componentType.DerivesFromOrEqual(context.MemberType));

            if (_instance == null)
            {
                Assert.That(!_container.IsValidating,
                    "Tried to instantiate a MonoBehaviour with type '{0}' during validation. Object graph: {1}", _componentType, context.GetObjectGraphString());

                // Note that we always want to cache _container instead of using context.Container 
                // since for singletons, the container they are accessed from should not determine
                // the container they are instantiated with
                // Transients can do that but not singletons

                _instance = _gameObject.AddComponent(_componentType);
                Assert.That(_instance != null);

                _container.Inject(_instance);
            }

            return _instance;
        }

        public override IEnumerable<ZenjectResolveException> ValidateBinding(InjectContext context)
        {
            return _container.ValidateObjectGraph(_componentType, context);
        }
    }
}

#endif
