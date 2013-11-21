USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[GetOPSActivities]    Script Date: 11/21/2013 8:33:09 PM ******/
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
						DataName NVARCHAR(4000), [Mid2014] DECIMAL(18,2), [2014] DECIMAL(18,2),
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
	--JOIN	StrategicObjectives so ON so.StrategicObjectiveId = co.StrategicObjectiveId
	JOIN	IndicatorActivities ia ON ia.ObjectiveIndicatorId = oi.ObjectiveIndicatorId
	JOIN	ActivityData ad ON ad.IndicatorActivityId = ia.IndicatorActivityId
	--JOIN	UserActivityData uad ON ia.IndicatorActivityId = uad.IndicatorActivityId
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


------------------------------------------------------------------------------------------------------------------

USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[GetEmergencyClusterObjectives]    Script Date: 11/21/2013 2:46:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Feb 19, 2013
-- Description:	Get all objectives of cluster given
-- =============================================
create PROCEDURE [dbo].[GetEmergencyClusterObjectivesWithSpcObjId]
	@emgClusterId INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	CONVERT(VARCHAR(5), co.StrategicObjectiveId) + '_' + CONVERT(VARCHAR(5), co.ClusterObjectiveId) AS StrSpcObjId ,
			co.ObjectiveName
			
	FROM	EmergencyClusters ec JOIN	ClusterObjectives co on ec.EmergencyClusterId = co.EmergencyClusterId
	
	WHERE	ec.EmergencyClusterId = @emgClusterId
	ORDER BY co.ObjectiveName
END

---------------------------------------------------------------------------------------------------------------------------

USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[GetOPSEmergencyId]    Script Date: 11/21/2013 11:25:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Nov 20, 2013
-- Description:	Get Emergency Clusters
-- =============================================
ALTER PROCEDURE [dbo].[GetOPSEmergencyId]
	@countryName nvarchar(50) = null,
	@isOPSEmergency bit = null
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT	le.LocationEmergencyId
			
	FROM	LocationEmergencies le JOIN Locations l ON le.LocationId = l.LocationId
			JOIN Emergency e on e.EmergencyId = le.EmergencyId
	WHERE	l.LocationName = @countryName
	AND		le.IsOPSEmergency = @isOPSEmergency
END
