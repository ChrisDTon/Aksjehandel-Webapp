using aksjehandel.DAL.StockLayer;

namespace AksjehandelTest
{
    public class StockControllerTest
    {
        private const string _userId = "userId";

        private readonly Mock<IStockRepository> mock = new Mock<IStockRepository>();// Mocks the repository i.e. what makes interacts with the database
        private readonly Mock<ILogger<StockController>> logMock = new Mock<ILogger<StockController>>();// Mocks log

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        [Fact]// public void if not async
        public async Task GetAll_CallValid()
        {
            // Arrange
            var expected = new List<Stock>
            {
                // Arrange
                new Stock
                {
                    id = 1,
                    name = "Jim",
                    price = 0.01,
                    change = 0.10,
                    market_cap = 1
                },
                new Stock
                {
                    id = 2,
                    name = "Gordon",
                    price = 0.02,
                    change = 0.20,
                    market_cap = 2
                },
                new Stock
                {
                    id = 3,
                    name = "Repsils",
                    price = 0.03,
                    change = 0.30,
                    market_cap = 3
                },
                new Stock
                {
                    id = 4,
                    name = "Triple Action",
                    price = 0.04,
                    change = 0.40,
                    market_cap = 4
                },
                new Stock
                {
                    id = 5,
                    name = "Sukkerfri",
                    price = 0.05,
                    change = 0.50,
                    market_cap = 5
                }
            };

            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.GetAll()).ReturnsAsync(expected);// Makes it so that when we make a call to the mock to get all we are returned with expected list
            var ctrlr = new StockController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            List<Stock> actual = await ctrlr.GetAll();// Makes the call to controller and saves the return

            // Assert
            Assert.Equal<List<Stock>>(actual, expected);// Check that the lists are one to one copies of each other. Only possible since both lists are of same type.
        }
        
        [Fact]// public void if not async
        public async Task GetAll_CallInvalidl()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            var mock = new Mock<IStockRepository>();// Mocks the repository i.e. what makes interacts with the database
            mock.Setup(k => k.GetAll()).ReturnsAsync(() => null);// Makes it so that when we make a call to the mock to get all we are returned with null
            var ctrlr = new StockController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            List<Stock> actual = await ctrlr.GetAll();// Makes the call to controller and saves the return

            // Assert
            Assert.Null(actual);// Check that the lists are one to one copies of each other. Only possible since both lists are of same type.
        }
    }
}