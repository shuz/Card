USE [master]
GO
CREATE DATABASE [F:\WORKING\CARD\CARD\BIN\RELEASE\CARDRESULT.MDF] ON 
( FILENAME = N'F:\Working\Card\Card\bin\Release\CardResult.mdf' ),
( FILENAME = N'F:\Working\Card\Card\bin\Release\CardResult_log.ldf' )
 FOR ATTACH
GO
if not exists (select name from master.sys.databases sd where name = N'F:\WORKING\CARD\CARD\BIN\RELEASE\CARDRESULT.MDF' and SUSER_SNAME(sd.owner_sid) = SUSER_SNAME() ) EXEC [F:\WORKING\CARD\CARD\BIN\RELEASE\CARDRESULT.MDF].dbo.sp_changedbowner @loginame=N'SHUZENTERTAIN\shuz', @map=false
GO

USE [F:\WORKING\CARD\CARD\BIN\RELEASE\CARDRESULT.MDF]
GO

SELECT * FROM FinalExprCount
ORDER BY [Count] DESC

SELECT SUM([Count])
FROM FinalExprCount

GO

UPDATE CardSolutions
SET Easiness = ISNULL((
	SELECT SUM([Count]) FROM FinalExprCount
	WHERE FinalExprCount.FinalExpr IN (
		SELECT FinalExpr FROM SolutionDetail
		WHERE SolutionDetail.Card = CardSolutions.Card
    )), 0)
GO

SELECT * FROM CardSolutions
ORDER BY Easiness ASC, SolutionCount ASC

SELECT * FROM SolutionDetail

SELECT COUNT(*) FROM CardSolutions
WHERE SolutionCount != 0

GO

USE [master]
GO
EXEC master.dbo.sp_detach_db @dbname = N'F:\WORKING\CARD\CARD\BIN\RELEASE\CARDRESULT.MDF', @keepfulltextindexfile=N'true'
GO
