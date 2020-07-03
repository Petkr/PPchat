namespace PPchatLibrary
{
	interface IParser<Caller, Input, Parameter> : IInvoker<Caller, Input>
	{
		(IInvoker<Caller, Parameter>, Parameter) Parse(Input input);
		void IInvoker<Caller, Input>.Invoke(Caller caller, Input input)
		{
			var (invoker, parameter) = Parse(input);
			invoker.Invoke(caller, parameter);
		}
	}
}
