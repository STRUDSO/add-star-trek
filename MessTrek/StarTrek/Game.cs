using System;
using System.Collections.Generic;
using Untouchables;

public class WebGadgetProxy
{
	private readonly WebGadget _wg;
	public string Parameter(string parameterName)
	{
		return _wg.Parameter(parameterName);
	}

	public object Variable(string variableName)
	{
		return _wg.Variable(variableName);
	}

	public virtual void WriteLine(string message)
	{
		_wg.WriteLine(message);
	}

	public WebGadgetProxy(WebGadget wg)
	{
		_wg = wg;
	}
        
}
public class Game {
	public int e = 10000;
	private int t = 8;

	public void FireWeapon(WebGadget wg)
	{
		FireWeapon_(new WebGadgetProxy(wg),
			s => wg.WriteLine(s),
			i => Rnd(i),
			x => x.Delete());
	}

	public void FireWeapon_(WebGadgetProxy wg, Action<string> writeLine, Func<int, int> rnd, Action<Klingon> deleteKlingon)
	{
		if (wg.Parameter("command").Equals("phaser")) {
			int amount = int.Parse(wg.Parameter("amount"));
			Klingon enemy = (Klingon) wg.Variable("target");
			if (e >= amount) {
				int distance = enemy.Distance();
				if (distance > 4000) {
					writeLine("Klingon out of range of phasers at " + distance + " sectors...");
				} else {
					int damage = amount - (((amount /20)* distance /200) + rnd(200));
					if (damage < 1)
						damage = 1;
					writeLine("Phasers hit Klingon at " + distance + " sectors with " + damage + " units");
					if (damage < enemy.GetEnergy()) {
						enemy.SetEnergy(enemy.GetEnergy() - damage);
						writeLine("Klingon has " + enemy.GetEnergy() + " remaining");
					} else
					{
						writeLine("Klingon destroyed!");
						deleteKlingon(enemy);
					}
				}
				e -= amount;

			} else {
				writeLine("Insufficient energy to fire phasers!");
			}

		} else if (wg.Parameter("command").Equals("photon")) {
			Klingon enemy = (Klingon) wg.Variable("target");
			if (t  > 0) {
				int distance = enemy.Distance();
				if ((rnd(4) + ((distance / 500) + 1) > 7)) {
					writeLine("Torpedo missed Klingon at " + distance + " sectors...");
				} else {
					int damage = 800 + rnd(50);
					writeLine("Photons hit Klingon at " + distance + " sectors with " + damage + " units");
					if (damage < enemy.GetEnergy()) {
						enemy.SetEnergy(enemy.GetEnergy() - damage);
						writeLine("Klingon has " + enemy.GetEnergy() + " remaining");
					} else {
						writeLine("Klingon destroyed!");
						deleteKlingon(enemy);
					}
				}
				t -= 1;

			} else {
				writeLine("No more photon torpedoes!");
			}
		}
	}

	private static Random generator = new Random();
	internal static int Rnd(int maximum) {
		return generator.Next(maximum);
	}
}
