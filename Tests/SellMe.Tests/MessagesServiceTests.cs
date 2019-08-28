namespace SellMe.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Common;
    using Data.Models;
    using Moq;
    using Services;
    using Services.Interfaces;
    using Web.ViewModels.BindingModels.Messages;
    using Web.ViewModels.ViewModels.Messages;
    using Xunit;

    public class MessagesServiceTests
    {
        private IMessagesService messagesService;

        public MessagesServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task GetMessageViewModelByAdIdAsync_WithInvalidAdId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Ad with the given id doesn't exist!";

            var moqAdsService = new Mock<IAdsService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqIMapper = new Mock<IMapper>();
            var context = InitializeContext.CreateContextForInMemory();

            messagesService = new MessagesService(context, moqAdsService.Object, moqUsersService.Object, moqIMapper.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                messagesService.GetMessageViewModelByAdIdAsync(1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task GetMessageViewModelByAdIdAsync_WithValidData_ShouldReturnCorrectResult()
        {
            //Arrange
            var expected = new SendMessageViewModel
            {
                AdId = 1,
                AdPrice = 120,
                AdTitle = "Iphone 6s",
                RecipientId = "RecipientId",
                SenderId = "SenderId",
                SellerPhone = "0895335532"
            };

            var moqAdsService = new Mock<IAdsService>();
            moqAdsService.Setup(x => x.GetAdByIdAsync(1))
                .ReturnsAsync(new Ad
                {
                    Id = 1,
                    Title = "Iphone 6s",
                    Description = "PerfectCondition",
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddDays(30),
                    AvailabilityCount = 1,
                    Price = 120,
                    SellerId = "SellerId",
                    Condition = new Condition { Name = "Brand New" },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                        Street = "Ivan Vazov",
                        District = "Student city",
                        ZipCode = 1000,
                        PhoneNumber = "0895335532",
                        EmailAddress = "Ivan@gmail.com"
                    }
                });

            var moqUsersService = new Mock<IUsersService>();
            moqUsersService.Setup(x => x.GetCurrentUserId())
                .Returns("SenderId");

            var adFroMock = CreateTestingAd();
            var moqMapper = new Mock<IMapper>();
            moqMapper.Setup(x => x.Map<SendMessageViewModel>(It.IsAny<Ad>()))
                .Returns(new SendMessageViewModel
                {
                    AdId = 1,
                    AdPrice = 120,
                    AdTitle = "Iphone 6s",
                    SellerPhone = "0895335532",
                    RecipientId = "RecipientId"
                });

            var context = InitializeContext.CreateContextForInMemory();

            messagesService = new MessagesService(context, moqAdsService.Object, moqUsersService.Object, moqMapper.Object);

            var testingAd = CreateTestingAd();
            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            //Act
            var actual = await messagesService.GetMessageViewModelByAdIdAsync(1);

            Assert.Equal(expected.AdId, actual.AdId);
            Assert.Equal(expected.AdPrice, actual.AdPrice);
            Assert.Equal(expected.AdTitle, actual.AdTitle);
            Assert.Equal(expected.RecipientId, actual.RecipientId);
            Assert.Equal(expected.SenderId, actual.SenderId);
            Assert.Equal(expected.SellerPhone, actual.SellerPhone);
        }

        [Fact]
        public async Task GetMessageBindingModelByAdIdAsync_WithInvalidAdId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Ad with the given id doesn't exist!";

            var moqAdsService = new Mock<IAdsService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqIMapper = new Mock<IMapper>();
            var context = InitializeContext.CreateContextForInMemory();

            messagesService = new MessagesService(context, moqAdsService.Object, moqUsersService.Object, moqIMapper.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                messagesService.GetMessageBindingModelByAdIdAsync(1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task GetMessageBindingModelByAdIdAsync_WithValidData_ShouldReturnCorrectResult()
        {
            //Arrange
            var expectedResult = new SendMessageBindingModel
            {
                ViewModel = new SendMessageViewModel
                {
                    AdId = 1,
                    AdPrice = 120,
                    AdTitle = "Iphone 6s",
                    RecipientId = "RecipientId",
                    SenderId = "SenderId",
                    SellerPhone = "0895335532"
                }
            };

            var moqAdsService = new Mock<IAdsService>();
            moqAdsService.Setup(x => x.GetAdByIdAsync(1))
                .ReturnsAsync(new Ad
                {
                    Id = 1,
                    Title = "Iphone 6s",
                    Description = "PerfectCondition",
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddDays(30),
                    AvailabilityCount = 1,
                    Price = 120,
                    SellerId = "SellerId",
                    Condition = new Condition { Name = "Brand New" },
                    Address = new Address
                    {
                        Country = "Bulgaria",
                        City = "Sofia",
                        Street = "Ivan Vazov",
                        District = "Student city",
                        ZipCode = 1000,
                        PhoneNumber = "0895335532",
                        EmailAddress = "Ivan@gmail.com"
                    }
                });

            var moqUsersService = new Mock<IUsersService>();
            moqUsersService.Setup(x => x.GetCurrentUserId())
                .Returns("SenderId");

            var moqMapper = new Mock<IMapper>();
            moqMapper.Setup(x => x.Map<SendMessageViewModel>(It.IsAny<Ad>()))
                .Returns(new SendMessageViewModel
                {
                    AdId = 1,
                    AdPrice = 120,
                    AdTitle = "Iphone 6s",
                    SellerPhone = "0895335532",
                    RecipientId = "RecipientId"
                });

            var context = InitializeContext.CreateContextForInMemory();

            messagesService = new MessagesService(context, moqAdsService.Object, moqUsersService.Object, moqMapper.Object);

            var testingAd = CreateTestingAd();
            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            //Act
            var actual = await messagesService.GetMessageBindingModelByAdIdAsync(1);

            Assert.Equal(expectedResult.ViewModel.AdId,actual.ViewModel.AdId);
            Assert.Equal(expectedResult.ViewModel.AdTitle, actual.ViewModel.AdTitle);
            Assert.Equal(expectedResult.ViewModel.AdPrice, actual.ViewModel.AdPrice);
            Assert.Equal(expectedResult.ViewModel.RecipientId, actual.ViewModel.RecipientId);
            Assert.Equal(expectedResult.ViewModel.SenderId, actual.ViewModel.SenderId);
            Assert.Equal(expectedResult.ViewModel.SellerPhone, actual.ViewModel.SellerPhone);
        }

        [Fact]
        public async Task CreateMessageAsync_WithInvalidAdId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Ad with the given id doesn't exist!";

            var moqAdsService = new Mock<IAdsService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqIMapper = new Mock<IMapper>();
            var context = InitializeContext.CreateContextForInMemory();

            messagesService = new MessagesService(context, moqAdsService.Object, moqUsersService.Object, moqIMapper.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                messagesService.CreateMessageAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 1, Guid.NewGuid().ToString()));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task CreateMessageAsync_WithValidData_ShouldCreateAMessage()
        {
            //Arrange
            var expected = new MessageDetailsViewModel
            {
                AdTitle = "Iphone 6s",
                Content = "Content for the message",
                Sender = "Ivan",
                SentOn = "31/01/2019"
            };

            var moqAdsService = new Mock<IAdsService>();
            var moqUsersService = new Mock<IUsersService>();
            moqUsersService.Setup(x => x.GetUserByIdAsync("SenderId"))
                .ReturnsAsync(new SellMeUser
                {
                    Id = "SenderId",
                    UserName = "Ivan"
                });

            var moqMapper = new Mock<IMapper>();
            moqMapper.Setup(x => x.Map<MessageDetailsViewModel>(It.IsAny<Message>()))
                .Returns(new MessageDetailsViewModel
                {
                    AdTitle = "Iphone 6s",
                    Content = "Content for the message",
                    Sender = "Ivan",
                    SentOn = "31/01/2019"
                });

            var context = InitializeContext.CreateContextForInMemory();

            messagesService = new MessagesService(context, moqAdsService.Object, moqUsersService.Object, moqMapper.Object);

            var testingAd = CreateTestingAd();
            await context.Ads.AddAsync(testingAd);
            await context.SaveChangesAsync();

            //Act and assert
            var actual = await messagesService.CreateMessageAsync("SenderId", "RecipientId",
                1, "Content for the message");

            Assert.Equal(expected.AdTitle, actual.AdTitle);
            Assert.Equal(expected.Content, actual.Content);
            Assert.Equal(expected.Sender, actual.Sender);
            Assert.Equal(expected.SentOn, actual.SentOn);
        }

        [Fact]
        public async Task GetMessageDetailsViewModelsAsync_WithInvalidAdId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "Ad with the given id doesn't exist!";

            var moqAdsService = new Mock<IAdsService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqIMapper = new Mock<IMapper>();
            var context = InitializeContext.CreateContextForInMemory();

            messagesService = new MessagesService(context, moqAdsService.Object, moqUsersService.Object, moqIMapper.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                messagesService.GetMessageDetailsViewModelsAsync(1, "SenderId", "RecipientId"));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task GetMessageDetailsViewModelsAsync_WithCurrentUserNotParticipantInConversation_ShouldThrowAnInvalidOperationException()
        {
            //Arrange
            var expectedErrorMessage = "You are not participant in this conversation!";

            var moqAdsService = new Mock<IAdsService>();
            var moqUsersService = new Mock<IUsersService>();
            moqUsersService.Setup(x => x.GetCurrentUserId())
                .Returns("FakeUserId");
            var moqIMapper = new Mock<IMapper>();
            var context = InitializeContext.CreateContextForInMemory();

            var testingAd = CreateTestingAd();
            await context.AddAsync(testingAd);
            await context.SaveChangesAsync();

            messagesService = new MessagesService(context, moqAdsService.Object, moqUsersService.Object, moqIMapper.Object);

            //Act and assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                messagesService.GetMessageDetailsViewModelsAsync(1, "SenderId", "RecipientId"));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task GetMessageDetailsViewModelsAsync_WithValidData_ShouldReturnCorrectResult()
        {
            //Arrange
            var expectedCollectionCount = 2;
            var expected = new List<MessageDetailsViewModel>
            {
                new MessageDetailsViewModel
                {
                    AdTitle = "Iphone 6s",
                    Content = "Content1",
                    Sender = "Pesho"
                },
                new MessageDetailsViewModel
                {
                    AdTitle = "Iphone 6s",
                    Content = "Content2",
                    Sender = "Pesho"
                }
            };

            var moqAdsService = new Mock<IAdsService>();
            var moqUsersService = new Mock<IUsersService>();
            moqUsersService.Setup(x => x.GetCurrentUserId())
                .Returns("SenderId");
            var moqIMapper = new Mock<IMapper>();
            var context = InitializeContext.CreateContextForInMemory();

            messagesService = new MessagesService(context, moqAdsService.Object, moqUsersService.Object, moqIMapper.Object);

            var testingAd = CreateTestingAd();

            var messages = new List<Message>
            {
                new Message
                {
                    AdId = 1,
                    SenderId = "SenderId",
                    RecipientId = "RecipientId",
                    Content = "Content1"
                },
                new Message
                {
                    AdId = 1,
                    SenderId = "SenderId",
                    RecipientId = "RecipientId",
                    Content = "Content2"
                },
                new Message
                {
                    AdId = 1,
                    SenderId = "FakeSenderId",
                    RecipientId = "FakeRecipientId",
                    Content = "Content3"
                }
            };

            var sender = new SellMeUser
            {
                Id = "SenderId",
                UserName = "Pesho"
            };

            await context.SellMeUsers.AddAsync(sender);
            await context.Ads.AddAsync(testingAd);
            await context.Messages.AddRangeAsync(messages);
            await context.SaveChangesAsync();

            //Act
            var actual = messagesService.GetMessageDetailsViewModelsAsync(1, "SenderId", "RecipientId").GetAwaiter().GetResult().ToList();

            //Assert
            Assert.Equal(expectedCollectionCount, actual.Count);
            Assert.Collection(actual,
                elem1 =>
                {
                    Assert.Equal(expected[0].AdTitle, elem1.AdTitle);
                    Assert.Equal(expected[0].Content, elem1.Content);
                    Assert.Equal(expected[0].Sender, elem1.Sender);
                },
                elem2 =>
                {
                    Assert.Equal(expected[1].AdTitle, elem2.AdTitle);
                    Assert.Equal(expected[1].Content, elem2.Content);
                    Assert.Equal(expected[1].Sender, elem2.Sender);
                });
        }

        [Fact]
        public async Task GetMessageDetailsViewModelsAsync_WithCurrentUserEqualsToRecipient_ShouldMarkMessagesAsRead()
        {
            //Arrange
            var moqAdsService = new Mock<IAdsService>();
            var moqUsersService = new Mock<IUsersService>();
            moqUsersService.Setup(x => x.GetCurrentUserId())
                .Returns("RecipientId");

            var moqIMapper = new Mock<IMapper>();
            var context = InitializeContext.CreateContextForInMemory();

            messagesService = new MessagesService(context, moqAdsService.Object, moqUsersService.Object, moqIMapper.Object);

            var testingAd = CreateTestingAd();

            var messages = new List<Message>
            {
                new Message
                {
                    AdId = 1,
                    SenderId = "SenderId",
                    RecipientId = "RecipientId",
                    Content = "Content1"
                },
                new Message
                {
                    AdId = 1,
                    SenderId = "SenderId",
                    RecipientId = "RecipientId",
                    Content = "Content2"
                },
                new Message
                {
                    AdId = 1,
                    SenderId = "FakeSenderId",
                    RecipientId = "FakeRecipientId",
                    Content = "Content3"
                }
            };

            var sender = new SellMeUser
            {
                Id = "SenderId",
                UserName = "Pesho"
            };

            await context.SellMeUsers.AddAsync(sender);
            await context.Ads.AddAsync(testingAd);
            await context.Messages.AddRangeAsync(messages);
            await context.SaveChangesAsync();

            //Act
            var result = messagesService.GetMessageDetailsViewModelsAsync(1, "SenderId", "RecipientId");

            //Assert
            Assert.Equal(2, context.Messages.Count(x => x.IsRead));
        }

        [Fact]
        public async Task GetInboxMessagesViewModelsAsync_WithoutLoggedInUser_ShouldThrowAndInvalidOperationException()
        {

            //Arrange
            var expectedErrorMessage = "User is not logged in!";

            var moqAdsService = new Mock<IAdsService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqIMapper = new Mock<IMapper>();
            var context = InitializeContext.CreateContextForInMemory();

            messagesService = new MessagesService(context, moqAdsService.Object, moqUsersService.Object, moqIMapper.Object);

            //Assert and act
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                messagesService.GetInboxMessagesViewModelsAsync());
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task GetUnreadMessagesCountAsync_WithEmptyOrNullUserId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "User id can't be null or empty!";

            var moqAdsService = new Mock<IAdsService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqIMapper = new Mock<IMapper>();
            var context = InitializeContext.CreateContextForInMemory();

            messagesService = new MessagesService(context, moqAdsService.Object, moqUsersService.Object, moqIMapper.Object);

            //Assert and act
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                messagesService.GetUnreadMessagesCountAsync(string.Empty));
            var ex2 = await Assert.ThrowsAsync<ArgumentException>(() =>
                messagesService.GetUnreadMessagesCountAsync(null));
            Assert.Equal(expectedErrorMessage, ex.Message);
            Assert.Equal(expectedErrorMessage, ex2.Message);
        }

        [Fact]
        public async Task GetUnreadMessagesCountAsync_WithValidData_ShouldReturnCorrectResult()
        {
            //Arrange
            var expected = 2;

            var moqAdsService = new Mock<IAdsService>();
            var moqUsersService = new Mock<IUsersService>();
            var moqIMapper = new Mock<IMapper>();
            var context = InitializeContext.CreateContextForInMemory();

            messagesService = new MessagesService(context, moqAdsService.Object, moqUsersService.Object, moqIMapper.Object);

            var messages = new List<Message>
            {
                new Message
                {
                    AdId = 1,
                    SenderId = "SenderId",
                    RecipientId = "RecipientId",
                    Content = "Content1",
                    IsRead = false
                },
                new Message
                {
                    AdId = 1,
                    SenderId = "SenderId",
                    RecipientId = "RecipientId",
                    Content = "Content2",
                    IsRead = false

                },
                new Message
                {
                    AdId = 1,
                    SenderId = "SenderId",
                    RecipientId = "FakeRecipientId",
                    Content = "Content3",
                    IsRead = false
                },
                new Message
                {
                    AdId = 1,
                    SenderId = "SenderId",
                    RecipientId = "RecipientId",
                    Content = "Content3",
                    IsRead = true
                }
            };

            await context.Messages.AddRangeAsync(messages);
            await context.SaveChangesAsync();

            //Act
            var actual = await messagesService.GetUnreadMessagesCountAsync("RecipientId");

            //Assert
            Assert.Equal(expected, actual);
        }


        private Ad CreateTestingAd()
        {
            var testingAd = new Ad
            {
                Id = 1,
                Title = "Iphone 6s",
                Description = "PerfectCondition",
                ActiveFrom = DateTime.UtcNow,
                ActiveTo = DateTime.UtcNow.AddDays(30),
                AvailabilityCount = 1,
                Price = 120,
                SellerId = "SellerId",
                Condition = new Condition { Name = "Brand New" },
                Address = new Address
                {
                    Country = "Bulgaria",
                    City = "Sofia",
                    Street = "Ivan Vazov",
                    District = "Student city",
                    ZipCode = 1000,
                    PhoneNumber = "0895335532",
                    EmailAddress = "Ivan@gmail.com"
                }
            };

            return testingAd;
        }

        //[Fact]
        //public async Task GetInboxMessagesViewModelsAsync_WithValidData_ShouldReturnCorrectCount()
        //{
        //    //Arrange
        //    var expectedCount = 2;

        //    var moqAdsService = new Mock<IAdsService>();
        //    var moqUsersService = new Mock<IUsersService>();
        //    moqUsersService.Setup(x => x.GetCurrentUserId())
        //        .Returns("RecipientId");

        //    var moqIMapper = new Mock<IMapper>();
        //    var context = InitializeContext.CreateContextForInMemory();

        //    var testingAds = new List<Ad>
        //    {
        //        new Ad
        //        {
        //            Id = 1,
        //            Messages = new List<Message>
        //            {
        //                new Message{ Id = 1,CreatedOn = DateTime.UtcNow.AddDays(-5), SenderId = "SenderId", RecipientId = "RecipientId"},
        //                new Message{ Id = 2,CreatedOn = DateTime.UtcNow.AddDays(-10), SenderId = "SenderId", RecipientId = "RecipientId"},
        //                new Message{ Id = 3, CreatedOn = DateTime.UtcNow.AddDays(-10), SenderId = "SenderId2", RecipientId = "RecipientId"},
        //            }
        //        }
        //    };

        //    await context.Ads.AddRangeAsync(testingAds);
        //    await context.SaveChangesAsync();

        //    this.messagesService = new MessagesService(context, moqAdsService.Object, moqUsersService.Object, moqIMapper.Object);

        //    //Act
        //    var actual = await this.messagesService.GetInboxMessagesViewModelsAsync();

        //    //Assert
        //    Assert.Equal(expectedCount, actual.Count());
        //}
    }
}
