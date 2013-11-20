USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[GetIPData]    Script Date: 11/18/2013 7:31:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[GetIPDataLocations]
	@locEmergencyId INT,
	@locationIds VARCHAR(MAX),
	@officeID INT,
	@yearId INT,
	@monthId INT,
	@locIdsNotIncluded VARCHAR(MAX),
	@userId UNIQUEIDENTIFIER,
	@dataId INT
AS
BEGIN

	IF(@officeId > 0 AND @yearId > 0 AND @monthId > 0)
	BEGIN

		-- This temp table will be used to have locations passed as parameter (selected by user in front end)
		DECLARE @loc TABLE (LocId INT, LocName NVARCHAR(150))
		
		-- First query seperate LocationIds passed from front end in parameter
		-- Second query get all the locations saved for thsi user, office, year, month
		INSERT INTO @loc(LocId)
		SELECT * FROM dbo.fn_ParseCSVString (@locationIds, ',')
		UNION	
		SELECT rl.LocationId FROM Reports r JOIN ReportLocations rl ON r.ReportId = rl.ReportId
		WHERE	OfficeId = @officeId 
		AND		YearId = @yearId
		AND		MonthId = @monthId
		AND		CreatedById = @userId

		-- We need to delete all locations which user has deleted from report in front end.
		-- @locInsNotInclused parameter has all the locations which should not be included in report.
		DELETE FROM @loc WHERE LocId  IN (SELECT * FROM dbo.fn_ParseCSVString (@locIdsNotIncluded, ','))
		
		--select * From @loc
		
		UPDATE @loc SET LocName = l.LocationName
		FROM @loc lt JOIN Locations l ON lt.LocId = l.LocationId

		-- Create temp table to populate all the data and then we will update this table with Target,Achieved and other Ids.
		CREATE table #temp (ReportId INT, ClusterName NVARCHAR(500), IndicatorName NVARCHAR(4000), ActivityName NVARCHAR(4000), 
							DataName NVARCHAR(4000), Target DECIMAL(18,2), Achieved DECIMAL(18,2), 
							ActivityDataId INT, IsActive bit, Location VARCHAR(20),
							LocId INT, ReportLocationId INT)
							
		INSERT	INTO #temp(	ClusterName, IndicatorName, ActivityName, DataName, 
							ActivityDataId, IsActive, Location, LocId)
							
		SELECT	c.ClusterShortName, oi.IndicatorName, ActivityName, 
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
		JOIN	UserActivityData uad ON ia.IndicatorActivityId = uad.IndicatorActivityId AND uad.UserId = @userId
		LEFT	JOIN Units u ON ad.UnitId= u.UnitId
		CROSS	JOIN @loc
		WHERE	le.LocationEmergencyId= @locEmergencyId
		AND		uad.OfficeId = @officeId
		
		--select * from #temp
		-- Get report and report details
		SELECT	r.ReportId, rd.ActivityDataId, 
				rd.Target, rd.Achieved, 
				rd.IsActive, rl.LocationId
		INTO	#temp2
		FROM	Reports r JOIN ReportLocations rl ON r.ReportId = rl.ReportId
		LEFT	JOIN	ReportDetails rd ON rl.ReportLocationId = rd.ReportLocationId
		WHERE	rl.LocationId IN (SELECT LocId FROM @loc)
		AND		OfficeId= @officeId
		AND		YearId = @yearId
		AND		MonthId = @monthId
		AND		r.CreatedById = @userId
		
		UPDATE #temp SET ReportId = (SELECT TOP 1 ReportId FROM #temp2)
		UPDATE #temp SET ReportLocationId = (SELECT TOP 1 ReportId FROM #temp2)	

		UPDATE	#temp SET Target = t2.Target, Achieved = t2.Achieved 
		FROM	#temp t JOIN #temp2 t2 ON t.ActivityDataId = t2.ActivityDataId
		JOIN	@loc l ON t.LocId = l.LocId AND t2.LocationId = l.LocId

		UPDATE	#temp SET IsActive = ISNULL(t2.IsActive, 0)
		FROM	#temp t JOIN #temp2 t2 ON t.ActivityDataId = t2.ActivityDataId

		
		-- Variables for dynamic query to build pivot table.		
		DECLARE @cols AS NVARCHAR(MAX)
		DECLARE @query AS NVARCHAR(MAX)
		
		
		SELECT * FROM #temp Where ActivityDataId = @dataId
		drop table #temp
		drop table #temp2
	END	
