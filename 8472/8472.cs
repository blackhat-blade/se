#use Dumper
#use Inventarizer
#use CargoLeveler 
#use Multiplexer
#use MultiplexHelp
#use MultiplexInterfaceRotor
#use Multiplexed
 
string[] 	cargoNames	= { "8472 Cargo", "8472 Drill" }; 
string 		overallName 	= "8472 Cockpit"; 

string 		dumpName 	= "8472 Cockpit"; 

Dumper 		dumper;
CargoLeveler 	leveler; 
/*
public class JobInventarizer : Multiplexed
{
	Inventarizer o;
	Dumper	     dumper;
	
	public JobInventarizer(Dumper d, string[] names, IMyGridTerminalSystem gts)
	{
		name        = "Cargo Items";
		description = "Show amount of each Item in Cargo";
		dumper 	    = d;
		o           = new Inventarizer(names, gts, d);
	}

	public override void deactivate()
	{
		dumper.clear();
	}

	public override void run()
	{
		dumper.clear(); 
		o.calculateCargoAmount();
		o.dumpCargoAmount(); 
	}
}
*/

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
        leveler = new CargoLeveler(overallName, cargoNames, GridTerminalSystem);
	
	ui = new MultiplexInterfaceRotor(GridTerminalSystem);
	m  = new Multiplexer(ui);
	h  = new MultiplexHelp(dumper, m);
	
	m.addJob(h);
	//m.addJob(new JobInventarizer(dumper, cargoNames, GridTerminalSystem));

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

