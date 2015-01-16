#use Dumper
#use Inventarizer
#use CargoLeveler 


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


public class Multiplexed
{
	public string name;
	public string description;
	

	public virtual void activate()
	{
	}
	
	public virtual void deactivate()
	{
	}

	public virtual void run()
	{
	}

	public virtual void backgroundRun()
	{
	}
}

public class MultiplexedOff : Multiplexed
{

	public MultiplexedOff()
	{ 
		name        = "Off"; 
		description = "No foreground function will be executed";
	}
}

public class MultiplexHelp  : Multiplexed
{
	Dumper dumper;
	Multiplexer mpx;

	public MultiplexHelp (Dumper d, Multiplexer m)
	{
		name        = "Help";		
		description = "Show this help";
		dumper 	    = d;
		mpx	    = m;
	}	


	public override void activate()
	{
		int[] jobs;
		int   i;

		jobs = mpx.getJobList();
		dumper.clear();
		
		for (i = 0; i < jobs.Length; i++)
		{
			string tmp;
			var    info = mpx.getJobInfo(jobs[i]);
			tmp         =  String.Format("{0}: {1} - {2}", info.uiid, info.name, info.description);
			dumper.dump(tmp);
		}
	}

	public override void deactivate()
	{
		dumper.clear();
	}
}

public class MultiplexInterface
{
	public virtual int getJobId()
	{
		return 0;
	}
	public virtual string translateJobId(int id)
	{
		return "";
	}
	
	
}

public class MultiplexJobInfo
{
	public string name;
	public string description;
	public string uiid;
	public bool   active = false;	
}

public class Multiplexer
{
	Dictionary<int, Multiplexed> jobs;
	Multiplexed current;
	MultiplexInterface ui; 
	Multiplexed defaultJob;

	public Multiplexer(MultiplexInterface mui, List<Multiplexed> joblist = null)
	{
		ui = mui;
		current    = new MultiplexedOff();
		defaultJob = current;
		jobs       = new Dictionary<int, Multiplexed>();
		jobs[0]    = current;
	}
	

	public void addJobs(List<Multiplexed> joblist)
	{
		int i;

		for (i = 0; i < joblist.Count; ++i)
		{
			addJob(joblist[i]);
		}
	}

	public void addJob(Multiplexed job)
	{
		jobs[jobs.Count] = job;
	}

	Multiplexed nextJob()
	{

		int jobId = ui.getJobId();
		
		if (jobs.ContainsKey(jobId))
		{
			return jobs[jobId];
		}
		return defaultJob;
	} 

	public void run()
	{
		
		var next = nextJob();
		
		if (current != next)
		{

			current.deactivate();
			next.activate();
			current = next;
		}

		runBackgroundJobs();
		current.run();
		
	}

	public MultiplexJobInfo getJobInfo(int jobId)
	{
		var info = new MultiplexJobInfo();
		var job  = jobs[jobId];
		
		info.name 	 = job.name;
		info.description = job.description;
		info.uiid	 = ui.translateJobId(jobId);
		info.active	 = job == current;

		return info;
	}

	public int[] getJobList()
	{
		int[] list = new int[jobs.Count];
		
		jobs.Keys.CopyTo(list, 0);
		Array.Sort(list);
		
		return list;
	}

	void runBackgroundJobs()
	{
		var i = jobs.Values.GetEnumerator();

		while (i.MoveNext())
		{
			i.Current.backgroundRun();
		} 
	}
}


