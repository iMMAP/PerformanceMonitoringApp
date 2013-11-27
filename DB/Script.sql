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

----------------------------------------------------------------------------------------------------

-- Change OPSReportDetails Targets from decimal to int
-------------------------------------------------------------------------------------------------------


-----------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[GetOPSActivities]    Script Date: 11/27/2013 2:18:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[GetOPSActivities]
	@locEmergencyId INT,
	@locationIds VARCHAR(MAX),	
	@locIdsNotIncluded VARCHAR(MAX),
	@opsProjectId INT,
	@emgClusterId INT
	
AS
BEGIN
	-- This temp table will be used to have locations passed as parameter (selected by user in front end)
	DECLARE @loc TABLE (LocId INT, LocName NVARCHAR(150))
		
	-- First query seperate LocationIds passed from front end in parameter
	-- Second query get all the locations saved for thsi user, office, year, month
	INSERT INTO @loc(LocId)
	SELECT * FROM dbo.fn_ParseCSVString (@locationIds, ',')
	UNION	
	SELECT rl.LocationId FROM OPSReports r JOIN OPSReportLocations rl ON r.OPSReportId = rl.OPSReportId
	WHERE	OPSProjectId = @opsProjectId
	AND		rl.EmergencyClusterId = @emgClusterId

	-- We need to delete all locations which user has deleted from report in front end.
	-- @locInsNotInclused parameter has all the locations which should not be included in report.
	DELETE FROM @loc WHERE LocId  IN (SELECT * FROM dbo.fn_ParseCSVString (@locIdsNotIncluded, ','))
		
	--select * From @loc
		
	UPDATE @loc SET LocName = l.LocationName
	FROM @loc lt JOIN Locations l ON lt.LocId = l.LocationId

	-- Create temp table to populate all the data and then we will update this table with Target,Achieved and other Ids.
	CREATE table #temp (OPSReportId INT, StrObjName nvarchar(1000), SpcObjName nvarchar(1000), 
						ClusterName NVARCHAR(500), IndicatorName NVARCHAR(4000), ActivityName NVARCHAR(4000), 
						DataName NVARCHAR(4000), [Mid2014] INT, [2014] INT,
						ActivityDataId INT, IsActive bit, Location VARCHAR(20),
						LocId INT, OPSReportLocationId INT)
							
	INSERT	INTO #temp( StrObjName, SpcObjName,	ClusterName, IndicatorName, ActivityName, DataName, 
						ActivityDataId, IsActive, Location, LocId)
							
	SELECT	co.StrategicObjectiveId, co.ClusterObjectiveId, c.ClusterShortName, oi.IndicatorName, ActivityName, 
			CASE WHEN Unit IS NULL THEN DataName Else DataName + '  (' + ISNULL(Unit, '') + ')' END AS DataName,				
			ad.ActivityDataId,0, LocName, LocId
				
	FROM	Locations l JOIN LocationEmergencies le ON l.LocationId = le.LocationId
	JOIN	EMERGENCY e ON le.EmergencyId = e.EmergencyId
	JOIN	EmergencyClusters ec ON ec.LocationEmergencyId = le.LocationEmergencyId
	JOIN	Clusters c ON ec.ClusterId = c.ClusterId
	JOIN	ClusterObjectives co ON co.EmergencyClusterId = ec.EmergencyClusterId
	JOIN	ObjectiveIndicators oi ON oi.ClusterObjectiveId = co.ClusterObjectiveId	
	JOIN	IndicatorActivities ia ON ia.ObjectiveIndicatorId = oi.ObjectiveIndicatorId
	JOIN	ActivityData ad ON ad.IndicatorActivityId = ia.IndicatorActivityId	
	LEFT	JOIN Units u ON ad.UnitId= u.UnitId
	CROSS	JOIN @loc
	WHERE	le.LocationEmergencyId= @locEmergencyId
	AND		ec.EmergencyClusterId = @emgClusterId	
		
		
	--select * from #temp
	-- Get report and report details
	SELECT	r.OPSReportId, rd.ActivityDataId, 
			rd.TargetMid2014, rd.Target2014, 
			rd.IsActive, rl.LocationId
	INTO	#temp2
	FROM	OPSReports r JOIN OPSReportLocations rl ON r.OPSReportId = rl.OPSReportId
	LEFT	JOIN	OPSReportDetails rd ON rl.OPSReportLocationId = rd.OPSReportLocationId
	WHERE	rl.LocationId IN (SELECT LocId FROM @loc)
	AND		r.OPSProjectId = @opsProjectId
		
		
	UPDATE #temp SET OPSReportId = (SELECT TOP 1 OPSReportId FROM #temp2)
	UPDATE #temp SET OPSReportLocationId = (SELECT TOP 1 OPSReportId FROM #temp2)	

	UPDATE	#temp SET [Mid2014] = t2.TargetMid2014, [2014] = t2.Target2014
	FROM	#temp t JOIN #temp2 t2 ON t.ActivityDataId = t2.ActivityDataId
	JOIN	@loc l ON t.LocId = l.LocId AND t2.LocationId = l.LocId

	UPDATE	#temp SET IsActive = ISNULL(t2.IsActive, 0)
	FROM	#temp t JOIN #temp2 t2 ON t.ActivityDataId = t2.ActivityDataId

		
	-- Variables for dynamic query to build pivot table.		
	DECLARE @cols AS NVARCHAR(MAX)
	DECLARE @query AS NVARCHAR(MAX)
		
	IF EXISTS( SELECT 1 FROM #temp WHERE OPSReportId IS NOT NULL OR @locationIds != '')
	BEGIN
	SELECT @cols = STUFF((SELECT ',' + QUOTENAME(CAST(locid AS VARCHAR(5))+'^'+Location+'_'+t.tasks) 
						FROM #temp
						cross apply
						(
							SELECT 'Mid2014' tasks
							union all
							SELECT '2014'
						) t
						group by locid,location, tasks
						order by location
				FOR XML PATH(''), TYPE
				).value('.', 'NVARCHAR(MAX)') 
			,1,1,'')

	set @query = ';WITH unpiv AS
					(
					SELECT IsActive, OPSReportId, StrObjName, SpcObjName, ClusterName, IndicatorName, ActivityDataId, ActivityName, 
						DataName, 
						CAST(locid AS VARCHAR(5))+''^''+Location+''_''+col AS col, 
						value
						FROM
						(
						SELECT IsActive, OPSReportId, StrObjName, SpcObjName, ClusterName, IndicatorName, ActivityDataId, ActivityName,
							DataName,
							CAST(ISNULL([Mid2014], -1) AS VARCHAR(50)) [Mid2014],
							CAST(ISNULL([2014], -1 ) AS VARCHAR(50)) [2014],
							Location,
							locid
						FROM #temp
						) src
						unpivot
						(
						value
						for col in ([Mid2014], [2014])
						) unpiv
					),
					piv AS
					(
					SELECT IsActive, OPSReportId, StrObjName, SpcObjName, ClusterName, IndicatorName, ActivityDataId, ActivityName,
						DataName,
						row_number() over(partition by ActivityName order by ActivityName, DataName) rn,
						'+@cols+'
					FROM unpiv
					pivot
					(
						max(value)
						for col in ('+@cols+')
					) piv
					)
					SELECT IsActive, OPSReportId, StrObjName, SpcObjName, ClusterName, IndicatorName, ActivityDataId, case when rn = 1 then ActivityName else ActivityName end ActivityName,
					DataName,
					'+@cols+'
		                          
					FROM piv
					Order By ClusterName, IndicatorName, ActivityName, DataName
					'
					  
		print @query

		execute(@query)
		END
		drop table #temp
		drop table #temp2
END	
