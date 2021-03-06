﻿using System;
using PostSharp.Aspects;
using SmartReactives.Core;

namespace SmartReactives.PostSharp
{
    /// <summary>
    /// Allows this property to be used by the reactive framework.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property)]
    public class ReactiveVariableAttribute : LocationInterceptionAspect, IInstanceScopedAspect
    {
        public object CreateInstance(AdviceArgs adviceArgs)
        {
            return new ReactiveVariableAttribute();
        }

        public void RuntimeInitializeInstance()
        {
        }

        /// <inheritdoc />
        public sealed override void OnGetValue(LocationInterceptionArgs args)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (this != null) //This is because of the case in TestPropertyAccessDuringConstructionReflection
            {
                ReactiveManager.WasRead(this);
            }

            args.ProceedGetValue();
        }

        /// <inheritdoc />
        public sealed override void OnSetValue(LocationInterceptionArgs args)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (this == null) //This is because of the case in TestPropertyAccessDuringConstructionReflection
            {
                args.ProceedSetValue();
                return;
            }

            args.ProceedSetValue();
            ReactiveManager.WasChanged(this);
        }
    }
}