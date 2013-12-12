USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[GetAllLogFrameData]    Script Date: 11/28/2013 4:53:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Nov 28 24, 2013
-- Description:	Get All Emergney, Cluster, Objective, Indicator, Activity and Data elements.
-- =============================================
ALTER PROCEDURE [dbo].[GetAllLogFrameDataOnLocation] 
	@countryId INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT	ActivityDataId,
			c.ClusterShortName + ' -> ' + ActivityName + ' --> ' + ad.DataName as DataName
	
	FROM	ActivityData ad JOIN IndicatorActivities ia ON ad.IndicatorActivityId = ia.IndicatorActivityId
	JOIN	ObjectiveIndicators oi ON ia.ObjectiveIndicatorId = oi.ObjectiveIndicatorId
	JOIN	ClusterObjectives o ON oi.ClusterObjectiveId = o.ClusterObjectiveId
	JOIN	EmergencyClusters ec ON o.EmergencyClusterId = ec.EmergencyClusterId
	JOIN	Clusters c on ec.ClusterId = c.ClusterId
	JOIN	LocationEmergencies le ON le.LocationEmergencyId = ec.LocationEmergencyId
	JOIN	Emergency e ON le.EmergencyId = e.EmergencyId
	WHERE	LocationId = @countryId
	ORDER BY e.EmergencyName, c.ClusterName, o.ObjectiveName, oi.IndicatorName, ia.ActivityName, ad.DataName

END


