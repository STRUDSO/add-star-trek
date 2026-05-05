public class TorpedoCannon(int torpedos)
{
    public int Torpedoes { get; set; } = torpedos;

    public void FireAtTarget(Galaxy galaxy)
    {
        Klingon target = galaxy.Target();
        if (this.Torpedoes  > 0) {
            if (Game.Rnd(4) + ((target.Distance() / 500) + 1) > 7) {
                galaxy.WriteLine("Torpedo missed Klingon at " + target.Distance() + " sectors...");
            } else {
                int damage = 800 + Game.Rnd(50);
                galaxy.WriteLine("Photons hit Klingon at " + target.Distance() + " sectors with " + damage + " units");
                if (damage < target.GetEnergy()) {
                    target.SetEnergy(target.GetEnergy() - damage);
                    galaxy.WriteLine("Klingon has " + target.GetEnergy() + " remaining");
                } else {
                    galaxy.WriteLine("Klingon destroyed!");
                    target.Delete();
                }
            }
            this.Torpedoes -= 1;
		    
			
        } else {
            galaxy.WriteLine("No more photon torpedoes!");
        }
    }
}