public class FireTorpedoes(Klingon target)
{
    public Klingon Target()
    {
        return target;
    }
}

public class TorpedoCannon(int torpedos)
{
    public int Torpedoes { get; set; } = torpedos;

    public void FireAtTarget(FireTorpedoes fireTorpedoes, Action<string> log)
    {
        if (this.Torpedoes  > 0) {
            if (Game.Rnd(4) + ((fireTorpedoes.Target().Distance() / 500) + 1) > 7) {
                log("Torpedo missed Klingon at " + fireTorpedoes.Target().Distance() + " sectors...");
            } else {
                int damage = 800 + Game.Rnd(50);
                log("Photons hit Klingon at " + fireTorpedoes.Target().Distance() + " sectors with " + damage + " units");
                if (damage < fireTorpedoes.Target().GetEnergy()) {
                    fireTorpedoes.Target().SetEnergy(fireTorpedoes.Target().GetEnergy() - damage);
                    log("Klingon has " + fireTorpedoes.Target().GetEnergy() + " remaining");
                } else {
                    log("Klingon destroyed!");
                    fireTorpedoes.Target().Delete();
                }
            }
            this.Torpedoes -= 1;
		    
			
        } else {
            log("No more photon torpedoes!");
        }
    }
}