using System;
using System.Collections.Generic;

namespace PPchatLibrary
{
	class OneLongArgumentCommandArgumentCountDictionary : ICommandArgumentCountDictionary
	{
		readonly ISimpleCollection<ICommandDescriptor> DescriptorCollection;

		public ISimpleCollection<ICommandDescriptor>? GetIfOneLongArgument
			=> DescriptorCollection;

		public OneLongArgumentCommandArgumentCountDictionary()
			=> DescriptorCollection = new SimpleCollection<List<ICommandDescriptor>, ICommandDescriptor>();

		public void Add(int _, ISimpleCollection<ICommandDescriptor> __)
			=> throw new Exception();

		public ISimpleCollection<ICommandDescriptor>? GetValue(int _)
			=> throw new Exception();

		public void AddIfOneLongArgument(ICommandDescriptor descriptor)
			=> DescriptorCollection.Add(descriptor);
	}
}
