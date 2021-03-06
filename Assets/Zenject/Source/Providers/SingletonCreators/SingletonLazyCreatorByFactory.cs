using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;

namespace Zenject
{
    internal class SingletonLazyCreatorByFactory<TContract, TFactory> : SingletonLazyCreatorBase
        where TFactory : IFactory<TContract>
    {
        readonly DiContainer _container;

        object _instance;

        public SingletonLazyCreatorByFactory(
            SingletonId id, SingletonProviderMap owner, DiContainer container)
            : base(id, owner)
        {
            _container = container;
        }

        public override object GetInstance(InjectContext context)
        {
            if (_instance != null)
            {
                return _instance;
            }

            // Note that we always want to cache _container instead of using context.Container 
            // since for singletons, the container they are accessed from should not determine
            // the container they are instantiated with
            // Transients can do that but not singletons
            _instance = _container.Instantiate<TFactory>().Create();

            if (_instance == null)
            {
                throw new ZenjectResolveException(
                    "Failed to instantiate type '{0}' in SingletonLazyCreatorByFactory".Fmt(context.MemberType));
            }

            return _instance;
        }

        public override IEnumerable<ZenjectResolveException> ValidateBinding(InjectContext context)
        {
            if (typeof(TFactory).DerivesFrom<IValidatable>())
            {
                var factory = _container.Instantiate<TFactory>(context);
                return ((IValidatable)factory).Validate();
            }

            return Enumerable.Empty<ZenjectResolveException>();
        }
    }
}

