#use Dumper
#use Inventarizer
#use CargoLeveler 
#use Multiplexer
#use MultiplexHelp
#use MultiplexInterfaceRotor
 
string[] 	cargoNames	= { "8472 Cargo", "8472 Drill" }; 
string 		overallName 	= "8472 Cockpit"; 

string 		dumpName 	= "8472 Cockpit"; 

Dumper 		dumper;
Inventarizer 	foobar; //if you need to know what items we have, ask foobar
CargoLeveler 	leveler; 

MultiplexInterfaceRotor ui;
Multiplexer		m;
MultiplexHelp		h;
 
bool setup = false; 
 
void init()
{
	if (setup)
	{
		return;
	}
	setup = true;
	dumper  = new Dumper(dumpName, GridTerminalSystem);
	foobar  = new Inventarizer(cargoNames, GridTerminalSystem, dumper);
        leveler = new CargoLeveler(overallName, cargoNames, GridTerminalSystem);
	
	ui = new MultiplexInterfaceRotor(GridTerminalSystem);
	m  = new Multiplexer(ui);
	h  = new MultiplexHelp(dumper, m);
	m.addJob(h);

}

void Main()
{
	init();
	m.run();
	
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

