using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AksjehandelTest
{
    public class CustomerControllerTest
    {
        private const string _userId = "userId";
        private const string _noUserId = "";

        private readonly Mock<ICustomerRepository> mock = new Mock<ICustomerRepository>();// Mocks the repository i.e. what makes interacts with the database
        private readonly Mock<ILogger<CustomerController>> logMock = new Mock<ILogger<CustomerController>>();// Mocks log

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        private readonly Customer expected = new Customer
        {
            id = 1,
            email = "yagoo@cover.com",
            firstname = "Tanigo",
            lastname = "Motoaki"
        };
        private readonly CreateCustomer createCustomer = new CreateCustomer
        {
            firstname = "Tanigo",
            lastname = "Motoaki",
            email = "yagoo@cover.com",
            password = "baseball"
        };
        private readonly LoginCustomer loginCustomer = new LoginCustomer
        {
            email = "yagoo@cover.com",
            password = "baseball"
        };
        private readonly UpdateCustomer updateCustomer = new UpdateCustomer
        {
            firstname = "Tanigo",
            lastname = "Motoaki",
            email = "yagoo@cover.com"
        };
        private readonly ChangePassword changePassword = new ChangePassword
        {
            password = "baseball"
        };

        [Fact]
        public async Task Get_CallValid()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Get(It.IsAny<int>())).ReturnsAsync(expected);// Makes it so that when we make a call to the mock to get all we are returned with expected list
            var ctrlr = new CustomerController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Get() as OkObjectResult;// Makes the call to controller and saves the return

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);// Checks that status code is OK
            Assert.IsType<OkObjectResult>(actual);// Check that on successful GetAll(), there is a success msg back
            Assert.NotNull(actual.Value);// Makes sure there are values
            Assert.Equal(actual.Value, expected);// Checks that the expected list is the same as the called and returned list
        }

        [Fact]
        public async Task Get_CallInvalid_NoUserId()
        {
            // Arrange
            mockSession[_userId] = _noUserId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Get(It.IsAny<int>())).ReturnsAsync(expected);// Makes it so that when we make a call to the mock to get all we are returned with expected list
            var ctrlr = new CustomerController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Get() as UnauthorizedResult;// Makes the call to controller and saves the return

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, actual.StatusCode);// Checks that status code is unauthorized
            Assert.IsType<UnauthorizedResult>(actual);// Check that on successful GetAll(), there is a success msg back
        }

        [Fact]
        public void Logout_CallValid()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            var ctrlr = new CustomerController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session
            
            // Act
            ctrlr.Logout();// Makes the call to controller
            
            // Assert
            Assert.Equal(_noUserId, mockSession[_userId]);// Checks that the userId is empty
        }

        [Fact]
        public async Task Login_CallValid()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Login(loginCustomer)).ReturnsAsync(expected.id);// Makes it so that when we make a call to the mock to get all we are returned with users id
            var ctrlr = new CustomerController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Login(loginCustomer) as OkObjectResult;// Makes the call to controller and saves the return

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);// Checks that status code is OK
            Assert.IsType<OkObjectResult>(actual);// Check that on successful Login(), there is a success msg back
            Assert.NotNull(actual.Value);// Makes sure there are values
            Assert.Equal(actual.Value, "Login successful");// Checks that the correct success msg is returned
        }

        [Fact]
        public async Task Login_CallInvalid_Fail()
        {
            // Arrange
            mockSession[_userId] = _noUserId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Login(loginCustomer)).ReturnsAsync(() => null);// Makes it so that when we make a call to the mock to get all we are returned with users id
            var ctrlr = new CustomerController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Login(loginCustomer) as BadRequestObjectResult;// Makes the call to controller and saves the return

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);// Checks that status code is BadRequest
            Assert.IsType<BadRequestObjectResult>(actual);// Check that on unsuccessful Login(), there is an error msg back
            Assert.NotNull(actual.Value);// Makes sure there are values
            Assert.Equal(actual.Value, "Wrong login information");// Checks that the correct error msg is returned
        }

        [Fact]
        public async Task Login_CallInvalid_ModelStateInvalid()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Login(loginCustomer)).ReturnsAsync(expected.id);// Makes it so that when we make a call to the mock to get all we are returned with users id
            var ctrlr = new CustomerController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ModelState.AddModelError("", "");
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Login(loginCustomer) as BadRequestObjectResult;// Makes the call to controller and saves the return

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);// Checks that status code is BadRequest
            Assert.IsType<BadRequestObjectResult>(actual);// Check that on unsuccessful Login(), there is an error msg back
            Assert.NotNull(actual.Value);// Makes sure there are values
            Assert.Equal(actual.Value, "Error in input validation");// Checks that the correct error msg is returned
        }

        [Fact]
        public async Task Register_CallValid()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Register(createCustomer)).ReturnsAsync(true);// Makes it so that when we make a call to the mock to get all we are returned with true
            var ctrlr = new CustomerController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Register(createCustomer) as OkObjectResult;// Makes the call to controller and saves the return

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);// Checks that status code is OK
            Assert.IsType<OkObjectResult>(actual);// Check that on successful Login(), there is a success msg back
            Assert.NotNull(actual.Value);// Makes sure there are values
            Assert.Equal(actual.Value, "Customer successfully registered");// Checks that the correct success msg is returned
        }

        [Fact]
        public async Task Register_CallInvalid_Fail()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Register(createCustomer)).ReturnsAsync(false);// Makes it so that when we make a call to the mock to get all we are returned with true
            var ctrlr = new CustomerController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Register(createCustomer) as BadRequestObjectResult;// Makes the call to controller and saves the return

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);// Checks that status code is OK
            Assert.IsType<BadRequestObjectResult>(actual);// Check that on successful Login(), there is a success msg back
            Assert.NotNull(actual.Value);// Makes sure there are values
            Assert.Equal(actual.Value, "Wrong registration information");// Checks that the correct error msg is returned
        }

        [Fact]
        public async Task Register_CallInvalid_ModelStateInvalid()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Register(createCustomer)).ReturnsAsync(false);// Makes it so that when we make a call to the mock to get all we are returned with true
            var ctrlr = new CustomerController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ModelState.AddModelError("", "");
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Register(createCustomer) as BadRequestObjectResult;// Makes the call to controller and saves the return

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);// Checks that status code is OK
            Assert.IsType<BadRequestObjectResult>(actual);// Check that on successful Login(), there is a success msg back
            Assert.NotNull(actual.Value);// Makes sure there are values
            Assert.Equal(actual.Value, "Error in input validation");// Checks that the correct error msg is returned
        }

        [Fact]
        public async Task Update_CallValid()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Update(updateCustomer, It.IsAny<int>())).ReturnsAsync(true);// Makes it so that when we make a call to the mock to get all we are returned with true
            var ctrlr = new CustomerController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Update(updateCustomer) as OkObjectResult;// Makes the call to controller and saves the return

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);// Checks that status code is OK
            Assert.IsType<OkObjectResult>(actual);// Check that on successful Update(), there is a success msg back
            Assert.NotNull(actual.Value);// Makes sure there are values
            Assert.Equal(actual.Value, "Profile successfully updated");// Checks that the correct success msg is returned
        }

        [Fact]
        public async Task Update_CallInvalid_Fail()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Update(updateCustomer, It.IsAny<int>())).ReturnsAsync(false);// Makes it so that when we make a call to the mock to get all we are returned with true
            var ctrlr = new CustomerController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Update(updateCustomer) as BadRequestObjectResult;// Makes the call to controller and saves the return

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);// Checks that status code is BadRequest
            Assert.IsType<BadRequestObjectResult>(actual);// Check that on successful Update(), there is an error msg back
            Assert.NotNull(actual.Value);// Makes sure there are values
            Assert.Equal(actual.Value, "Failed to update profile");// Checks that the correct error msg is returned
        }

        [Fact]
        public async Task Update_CallInvalid_NoUserId()
        {
            // Arrange
            mockSession[_userId] = _noUserId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Update(updateCustomer, It.IsAny<int>())).ReturnsAsync(true);// Makes it so that when we make a call to the mock to get all we are returned with true
            var ctrlr = new CustomerController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Update(updateCustomer) as UnauthorizedResult;// Makes the call to controller and saves the return

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, actual.StatusCode);// Checks that status code is unathorized
            Assert.IsType<UnauthorizedResult>(actual);// Check that on successful Update(), there is an error msg back
        }

        [Fact]
        public async Task Update_CallInvalid_ModelStateInvalid()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.Update(updateCustomer, It.IsAny<int>())).ReturnsAsync(false);// Makes it so that when we make a call to the mock to get all we are returned with true
            var ctrlr = new CustomerController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ModelState.AddModelError("", "");
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.Update(updateCustomer) as BadRequestObjectResult;// Makes the call to controller and saves the return

            // Assert
            Assert.NotNull(actual.Value);// Makes sure there are values
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);// Checks that status code is BadRequest
            Assert.IsType<BadRequestObjectResult>(actual);// Check that on unsuccessful Update(), there is an error msg back
            Assert.Equal(actual.Value, "Error in input validation");// Checks that the correct success msg is returned
        }

        [Fact]
        public async Task ChangePassword_CallValid()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.ChangePassword(changePassword.password, It.IsAny<int>())).ReturnsAsync(true);// Makes it so that when we make a call to the mock to get all we are returned with true
            var ctrlr = new CustomerController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.ChangePassword(changePassword) as OkObjectResult;// Makes the call to controller and saves the return

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);// Checks that status code is OK
            Assert.IsType<OkObjectResult>(actual);// Check that on successful Update(), there is a success msg back
            Assert.NotNull(actual.Value);// Makes sure there are values
            Assert.Equal(actual.Value, "Password changed");// Checks that the correct success msg is returned
        }

        [Fact]
        public async Task ChangePassword_CallInvalid_Fail()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.ChangePassword(changePassword.password, It.IsAny<int>())).ReturnsAsync(false);// Makes it so that when we make a call to the mock to get all we are returned with true
            var ctrlr = new CustomerController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.ChangePassword(changePassword) as BadRequestObjectResult;// Makes the call to controller and saves the return

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);// Checks that status code is OK
            Assert.IsType<BadRequestObjectResult>(actual);// Check that on unsuccessful Update(), there is a error msg back
            Assert.NotNull(actual.Value);// Makes sure there are values
            Assert.Equal(actual.Value, "Failed to change passworde");// Checks that the correct error msg is returned
        }

        [Fact]
        public async Task ChangePassword_CallInvalid_NoUserId()
        {
            // Arrange
            mockSession[_userId] = _noUserId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.ChangePassword(changePassword.password, It.IsAny<int>())).ReturnsAsync(true);// Makes it so that when we make a call to the mock to get all we are returned with true
            var ctrlr = new CustomerController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.ChangePassword(changePassword) as UnauthorizedResult;// Makes the call to controller and saves the return

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, actual.StatusCode);// Checks that status code is OK
            Assert.IsType<UnauthorizedResult>(actual);// Check that on successful Update(), there is a success msg back
        }

        [Fact]
        public async Task ChangePassword_CallInvalid_ModelStateInvalid()
        {
            // Arrange
            mockSession[_userId] = _userId;// Makes it so that when repo checks for userId for session one is given.
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);// When call is made to HttpContext the session with userId is returned
            mock.Setup(k => k.ChangePassword(changePassword.password, It.IsAny<int>())).ReturnsAsync(true);// Makes it so that when we make a call to the mock to get all we are returned with true
            var ctrlr = new CustomerController(mock.Object, logMock.Object);// Makes a controller that will be used to make the call
            ctrlr.ModelState.AddModelError("", "");
            ctrlr.ControllerContext.HttpContext = mockHttpContext.Object;// Session used for controller is made mock session

            // Act
            var actual = await ctrlr.ChangePassword(changePassword) as BadRequestObjectResult;// Makes the call to controller and saves the return

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);// Checks that status code is OK
            Assert.IsType<BadRequestObjectResult>(actual);// Check that on successful Update(), there is a success msg back
            Assert.NotNull(actual.Value);// Makes sure there are values
            Assert.Equal(actual.Value, "Error in input validation");// Checks that the correct success msg is returned
        }
    }
}