
public class AirLockDoor
{
	IMyGridTerminalSystem	GTS;
	IMyTerminalBlock	DOOR_1;
	IMyTerminalBlock	DOOR_2;
	

	public AirLockDoor (string n1, string n2 IMyGridTerminalSystem gts)
	{
		GTS       = gts;
		DOOR_1	  = by_name(n1);
		DOOR_2	  = by_name(n2);
	
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

	

}

void Main()
{
	var airlock = new AirLockDoor("Airlock Top", "Airlock Bottom", GridTerminalSystem);
}
