using System;
using System.Collections.Generic;
using Untouchables;

public class Game {

	private int energy = 10000;
	private int torpedos = 8;

    public int EnergyRemaining() {
        return energy;
    }

    public int Torpedoes {
        set {
            torpedos = value;
        }
        get {
            return torpedos;
        }
    }

    public void FireWeapon(WebGadget wg) {
        FireWeapon(new Galaxy(wg));
    }

    public void FireWeapon(Galaxy galaxy)
    {
	    var target = galaxy.Target();
	    if (galaxy.IsPhaser())
        {
	        if (EnoughEnergyForPhaser(energy, galaxy)) {
		        if (OutOfRangeForPhase(target)) {
			        galaxy.WriteLine("Klingon out of range of phasers at " + target.Distance() + " sectors...");
		        } else {
			        var damage = CalculateDamage(galaxy, target);
			        galaxy.WriteLine("Phasers hit Klingon at " + target.Distance() + " sectors with " + damage + " units");
			        CalculateImpact(galaxy, damage, target);
		        }
		        
		        energy -= galaxy.PhaserEnergy();
	        } else {
		        galaxy.WriteLine("Insufficient energy to fire phasers!");
	        }
        } else if (galaxy.IsPhoton())
        {
	        var enemy = target;
	        if (torpedos  > 0) {
		        int distance = enemy.Distance();
		        if ((Rnd(4) + ((distance / 500) + 1) > 7)) {
			        galaxy.WriteLine("Torpedo missed Klingon at " + distance + " sectors...");
		        } else {
			        int damage = 800 + Rnd(50);
			        galaxy.WriteLine("Photons hit Klingon at " + distance + " sectors with " + damage + " units");
			        if (damage < enemy.GetEnergy()) {
				        enemy.SetEnergy(enemy.GetEnergy() - damage);
				        galaxy.WriteLine("Klingon has " + enemy.GetEnergy() + " remaining");
			        } else {
				        galaxy.WriteLine("Klingon destroyed!");
				        enemy.Delete();
			        }
		        }
		        torpedos -= 1;

	        } else {
		        galaxy.WriteLine("No more photon torpedoes!");
	        }
        }
    }

    private static void CalculateImpact(Galaxy galaxy, int damage, Klingon target)
    {
	    if (damage < target.GetEnergy()) {
		    target.SetEnergy(target.GetEnergy() - damage);
		    galaxy.WriteLine("Klingon has " + target.GetEnergy() + " remaining");
	    } else {
		    galaxy.WriteLine("Klingon destroyed!");
		    target.Delete();
	    }
    }

    private static int CalculateDamage(Galaxy galaxy, Klingon target)
    {
	    int damage = galaxy.PhaserEnergy() - (((galaxy.PhaserEnergy() /20)* target.Distance() /200) + Rnd(200));
	    if (damage < 1)
		    damage = 1;
	    return damage;
    }

    private static bool OutOfRangeForPhase(Klingon target)
    {
	    return target.Distance() > 4000;
    }

    private static bool EnoughEnergyForPhaser(int i, Galaxy galaxy)
	{
		return i >= galaxy.PhaserEnergy();
	}


	// note we made generator public in order to stub it in tests;
    // it's ugly, but it's telling us something about our *design!* ;-)
	public static Random generator = new Random();
	internal static int Rnd(int maximum) {
		return generator.Next(maximum);
	}


}
