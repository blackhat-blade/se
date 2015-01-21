/*
* Hibernation Program
* checks if ship is currently docked with at least one of its connectors
* if docked it will shut down all thrusters and lights
* if undocked it will start all thrusters and lights
* 
* ISSUES:
* This will not preserv previous states, ALL thrusters will be activated as there is no way to save the thrusters status over sessions, yet.
* Hibernation instance needs to be saved over multiple runs (via programm instance variable)
*/
public class Hibernate
{
        IMyGridTerminalSystem GridTerminalSystem;
        
        IMyTerminalBlock identifier;
        bool isDocked;
        
        
        public Hibernate(string shipIdentifier, IMyGridTerminalSystem gts)
        {
                GridTerminalSystem = gts;
                identifier = gts.GetBlockWithName(shipIdentifier);
                
                if (identifier == null)
                {
                        throw new Exception("Hibernate: No ship identifier found");
                }
        }
        
        public void Run(IMyGridTerminalSystem gts)
        {
                GridTerminalSystem = gts;
                Run();
        }
        
        public void Run()
        {
                bool checkDock = CheckConnectors();
                
                // if ship is docked set 'isDocked' to true and deactivate the blocks
                if (checkDock)
                {
                        Sleep();
                        isDocked = true;
                }
                // if ship is not docked but 'isDocked' is true, set 'isDocked' to false and start the ship
                else if (!checkDock && isDocked)
                {
                        Raise();
                        isDocked = false;
                }
        }
        
        
        /*
        * return true if one connector is docked
        * false otherwise
        */
        public bool CheckConnectors()
        {
                var connectors = GetBlocksInSameGrid<IMyShipConnector>();
                for (int i = 0; i < connectors.Count; i++)
                {
                        if (IsLocked(connectors[i]))
                        {
                                return true;
                        }
                }
                
                return false;
        }
        
        public bool IsLocked(IMyTerminalBlock block)
        {
                var builder = new StringBuilder();
                block.GetActionWithName("SwitchLock").WriteValue(block, builder);
                return builder.ToString() == "Locked";
        }
        
        
        
        /*
        * de- or activates relevant blocks
        */
        public void ChangeBlockStatus(bool status)
        {
                var blocks = GetHibernationBlocks();
                
                for (int i = 0; i < blocks.Count; i++)
                {
                        if (blocks[i] is IMyFunctionalBlock)
                        {
                                blocks[i].GetActionWithName(status ? "OnOff_On" : "OnOff_Off").Apply(blocks[i]);
                        }
                }
        }
        
        /*
        * activates all relevant blocks
        */
        public void Raise()
        {
                ChangeBlockStatus(true);
        }
        
        /*
        * deactivates all relevant blocks
        */
        public void Sleep()
        {
                ChangeBlockStatus(false);
        }
        
        /*
        * returns List of all relevant blocks
        * relevant blocks are currently:
        * - Thrusters
        * - Lighting
        */
        public List<IMyTerminalBlock> GetHibernationBlocks()
        {
                var thrusters = GetBlocksInSameGrid<IMyThrust>();
                var lights = GetBlocksInSameGrid<IMyLightingBlock>();
                
                thrusters.AddList(lights);
                
                return thrusters;
                
        }
        
        
        /*
        * get blocks of the same grid as the block in 'identifier'
        */
        public List<IMyTerminalBlock> GetBlocksInSameGrid<T>()
        {
                var blocks = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocksOfType<T>(blocks, IsSameGrid);
                return blocks;
                
        }
        
        
        public bool IsSameGrid(IMyTerminalBlock block)
        {
                return identifier.CubeGrid == block.CubeGrid;
        }
        
}