end

---------------------------------------------------------------------------------------------------------------------------------------------

USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[GetReportId]    Script Date: 11/19/2013 4:24:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Feb 13, 2013
-- Description:	GetReportId
-- =============================================
create PROCEDURE [dbo].[GetOPSReportId]
	@locEmergencyId INT,
	@opsProjectId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	OPSReportId
	FROM	OPSReports
	WHERE	LocationEmergencyId = @locEmergencyId
	AND		OPSProjectId = @opsProjectId
END

----------------------------------------------------------------------------------------------------------------------------------------------------

USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[GetIPData]    Script Date: 11/19/2013 1:16:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetOPSActivities]
	@locEmergencyId INT,
	@locationIds VARCHAR(MAX),	
	@locIdsNotIncluded VARCHAR(MAX),
	@opsProjectId INT,
	@emgClusterId INT
	
AS
BEGIN

	
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

		-- We need to delete all locations which user has deleted from report in front end.
		-- @locInsNotInclused parameter has all the locations which should not be included in report.
		DELETE FROM @loc WHERE LocId  IN (SELECT * FROM dbo.fn_ParseCSVString (@locIdsNotIncluded, ','))
		
		--select * From @loc
		
		UPDATE @loc SET LocName = l.LocationName
		FROM @loc lt JOIN Locations l ON lt.LocId = l.LocationId

		-- Create temp table to populate all the data and then we will update this table with Target,Achieved and other Ids.
		CREATE table #temp (OPSReportId INT, ClusterName NVARCHAR(500), IndicatorName NVARCHAR(4000), ActivityName NVARCHAR(4000), 
							DataName NVARCHAR(4000), [2014] DECIMAL(18,2), [2015] DECIMAL(18,2),  [2016] DECIMAL(18,2), 
							ActivityDataId INT, IsActive bit, Location VARCHAR(20),
							LocId INT, OPSReportLocationId INT)
							
		INSERT	INTO #temp(	ClusterName, IndicatorName, ActivityName, DataName, 
							ActivityDataId, IsActive, Location, LocId)
							
		SELECT	c.ClusterShortName, oi.IndicatorName, ActivityName, 
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
		JOIN	UserActivityData uad ON ia.IndicatorActivityId = uad.IndicatorActivityId
		LEFT	JOIN Units u ON ad.UnitId= u.UnitId
		CROSS	JOIN @loc
		WHERE	le.LocationEmergencyId= @locEmergencyId
		AND		ec.EmergencyClusterId = @emgClusterId
		
		
		--select * from #temp
		-- Get report and report details
		SELECT	r.OPSReportId, rd.ActivityDataId, 
				rd.Target2014, rd.Target2015, rd.Target2016,
				rd.IsActive, rl.LocationId
		INTO	#temp2
		FROM	OPSReports r JOIN OPSReportLocations rl ON r.OPSReportId = rl.OPSReportId
		LEFT	JOIN	OPSReportDetails rd ON rl.OPSReportLocationId = rd.OPSReportLocationId
		WHERE	rl.LocationId IN (SELECT LocId FROM @loc)
		
		
		UPDATE #temp SET OPSReportId = (SELECT TOP 1 OPSReportId FROM #temp2)
		UPDATE #temp SET OPSReportLocationId = (SELECT TOP 1 OPSReportId FROM #temp2)	

		UPDATE	#temp SET [2014] = t2.Target2014, [2015] = t2.Target2015, [2016] = Target2016 
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
							  SELECT '2014' tasks
							  union all
							  SELECT '2015'
							  union all
							  SELECT '2016'
							) t
							group by locid,location, tasks
							order by location
					FOR XML PATH(''), TYPE
					).value('.', 'NVARCHAR(MAX)') 
				,1,1,'')

		set @query = ';WITH unpiv AS
					  (
						SELECT IsActive, OPSReportId, ClusterName, IndicatorName, ActivityDataId, ActivityName, 
							DataName, 
							CAST(locid AS VARCHAR(5))+''^''+Location+''_''+col AS col, 
							value
						  FROM
						  (
							SELECT IsActive, OPSReportId, ClusterName, IndicatorName, ActivityDataId, ActivityName,
							  DataName,
							  CAST(ISNULL([2014], -1) AS VARCHAR(50)) [2014],
							  CAST(ISNULL([2015], -1 ) AS VARCHAR(50)) [2015],
							  CAST(ISNULL([2016], -1 ) AS VARCHAR(50)) [2016],
							  Location,
							  locid
							FROM #temp
						  ) src
						  unpivot
						  (
							value
							for col in ([2014], [2015], [2016])
						  ) unpiv
					  ),
					  piv AS
					  (
						SELECT IsActive, OPSReportId, ClusterName, IndicatorName, ActivityDataId, ActivityName,
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
					  SELECT IsActive, OPSReportId, ClusterName, IndicatorName, ActivityDataId, case when rn = 1 then ActivityName else ActivityName end ActivityName,
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
end

---------------------------------------------------------------------------------------------------------------------------------------------

USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[GetAllSpecifObjectivesOfAStrObjective]    Script Date: 11/19/2013 6:32:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Nov 18, 2013
-- Description:	Get all specifi objectives of a strategic objecitve
-- =============================================
ALTER PROCEDURE [dbo].[GetAllSpecifObjectivesOfAStrObjective]
	@strObjId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	ClusterObjectiveId,
			ObjectiveName
	FROM	ClusterObjectives
	WHERE	StrategicObjectiveId = @strObjId
END


-----------------------------------------------------------------------------------------------------------------------------

USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[GetThirdLevelChildLocations]    Script Date: 11/20/2013 11:49:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Feb 5, 2013
-- Description:	Get All Child Locations Of Given Location
-- =============================================
alter PROCEDURE [dbo].[GetSecondLevelChildLocations]
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
	
	SELECT * FROM ChildLocations WHERE LocationTypeId = 3 ORDER BY LocationName
	
END

-------------------------------------------------------------------------------------------------------------------------------------------

USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[InsertReport]    Script Date: 11/20/2013 3:38:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Nov 20, 2013
-- Description:	Save report
-- =============================================
alter PROCEDURE [dbo].[InsertOPSReport]	
   @locationEmergencyId INT
   ,@opsProjectId INT
   ,@opsUserId INT
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
           ,CreatedByIdOPSUser
		)
	OUTPUT INSERTED.OPSReportId INTO @InsertedRecords
     VALUES
		(
           @opsProjectId
		   ,@locationEmergencyId		   
		   ,@opsUserId
		)
		
		SELECT @UID = id FROM @InsertedRecords
