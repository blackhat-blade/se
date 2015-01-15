#use Dumper
#use Inventarizer
#use CargoLeveler 

string 	functionSwitchRotor = "FunctionSwitchRotor";
string 	currentFunction;
int 	functionSwitchFactor = 3;	     	

Dictionary<int, string> programms = new Dictionary<int, string>
{ 
	{ 0, "help" },
	{ 1, "level" },
	{ 2, "cargo" },
};


string[] 	cargoNames	= { "8472 Cargo", "8472 Drill" }; 
string 		overallName 	= "8472 Cockpit"; 

string 		dumpName 	= "8472 Cockpit"; 

Dumper 		dumper;
Inventarizer 	foobar; //if you need to know what items we have, ask foobar
CargoLeveler 	leveler; 





void Main()
{
	//TODO: write some initialisation code that sets up required instances. *once* not each run.

	dumper  = new Dumper(dumpName, GridTerminalSystem);
	foobar  = new Inventarizer(cargoNames, GridTerminalSystem, dumper);
        leveler = new CargoLeveler(overallName, cargoNames, GridTerminalSystem);

	var newFunction = chooseFunction();
	if (currentFunction != newFunction)
	{
		dumper.clear();
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
	var keys = new int[programms.Count]; 
	
	programms.Keys.CopyTo(keys, 0); 
	Array.Sort(keys); 
	 
	dumper.clear();
	dumper.dump("Usage:");
	
	for (int i=0; i < programms.Count; ++i) 
	{ 
		int key = keys[i]; 
		string tmp = String.Format("{0,-12}:{1,2}", programms[key], key * 3);
		dumper.dump(tmp); 
	} 
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
	//that much?
	leveler.displayLevel();
}

// blade -

void displayCargo() 
{ 
	dumper.clear(); 
	foobar.calculateCargoAmount();
	foobar.dumpCargoAmount(); 
}
