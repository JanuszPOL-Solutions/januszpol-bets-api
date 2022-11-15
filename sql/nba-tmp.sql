delete from EventBet
update [Event] set BetCost=0 where Id in (1,2,3)


INSERT INTO [dbo].[Event] ([Name], [Description], [BetCost], [WinValue], [EventTypeId])
     VALUES ('[TMP] Over 100pkt team1', 'Team 1 rzuci powyżej 100pkt (tymczasowy event)', 2, 4, 2)
INSERT INTO [dbo].[Event] ([Name], [Description], [BetCost], [WinValue], [EventTypeId])
     VALUES ('[TMP] Over 100pkt team2', 'Team 2 rzuci powyżej 100pkt (tymczasowy event)', 2, 4, 2)


insert into Teams ([Name]) values('Grizzlies')
insert into Teams ([Name]) values('Pelicans')
insert into Teams ([Name]) values('LA Clippers')
insert into Teams ([Name]) values('Mavericks')
insert into Teams ([Name]) values('NYC')
insert into Teams ([Name]) values('Utah Jazz')
insert into Teams ([Name]) values('SAS')
insert into Teams ([Name]) values('Portland')
insert into Teams ([Name]) values('Brooklyn')
insert into Teams ([Name]) values('Kings')
