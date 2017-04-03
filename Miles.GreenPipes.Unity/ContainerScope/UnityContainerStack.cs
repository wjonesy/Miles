/*
 *     Copyright 2017 Adam Burton (adz21c@gmail.com)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using GreenPipes;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Miles.GreenPipes.ContainerScope;
using System;
using System.Collections.Generic;

namespace Miles.GreenPipes.Unity.ContainerScope
{
    class UnityContainerStack : IContainerStack, IUnityContainer
    {
        private readonly Stack<IUnityContainer> stack = new Stack<IUnityContainer>();

        public UnityContainerStack(IUnityContainer rootContainer)
        {
            stack.Push(rootContainer);
        }

        public void PushScope<TContext>(TContext context)
            where TContext : class, PipeContext
        {
            var childContainer = stack.Peek().CreateChildContainer();
            childContainer.RegisterInstance(context);

            stack.Push(childContainer);
        }

        public void PopScope()
        {
            var childContainer = stack.Pop();
            childContainer.Dispose();
        }

        #region IServiceLocator

        object IServiceLocator.GetInstance(Type serviceType)
        {
            return new UnityServiceLocator(stack.Peek()).GetInstance(serviceType);
        }

        object IServiceLocator.GetInstance(Type serviceType, string key)
        {
            return new UnityServiceLocator(stack.Peek()).GetInstance(serviceType, key);
        }

        IEnumerable<object> IServiceLocator.GetAllInstances(Type serviceType)
        {
            return new UnityServiceLocator(stack.Peek()).GetAllInstances(serviceType);
        }

        TService IServiceLocator.GetInstance<TService>()
        {
            return new UnityServiceLocator(stack.Peek()).GetInstance<TService>();
        }

        TService IServiceLocator.GetInstance<TService>(string key)
        {
            return new UnityServiceLocator(stack.Peek()).GetInstance<TService>(key);
        }

        IEnumerable<TService> IServiceLocator.GetAllInstances<TService>()
        {
            return new UnityServiceLocator(stack.Peek()).GetAllInstances<TService>();
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            return new UnityServiceLocator(stack.Peek()).GetService(serviceType);
        }

        #endregion

        #region IUnityContainer

        IUnityContainer IUnityContainer.Parent => stack.Peek().Parent;

        IEnumerable<ContainerRegistration> IUnityContainer.Registrations => stack.Peek().Registrations;

        IUnityContainer IUnityContainer.RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            stack.Peek().RegisterType(from, to, name, lifetimeManager, injectionMembers);
            return this;
        }

        IUnityContainer IUnityContainer.RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
        {
            stack.Peek().RegisterInstance(t, name, instance, lifetime);
            return this;
        }

        object IUnityContainer.Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
        {
            return stack.Peek().Resolve(t, name, resolverOverrides);
        }

        IEnumerable<object> IUnityContainer.ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
        {
            return stack.Peek().ResolveAll(t, resolverOverrides);
        }

        object IUnityContainer.BuildUp(Type t, object existing, string name, params ResolverOverride[] resolverOverrides)
        {
            return stack.Peek().BuildUp(t, existing, name, resolverOverrides);
        }

        void IUnityContainer.Teardown(object o)
        {
            stack.Peek().Teardown(o);
        }

        IUnityContainer IUnityContainer.AddExtension(UnityContainerExtension extension)
        {
            stack.Peek().AddExtension(extension);
            return this;
        }

        object IUnityContainer.Configure(Type configurationInterface)
        {
            stack.Peek().Configure(configurationInterface);
            return this;
        }

        IUnityContainer IUnityContainer.RemoveAllExtensions()
        {
            stack.Peek().RemoveAllExtensions();
            return this;
        }

        IUnityContainer IUnityContainer.CreateChildContainer()
        {
            return stack.Peek().CreateChildContainer();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}