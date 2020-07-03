namespace PPchatLibrary
{
	public interface IReader<out T>
	{
		T Read();
	}
}
