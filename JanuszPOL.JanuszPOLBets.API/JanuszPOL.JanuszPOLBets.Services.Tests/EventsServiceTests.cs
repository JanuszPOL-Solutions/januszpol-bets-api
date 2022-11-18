using JanuszPOL.JanuszPOLBets.Data._DbContext.Mappings;
using JanuszPOL.JanuszPOLBets.Data.Entities;
using JanuszPOL.JanuszPOLBets.Repository.Events;
using JanuszPOL.JanuszPOLBets.Repository.Events.Dto;
using JanuszPOL.JanuszPOLBets.Repository.Games;
using JanuszPOL.JanuszPOLBets.Repository.Games.Dto;
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

            IEnumerable<EventBetResultDto> scores = new List<EventBetResultDto>
            {
                new EventBetResultDto
                {
                    BetCost = 0,
                    Result = true,
                    EventId = 1
                }
            };

            _eventsRepositoryMock.Setup(x => x.GetUserBets(It.IsAny<long>())).Returns(Task.FromResult(scores));
            _eventsService = new EventsService(_eventsRepositoryMock.Object, _gamesRepositoryMock.Object);
        }

        [Test]
        public async Task BaseBetWorks() 
        {
            int accountId = 1;
            int gameId = 1;
            int eventBetId = 99;
            int eventId = EventMapping.TeamOneWinEventId;

            _eventsRepositoryMock.Setup(x => x.ValidateUserPointsForBet(accountId, eventId, gameId)).ReturnsAsync(await Task.FromResult(true));

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

            _gamesRepositoryMock.Setup(x => x.GetGameById(gameId))
                .ReturnsAsync(await Task.FromResult(new SingleGameDto
                {
                    Started = false
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
        public async Task CannotPlaceBetAfteMatchStarted()
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

            _gamesRepositoryMock.Setup(x => x.GetGameById(gameId))
                .ReturnsAsync(await Task.FromResult(new SingleGameDto
                {
                    Started = true
                }));

            var result = await _eventsService.AddBaseEventBet(new BaseEventBetInput
            {
                AccountId = 1,
                BetType = BaseBetType.Team1,
                GameId = gameId
            });

            Assert.IsTrue(result.Errors.Count > 0);
        }

        [Test]
        public async Task BaseBetEditingWorks() 
        {
            int accountId = 1;
            int gameId = 1;
            int baseEventBetId = 12;

            _eventsRepositoryMock.Setup(x => x.ValidateUserPointsForBet(accountId, EventMapping.TeamTwoWinEventId, gameId))
                .ReturnsAsync(await Task.FromResult(true));

            _eventsRepositoryMock.Setup(x => x.GetEvent(EventMapping.TeamOneWinEventId)).ReturnsAsync(await Task.FromResult(new EventDto
            {
                BetCost = 0,
                WinValue = 3,
                Id = EventMapping.TeamOneWinEventId
            }));
            _eventsRepositoryMock.Setup(x => x.GetEvent(EventMapping.TeamTwoWinEventId)).ReturnsAsync(await Task.FromResult(new EventDto
            {
                BetCost = 0,
                WinValue = 3,
                Id = EventMapping.TeamTwoWinEventId
            }));

            _eventsRepositoryMock
                .Setup(x => x.GetEventBetsForGameAndUser(gameId, accountId))
                .ReturnsAsync(await Task.FromResult(new List<ExistingEventBetDto>
                {
                    new ExistingEventBetDto
                    {
                        AccountId = accountId,
                        GameId = gameId,
                        EventId = 5,
                        EventBetId = 1
                    },
                    new ExistingEventBetDto
                    {
                        AccountId = accountId,
                        GameId = gameId,
                        EventId = EventMapping.TeamOneWinEventId,
                        EventBetId = baseEventBetId,
                        EventType = RuleType.BaseBet
                    }
                }));

            _eventsRepositoryMock.Setup(x => x.UpdateEventBet(It.Is<ExistingEventBetDto>(x =>
                x.EventBetId == baseEventBetId &&
                x.EventId == EventMapping.TeamTwoWinEventId &&
                x.AccountId == accountId &&
                x.GameId == gameId &&
                x.Value1 == null &&
                x.Value2 == null))).ReturnsAsync(await Task.FromResult(new Repository.Games.Dto.GameEventBetDto
                {
                    EventId = EventMapping.TeamTwoWinEventId,
                    EventTypeId = Data.Entities.Events.EventType.RuleType.BaseBet,
                    Id = baseEventBetId
                }));

            _gamesRepositoryMock.Setup(x => x.GetGameById(gameId))
                .ReturnsAsync(await Task.FromResult(new SingleGameDto
                {
                    Started = false
                }));

            var result = await _eventsService.AddBaseEventBet(new BaseEventBetInput
            {
                AccountId = 1,
                BetType = BaseBetType.Team2,
                GameId = gameId
            });

            Assert.AreEqual(RuleType.BaseBet, result.Result.EventTypeId);
            Assert.AreEqual(EventMapping.TeamTwoWinEventId, result.Result.EventId);
            Assert.AreEqual(baseEventBetId, result.Result.Id);
        }

        [Test]
        public async Task EventBet_NewBooleanEventBetWorks() 
        {
            int accountId = 1;
            int gameId = 1;
            int eventBetId = 99;
            int eventId = 5;

            _eventsRepositoryMock.Setup(x => x.GetEvent(eventId)).ReturnsAsync(await Task.FromResult(new EventDto
            {
                BetCost = 1,
                WinValue = 2,
                Id = eventId,
                EventType = RuleType.Boolean
            }));

            _eventsRepositoryMock.Setup(x => x.ValidateUserPointsForBet(accountId, eventId, gameId))
                .ReturnsAsync(await Task.FromResult(true));

            _eventsRepositoryMock
                .Setup(x => x.GetEventBetsForGameAndUser(gameId, accountId))
                .ReturnsAsync(await Task.FromResult(new List<ExistingEventBetDto>
                {
                    new ExistingEventBetDto
                    {
                        AccountId = accountId,
                        GameId = 2,
                        EventId = 1,
                        EventBetId = 1
                    },
                    new ExistingEventBetDto
                    {
                        AccountId = accountId,
                        GameId = 2,
                        EventId = 4,
                        EventBetId = 2
                    }
                }));

            _eventsRepositoryMock.Setup(x => x.AddEventBet(It.Is<AddEventBetDto>(x =>
                x.EventId == eventId &&
                x.AccountId == accountId &&
                x.GameId == gameId &&
                x.Value1 == null &&
                x.Value2 == null))).ReturnsAsync(await Task.FromResult(new Repository.Games.Dto.GameEventBetDto
                {
                    EventId = eventId,
                    EventTypeId = Data.Entities.Events.EventType.RuleType.Boolean,
                    Id = eventBetId
                }));

            _gamesRepositoryMock.Setup(x => x.GetGameById(gameId))
                .ReturnsAsync(await Task.FromResult(new SingleGameDto
                {
                    Started = false
                }));

            var result = await _eventsService.AddEventBet(new EventBetInput
            {
                AccountId = accountId,
                EventId = eventId,
                GameId = gameId,
                IsBaseBet = false
            });

            Assert.AreEqual(RuleType.Boolean, result.Result.EventTypeId);
            Assert.AreEqual(eventId, result.Result.EventId);
            Assert.AreEqual(eventBetId, result.Result.Id);
        }

        [Test]
        public async Task EventBet_EditingEventBetWorks() 
        {
            int accountId = 1;
            int gameId = 1;
            int eventBetId = 12;
            int eventId = 5;
            int value1 = 2;
            int value2 = 3;

            _eventsRepositoryMock.Setup(x => x.GetEvent(eventId)).ReturnsAsync(await Task.FromResult(new EventDto
            {
                BetCost = 1,
                WinValue = 2,
                Id = eventId
            }));

            _eventsRepositoryMock.Setup(x => x.ValidateUserPointsForBet(accountId, eventId, gameId))
                .ReturnsAsync(await Task.FromResult(true));

            _eventsRepositoryMock
                .Setup(x => x.GetEventBetsForGameAndUser(gameId, accountId))
                .ReturnsAsync(await Task.FromResult(new List<ExistingEventBetDto>
                {
                    new ExistingEventBetDto
                    {
                        AccountId = accountId,
                        GameId = gameId,
                        EventId = eventId,
                        EventBetId = eventBetId,
                        EventType = RuleType.TwoExactValues
                    },
                    new ExistingEventBetDto
                    {
                        AccountId = accountId,
                        GameId = gameId,
                        EventId = 6,
                        EventBetId = 22
                    },
                    new ExistingEventBetDto
                    {
                        AccountId = accountId,
                        GameId = gameId,
                        EventId = EventMapping.TeamOneWinEventId,
                        EventBetId = 33
                    }
                }));

            _eventsRepositoryMock.Setup(x => x.UpdateEventBet(It.Is<ExistingEventBetDto>(x =>
                x.EventBetId == eventBetId &&
                x.EventId == eventId &&
                x.AccountId == accountId &&
                x.GameId == gameId &&
                x.Value1 == value1 &&
                x.Value2 == value2))).ReturnsAsync(await Task.FromResult(new Repository.Games.Dto.GameEventBetDto
                {
                    EventId = EventMapping.TeamTwoWinEventId,
                    EventTypeId = Data.Entities.Events.EventType.RuleType.BaseBet,
                    Id = eventBetId
                }));

            _gamesRepositoryMock.Setup(x => x.GetGameById(gameId))
                .ReturnsAsync(await Task.FromResult(new SingleGameDto
                {
                    Started = false
                }));

            var result = await _eventsService.AddEventBet(new EventBetInput
            {
                GameId = gameId,
                AccountId = accountId,
                IsBaseBet = false,
                EventId = eventId,
                Value1 = value1,
                Value2 = value2
            });

            Assert.AreEqual(RuleType.BaseBet, result.Result.EventTypeId);
            Assert.AreEqual(EventMapping.TeamTwoWinEventId, result.Result.EventId);
            Assert.AreEqual(eventBetId, result.Result.Id);
        }

        [Test]
        public async Task EventBet_CannotHaveMoreThanTwoEvents() 
        {
            int accountId = 1;
            int gameId = 1;
            int eventId = 6;

            _eventsRepositoryMock.Setup(x => x.GetEvent(eventId)).ReturnsAsync(await Task.FromResult(new EventDto
            {
                BetCost = 1,
                WinValue = 2,
                Id = eventId,
                EventType = RuleType.Boolean
            }));

            _eventsRepositoryMock.Setup(x => x.ValidateUserPointsForBet(accountId, eventId, gameId))
                .ReturnsAsync(await Task.FromResult(true));

            _eventsRepositoryMock
                .Setup(x => x.GetEventBetsForGameAndUser(gameId, accountId))
                .ReturnsAsync(await Task.FromResult(new List<ExistingEventBetDto>
                {
                    new ExistingEventBetDto
                    {
                        AccountId = accountId,
                        GameId = 2,
                        EventId = 1,
                        EventBetId = 1
                    },
                    new ExistingEventBetDto
                    {
                        AccountId = accountId,
                        GameId = 2,
                        EventId = 4,
                        EventBetId = 2
                    },
                    new ExistingEventBetDto
                    {
                        AccountId = accountId,
                        GameId = 2,
                        EventId = 5,
                        EventBetId = 2
                    }
                }));

            _gamesRepositoryMock.Setup(x => x.GetGameById(gameId))
                .ReturnsAsync(await Task.FromResult(new SingleGameDto
                {
                    Started = false
                }));

            var result = await _eventsService.AddEventBet(new EventBetInput
            {
                AccountId = accountId,
                EventId = eventId,
                GameId = gameId,
                IsBaseBet = false
            });

            Assert.IsTrue(result.Errors.Count > 0);
        }

        [Test]
        public async Task EventBet_CannotBetAnEventIfNoPointsLeft() 
        {
            IEnumerable<EventBetResultDto> scores = new List<EventBetResultDto>
            {
                new EventBetResultDto
                {
                    BetCost = 1,
                    EventId = 5
                }
            };
            _eventsRepositoryMock.Setup(x => x.GetUserBets(It.IsAny<long>())).Returns(Task.FromResult(scores));

            int accountId = 1;
            int gameId = 1;
            int eventId = 6;

            _eventsRepositoryMock.Setup(x => x.ValidateUserPointsForBet(accountId, eventId, gameId)).ReturnsAsync(await Task.FromResult(false));

            _eventsRepositoryMock.Setup(x => x.GetEvent(eventId)).ReturnsAsync(await Task.FromResult(new EventDto
            {
                BetCost = 1,
                WinValue = 2,
                Id = eventId,
                EventType = RuleType.Boolean
            }));

            _eventsRepositoryMock
                .Setup(x => x.GetEventBetsForGameAndUser(gameId, accountId))
                .ReturnsAsync(await Task.FromResult(new List<ExistingEventBetDto>
                {
                    new ExistingEventBetDto
                    {
                        AccountId = accountId,
                        GameId = 2,
                        EventId = 1,
                        EventBetId = 1
                    }
                }));

            _gamesRepositoryMock.Setup(x => x.GetGameById(gameId))
                .ReturnsAsync(await Task.FromResult(new SingleGameDto
                {
                    Started = false
                }));

            var result = await _eventsService.AddEventBet(new EventBetInput
            {
                AccountId = accountId,
                EventId = eventId,
                GameId = gameId,
                IsBaseBet = false
            });

            Assert.IsTrue(result.Errors.Count > 0);
        }

        [Test]
        public async Task EventBet_CanBetBaseBetIfNoPointsLeft()
        {
            IEnumerable<EventBetResultDto> scores = new List<EventBetResultDto>
            {
                new EventBetResultDto
                {
                    BetCost = 1,
                    EventId = 5
                }
            };


            _eventsRepositoryMock.Setup(x => x.GetUserBets(It.IsAny<long>())).Returns(Task.FromResult(scores));

            int accountId = 1;
            int gameId = 1;
            int eventBetId = 99;
            int eventId = EventMapping.TeamOneWinEventId;

            _eventsRepositoryMock.Setup(x => x.ValidateUserPointsForBet(accountId, eventId, gameId)).ReturnsAsync(await Task.FromResult(true));

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

            _gamesRepositoryMock.Setup(x => x.GetGameById(gameId))
                .ReturnsAsync(await Task.FromResult(new SingleGameDto
                {
                    Started = false
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
    }
}
