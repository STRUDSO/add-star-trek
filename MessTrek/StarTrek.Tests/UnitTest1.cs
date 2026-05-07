using Untouchables;

namespace StarTrek.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        IEnumerable<string> commands = [
            "phaser",
            "photon",
        ];
        IEnumerable<int> amounts = [
            1000,
        ];
        
        await Combination().Verify(Target, commands, amounts);
        return;

        static object Target(string command, int amount)
        {
            List<string> messages = [];
        
            var game = new Game();
            var klingon = new KlingnonSpy(0, 0);
            var webGadget = new WebGadget(command, amount.ToString(), klingon);
            game.FireWeapon_(webGadget, messages.Add, (_dontcare) => 42);

            return new { messages, klingon, game };
        }
    }

    class KlingnonSpy : Klingon {
        public bool deleted;
        public KlingnonSpy(int distance, int energy) : base(distance, energy) {}

        override public void Delete()
        {
            this.deleted = true;
        }
    }
}
