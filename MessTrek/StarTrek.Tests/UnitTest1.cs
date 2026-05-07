using Untouchables;

namespace StarTrek.Tests;

public class UnitTest1
{
    [Fact]
    public async Task CombinationApprovals()
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
    }

    private static object Target(string command, int amount)
    {
        var game = new Game();
        var webGadget = new WebGadget(command, amount.ToString(), new Klingon(0, 0));
        bool deleteCalled = false;
        var webGadgetProxy = new WebGadgetSpy(webGadget);
        game.FireWeapon_(webGadgetProxy, webGadgetProxy.WriteLine, (_) => 42, _ => deleteCalled = true);

        return new { webGadgetProxy, deleteCalled, game };
    }
}

internal class WebGadgetSpy : WebGadgetProxy
{
    public List<string> messages = [];
    public WebGadgetSpy(WebGadget webGadget) : base(webGadget)
    {
        
    }

    public override void WriteLine(string message) => messages.Add(message);
}
