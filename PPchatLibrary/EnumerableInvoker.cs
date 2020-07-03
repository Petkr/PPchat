using System;
using System.Collections.Generic;

namespace PPchatLibrary
{
	readonly struct EnumerableInvoker<Caller, Parameter> : IInvoker<Caller, Parameter>
	{
		readonly IEnumerable<IInvoker<Caller, Parameter>> Invokers;

		public EnumerableInvoker(IEnumerable<IInvoker<Caller, Parameter>> invokers)
		{
			Invokers = invokers;
		}

		public void Invoke(Caller caller, Parameter parameter)
		{
			Exception? last_exception = new Exception();

			foreach (var invoker in Invokers)
			{
				try
				{
					invoker.Invoke(caller, parameter);
					last_exception = null;
					break;
				}
				catch (Exception e)
				{
					last_exception = e;
				}
			}

			if (last_exception != null)
				throw last_exception;
		}
	}
}
