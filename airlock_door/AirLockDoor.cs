

AirLockDoor airlock = null;



public class AirLockDoor
{
	IMyGridTerminalSystem	GTS;
	IMyTerminalBlock	DOOR_1;
	IMyTerminalBlock	DOOR_2;
	

	public AirLockDoor (string n1, string n2, IMyGridTerminalSystem gts)
	{
		GTS       = gts;
		DOOR_1	  = by_name(n1);
		DOOR_2	  = by_name(n2);
	
	}

	IMyTerminalBlock by_name(string n)
	{
		var blocks = new List<IMyTerminalBlock>();
		GTS.SearchBlocksOfName(n, blocks);

		if (blocks.Count > 0 )
		{
			return blocks[0];
		}
		return null;
	}

	void open1()
	{
		close(DOOR2);
		open(DOOR1);
	}

	void open2()
	{
		close(DOOR1);
		open(DOOR2);
	}
 
	void close(IMyTerminalBlock d)
	{
		d.GetActionWithName("Close").Apply(d);
	}
 
	void open(IMyTerminalBlock d)
	{
		d.GetActionWithName("Open").Apply(d);
	}
	

}

void Main(string arg)
{
	if (airlock == null)
	{
		airlock = new AirLockDoor("Airlock Top",
		                          "Airlock Bottom", 
	                                  GridTerminalSystem);
	}
}
