DELETE EventBet
DELETE Games
DELETE Teams

DELETE AspNetUsers
DELETE [Event] where Id > 3 and ID != 8

DBCC CHECKIDENT ('Games', RESEED, 0);
DBCC CHECKIDENT ('Teams', RESEED, 0);
DBCC CHECKIDENT ('EventBet', RESEED, 0);
GO

CREATE TABLE [dbo].[TmpGames](
	[Date] [datetime] NOT NULL,
	[Group] [nvarchar](50) NOT NULL,
	[Team1] [nvarchar](150) NOT NULL,
	[Team2] [nvarchar](150) NOT NULL
) ON [PRIMARY]
GO

INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/20/2022 17:00','A','Katar','Ekwador');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/21/2022 17:00','A','Senegal','Holandia');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/21/2022 14:00','B','Anglia','Iran');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/21/2022 20:00','B','USA','Walia');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/22/2022 14:00','D','Dania','Tunezja');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/22/2022 20:00','D','Francja','Australia');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/22/2022 11:00','C','Argentyna','Arabia S.');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/22/2022 17:00','C','Meksyk','Polska');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/23/2022 20:00','F','Belgia','Kanada');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/23/2022 17:00','E','Hiszpania','Kostaryka');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/23/2022 14:00','E','Niemcy','Japonia');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/23/2022 11:00','F','Maroko','Chorwacja');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/24/2022 11:00','G','Szwajcaria','Kamerun');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/24/2022 14:00','H','Urugwaj','Korea Płd');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/24/2022 17:00','H','Portugalia','Ghana');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/24/2022 20:00','G','Brazylia','Serbia');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/25/2022 11:00','B','Walia','Iran');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/25/2022 14:00','A','Katar','Senegal');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/25/2022 17:00','A','Holandia','Ekwador');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/25/2022 20:00','B','Anglia','USA');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/26/2022 11:00','D','Tunezja','Australia');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/26/2022 14:00','C','Polska','Arabia S.');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/26/2022 17:00','D','Francja','Dania');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/26/2022 20:00','C','Argentyna','Meksyk');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/27/2022 11:00','E','Japonia','Kostaryka');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/27/2022 14:00','F','Belgia','Maroko');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/27/2022 17:00','F','Chorwacja','Kanada');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/27/2022 20:00','E','Hiszpania','Niemcy');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/28/2022 11:00','G','Kamerun','Serbia');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/28/2022 14:00','H','Korea Płd','Ghana');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/28/2022 17:00','G','Brazylia','Szwajcaria');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/28/2022 20:00','H','Portugalia','Urugwaj');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/29/2022 20:00','B','Walia','Anglia');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/29/2022 20:00','B','Iran','USA');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/29/2022 16:00','A','Ekwador','Senegal');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/29/2022 16:00','A','Holandia','Katar');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/30/2022 16:00','D','Australia','Dania');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/30/2022 16:00','D','Tunezja','Francja');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/30/2022 20:00','C','Polska','Argentyna');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('11/30/2022 20:00','C','Arabia S.','Meksyk');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('12/1/2022 16:00','F','Chorwacja','Belgia');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('12/1/2022 16:00','F','Kanada','Maroko');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('12/1/2022 20:00','E','Japonia','Hiszpania');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('12/1/2022 20:00','E','Kostaryka','Niemcy');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('12/2/2022 16:00','H','Ghana','Urugwaj');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('12/2/2022 16:00','H','Korea Płd','Portugalia');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('12/2/2022 20:00','G','Serbia','Szwajcaria');
INSERT INTO TmpGames([Date], [Group], Team1, Team2) VALUES('12/2/2022 20:00','G','Kamerun','Brazylia');

INSERT INTO Teams (Name)  (
SELECT Team1 FROM TmpGames
UNION 
Select Team2 FRom TMpGames
)

INSERT INTO Games (GameDate, PhaseName, PhaseId, Team1Id, Team2Id)
SELECT [Date] as GameDate, CONCAT('Grupa ', [Group]) as PhaseName, 0 as PhaseId, t1.Id as Team1Id, t2.Id as Team2Id
FROM TmpGames tg
LEFT JOIN Teams t1 on t1.Name=tg.Team1
LEFT JOIN Teams t2 on t2.Name=tg.Team2

DROP TABLE TmpGames


INSERT [Event] (Name, BetCost, WinValue, EventTypeId) VALUES ('Rezultat meczu', 1, 6, 2)
INSERT [Event] (Name, BetCost, WinValue, EventTypeId) VALUES ('Drużyna 1 strzeli gola', 1, 2, 2)
INSERT [Event] (Name, BetCost, WinValue, EventTypeId) VALUES ('Drużyna 2 strzeli gola', 1, 2, 2)
INSERT [Event] (Name, BetCost, WinValue, EventTypeId) VALUES ('Obie strzelą gola', 1, 3, 2)
INSERT [Event] (Name, BetCost, WinValue, EventTypeId) VALUES ('Pierwszy gol drużyny 1', 1, 3, 2)
INSERT [Event] (Name, BetCost, WinValue, EventTypeId) VALUES ('Pierwszy gol drużyny 2', 1, 3, 2)
INSERT [Event] (Name, BetCost, WinValue, EventTypeId) VALUES ('Nie będzie żółtej kartki', 1, 7, 2)
INSERT [Event] (Name, BetCost, WinValue, EventTypeId) VALUES ('Wystąpi czerwona kartka', 1, 5, 2)
INSERT [Event] (Name, BetCost, WinValue, EventTypeId) VALUES ('1 połowa bez remisu', 1, 2, 2)
INSERT [Event] (Name, BetCost, WinValue, EventTypeId) VALUES ('Gol w obu połowach', 1, 2, 2)
INSERT [Event] (Name, BetCost, WinValue, EventTypeId) VALUES ('Gol w minucie meczu (85:00 lub później)', 1, 5, 2)
INSERT [Event] (Name, BetCost, WinValue, EventTypeId) VALUES ('Pseudokibice przerwą mecz', 1, 10, 2)
INSERT [Event] (Name, BetCost, WinValue, EventTypeId) VALUES ('Przerwany mecz z powodu za wysokiej temperatury', 1, 10, 2)
INSERT [Event] (Name, BetCost, WinValue, EventTypeId) VALUES ('Piłkarz zemdleje (odwodnienie, za wysoka temp etc.)', 1, 10, 2)
INSERT [Event] (Name, BetCost, WinValue, EventTypeId) VALUES ('Góral czerwo', 1, 5, 2)
INSERT [Event] (Name, BetCost, WinValue, EventTypeId) VALUES ('Krycha strata, po której tracimy gola', 1, 5, 2)
