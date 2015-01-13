string 	functionSwitchRotor = "FunctionSwitchRotor";
string 	currentFunction;
int 	functionSwitchFactor = 3;	     	


Dictionary<int, string> programms = new Dictionary<int, string>{ 
						    { 0, "help" },
						    { 1, "level" },
						    { 2, "cargo" },
					     };


string[] 	cargoNames	= { "8472 Cargo", "8472 Drill" }; 
string 		overallName 	= "8472 Cockpit"; 

string 		dumpName 	= "8472 Cockpit"; 
Dictionary<string, double> cargoAmount;

void Main()
{
	var newFunction = chooseFunction();
	if (currentFunction != newFunction)
	{
		clear();
		currentFunction = newFunction;
	}

	switch (currentFunction)
	{
		case "level":
		     displayLevel();
	     	     break;
		case "cargo":
		     displayCargo();
		     displayLevel();
		     break;
		case "help":
		default:     
		     help();
		     break;			     
			
	}
}	

void help()
{
	//clear();
	dump("RTFM");
}

string chooseFunction()
{
	return GetProgrammNameByRotor(functionSwitchRotor);
}	

string GetProgrammNameByRotor(string rotorName)
{

    var block = GridTerminalSystem.GetBlockWithName(rotorName) as IMyMotorStator;
    int vel;
    if (block != null)
    {   
	vel = (int) Math.Round(block.Velocity / functionSwitchFactor);    
    	if (programms.ContainsKey(vel))
    	{
		return programms[vel];
    	}
    }
    return null;
}



// B3rT 
 
void displayLevel() 
{ 
        double overallMax = 0; 
        double overallCurrent = 0; 
         
        for (int c = 0; c < cargoNames.Length; c++) 
        { 
                var blocks = new List<IMyTerminalBlock>(); 
                GridTerminalSystem.SearchBlocksOfName(cargoNames[c], blocks); 
                 
                for (int i = 0; i < blocks.Count; i++) 
                { 
                        try 
                        { 
                                double max = ConvertFixedPoint(GetInventory(blocks[i]).MaxVolume); 
                                double current = ConvertFixedPoint(GetInventory(blocks[i]).CurrentVolume); 
                                 
                                overallMax += max; 
                                overallCurrent += current; 
                                 
                                SetCargoLevelName(blocks[i], current / max); 
                                 
                        } 
                        catch (System.Exception e) 
                        { 
                                // no inventory - go to next iteration 
                        } 
                } 
                 
        } 
         
        if (overallName.Length > 0 && overallMax > 0) 
        { 
                var oblocks = new List<IMyTerminalBlock>(); 
                GridTerminalSystem.SearchBlocksOfName(overallName, oblocks); 
                IMyTerminalBlock overallBlock = oblocks[0]; 
                if (overallBlock != null) 
                { 
                        SetCargoLevelName(overallBlock, overallCurrent / overallMax); 
                } 
        } 
         
         
         
         
} 
 
void SetCargoLevelName(IMyTerminalBlock block, double level) 
{ 
        block.SetCustomName(Prefix(block.CustomName, string.Format("{0:f}", level * 100) + "%")); 
} 
 
IMyInventory GetInventory(IMyTerminalBlock block) 
{ 
        IMyInventoryOwner inventoryOwner = block as IMyInventoryOwner; 
        if (inventoryOwner.InventoryCount > 0) 
        { 
                return inventoryOwner.GetInventory(0); 
        } 
         
        throw new System.Exception(); 
         
} 
 
double ConvertFixedPoint(VRage.MyFixedPoint fp) 
{ 
        double var = 0; 
        double.TryParse("" + fp, out var); 
        return var; 
         
} 
 
double GetInventoryLevel(IMyTerminalBlock block) 
{ 
        double level = 0; 
         
        IMyInventoryOwner inventoryOwner = block as IMyInventoryOwner; 
         
        if (inventoryOwner.InventoryCount > 0) 
        { 
                IMyInventory inventory = inventoryOwner.GetInventory(0); 
                level = ConvertFixedPoint(inventory.CurrentVolume) / ConvertFixedPoint(inventory.MaxVolume); 
        } 
         
        return level; 
} 
 
 
 
 
string Prefix(string input, object append) 
{ 
        System.Text.RegularExpressions.Regex exp = new System.Text.RegularExpressions.Regex(@"^\[[^\]]+\]"); 
         
        string output; 
        string prefix = "[" + append + "]"; 
         
        // check if suffix 
        // then append 
        if (!exp.IsMatch(input)) 
        { 
                output = prefix + input; 
        } 
        // or change suffix 
        else 
        { 
                output = exp.Replace(input, prefix); 
        } 
         
        return output; 
} 

// blade -

void displayCargo() 
{ 
	clear(); 
	initCargoAmount();

	for (int i = 0; i < cargoNames.Length; ++i)
	{       	
		calculateCargoAmount(cargoNames[i]);
	}

	dumpCargoAmount(); 
}

void calculateCargoAmount(string cargoName)
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
	Array.Sort(keys); 
	 
	for (int i=0; i < cargoAmount.Count; ++i) 
	{ 
		string key = keys[i]; 
		string tmp = key + ":" + " " + cargoAmount[key].ToString("N0");  
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
	setDumpName(getDumpName() + "\r\n" + "                                                                  " + str); 
}	 


