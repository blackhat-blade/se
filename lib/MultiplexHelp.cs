#use Dumper
#use Multiplexer
#use MultiplexJobInfo
#use Multiplexed

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