END

---------------------------------------------------------------------------------------------------------------

USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[InsertReportLocations]    Script Date: 11/20/2013 3:44:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Nov 20, 2013
-- Description:	Insert OPS Report Locations
-- =============================================
CREATE PROCEDURE [dbo].[InsertOPSReportLocations]
	@opsReportId INT,
	@locationId INT,
	@UID int = NULL OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @InsertedRecords TABLE(id int)
	
    -- Insert statements for procedure here
	INSERT	INTO OPSReportLocations
	(
		OPSReportId,
		LocationId
	)
	OUTPUT INSERTED.OPSReportLocationId INTO @InsertedRecords
	VALUES
	(
		@opsReportId,
		@locationId
	)
	
	SELECT @UID = id FROM @InsertedRecords
	
END


-----------------------------------------------------------------------------------------------------
USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[InsertReportDetails]    Script Date: 11/20/2013 3:48:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Nov 20, 2013
-- Description:	Save report details
-- =============================================
create PROCEDURE [dbo].[InsertOPSReportDetails]
	@opsReportId INT
   ,@activityDataId INT
   ,@locationId INT
   ,@targetMid2014 DECIMAL(18,2) = NULL
   ,@target2014 DECIMAL(18,2) = NULL
   ,@isActive INT   
   ,@UID int = NULL OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @opsReportLocationId INT = 0
	SELECT @opsReportLocationId = OPSReportLocationId FROM OPSReportLocations WHERE OPSReportId = @opsReportId AND LocationId = @locationId

	IF(@opsReportLocationId > 0)
	BEGIN
	
		DECLARE @InsertedRecords TABLE(id int)
		
		INSERT INTO OPSReportDetails
		(
			OPSReportId
           ,OPSReportLocationid
           ,ActivityDataId
           ,TargetMid2014
           ,Target2014
           ,IsActive
		)
		OUTPUT INSERTED.OPSReportDetailId INTO @InsertedRecords
		VALUES
		(
			@opsReportId
			,@opsReportLocationId	   
			,@activityDataId
			,@targetMid2014
			,@target2014
			,@isActive			
		)
		
		SELECT @UID = id FROM @InsertedRecords
	END
	ELSE
	BEGIN
		SELECT @UID = 0
	END
