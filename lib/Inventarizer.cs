#use Dumper


public class Inventarizer
{
	//TODO: as cargoAmount is private now, (and should be, since it's rather volatile)
	//      we should provide some api to access it. (...)

	IMyGridTerminalSystem 		GridTerminalSystem;
	string[]	      		cargoNames;
	Dumper		      		dumper;
 	Dictionary<string, double> 	cargoAmount;

	public Inventarizer(string[] blockNames, IMyGridTerminalSystem gts, Dumper dumperInstance = null)
	{
		cargoNames 		= blockNames;
		GridTerminalSystem	= gts;
		dumper			= dumperInstance;
	}

	public void calculateCargoAmount()
	{
		cargoAmount = new Dictionary<string, double>(); 

		for (int i = 0; i < cargoNames.Length; ++i)
		{       	
			calculateCargoAmountByName(cargoNames[i]);
		}
	}

	void calculateCargoAmountByName(string cargoName)
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
	 
	public void dumpCargoAmount() 
	{
	        // you better supplied an instance of dumper. 	
		var keys = new string[cargoAmount.Count]; 
		cargoAmount.Keys.CopyTo(keys, 0); 
		Array.Sort(keys); 
		 
		for (int i=0; i < cargoAmount.Count; ++i) 
		{ 
			string key = keys[i]; 
			string tmp = key + ":" + " " + cargoAmount[key].ToString("N0");  
			dumper.dump(tmp); 
		} 
	}	 
	 
	public double getCargoItemAmount(string itemType) 
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
 

}
