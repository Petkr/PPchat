using System;

namespace PPchatLibrary
{
	class UniqueNameCommandArgumentCountDictionary : ICommandArgumentCountDictionary
	{
		readonly ICommandDescriptor Descriptor;
		protected readonly ISimpleCollection<ICommandDescriptor> DescriptorCollection;

		public virtual ISimpleCollection<ICommandDescriptor>? GetIfOneLongArgument => null;

		public UniqueNameCommandArgumentCountDictionary(ICommandDescriptor descriptor)
		{
			Descriptor = descriptor;
			DescriptorCollection = new FakeCollection<ICommandDescriptor>(Descriptor.AsSingleEnumerable());
		}

		public void Add(int _, ISimpleCollection<ICommandDescriptor> __)
			=> throw new Exception();

		public virtual ISimpleCollection<ICommandDescriptor>? GetValue(int from)
		{
			if (from == Descriptor.ArgumentCount)
				return DescriptorCollection;
			else
				return null;
		}

		public void AddIfOneLongArgument(ICommandDescriptor _)
			=> throw new Exception();
	}
}
