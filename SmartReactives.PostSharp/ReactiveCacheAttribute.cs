﻿using System;
using PostSharp.Aspects;
using SmartReactives.Common;

namespace SmartReactives.PostSharp
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property)]
    public class ReactiveCacheAttribute : LocationInterceptionAspect, IInstanceScopedAspect
    {
        [NonSerialized] ReactiveCache<object> cache;

        public object CreateInstance(AdviceArgs adviceArgs)
        {
            return new ReactiveCacheAttribute();
        }

        public void RuntimeInitializeInstance()
        {
        }

        public sealed override void OnGetValue(LocationInterceptionArgs args)
        {
            cache = cache ?? new ReactiveCache<object>(args.GetCurrentValue);
            args.Value = cache.Get();
        }
    }
}