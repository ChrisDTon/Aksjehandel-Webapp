using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AksjehandelTest
{
    public class PurchaseControllerTest
    {
        private const string _userId = "userId";
        private const string _noUserId = "";

        private readonly Mock<IPurchaseRepository> mock = new Mock<IPurchaseRepository>();// Mocks the repository i.e. what makes interacts with the database
        private readonly Mock<ILogger<PurchaseController>> logMock = new Mock<ILogger<PurchaseController>>();// Mocks log

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        private readonly StockPurchase innPurchase = new StockPurchase// Stock that we will attempt to save
        {
            id = 1,
            stock_id = 2,
            quantity = 3,
            price = 4
        };

        [Fact]// public void if not async
        public async Task GetAll_CallValid()
        {
            // Arrange
            var expected = new List<StockPurchase>
            {
                new StockPurchase
                {
                    id = 1,
                    stock_id = 2,
                    quantity= 3,
                    price= 4
                },
                new StockPurchase
                {
                    id = 2,
                    stock_id = 3,
                    quantity= 4,
                    price= 5
                },
                new StockPurchase
                {
                    id = 3,
                    stock_id = 4,
                    quantity= 5,
                    price= 6
                },
                new StockPurchase
                {
                    id = 4,
                    stock_id = 5,
                    quantity= 6,
                    price= 7
                },
                new StockPurchase
                {
                    id = 5,
                    stock_id = 6,
                    quantity= 7,
                    price= 8
                }
            };

            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.GetAll(It.IsAny<int>())).ReturnsAsync(expected);// Makes it so that when we make a call to the mock to get all we are returned with expected list
            var ctrlr = new PurchaseController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.GetAll() as OkObjectResult;// Makes the call to controller and saves the return

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);// Checks that status code is OK
            Assert.IsType<OkObjectResult>(actual);// Check that on successful GetAll(), there is a success msg back
            Assert.NotNull(actual.Value);// Makes sure there are values
            Assert.Equal<List<StockPurchase>>((List<StockPurchase>)actual.Value, expected);// Checks that the expected list is the same as the called and returned list
        }
        
        [Fact]
        public async Task GetAll_CallInvalid()
        {
            // Arrange
            mockSession[_userId] = _noUserId;// Makes it so that when repo checks for userId for session empty one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.GetAll(1)).ReturnsAsync(() => null);// Makes it so that when we make a call to the mock to get all we are returned with null
            var ctrlr = new PurchaseController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.GetAll() as UnauthorizedResult;// Makes the call to controller and saves the return

            // Assert
            Assert.NotNull(actual);// If GetAll() doesn't return unauthorized result var actual is null
            Assert.IsType<UnauthorizedResult>(actual);// Checks that when no user id is given there is an error msg
            Assert.Equal((int)HttpStatusCode.Unauthorized, actual.StatusCode);// Checks that status code is Unauthorized
        }
        
        [Fact]
        public async Task Save_CallValid()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Save(innPurchase, It.IsAny<int>())).ReturnsAsync(true);// Makes it so that when we make a call to the mock to save we are returned with true
            var ctrlr = new PurchaseController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Save(innPurchase) as OkObjectResult;// Makes the call to controller and saves the return

            // Assert
            Assert.NotNull(actual);// If Save() doesn't return OkObjectResult, var actual is null
            Assert.IsType<OkObjectResult>(actual);// Check that on successful save, there is a success msg back
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);// Checks that status code is OK
            Assert.Equal(actual.Value, "Stock purchase successful");// Checks that correct successs msg is given
        }

        [Fact]
        public async Task Save_CallInvalid_NoUserId()
        {
            // Arrange
            mockSession[_userId] = _noUserId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Save(innPurchase, It.IsAny<int>())).ReturnsAsync(false);// Makes it so that when we make a call to the mock to save we are returned with false
            var ctrlr = new PurchaseController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Save(innPurchase) as UnauthorizedResult;// Makes the call to controller and saves the return

            // Assert
            Assert.NotNull(actual);// If Save() doesn't return BadRequestObjectResult, var actual is null
            Assert.IsType<UnauthorizedResult>(actual);// Check that on unsuccessful save, there is an error msg back
            Assert.Equal((int?)HttpStatusCode.Unauthorized, actual.StatusCode);// Checks that status code is unauthorized
        }

        [Fact]
        public async Task Save_CallInvalid_Fail()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Save(innPurchase, It.IsAny<int>())).ReturnsAsync(false);// Makes it so that when we make a call to the mock to save we are returned with false
            var ctrlr = new PurchaseController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Save(innPurchase) as BadRequestObjectResult;// Makes the call to controller and saves the return

            // Assert
            Assert.NotNull(actual);// If Save() doesn't return BadRequestObjectResult, var actual is null
            Assert.IsType<BadRequestObjectResult>(actual);// Check that on unsuccessful save, there is an error msg back
            Assert.Equal((int?)HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.Equal(actual.Value, "Failed to save stock purchase");
        }

        [Fact]
        public async Task Save_CallInvalid_ModelStateInvalid()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Save(innPurchase, It.IsAny<int>())).ReturnsAsync(false);// Makes it so that when we make a call to the mock to save we are returned with false
            var ctrlr = new PurchaseController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ModelState.AddModelError("", "");
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Save(innPurchase) as BadRequestObjectResult;// Makes the call to controller and saves the return

            // Assert
            Assert.NotNull(actual);// If Save() doesn't return BadRequestObjectResult, var actual is null
            Assert.IsType<BadRequestObjectResult>(actual);// Check that on unsuccessful save, there is an error msg back
            Assert.Equal((int?)HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.Equal(actual.Value, "Error in input validation");
        }

        [Fact]
        public async Task Update_CallValid()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Update(innPurchase, It.IsAny<int>())).ReturnsAsync(true);// Makes it so that when we make a call to the mock to update we are returned with true
            var ctrlr = new PurchaseController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Update(innPurchase) as OkObjectResult;// Makes the call to controller and saves the return

            // Assert
            Assert.NotNull(actual);// If Save() doesn't return OkObjectResult, var actual is null
            Assert.IsType<OkObjectResult>(actual);// Check that on successful save, there is a success msg back
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);// Checks that status code is OK
            Assert.Equal(actual.Value, "Update successful");// Checks that correct successs msg is given
        }

        [Fact]
        public async Task Update_CallInvalid_NoUserId()
        {
            // Arrange
            mockSession[_userId] = _noUserId;// Makes it so that when repo checks for userId for session empty is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Update(innPurchase, It.IsAny<int>())).ReturnsAsync(true);// Makes it so that when we make a call to the mock to update we are returned with true
            var ctrlr = new PurchaseController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Update(innPurchase) as UnauthorizedResult;// Makes the call to controller and saves the return

            // Assert
            Assert.NotNull(actual);// If Save() doesn't return Unathorized, var actual is null
            Assert.IsType<UnauthorizedResult>(actual);// Check that on unsuccessful save, there is a non success msg back
            Assert.Equal((int)HttpStatusCode.Unauthorized, actual.StatusCode);// Checks that status code is OK
        }

        [Fact]
        public async Task Update_CallInvalid_Fail()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Update(innPurchase, It.IsAny<int>())).ReturnsAsync(false);// Makes it so that when we make a call to the mock to update we are returned with false
            var ctrlr = new PurchaseController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Update(innPurchase) as BadRequestObjectResult;// Makes the call to controller and saves the return

            // Assert
            Assert.NotNull(actual);// If Save() doesn't return BadRequestObjectResult, var actual is null
            Assert.IsType<BadRequestObjectResult>(actual);// Check that on unsuccessful save, there is an error msg back
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);// Checks that status code is OK
            Assert.Equal(actual.Value, "Failed to update purchase");// Checks that correct successs msg is given
        }

        [Fact]
        public async Task Update_CallInvalid_ModelStateInvalid()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned        
            mock.Setup(k => k.Update(innPurchase, It.IsAny<int>())).ReturnsAsync(false);// Makes it so that when we make a call to the mock to update we are returned with false
            var ctrlr = new PurchaseController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ModelState.AddModelError("", "");
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Update(innPurchase) as BadRequestObjectResult;// Makes the call to controller and saves the return

            // Assert
            Assert.NotNull(actual);// If Save() doesn't return BadRequestObjectResult, var actual is null
            Assert.IsType<BadRequestObjectResult>(actual);// Check that on unsuccessful save, there is an error msg back
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);// Checks that status code is OK
            Assert.Equal(actual.Value, "Error in input validation");// Checks that correct successs msg is given
        }
        
        [Fact]
        public async Task Delete_CallValid()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Delete(1, It.IsAny<int>())).ReturnsAsync(true);// Makes it so that when we make a call to the mock to delete we are returned with true
            var ctrlr = new PurchaseController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Delete(1) as OkObjectResult;// Makes the call to controller and saves the return.

            // Assert
            Assert.NotNull(actual);// If Save() doesn't return OkObjectResult, var actual is null
            Assert.IsType<OkObjectResult>(actual);// Check that on successful save, there is a success msg back
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);// Checks that status code is ok
            Assert.Equal(actual.Value, "Purchase deleted");// Checks that correct successs msg is given
        }

        [Fact]
        public async Task Delete_CallInvalid_NoUserId()
        {
            // Arrange
            mockSession[_userId] = _noUserId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Delete(1, It.IsAny<int>())).ReturnsAsync(true);// Makes it so that when we make a call to the mock to delete we are returned with true
            var ctrlr = new PurchaseController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Delete(1) as UnauthorizedResult;// Makes the call to controller and saves the return.

            // Assert
            Assert.NotNull(actual);// If Save() doesn't return OkObjectResult, var actual is null
            Assert.IsType<UnauthorizedResult>(actual);// Check that on unsuccessful save, there is an error msg back
            Assert.Equal((int)HttpStatusCode.Unauthorized, actual.StatusCode);// Checks that status code is unathorized
        }

        [Fact]
        public async Task Delete_CallInvalid_Fail()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Delete(1, It.IsAny<int>())).ReturnsAsync(false);// Makes it so that when we make a call to the mock to delete we are returned with false
            var ctrlr = new PurchaseController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Delete(1) as NotFoundObjectResult;// Makes the call to controller and saves the return. The value inside of delete has to be the same as in Setup to work.

            // Assert
            Assert.NotNull(actual);// If Save() doesn't return BadRequestObjectRessult, var actual is null
            Assert.IsType<NotFoundObjectResult>(actual);// Check that on unsuccessful save, there is an error msg back
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);// Checks that status code is unathorized
            Assert.Equal(actual.Value, "Failed to delete purchase");// Checks correct error msg is given
        }
    }
}