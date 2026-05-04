using Xunit;

namespace GettingItWorking
{
    public class TestMyMaths
    {
        [Fact]
        public void Square_CanSquareTwo()
        {
            Assert.Equal(4, MyMaths.Square(2));
        }

        [Fact]
        public void Square_CanSquareThree()
        {
            Assert.Equal(9, MyMaths.Square(3));
        }

    }
}
