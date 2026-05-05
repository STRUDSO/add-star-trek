using System;
using System.Collections.Generic;
using Untouchables;

public class Game {
	private readonly Phaser phaser = new(10000);
	private readonly TorpedoCannon cannon = new(8);

	public int EnergyRemaining() {
        return phaser.energy;
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
		    var command = new FirePhaser(galaxy.PhaserEnergy(), galaxy.Target());
		    phaser.FireAtTarget(command, galaxy.WriteLine);
	    } else if (galaxy.IsPhoton())
	    {
		    cannon.FireAtTarget(galaxy);
	    }
    }


    // note we made generator public in order to stub it in tests;
    // it's ugly, but it's telling us something about our *design!* ;-)
	public static Random generator = new Random();
	internal static int Rnd(int maximum) {
		return generator.Next(maximum);
	}


}