GO
/****** Object:  StoredProcedure [dbo].[GetSecondLevelChildLocations]    Script Date: 12/11/2013 9:47:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Feb 5, 2013
-- Description:	Get All Child Locations Of Given Location
-- =============================================
CREATE PROCEDURE [dbo].[GetSecondLevelChildLocationsAndCountry]
	@parentLocationId INT
AS
BEGIN
	
	SET NOCOUNT ON;

	;WITH ChildLocations AS
	(
		SELECT l.LocationID, l.LocationName, l.LocationParentId, l.LocationTypeId
		FROM Locations l
		WHERE LocationID = @parentLocationId 
		
		UNION ALL
		
		SELECT ll.LocationID, ll.LocationName, ll.LocationParentId, ll.LocationTypeId
		FROM Locations ll inner join ChildLocations cl ON ll.LocationParentId = cl.LocationId
		WHERE ll.LocationTypeId = 3		
	)
	
	SELECT * FROM ChildLocations ORDER BY LocationTypeId, LocationName
	
END

--------------------------------------------------------------------------------------------------------------------------------------


GO
/****** Object:  StoredProcedure [dbo].[GetOPSActivities]    Script Date: 12/11/2013 10:17:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[GetOPSActivities]
	@locEmergencyId INT,
	@locationIds VARCHAR(MAX),	
	@locIdsNotIncluded VARCHAR(MAX),
	@opsProjectId INT,
	@emgClusterId INT,
	@langId INT = 1
	
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
							
	SELECT	o.ObjectiveId, hp.HumanitarianPriorityId, c.ClusterName, c2.ClusterName, ActivityName, 
			CASE WHEN Unit IS NULL THEN DataName Else DataName + '  (' + ISNULL(Unit, '') + ')' END AS DataName,				
			ad.ActivityDataId,0, LocName, LocId
				
	FROM	Locations l JOIN LocationEmergencies le ON l.LocationId = le.LocationId
	JOIN	EMERGENCY e ON le.EmergencyId = e.EmergencyId
	JOIN	EmergencyClusters ec ON ec.EmergencyId = e.EmergencyId
	JOIN	Clusters c ON ec.ClusterId = c.ClusterId
	JOIN	ClusterObjectives co ON co.EmergencyClusterId = ec.EmergencyClusterId
	join	Objectives o on o.ObjectiveId = co.ObjectiveId
	join	ObjectivePriorities op on op.ClusterObjectiveId = co.ClusterObjectiveId
	join	HumanitarianPriorities hp on hp.HumanitarianPriorityId = op.HumanitarianPriorityId
	join	PriorityActivities pa on pa.ObjectivePriorityId = op.ObjectivePriorityId
	join	ActivityData ad on ad.PriorityActivityId = pa.PriorityActivityId
	LEFT	JOIN Units u ON ad.UnitId= u.UnitId
	LEFT	JOIN ActivitySecondaryClusters sc ON pa.PriorityActivityId = sc.PriorityActivityId
	LEFT	JOIN Clusters c2 ON sc.ClusterId = c2.ClusterId and c2.SiteLanguageId = @langId
	CROSS	JOIN @loc
	WHERE	e.EmergencyId= @locEmergencyId
	AND		ec.EmergencyClusterId = @emgClusterId	
	and c.SiteLanguageId = @langId
	and e.SiteLanguageId = @langId
	and o.SiteLanguageId = @langId
	and hp.SiteLanguageId = @langId	
	and pa.SiteLanguageId = @langId
	and ad.SiteLanguageId = @langId
	and pa.IsUserActivity = 0	

	INSERT	INTO #temp( StrObjName, SpcObjName,	ClusterName, IndicatorName, ActivityName, DataName, 
						ActivityDataId, IsActive, Location, LocId)
							
	SELECT	o.ObjectiveId, hp.HumanitarianPriorityId, c.ClusterName, c2.ClusterName, ActivityName, 
			CASE WHEN Unit IS NULL THEN DataName Else DataName + '  (' + ISNULL(Unit, '') + ')' END AS DataName,				
			ad.ActivityDataId,0, LocName, LocId
				
	FROM	Locations l JOIN LocationEmergencies le ON l.LocationId = le.LocationId
	JOIN	EMERGENCY e ON le.EmergencyId = e.EmergencyId
	JOIN	EmergencyClusters ec ON ec.EmergencyId = e.EmergencyId
	JOIN	Clusters c ON ec.ClusterId = c.ClusterId
	JOIN	ClusterObjectives co ON co.EmergencyClusterId = ec.EmergencyClusterId
	join	Objectives o on o.ObjectiveId = co.ObjectiveId
	join	ObjectivePriorities op on op.ClusterObjectiveId = co.ClusterObjectiveId
	join	HumanitarianPriorities hp on hp.HumanitarianPriorityId = op.HumanitarianPriorityId
	join	PriorityActivities pa on pa.ObjectivePriorityId = op.ObjectivePriorityId
	join	ActivityData ad on ad.PriorityActivityId = pa.PriorityActivityId
	LEFT	JOIN Units u ON ad.UnitId= u.UnitId
	LEFT	JOIN ActivitySecondaryClusters sc ON pa.PriorityActivityId = sc.PriorityActivityId
	LEFT	JOIN Clusters c2 ON sc.ClusterId = c2.ClusterId and c2.SiteLanguageId = @langId
	CROSS	JOIN @loc
	WHERE	e.EmergencyId= @locEmergencyId
	AND		ec.EmergencyClusterId = @emgClusterId	
	and c.SiteLanguageId = @langId
	and e.SiteLanguageId = @langId
	and o.SiteLanguageId = @langId
	and hp.SiteLanguageId = @langId	
	and pa.SiteLanguageId = @langId
	and ad.SiteLanguageId = @langId
	and pa.IsUserActivity = 1
	AND pa.OPSProjectId = @opsProjectId
		
		
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
					Order By ActivityDataId
					'
					  
		print @query

		execute(@query)
		END
		drop table #temp
		drop table #temp2
END	

--------------------------------------------------------------------------------------------------------------------------------------------------


GO
/****** Object:  StoredProcedure [dbo].[InsertOPSReport]    Script Date: 12/12/2013 6:01:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Nov 20, 2013
-- Description:	Save report
-- =============================================
ALTER PROCEDURE [dbo].[InsertOPSReport]	
   @locationEmergencyId INT
   ,@opsProjectId INT
   ,@emgClusterId INT
   ,@opsUserId INT
   ,@lngId TINYINT
   ,@UID int = NULL OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
DECLARE @InsertedRecords TABLE(id int)		
    
    INSERT INTO OPSReports
		(
           OPSProjectId
           ,LocationEmergencyId           
		   ,EmergencyClusterId
           ,CreatedByIdOPSUser
		   ,SiteLanguageId
		)
	OUTPUT INSERTED.OPSReportId INTO @InsertedRecords
     VALUES
		(
           @opsProjectId
		   ,@locationEmergencyId		   
		   ,@emgClusterId
		   ,@opsUserId
		   ,@lngId
		)
		
		SELECT @UID = id FROM @InsertedRecords
		select @UID
END
------------------------------------------------------------------------------------------------
	
	
	
GO
/****** Object:  StoredProcedure [dbo].[GetOPSActivities]    Script Date: 12/12/2013 4:52:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[GetOPSActivities]
	@locEmergencyId INT,
	@locationIds VARCHAR(MAX),	
	@locIdsNotIncluded VARCHAR(MAX),
	@opsProjectId INT,
	@emgClusterId INT,
	@langId INT = 1
	
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
	CREATE table #temp (OPSReportId INT, ObjectiveId INT, Objective nvarchar(1000), HumanitarianPriorityId INT, HumanitarianPriority nvarchar(500), 
						ClusterName NVARCHAR(100), SecondaryCluster NVARCHAR(100), ActivityName NVARCHAR(1000), 
						DataName NVARCHAR(1000), [Mid2014] INT, [2014] INT,
						ActivityDataId INT, IsActive bit, Location VARCHAR(20),
						LocId INT, OPSReportLocationId INT)
							
	INSERT	INTO #temp( ObjectiveId, Objective, HumanitarianPriorityId, HumanitarianPriority,	ClusterName, SecondaryCluster, ActivityName, DataName, 
						ActivityDataId, IsActive, Location, LocId)
							
	SELECT	o.ObjectiveId, o.Objective, hp.HumanitarianPriorityId, hp.HumanitarianPriority, c.ClusterName, c2.ClusterName, ActivityName, 
			CASE WHEN Unit IS NULL THEN DataName Else DataName + '  (' + ISNULL(Unit, '') + ')' END AS DataName,				
			ad.ActivityDataId,0, LocName, LocId
				
	FROM	Locations l JOIN LocationEmergencies le ON l.LocationId = le.LocationId
	JOIN	EMERGENCY e ON le.EmergencyId = e.EmergencyId
	JOIN	EmergencyClusters ec ON ec.EmergencyId = e.EmergencyId
	JOIN	Clusters c ON ec.ClusterId = c.ClusterId
	JOIN	ClusterObjectives co ON co.EmergencyClusterId = ec.EmergencyClusterId
	join	Objectives o on o.ObjectiveId = co.ObjectiveId
	join	ObjectivePriorities op on op.ClusterObjectiveId = co.ClusterObjectiveId
	join	HumanitarianPriorities hp on hp.HumanitarianPriorityId = op.HumanitarianPriorityId
	join	PriorityActivities pa on pa.ObjectivePriorityId = op.ObjectivePriorityId
	join	ActivityData ad on ad.PriorityActivityId = pa.PriorityActivityId
	LEFT	JOIN Units u ON ad.UnitId= u.UnitId
	LEFT	JOIN ActivitySecondaryClusters sc ON pa.PriorityActivityId = sc.PriorityActivityId
	LEFT	JOIN Clusters c2 ON sc.ClusterId = c2.ClusterId and c2.SiteLanguageId = @langId
	CROSS	JOIN @loc
	WHERE	e.EmergencyId= @locEmergencyId
	AND		ec.EmergencyClusterId = @emgClusterId	
	and c.SiteLanguageId = @langId
	and e.SiteLanguageId = @langId
	and o.SiteLanguageId = @langId
	and hp.SiteLanguageId = @langId	
	and pa.SiteLanguageId = @langId
	and ad.SiteLanguageId = @langId
		
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
					SELECT IsActive, OPSReportId, ObjectiveId, Objective, HumanitarianPriorityId, HumanitarianPriority, ClusterName, SecondaryCluster, ActivityDataId, ActivityName, 
						DataName, 
						CAST(locid AS VARCHAR(5))+''^''+Location+''_''+col AS col, 
						value
						FROM
						(
						SELECT IsActive, OPSReportId, ObjectiveId, Objective, HumanitarianPriorityId, HumanitarianPriority, ClusterName, SecondaryCluster, ActivityDataId, ActivityName,
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
					SELECT IsActive, OPSReportId, ObjectiveId, Objective, HumanitarianPriorityId, HumanitarianPriority , ClusterName, SecondaryCluster, ActivityDataId, ActivityName,
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
					SELECT IsActive, OPSReportId, ObjectiveId, Objective, HumanitarianPriorityId, HumanitarianPriority, ClusterName, SecondaryCluster, ActivityDataId, case when rn = 1 then ActivityName else ActivityName end ActivityName,
					DataName,
					'+@cols+'
		                          
					FROM piv
					Order By ActivityDataId
					'
					  
		print @query

		execute(@query)
		END
		drop table #temp
		drop table #temp2
END	

-------------------------------------------------------------------------------------------------------------