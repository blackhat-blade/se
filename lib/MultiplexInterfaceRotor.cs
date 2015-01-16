#use MultiplexInterface

public class MultiplexInterfaceRotor :  MultiplexInterface
{
	string 			rotorName;
	int    			factor;
	IMyGridTerminalSystem  	GridTerminalSystem;

	public  MultiplexInterfaceRotor(IMyGridTerminalSystem  	gts, 
					string name = "FunctionSwitchRotor", 
					int f = 3)
	{
		GridTerminalSystem = gts;
		rotorName	   = name;
		factor		   = f;
	}

		
	public override int getJobId()
	{
		var block = GridTerminalSystem.GetBlockWithName(rotorName) as IMyMotorStator;
		int vel;

		if (block != null)
		{   
			vel = (int) Math.Round(block.Velocity / factor);
			return vel;    
		}
		return 0;
	}

	public override string translateJobId(int id)
	{
		return (id * factor).ToString();
	}
}

