public class CounterAction
{
	public delegate void CounterActionDelegate();

	public CounterActionDelegate OnCount { get; set; }
	public uint TargetCount { get; set; }

	private uint count;

	public uint Count
	{
		get { return count; }
		private set
		{
			count = value;
			if (count >= TargetCount)
			{
				OnCount();
				count = 0;
			}
		}
	}

	public void Increment()
	{
		Count++;
	}

	public void Decremenet()
	{
		Count--;
	}
}
