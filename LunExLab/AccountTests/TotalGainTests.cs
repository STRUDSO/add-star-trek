using Accounts;
using ITC;
using Moq;
using Xunit;

namespace AccountTests
{
    public class TotalGainTests
    {
        [Fact]
        public void WhenUsingLunEx()
        {
            // given
            var firstLot = new Lot(100, 3000L);
            var latestLot = new Lot(10, 400L);
            Lot[] lots = { firstLot, latestLot };
            Account account = new Account();
            string symbol = "HE3";
            // Current price of HE3 was 42 when we wrote this...
            // Mock<SecurityExchangeTransmissionInterface> service = new Mock<SecurityExchangeTransmissionInterface>();
            // service.Setup(s => s.CurrentPrice(symbol)).Returns(42).Verifiable(Times.Once);

            var service = Mock
                .Of<SecurityExchangeTransmissionInterface>(s => s.CurrentPrice(symbol) == 42);

            // when/then
            Assert.Equal(1220L, account.TotalGain(lots, symbol, service));
            
            Mock.Get(service).Verify(s => s.CurrentPrice(symbol), Times.Once);
        }

        [Fact]
        public void WhenUsingInteger()
        {
            // given
            var firstLot = new Lot(100, 3000L);
            var latestLot = new Lot(10, 400L);
            Lot[] lots = { firstLot, latestLot };
            Account account = new Account();

            // when/then
            Assert.Equal(1220L, account.TotalGain(lots, 42));
        }
    }
}
