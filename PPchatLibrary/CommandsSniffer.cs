using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

namespace PPchatLibrary
{
	class FakeCollection<T> : ISimpleCollection<T>
	{
		readonly IEnumerable<T> Enumerable;

		public FakeCollection(IEnumerable<T> enumerable)
			=> Enumerable = enumerable;
		public void Add(T item)
			=> throw new Exception();
		public IEnumerator<T> GetEnumerator()
			=> Enumerable.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator()
			=> Enumerable.GetEnumerator();
	}

	class UniqueNameCommandArgumentCountDictionary : ICommandArgumentCountDictionary
	{
		readonly ICommandDescriptor Descriptor;
		readonly ISimpleCollection<ICommandDescriptor> DescriptorCollection;

		public ISimpleCollection<ICommandDescriptor>? GetIfOneLongArgument => null;

		public UniqueNameCommandArgumentCountDictionary(ICommandDescriptor descriptor)
		{
			Descriptor = descriptor;
			DescriptorCollection = new FakeCollection<ICommandDescriptor>(Descriptor.AsSingleEnumerable());
		}

		public void Add(int key, ISimpleCollection<ICommandDescriptor> value)
			=> throw new Exception();

		public ISimpleCollection<ICommandDescriptor>? GetValue(int from)
		{
			if (from == Descriptor.ArgumentCount)
				return DescriptorCollection;
			else
				return null;
		}

		public void AddIfOneLongArgument(ICommandDescriptor descriptor)
			=> throw new Exception();
	}

	class SimpleCollection<Collection, T> : ISimpleCollection<T>
	where Collection : ICollection<T>, new()
	{
		readonly ICollection<T> collection;

		public SimpleCollection() => collection = new Collection();
		public void Add(T item) => collection.Add(item);
		public IEnumerator<T> GetEnumerator() => collection.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => collection.GetEnumerator();
	}

	class SimpleDictionary<Dictionary, Key, Value> : ISimpleDictionary<Key, Value>
		where Value : class
		where Dictionary : IDictionary<Key, Value>, new()
	{
		readonly IDictionary<Key, Value> dictionary;

		public SimpleDictionary() => dictionary = new Dictionary();

		public void Add(Key key, Value value) => dictionary.Add(key, value);
		public Value? GetValue(Key key)
		{
			dictionary.TryGetValue(key, out var value);
			return value;
		}
	}

	class BasicCommandArgumentCountDictionary :
		SimpleDictionary<Dictionary<int, ISimpleCollection<ICommandDescriptor>>, int, ISimpleCollection<ICommandDescriptor>>,
		ICommandArgumentCountDictionary
	{
		public ISimpleCollection<ICommandDescriptor>? GetIfOneLongArgument => null;

		public void AddIfOneLongArgument(ICommandDescriptor _)	=> throw new Exception();
	}

	class OneLongArgumentCommandArgumentCountDictionary : ICommandArgumentCountDictionary
	{
		readonly ISimpleCollection<ICommandDescriptor> DescriptorCollection;

		public ISimpleCollection<ICommandDescriptor>? GetIfOneLongArgument => DescriptorCollection;

		public OneLongArgumentCommandArgumentCountDictionary()
		{
			DescriptorCollection = new SimpleCollection<List<ICommandDescriptor>, ICommandDescriptor>();
		}

		public void Add(int key, ISimpleCollection<ICommandDescriptor> value)
			=> throw new Exception();

		public ISimpleCollection<ICommandDescriptor>? GetValue(int from)
			=> throw new Exception();

		public void AddIfOneLongArgument(ICommandDescriptor descriptor)
			=> DescriptorCollection.Add(descriptor);
	}

	class CommandsSniffer<Application> : BasicSniffer<string, ICommandArgumentCountDictionary, Application>, ICommandsInfo
		where Application : IApplication
	{
		public ICommandDescriptor NotFoundCommand { get; }
		public ICommandDescriptor BadArgumentCountCommand { get; }

		public CommandsSniffer()
			: base(typeof(ICommandHandler<>))
		{
			NotFoundCommand = new CommandDescriptor<Application, NotFoundCommandArgument>();
			BadArgumentCountCommand = new CommandDescriptor<Application, BadArgumentCountCommandArgument>();
		}

		protected override void Handle(Type commandArgumentType)
		{
			if (commandArgumentType == typeof(NotFoundCommandArgument) ||
				commandArgumentType == typeof(BadArgumentCountCommandArgument))
				return;

			var attribute = commandArgumentType.GetCustomAttribute<CommandAttribute>()!;
			var argumentCount = attribute.HasOneLongArgument ? -1 : commandArgumentType.GetFields().Length;
			var command = new CommandDescriptor<Application>(commandArgumentType, argumentCount, attribute.Priority);

			if (attribute.HasUniqueName)
				Add(attribute.Name, new UniqueNameCommandArgumentCountDictionary(command));
			else
			{
				var commands = GetValue(attribute.Name);

				if (attribute.HasOneLongArgument)
				{
					if (commands == null)
					{
						commands = new OneLongArgumentCommandArgumentCountDictionary();
						Add(attribute.Name, commands);
					}
					commands.AddIfOneLongArgument(command);
				}
				else
				{
					if (commands == null)
						commands = new BasicCommandArgumentCountDictionary();

					if (attribute.HasUniqueArgumentCount)
						commands.Add(argumentCount, new FakeCollection<ICommandDescriptor>(command.AsSingleEnumerable()));
					else
					{
						var set = new SimpleCollection<SortedSet<ICommandDescriptor>, ICommandDescriptor>();
						set.Add(command);
						commands.Add(argumentCount, set);
					}
				}
			}
		}

		ICommandArgumentCountReadonlyDictionary? ISimpleReadonlyDictionary<string, ICommandArgumentCountReadonlyDictionary>.GetValue(string from)
			=> GetValue(from);
	}
}
