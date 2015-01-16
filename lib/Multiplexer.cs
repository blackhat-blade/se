#use MultiplexInterface
#use Multiplexed
#use MultiplexedOff
#use MultiplexJobInfo 

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
