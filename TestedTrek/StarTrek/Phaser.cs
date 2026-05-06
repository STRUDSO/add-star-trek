internal class Phaser(int energy)
{
    public int energy = energy;

    public void FireAtTarget(FirePhaser command, Action<string> log)
    {
        if (!(energy >= command.phaserEnergy))
        {
            log("Insufficient energy to fire phasers!");
            return;
        }

        var distance = command.target.Distance();
        if (distance > 4000)
        {
            log("Klingon out of range of phasers at " + distance + " sectors...");
        }
        else
        {
            var damage = CalculateDamage(command);
            log("Phasers hit Klingon at " + distance + " sectors with " + damage + " units");
            CalculateImpact(damage, command, log);
        }

        energy -= command.phaserEnergy;
    }

    private static void CalculateImpact(int damage, FirePhaser command, Action<string> log)
    {
        if (damage < command.target.GetEnergy()) {
            command.target.SetEnergy(command.target.GetEnergy() - damage);
            log("Klingon has " + command.target.GetEnergy() + " remaining");
        } else {
            log("Klingon destroyed!");
            command.target.Delete();
        }
    }

    private static int CalculateDamage(FirePhaser command)
    {
        int damage = command.phaserEnergy - (((command.phaserEnergy /20)* command.target.Distance() /200) + Game.Rnd(200));
        if (damage < 1)
            return 1;
        
        return damage;
    }
}

internal record FirePhaser(int phaserEnergy, Klingon target);