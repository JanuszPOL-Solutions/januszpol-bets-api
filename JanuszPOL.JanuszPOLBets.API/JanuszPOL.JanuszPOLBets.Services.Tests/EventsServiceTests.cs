using JanuszPOL.JanuszPOLBets.Data._DbContext.Mappings;
using JanuszPOL.JanuszPOLBets.Repository.Events;
using JanuszPOL.JanuszPOLBets.Repository.Events.Dto;
using JanuszPOL.JanuszPOLBets.Repository.Games;
using JanuszPOL.JanuszPOLBets.Services.Events;
using JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels;
using Moq;
using static JanuszPOL.JanuszPOLBets.Data.Entities.Events.EventType;

namespace JanuszPOL.JanuszPOLBets.Services.Tests
{
    internal class EventsServiceTests
    {
        private EventsService _eventsService;
        private Mock<IEventsRepository> _eventsRepositoryMock;
        private Mock<IGamesRepository> _gamesRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _eventsRepositoryMock = new Mock<IEventsRepository>(MockBehavior.Strict);
            _gamesRepositoryMock = new Mock<IGamesRepository>(MockBehavior.Strict);

            _eventsRepositoryMock.Setup(x => x.Team1WinEventId).Returns(EventMapping.TeamOneWinEventId);
            _eventsRepositoryMock.Setup(x => x.Team2WinEventId).Returns(EventMapping.TeamTwoWinEventId);
            _eventsRepositoryMock.Setup(x => x.TieEventId).Returns(EventMapping.TieEventId);

            _eventsService = new EventsService(_eventsRepositoryMock.Object, _gamesRepositoryMock.Object);
        }

        [Test]
        public async Task BaseBetWorks() 
        {
            int accountId = 1;
            int gameId = 1;
            int eventBetId = 99;
            int eventId = EventMapping.TeamOneWinEventId;

            _eventsRepositoryMock.Setup(x => x.GetEvent(eventId)).ReturnsAsync(await Task.FromResult(new EventDto
            {
                BetCost = 0,
                WinValue = 3,
                Id = eventId
            }));

            _eventsRepositoryMock
                .Setup(x => x.GetEventBetsForGameAndUser(gameId, accountId))
                .ReturnsAsync(await Task.FromResult(new List<ExistingEventBetDto>
                {
                    new ExistingEventBetDto
                    {
                        AccountId = accountId,
                        GameId = 2,
                        EventId = 5,
                        EventBetId = 1
                    }
                }));

            _eventsRepositoryMock.Setup(x => x.AddEventBet(It.Is<AddEventBetDto>(x =>
                x.EventId == EventMapping.TeamOneWinEventId &&
                x.AccountId == accountId &&
                x.GameId == gameId &&
                x.Value1 == null &&
                x.Value2 == null))).ReturnsAsync(await Task.FromResult(new Repository.Games.Dto.GameEventBetDto
                {
                    EventId = eventId,
                    EventTypeId = Data.Entities.Events.EventType.RuleType.BaseBet,
                    Id = eventBetId
                }));

            var result = await _eventsService.AddBaseEventBet(new BaseEventBetInput
            {
                AccountId = 1,
                BetType = BaseBetType.Team1,
                GameId = gameId
            });

            Assert.AreEqual(RuleType.BaseBet, result.Result.EventTypeId);
            Assert.AreEqual(eventId, result.Result.EventId);
            Assert.AreEqual(eventBetId, result.Result.Id);
        }

        [Test]
        public void BaseBetEditingWorks() { }

        [Test]
        public void EventBet_NewEventBetWorks() { }

        [Test]
        public void EventBet_EditingEventBetWorks() { }

        [Test]
        public void EventBet_CannotHaveMoreThanTwoEvents() { }

        [Test]
        public void EventBet_CannotBetAnEventIfNoPointsLeft() { }
    }
}
