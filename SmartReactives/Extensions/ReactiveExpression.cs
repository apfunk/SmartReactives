using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using SmartReactives.Core;

namespace SmartReactives.Extensions
{
    /// <summary>
    /// Captures an expression whose value may change over time.
    /// The different values the expression has over time are exposed as an <see cref="IObservable{T}"/>. After subscribing an initial notification is pushed immediately.
    /// Make sure the provided expression only depends on constant values or reactive objects, such as <see cref="ReactiveVariable{T}"/> or <see cref="ReactiveExpression{T}"/>
    /// </summary>
    [Serializable]
	public class ReactiveExpression<T> : IObservable<Func<T>>, IListener
	{
	    readonly Func<T> expression;
	    readonly ISubject<Func<T>> subject = new Subject<Func<T>>();

		public ReactiveExpression(Func<T> expression)
		{
			this.expression = expression;
		}

		/// <summary>
		/// Evaluate the expression and return its value.
		/// </summary>
		public T Evaluate()
		{
			return ReactiveManager.Evaluate(this, expression);
		}

        void IListener.Notify()
		{
			subject.OnNext(Evaluate);
		}

		public IDisposable Subscribe(IObserver<Func<T>> observer)
		{
            observer.OnNext(Evaluate);
			return subject.Subscribe(observer);
		}

		// ReSharper disable once UnusedMember.Local
		/// <summary>
		/// For debugging purposes.
		/// </summary>
		internal IEnumerable<object> Dependents => ReactiveManager.GetDependents(this);
	}
}
