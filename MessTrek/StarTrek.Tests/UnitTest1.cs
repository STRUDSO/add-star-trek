using Untouchables;

namespace StarTrek.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        var command = "phaser";
        var amount = 1000;
        List<string> messages = [];
        
        var game = new Game();
        var klingon = new TestableKlignon(0, 0);
        var webGadget = new WebGadget(command, amount.ToString(), klingon);
        game.FireWeapon_(webGadget, messages.Add, (_dontcare) => 42);
        
        await Verify(new { messages, klingon, game });
        
    }
    
    class TestableKlignon : Klingon {
        public bool deleted;
        public TestableKlignon(int distance, int energy) : base(distance, energy) {}

        override public void Delete()
        {
            this.deleted = true;
        }
    }
}
