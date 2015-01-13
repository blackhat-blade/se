string dumpName      = "computer-hive-01"; 
string cargoName     = "Station Cargo";

Dictionary<string, double> cargoAmount; 
 
//string assemblerName = "Assembler"; 
  
 
 
void Main() 
{ 
	clear(); 
	dump("start.."); 
	initCargoAmount(); 

	calculateCargoAmount();



	dumpCargoAmount(); 
}

void calculateCargoAmount()
{
	List<IMyTerminalBlock> cargoBlocks = new List<IMyTerminalBlock>();
	GridTerminalSystem.SearchBlocksOfName(cargoName, cargoBlocks);

	for (int i = 0; i < cargoBlocks.Count; ++i)
	{
		var inventoryO = (IMyInventoryOwner) cargoBlocks[i];
		var inventory  = inventoryO.GetInventory(0);
		var items      = inventory.GetItems();

		for (int c = 0; c < items.Count; ++c)
		{
			string type;
			double amount;
			
			type   = items[c].Content.SubtypeName;
			amount = double.Parse(items[c].Amount.ToString());	
			
			addCargoItemAmount(type, amount);

		}
	}
}

 
void dumpCargoAmount() 
{ 
	var keys = new string[cargoAmount.Count]; 
	cargoAmount.Keys.CopyTo(keys, 0); 
	 
	 
	for (int i=0; i < cargoAmount.Count; ++i) 
	{ 
		string key = keys[i]; 
		string tmp = key + ":" + " " + cargoAmount[key].ToString();  
		dump(tmp); 
	} 
}	 



void initCargoAmount() 
{ 
	cargoAmount = new Dictionary<string, double>(); 
 
} 
 
double getCargoItemAmount(string itemType) 
{ 
	double val;  
	if (cargoAmount.ContainsKey(itemType)) 
	{ 
		cargoAmount.TryGetValue(itemType, out val); 
		return val;	 
	} 
	return 0;	 
}	 
 
void   setCargoItemAmount(string itemType, double val) 
{ 
		 
	if (!cargoAmount.ContainsKey(itemType)) 
	{ 
		cargoAmount.Add(itemType, val); 
	} 
	else 
	{ 
		cargoAmount[itemType] = val; 
	} 
 
} 
 
void  addCargoItemAmount(string itemType, double val) 
{ 
	setCargoItemAmount(itemType, getCargoItemAmount(itemType) + val );  
} 
 
IMyTerminalBlock getDumpBlock() 
{ 
	var oblocks = new List<IMyTerminalBlock>();  
	GridTerminalSystem.SearchBlocksOfName(dumpName, oblocks);  
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
 
void clear() 
{ 
	setDumpName(dumpName); 
} 
 
void dump(string str) 
{ 
	setDumpName(getDumpName() + "\r\n" + str); 
}	 

