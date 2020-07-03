using System;
using System.Reflection;

namespace PPchatLibrary
{
	class BasicDescriptor<CallerType, ParameterType, TypeToScan> : IInvoker<CallerType, ParameterType>
		where TypeToScan : CallerType
	{
		readonly static object[] arrayHelper = new object[1];

		readonly MethodInfo MethodInfo;

		public BasicDescriptor(Type genericInterfaceDefinitionType, Type typeArgumentType)
		{
			MethodInfo = typeof(TypeToScan)
							.GetInterfaceMap(genericInterfaceDefinitionType.MakeGenericType(typeArgumentType))
							.TargetMethods[0];
		}

		public void Invoke(CallerType caller, ParameterType parameter)
		{
			arrayHelper[0] = parameter!;
			try
			{
				MethodInfo.Invoke(caller, arrayHelper);
			}
			catch (TargetInvocationException e)
			{
				throw e.GetBaseException();
			}
		}
	}
}
