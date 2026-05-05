public class FireTorpedoes(Galaxy galaxy)
{
    public Galaxy Galaxy { get; } = galaxy;

    public Klingon Target()
    {
        return Galaxy.Target();
    }
}

public class TorpedoCannon(int torpedos)
{
    public int Torpedoes { get; set; } = torpedos;

    public void FireAtTarget(FireTorpedoes fireTorpedoes)
    {
        Klingon target = fireTorpedoes.Target();
        
        if (this.Torpedoes  > 0) {
            if (Game.Rnd(4) + ((target.Distance() / 500) + 1) > 7) {
                fireTorpedoes.Galaxy.WriteLine("Torpedo missed Klingon at " + target.Distance() + " sectors...");
            } else {
                int damage = 800 + Game.Rnd(50);
                fireTorpedoes.Galaxy.WriteLine("Photons hit Klingon at " + target.Distance() + " sectors with " + damage + " units");
                if (damage < target.GetEnergy()) {
                    target.SetEnergy(target.GetEnergy() - damage);
                    fireTorpedoes.Galaxy.WriteLine("Klingon has " + target.GetEnergy() + " remaining");
                } else {
                    fireTorpedoes.Galaxy.WriteLine("Klingon destroyed!");
                    target.Delete();
                }
            }
            this.Torpedoes -= 1;
		    
			
        } else {
            fireTorpedoes.Galaxy.WriteLine("No more photon torpedoes!");
        }
    }
}