public class CargoLeveler
{
	//  this one I just slapped into the class.
	//  should probably get reviewed, how to improve it.
	//  but it doesn't interfere much with it's environment, 
	//  so it's fine on its own.
	 
	string 			overallName; 
	string[] 		cargoNames;
	IMyGridTerminalSystem 	GridTerminalSystem;
	
	public CargoLeveler(string overallBlockName, string[] cargoBlockNames,  IMyGridTerminalSystem gts)
	{
		overallName 		= overallBlockName;
		cargoNames 		= cargoBlockNames;
		GridTerminalSystem 	= gts;
	}

	public void displayLevel() 
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
}
