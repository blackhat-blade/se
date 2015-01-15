public class Dumper
{
	string 			dumpBlockName;
	IMyGridTerminalSystem 	GridTerminalSystem;

	public Dumper(string blockName, IMyGridTerminalSystem gts)
	{
		dumpBlockName 		= blockName;
		GridTerminalSystem 	= gts;
	}

	IMyTerminalBlock getDumpBlock() 
	{ 
		var oblocks = new List<IMyTerminalBlock>();  
		GridTerminalSystem.SearchBlocksOfName(dumpBlockName, oblocks);  
		IMyTerminalBlock dumpBlock = oblocks[0];  
		 
		return dumpBlock; 
	 
	} 
	 
	void setDumpName(string str) 
	{ 
		var dumpBlock = getDumpBlock(); 
		
		if (dumpBlock != null)  
		{ 
			dumpBlock.SetCustomName(str);  
		}  
	} 
	 
	string getDumpName() 
	{ 
		var dumpBlock = getDumpBlock(); 
		 
		if (dumpBlock != null)  
		{ 
			return dumpBlock.CustomName; 
		} 
		return "";	 
	} 
	 
	public void clear() 
	{ 
		setDumpName(dumpBlockName); 
	} 
	 
	public void dump(string str) 
	{ 
		setDumpName
		(
			getDumpName() + 
			"\r\n" + 
			"                                                                  " + 
			str
		); 
	}	 
}