END

------------------------------------------------------------------------------------------------

USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[DeleteReport]    Script Date: 11/20/2013 3:56:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Nov 20, 2013
-- Description:	Delte ops report and its details.
-- =============================================
alter procedure [dbo].[DeleteOPSReport]
	@opsReportId int
	,@UID int=null output
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			DELETE	FROM OPSReportDetails 
			WHERE OPSReportLocationId IN (SELECT	OPSReportLocationId 
										FROM	OPSReportLocations 
										WHERE	OPSReportId = @opsReportId)

										
			DELETE	FROM OPSReportLocations WHERE OPSReportId = @opsReportId

			DELETE	FROM OPSReports WHERE OPSReportId = @opsReportId

		COMMIT
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		ROLLBACK
		
		DECLARE @ErrMsg nvarchar(4000), @ErrSeverity int
		SELECT @ErrMsg = ERROR_MESSAGE(),
			 @ErrSeverity = ERROR_SEVERITY()

		RAISERROR(@ErrMsg, @ErrSeverity, 1)
	END CATCH
	SELECT @UID = 0
END
--------------------------------------------------------------------------------------------------------------
USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[GetEmergencyClusters]    Script Date: 11/20/2013 5:52:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Nov 20, 2013
-- Description:	Get Emergency Clusters
-- =============================================
ALTER PROCEDURE [dbo].[GetEmergencyClustersId]
	@clusterName nvarchar(50) = null,
	@countryName nvarchar(50) = null,
	@isOPSEmergency bit = null
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT	ec.EmergencyClusterId
			
	FROM	EmergencyClusters ec
			JOIN LocationEmergencies le ON ec.LocationEmergencyId = le.LocationEmergencyId
			JOIN Clusters c ON ec.ClusterId = c.ClusterId
			JOIN Locations l ON le.LocationId = l.LocationId
	WHERE	c.ClusterName = @clusterName
	AND		l.LocationName = @countryName
	--AND		le.IsOPSEmergency = @isOPSEmergency	
	
END
--------------------------------------------------------------------------------------------------------------
USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[GetEmergencyClusters]    Script Date: 11/20/2013 5:52:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Nov 20, 2013
-- Description:	Get Emergency Clusters
-- =============================================
alter PROCEDURE [dbo].[GetOPSEmergencyId]
	@countryName nvarchar(50) = null,
	@emgName nvarchar(500) = null
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT	le.LocationEmergencyId
			
	FROM	LocationEmergencies le JOIN Locations l ON le.LocationId = l.LocationId
			JOIN Emergency e on e.EmergencyId = le.EmergencyId
	WHERE	l.LocationName = @countryName
	AND		e.EmergencyName LIKE @emgName	
END
---------------------------------------------------------------------------------------------------------------

USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[GetOPSActivities]    Script Date: 11/20/2013 11:44:12 AM ******/
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
	@strObjId INT = NULL,
	@spcObjId INT = NULL
	
AS
BEGIN

	
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

		-- We need to delete all locations which user has deleted from report in front end.
		-- @locInsNotInclused parameter has all the locations which should not be included in report.
		DELETE FROM @loc WHERE LocId  IN (SELECT * FROM dbo.fn_ParseCSVString (@locIdsNotIncluded, ','))
		
		--select * From @loc
		
		UPDATE @loc SET LocName = l.LocationName
		FROM @loc lt JOIN Locations l ON lt.LocId = l.LocationId

		-- Create temp table to populate all the data and then we will update this table with Target,Achieved and other Ids.
		CREATE table #temp (OPSReportId INT, ClusterName NVARCHAR(500), IndicatorName NVARCHAR(4000), ActivityName NVARCHAR(4000), 
							DataName NVARCHAR(4000), [Mid2014] DECIMAL(18,2), [2014] DECIMAL(18,2),
							ActivityDataId INT, IsActive bit, Location VARCHAR(20),
							LocId INT, OPSReportLocationId INT)
							
		INSERT	INTO #temp(	ClusterName, IndicatorName, ActivityName, DataName, 
							ActivityDataId, IsActive, Location, LocId)
							
		SELECT	c.ClusterShortName, oi.IndicatorName, ActivityName, 
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
		--JOIN	UserActivityData uad ON ia.IndicatorActivityId = uad.IndicatorActivityId
		LEFT	JOIN Units u ON ad.UnitId= u.UnitId
		CROSS	JOIN @loc
		WHERE	le.LocationEmergencyId= @locEmergencyId
		AND		ec.EmergencyClusterId = @emgClusterId
		AND		(co.StrategicObjectiveId = @strObjId OR @strObjId IS NULL)
		AND		(co.ClusterObjectiveId = @spcObjId OR @spcObjId IS NULL)
		
		
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
						SELECT IsActive, OPSReportId, ClusterName, IndicatorName, ActivityDataId, ActivityName, 
							DataName, 
							CAST(locid AS VARCHAR(5))+''^''+Location+''_''+col AS col, 
							value
						  FROM
						  (
							SELECT IsActive, OPSReportId, ClusterName, IndicatorName, ActivityDataId, ActivityName,
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
						SELECT IsActive, OPSReportId, ClusterName, IndicatorName, ActivityDataId, ActivityName,
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
					  SELECT IsActive, OPSReportId, ClusterName, IndicatorName, ActivityDataId, case when rn = 1 then ActivityName else ActivityName end ActivityName,
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
end

----------------------------------------------------------------------------------------------------------------------


USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[GetOPSReportId]    Script Date: 11/20/2013 7:17:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Feb 13, 2013
-- Description:	GetReportId
-- =============================================
ALTER PROCEDURE [dbo].[GetOPSReportId]
	@locEmergencyId INT,
	@opsProjectId INT,
	@emgClusterId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	OPSReportId
	FROM	OPSReports
	WHERE	LocationEmergencyId = @locEmergencyId
	AND		OPSProjectId = @opsProjectId
	AND		EmergencyClusterId = @emgClusterId
END


---------------------------------------------------------------------------------------

USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[InsertOPSReport]    Script Date: 11/20/2013 7:03:04 PM ******/
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
		)
	OUTPUT INSERTED.OPSReportId INTO @InsertedRecords
     VALUES
		(
           @opsProjectId
		   ,@locationEmergencyId		   
		   ,@emgClusterId
		   ,@opsUserId
		)
		
		SELECT @UID = id FROM @InsertedRecords
		select @UID
