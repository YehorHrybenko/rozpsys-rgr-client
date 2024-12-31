
public class SwarmServer 
{

    private static Leadable leader; 

    public static (bool, Leadable) GetLeader(Leadable client)
    {
        if (leader == null)
        {
            leader = client;
            return (true, leader);
        }
        return (false, leader);
    }
}
