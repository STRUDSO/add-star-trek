using System;
using System.Collections.Generic;
using Untouchables;

public class Game {

	private int energy = 10000;
	private readonly TorpedoCannon cannon = new(8);

	public int EnergyRemaining() {
        return energy;
    }

    public int Torpedoes {
        set => cannon.Torpedoes = value;
        get => cannon.Torpedoes;
    }

    public void FireWeapon(WebGadget wg) {
        FireWeapon(new Galaxy(wg));
    }

    public void FireWeapon(Galaxy galaxy)
    {
	    if (galaxy.IsPhaser())
	    {
		    if (!EnoughEnergyForPhaser(energy, galaxy))
		    {
			    galaxy.WriteLine("Insufficient energy to fire phasers!");
			    return;
		    }

		    if (OutOfRangeForPhase(galaxy.Target()))
		    {
			    galaxy.WriteLine("Klingon out of range of phasers at " + galaxy.Target().Distance() + " sectors...");
		    }
		    else
		    {
			    var damage = CalculateDamage(galaxy, galaxy.Target());
			    galaxy.WriteLine("Phasers hit Klingon at " + galaxy.Target().Distance() + " sectors with " + damage + " units");
			    CalculateImpact(galaxy, damage, galaxy.Target());
		    }

		    energy -= galaxy.PhaserEnergy();
	    } else if (galaxy.IsPhoton())
	    {
		    cannon.FireAtTarget(galaxy);
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