END
-------------------------------------------------------------------------------------------------------------------------------------
USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[DeleteOPSReport]    Script Date: 11/20/2013 6:59:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Nov 20, 2013
-- Description:	Delte ops report and its details.
-- =============================================
ALTER procedure [dbo].[DeleteOPSReport]
	@opsReportId int,
	@emgClusterId int,
	@UID int=null output
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			DELETE	FROM OPSReportDetails 
			WHERE OPSReportLocationId IN (SELECT	OPSReportLocationId 
										FROM	OPSReportLocations 
										WHERE	OPSReportId = @opsReportId
										AND		EmergencyClusterId = @emgClusterId)

										
			DELETE	FROM OPSReportLocations WHERE OPSReportId = @opsReportId AND EmergencyClusterId = @emgClusterId

			DELETE	FROM OPSReports WHERE OPSReportId = @opsReportId AND EmergencyClusterId = @emgClusterId

		COMMIT
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		ROLLBACK
		
		DECLARE @ErrMsg nvarchar(4000), @ErrSeverity int
		SELECT @ErrMsg = ERROR_MESSAGE(),
			 @ErrSeverity = ERROR_SEVERITY()

		RAISERROR(@ErrMsg, @ErrSeverity, 1)
	END CATCH
	SELECT @UID = 0
END
-------------------------------------------------------------------------------------------------------------------------------------

USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[InsertOPSReportLocations]    Script Date: 11/20/2013 6:40:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Nov 20, 2013
-- Description:	Insert OPS Report Locations
-- =============================================
ALTER PROCEDURE [dbo].[InsertOPSReportLocations]
	@opsReportId INT,
	@locationId INT,
	@emgClusterId INT,
	@UID int = NULL OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @InsertedRecords TABLE(id int)
	
    -- Insert statements for procedure here
	INSERT	INTO OPSReportLocations
	(
		OPSReportId,
		LocationId,
		EmergencyClusterId
	)
	OUTPUT INSERTED.OPSReportLocationId INTO @InsertedRecords
	VALUES
	(
		@opsReportId,
		@locationId,
		@emgClusterId
	)
	
	SELECT @UID = id FROM @InsertedRecords
	
END

-----------------------------------------------------------------------------------------------------------

USE [rowca]
GO
/****** Object:  StoredProcedure [dbo].[GetOPSActivities]    Script Date: 11/20/2013 11:44:12 AM ******/
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
	@strObjId INT = NULL,
	@spcObjId INT = NULL
	
AS
BEGIN

	
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
		CREATE table #temp (OPSReportId INT, ClusterName NVARCHAR(500), IndicatorName NVARCHAR(4000), ActivityName NVARCHAR(4000), 
							DataName NVARCHAR(4000), [Mid2014] DECIMAL(18,2), [2014] DECIMAL(18,2),
							ActivityDataId INT, IsActive bit, Location VARCHAR(20),
							LocId INT, OPSReportLocationId INT)
							
		INSERT	INTO #temp(	ClusterName, IndicatorName, ActivityName, DataName, 
							ActivityDataId, IsActive, Location, LocId)
							
		SELECT	c.ClusterShortName, oi.IndicatorName, ActivityName, 
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
		--JOIN	UserActivityData uad ON ia.IndicatorActivityId = uad.IndicatorActivityId
		LEFT	JOIN Units u ON ad.UnitId= u.UnitId
		CROSS	JOIN @loc
		WHERE	le.LocationEmergencyId= @locEmergencyId
		AND		ec.EmergencyClusterId = @emgClusterId
		AND		(co.StrategicObjectiveId = @strObjId OR @strObjId IS NULL)
		AND		(co.ClusterObjectiveId = @spcObjId OR @spcObjId IS NULL)
		
		
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
						SELECT IsActive, OPSReportId, ClusterName, IndicatorName, ActivityDataId, ActivityName, 
							DataName, 
							CAST(locid AS VARCHAR(5))+''^''+Location+''_''+col AS col, 
							value
						  FROM
						  (
							SELECT IsActive, OPSReportId, ClusterName, IndicatorName, ActivityDataId, ActivityName,
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
						SELECT IsActive, OPSReportId, ClusterName, IndicatorName, ActivityDataId, ActivityName,
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
					  SELECT IsActive, OPSReportId, ClusterName, IndicatorName, ActivityDataId, case when rn = 1 then ActivityName else ActivityName end ActivityName,
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
end
