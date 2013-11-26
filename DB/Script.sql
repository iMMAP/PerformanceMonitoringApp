USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[GetTargetAchievedByLocation]    Script Date: 11/26/2013 7:25:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[GetTargetAchievedByLocation]
	@locationIds VARCHAR(MAX),
	@locationType INT,
	@dataId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * INTO #locations FROM dbo.fn_ParseCSVString (@locationIds, ',')
	CREATE TABLE #temp1 (LocationId INT, LocationName NVARCHAR(150), LocationParentId INT, [Target] DECIMAL(18,2), Achieved DECIMAL(18, 2))

	;WITH cte AS
	(
		SELECT LocationId, LocationParentId
		FROM Locations
		WHERE LocationId IN (SELECT * FROM #locations)
		UNION ALL
		SELECT l.locationid, l.locationparentid FROM locations l join cte c on l.locationparentid = c.locationid
	)
	
	SELECT * INTO #temp FROM cte

	INSERT	INTO #temp1
	SELECT	l.LocationId, 
			locationname AS Location, 
			LocationParentId, 
			rd.Target, 
			rd.Achieved
			
	FROM	Reports r JOIN ReportLocations rl ON r.ReportId = rl.ReportId
			LEFT JOIN Reportdetails rd ON rl.ReportlocationId = rd.ReportlocationId
			LEFT JOIN ActivityData d on rd.ActivityDataId = d.ActivityDataId
			JOIN Locations l ON rl.LocationId = l.LocationId
			
	WHERE	l.LocationId IN (SELECT LocationId FROM #temp)
	AND		(d.ActivityDataId = @dataId OR @dataId IS NULL)
	ORDER BY LocationName

	UPDATE #temp1 SET Target = 0 WHERE Target IS NULL
	UPDATE #temp1 SET Achieved = 0 WHERE Achieved IS NULL

	IF(@locationType = 1)
	BEGIN
	
		SELECT	l2.LocationId
				,l2.Locationname AS Location
				, sum(target) AS Target
				, sum(achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
				
		FROM	#temp1 t JOIN locations l ON t.locationparentid = l.locationid
				JOIN locations l2 ON l.locationparentid = l2.locationid
		GROUP BY l2.locationid, l2.locationname
		ORDER BY WorkDone desc, Target desc
		
	END
	ELSE IF(@locationType = 2)
	BEGIN
	
		SELECT	l.LocationId
				,l.Locationname AS Location
				,SUM(Target) AS Target
				,SUM(Achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
		FROM	#temp1 t JOIN Locations l ON t.LocationParentId = l.LocationId
		GROUP BY l.locationid, l.LocationName
		ORDER BY WorkDone desc, Target desc
		
	END
	ELSE IF (@locationType = 3)
	BEGIN
	
		SELECT	LocationId
				,Locationname AS Location
				,SUM(Target) AS Target
				,SUM(Achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
		FROM	#temp1
		WHERE	LocationId IN (SELECT LocationId FROM #temp)
		GROUP BY LocationId, LocationName
		ORDER BY WorkDone desc, Target desc
	END

	DROP TABLE #locations
	DROP TABLE #temp
	DROP TABLE #temp1
    
END
