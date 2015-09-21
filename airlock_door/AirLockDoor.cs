
public class AirLockDoor
{
	IMyGridTerminalSystem	GTS;
	IMyTerminalBlock	DOOR_1;
	IMyTerminalBlock	DOOR_2;
	

	public AirLockDoor (string n1, string n2 IMyGridTerminalSystem gts)
	{
		GTS       = gts;
	}

	IMyTerminalBlock by_name(string n)
	{
		var blocks = new List<IMyTerminalBlock>();
		gts.SearchBlocksOfName(n, blocks);

		if (blocks.Count > 0 )
		{
			return blocks[0];
		}
		return null;
	}

	void 
}
