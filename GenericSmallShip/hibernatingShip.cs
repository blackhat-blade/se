#use Hibernate

Hibernate hibernate;

string shipIdentifier = "Blockname";

void Main()
{
        init();
        
        hibernate.Run(GridTerminalSystem);
}


public void init()
{
        if (hibernate == null)
        {
                hibernate = new Hibernate(shipIdentifier, GridTerminalSystem);
        }
        
}