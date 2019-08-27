namespace SellMe.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Data.Models;
    using Moq;
    using SellMe.Common;
    using Services;
    using Services.Interfaces;
    using Services.Paging;
    using Web.ViewModels.BindingModels.Reviews;
    using Web.ViewModels.ViewModels.Reviews;
    using Xunit;

    public class ReviewsServiceTests
    {
        private IReviewsService reviewsService;

        public ReviewsServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task GetReviewsBindingModelByUserId_WithEmptyOrNullUserId_ShouldThrowAnArgumentException()
        {
            //Arrange
            var expectedErrorMessage = "User id can't be null or empty!";
            var moqUsersService = new Mock<IUsersService>();
            var context = InitializeContext.CreateContextForInMemory();
            reviewsService = new ReviewsService(context, moqUsersService.Object);


            //Assert and act
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                reviewsService.GetReviewsBindingModelByUserId(string.Empty, GlobalConstants.DefaultPageNumber, GlobalConstants.DefaultPageSize));
            var ex2 = await Assert.ThrowsAsync<ArgumentException>(() =>
                reviewsService.GetReviewsBindingModelByUserId(null, GlobalConstants.DefaultPageNumber, GlobalConstants.DefaultPageSize));
            Assert.Equal(expectedErrorMessage, ex.Message);
            Assert.Equal(expectedErrorMessage, ex2.Message);
        }

        [Fact]
        public async Task GetReviewsBindingModelByUserId_WithValid_ShouldReturnCorrectResult()
        {
            //Arrange
            var expectedViewModelsCount = 3;
            var listOfReviewViewModels = new List<ReviewViewModel>
            {
                new ReviewViewModel
                {
                    Content = "Comment for the first review.",
                    Rating = 2,
                    Sender = "Creator"
                },
                new ReviewViewModel
                {
                    Content = "Comment for the second review.",
                    Rating = 3,
                    Sender = "Creator"
                },
                new ReviewViewModel
                {
                    Content = "Comment for the third review.",
                    Rating = 5,
                    Sender = "Creator"
                }
            };

            var expected = new ReviewsBindingModel
            {
                OwnerId = "OwnerId",
                OwnerUsername = "Owner",
                SenderId = "CreatorId",
                Votes = new List<int> {0, 0, 1, 1, 0, 1},
                AverageVote = new List<int>{2, 3, 5}.Average(),
                ViewModels = new PaginatedList<ReviewViewModel>(listOfReviewViewModels, 3, 1, 10)
            };

            var moqUsersService = new Mock<IUsersService>();
            moqUsersService.Setup(x => x.GetUserByIdAsync("OwnerId"))
                .ReturnsAsync(new SellMeUser
                {
                    Id = "OwnerId",
                    UserName = "Owner"
                });
            moqUsersService.Setup(x => x.GetCurrentUserId())
                .Returns("CreatorId");

            var context = InitializeContext.CreateContextForInMemory();
            reviewsService = new ReviewsService(context, moqUsersService.Object);

            var testingReviews = new List<Review>
            {
                new Review
                {
                    OwnerId = "OwnerId",
                    CreatorId = "CreatorId",
                    Comment = "Comment for the first review.",
                    Rating = 2
                },
                new Review
                {
                    OwnerId = "OwnerId",
                    CreatorId = "CreatorId",
                    Comment = "Comment for the second review.",
                    Rating = 3
                },
                new Review
                {
                    OwnerId = "OwnerId",
                    CreatorId = "CreatorId",
                    Comment = "Comment for the third review.",
                    Rating = 5
                }
            };

            var testingUsers = new List<SellMeUser>
            {
                new SellMeUser
                {
                    Id = "CreatorId",
                    UserName = "Creator"
                },
                new SellMeUser
                {
                    Id = "OwnerId",
                    UserName = "Owner"
                }
            };

            await context.Reviews.AddRangeAsync(testingReviews);
            await context.SellMeUsers.AddRangeAsync(testingUsers);
            await context.SaveChangesAsync();

            //Act
            var actual = await reviewsService.GetReviewsBindingModelByUserId("OwnerId", 1, 10);

            Assert.Equal(expected.OwnerId, actual.OwnerId);
            Assert.Equal(expected.OwnerUsername, actual.OwnerUsername);
            Assert.Equal(expected.SenderId, actual.SenderId);
            Assert.Equal(expected.AverageVote, actual.AverageVote);
            Assert.Equal(expectedViewModelsCount, actual.ViewModels.Count);
            Assert.Equal(expected.ViewModels[0].Content, actual.ViewModels[0].Content);
            Assert.Equal(expected.ViewModels[0].Rating, actual.ViewModels[0].Rating);
            Assert.Equal(expected.ViewModels[0].Sender, actual.ViewModels[0].Sender);
        }

        [Theory]
        [InlineData("Owner", "Creator", "")]
        [InlineData("", "Creator", "Content")]
        [InlineData(null, "Creator", "content")]
        [InlineData("Owner", "Creator", null)]
        [InlineData("Owner", "", null)]
        [InlineData("", "", "")]
        [InlineData(null, null, null)]
        public async Task CreateReview_WithNullOrEmptyArguments_ShouldThrowAnArgumentException(string ownerId, string creatorId, string content)
        {

            //Arrange
            var expectedErrorMessage = "Some of the arguments are null or empty!";
            var moqUsersService = new Mock<IUsersService>();
            var context = InitializeContext.CreateContextForInMemory();
            reviewsService = new ReviewsService(context, moqUsersService.Object);


            //Assert and act
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                reviewsService.CreateReview(ownerId, creatorId, content, 1));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        [InlineData(6)]
        [InlineData(int.MaxValue)]
        public async Task CreateReview_WithRatingOutOfRange_ShouldThrowAnArgumentException(int rating)
        {
            //Arrange
            var expectedErrorMessage = "The rating must be in range between 1 and 5";

            var moqUsersService = new Mock<IUsersService>();
            var context = InitializeContext.CreateContextForInMemory();
            reviewsService = new ReviewsService(context, moqUsersService.Object);


            //Assert and act
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                reviewsService.CreateReview("OwnerId", "CreatorId", "Content", rating));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task CreateReview_WithOwnerEqualToCreator_ShouldThrowAnInvalidOperationException()
        {
            //Arrange
            var expectedErrorMessage = "Seller of the ad can't leave reviews for his ads!";

            var moqUsersService = new Mock<IUsersService>();
            var context = InitializeContext.CreateContextForInMemory();
            reviewsService = new ReviewsService(context, moqUsersService.Object);


            //Assert and act
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                reviewsService.CreateReview("CreatorId", "CreatorId", "Content", 3));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task CreateReview_WithValidData_ShouldCreateAReview()
        {
            //Arrange
            var expectedReviewsCount = 1;

            var moqUsersService = new Mock<IUsersService>();
            var context = InitializeContext.CreateContextForInMemory();
            reviewsService = new ReviewsService(context, moqUsersService.Object);

            //Act
            await reviewsService.CreateReview("OwnerId", "CreatorId", "Content", 3);

            //Assert
            Assert.Equal(expectedReviewsCount, context.Reviews.Count());
        }

        [Fact]
        public void CheckOwnerIdAndSellerId_WithOwnerEqualsToCreator_ShouldReturnTrue()
        {
            //Arrange
            var moqUsersService = new Mock<IUsersService>();
            var context = InitializeContext.CreateContextForInMemory();
            reviewsService = new ReviewsService(context, moqUsersService.Object);

            //Act
            var actual =  reviewsService.CheckOwnerIdAndSellerId("CreatorId", "CreatorId");

            //Assert
            Assert.True(actual);
        }

        [Fact]
        public void CheckOwnerIdAndSellerId_WithOwnerDifferentFromCreator_ShouldReturnFalse()
        {
            //Arrange
            var moqUsersService = new Mock<IUsersService>();
            var context = InitializeContext.CreateContextForInMemory();
            reviewsService = new ReviewsService(context, moqUsersService.Object);

            //Act
            var actual = reviewsService.CheckOwnerIdAndSellerId("CreatorId", "AnotherUserId");

            //Assert
            Assert.False(actual);
        }
    }
}
