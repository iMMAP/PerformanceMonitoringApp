-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: 20 Sep, 2013
-- Description:	Get objectives of selected clusters
-- =============================================
CREATE PROCEDURE GetObjectivesOfMultipleClusters
	@ids VARCHAR(50)
AS
BEGIN
	
	SET NOCOUNT ON;
	SELECT * INTO #ids FROM dbo.fn_ParseCSVString (@ids, ',')

    SELECT	co.ClusterObjectiveId,
			co.ObjectiveName
			
	FROM	EmergencyClusters ec JOIN	ClusterObjectives co on ec.EmergencyClusterId = co.EmergencyClusterId
	
	WHERE	ec.EmergencyClusterId IN(SELECT * FROM #ids)
	ORDER BY co.ObjectiveName
END
GO

------------------------------------------------------------------------------------------------------------------------------

-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: 20 Sep, 2013
-- Description:	Get Indicators on passed objectives in parameter
-- =============================================
CREATE PROCEDURE GetIndicatorsOfMultipleObjectives
	@ids VARCHAR(50)
AS
BEGIN
	-- interfering with SELECT statements.
	SET NOCOUNT ON;	
	SELECT * INTO #ids FROM dbo.fn_ParseCSVString (@ids, ',')

    -- Insert statements for procedure here
	SELECT	oi.ObjectiveIndicatorId,
			oi.IndicatorName
	FROM	ObjectiveIndicators oi JOIN ClusterObjectives co ON oi.ClusterObjectiveId = co.ClusterObjectiveId
	WHERE	oi.ClusterObjectiveId IN (SELECT * FROM #ids)
	ORDER BY oi.IndicatorName
END
GO

-------------------------------------------------------------------------------------------------------------------------------------------

-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: 20 Sep, 2013
-- Description:	Get activities of indicators passed as parameters
-- =============================================
CREATE PROCEDURE GetActivitiesOfMultipleIndicators
	@ids VARCHAR(50)
AS
BEGIN
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT * INTO #ids FROM dbo.fn_ParseCSVString (@ids, ',')

	SELECT	ia.IndicatorActivityId,
			ia.ActivityName
	FROM	IndicatorActivities ia JOIN ObjectiveIndicators oi ON ia.ObjectiveIndicatorId = oi.ObjectiveIndicatorId
	WHERE	oi.ObjectiveIndicatorId IN (SELECT * FROM #ids)
	ORDER BY ia.ActivityName
END
GO

------------------------------------------------------------------------------------------------------------------------------

-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: 20 Sep, 2013
-- Description:	Get Data of activities passed as parameters
-- =============================================
CREATE PROCEDURE GetDatItemsOfMultipleActivities
	@ids VARCHAR(50)
AS
BEGIN
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT * INTO #ids FROM dbo.fn_ParseCSVString (@ids, ',')
	
	SELECT	ad.ActivityDataId,
			ad.DataName
			
	FROM	ActivityData ad JOIN IndicatorActivities ia on ad.IndicatorActivityId = ia.IndicatorActivityId
	WHERE	ia.IndicatorActivityId IN (SELECT * FROM #ids)
	ORDER BY ad.DataName

END
GO

---------------------------------------------------------------------------------

-- ================================================
-- Template generated FROM Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Oct 7, 2013
-- Description:	Get Targets, Achieved and WorkDone(percentage)
--				of both of these ON passed parameters.
--				This procedure is written to fetch data FROM charts.
--				This should also work for map (TODO:)
-- =============================================
CREATE PROCEDURE GetDataForChartsAndMaps
	@locationType INT = NULL,
	@locationIds VARCHAR(100),
	@clusterIds VARCHAR(100) = NULL,
	@organizationIds VARCHAR(100) = NULL,
	@fromYear INT = NULL,
	@fromMonth INT = NULL,
	@toYear INT = NULL,
	@toMonth INT = NULL,
	@logFrameSelected INT = NULL,
	@logFrameActual INT = NULL,
	@logFrameIds VARCHAR(100) = NULL,
	@durationType INT = NULL
AS
BEGIN
	
	SET NOCOUNT ON;
	
	IF(@organizationIds IS NULL)
	BEGIN
		SET @organizationIds = ''
	END
	
	IF(@logFrameIds IS NULL)
	BEGIN
		SET @logFrameIds = ''
	END
	

    DECLARE @query1 VARCHAR(max) = ''
	DECLARE @query2 VARCHAR(max) = ''

	-- Get YearId FROM year number passed as parameter
	DECLARE @fromYearId INT
	SELECT @fromYearId = YearId FROM Years WHERE [Year] = @fromYear
	DECLARE @toYearId INT
	SELECT @toYearId = YearId FROM Years WHERE [Year] = @toYear

	SET @query1 = '
	
	-- Get comma seperated values
	SELECT * INTO #locations FROM dbo.fn_ParseCSVString ( ''' + @locationIds + ''','','')
	SELECT * INTO #organizations FROM dbo.fn_ParseCSVString ( ''' + @organizationIds + ''','','')
	SELECT * INTO #logFrameIds FROM dbO.fn_ParseCSVString( ''' + @logFrameIds + ''','','')
    
    -- Get ActivityDataIds ON the basis of logFrameIs passed FROM parameters
    -- LogFrameType parameter will decide that what joins user need.
    -- 1 = Objective, 2 = Indicator, 3 = Activity, 4 = Data
    CREATE TABLE #DataId (dataId INT)		
    IF( ' + CONVERT(VARCHAR(5), @logFrameActual) + ' = 1)
    BEGIN
		INSERT INTO #DataId
		SELECT	DISTINCT ad.ActivityDataId
		FROM	ClusterObjectives co join ObjectiveIndicators oi ON co.ClusterObjectiveId = oi.ClusterObjectiveId
				JOIN IndicatorActivities ia ON oi.ObjectiveIndicatorId = ia.ObjectiveIndicatorId
				JOIN ActivityData ad ON ia.IndicatorActivityId = ad.IndicatorActivityId
				JOIN #logFrameIds lf ON lf.s = co.ClusterObjectiveId -- s here column name returned FROM fn.ParseCSVString
    END
    ELSE
    IF( ' + CONVERT(VARCHAR(5), @logFrameActual) + ' = 2)
    BEGIN
		INSERT INTO #DataId
		SELECT	DISTINCT ad.ActivityDataId
		FROM	ObjectiveIndicators oi JOIN IndicatorActivities ia ON oi.ObjectiveIndicatorId = ia.ObjectiveIndicatorId
				JOIN ActivityData ad ON ia.IndicatorActivityId = ad.IndicatorActivityId
				JOIN #logFrameIds lf ON lf.s = oi.ObjectiveIndicatorId -- s here column name returned FROM fn.ParseCSVString
    END
    ELSE IF( ' + CONVERT(VARCHAR(5), @logFrameActual) + ' = 3)
    BEGIN
		INSERT INTO #DataId
		SELECT	DISTINCT ad.ActivityDataId
		FROM	IndicatorActivities ia JOIN ActivityData ad ON
				ia.IndicatorActivityId = ad.IndicatorActivityId
				JOIN #logFrameIds lf ON lf.s = ia.IndicatorActivityId -- s here column name returned FROM fn.ParseCSVString
    END
    ELSE IF(' + CONVERT(VARCHAR(5), @logFrameActual) + ' = 4)
    BEGIN
		INSERT INTO #DataId
		SELECT * FROM #logFrameIds
    END    

	-- Recersively get all the child of locations in temp table #locations
	;WITH cte AS
	(
		SELECT LocationId, LocationParentId
		FROM Locations
		WHERE LocationId IN (SELECT * FROM #locations)
		UNION ALL
		SELECT l.locationid, l.locationparentid FROM locations l join cte c ON l.locationparentid = c.locationid
	)
	
	SELECT * INTO #temp FROM cte	
	
	-- #temp1 table.
	-- Apply all filters except locations and put data into this temp table.
	CREATE TABLE #temp1 (LocationId INT,
					 LocationName VARCHAR(50),
					 LocationParentId INT,
					 [Target] DECIMAL(18,2),
					 Achieved DECIMAL(18, 2),
					 DataId INT,
					 ActivityId INT,
					 IndicatorId INT,
					 ObjectiveId INT,
					 YearId INT,
					 MonthId INT,
					 QId INT
					 )

	INSERT	INTO #temp1
	SELECT	l.LocationId, 
			locationname AS Location, 
			l.LocationParentId, 
			rd.Target, 
			rd.Achieved,
			d.ActivityDataId,
			ia.IndicatorActivityId,
			oi.ObjectiveIndicatorId,
			co.ClusterObjectiveId,
			r.YearId,
			r.MonthId '
			
	IF (@durationType = 2)
	BEGIN
		SET @query1 = @query1 + ', Q.QNumber '
	END
	ELSE
	BEGIN
		SET @query1 = @query1 + ', 0 AS QId '
	END
			
	SET @query1 = @query1 + ' FROM	Reports r JOIN ReportLocations rl ON r.ReportId = rl.ReportId
			LEFT JOIN Reportdetails rd ON rl.ReportlocationId = rd.ReportlocationId
			LEFT JOIN ActivityData d ON rd.ActivityDataId = d.ActivityDataId
			JOIN IndicatorActivities ia ON ia.IndicatorActivityId = d.IndicatorActivityId
			JOIN ObjectiveIndicators oi ON oi.ObjectiveIndicatorId = ia.ObjectiveIndicatorId
			JOIN ClusterObjectives co ON co.ClusterObjectiveId = oi.ClusterObjectiveId
			JOIN Locations l ON rl.LocationId = l.LocationId
			JOIN #temp t ON l.Locationid = t.LocationId '
	-- If organizations are selected then add tables in resultset and then apply filters ON it.
	IF (@organizationIds != '''' OR @organizationIds IS NOT NULL)
	BEGIN
		SET @query1 = @query1 + '	JOIN Offices ofc ON r.OfficeId = ofc.OfficeId
									JOIN Organizations org ON ofc.OrganizationId = org.OrganizationId '
	END
	
	-- Duration is Monthly, Quarterly and Yearly.
	-- Monthly and Yearly is alredy in Reports table but we need to add Qurarterly table in resultset
	-- if user pass Quarterly FROM front end
	IF (@durationType = 2)
	BEGIN
		SET @query1 = @query1 + ' JOIN Quarters q ON Q.MonthId = r.MonthId '
	END
	
	SET @query1 = @query1 + '	WHERE	d.ActivityDataId IN (SELECT * FROM #DataId) '
	
	-- Filter data ON organizations, if it is passed by user FROM fron end
	IF (@organizationIds != '' AND @organizationids IS NOT NULL)
	BEGIN
		SET @query1 = @query1 + ' AND org.OrganizationId IN (SELECT * FROM #organizations) '
	END
	
	-- Filter ON Year
	IF (@fromYearId > 0 AND @toYearId > 0)
	BEGIN
		SET @query1 = @query1 + ' AND r.YearId >= ' + CONVERT(VARCHAR(2), @fromYearId) + ' AND r.YearId <= ' + CONVERT(VARCHAR(2), @toYearId)
	END
	ELSE IF (@fromYearId > 0 AND (@toYearId = 0 OR @toYearId IS NULL))
	BEGIN
		SET @query1 = @query1 + ' AND r.YearId >= ' + CONVERT(VARCHAR(2), @fromYearId)
	END
	ELSE IF (@toYearId > 0 AND (@fromYearId = 0 OR @fromYearId IS NULL))
	BEGIN
		SET @query1 = @query1 + ' AND r.YearId <= ' + CONVERT(VARCHAR(2), @toYearId)
	END
	
	-- Filter ON Months
	IF (@fromMonth IS NOT NULL AND @toMonth IS NOT NULL)
	BEGIN
		SET @query1 = @query1 + ' AND r.MonthId >= ' + CONVERT(VARCHAR(2), @fromMonth) + ' AND r.MonthId <= ' + CONVERT(VARCHAR(2), @toMonth)
	END
	ELSE IF (@fromMonth IS NOT NULL AND @toMonth IS NULL)
	BEGIN
		SET @query1 = @query1 + ' AND r.MonthId >= ' + CONVERT(VARCHAR(2), @fromMonth)
	END
	ELSE IF (@toMonth IS NOT NULL AND @fromMonth IS NULL)
	BEGIN
		SET @query1 = @query1 + ' AND r.MonthId <= ' + CONVERT(VARCHAR(2), @toMonth)
	END
	
	SET @query1 = @query1 + ' ORDER BY LocationName	

	UPDATE #temp1 SET Target = 0 WHERE Target IS NULL
	UPDATE #temp1 SET Achieved = 0 WHERE Achieved IS NULL
	
	--SELECT * FROM #temp1'
	SET @query2 = '
	IF(' + CONVERT(VARCHAR(10), @locationType) + ' = 1)
	BEGIN	
		SELECT	l2.LocationId
				,l2.Locationname AS Location '
				IF(@logFrameSelected = 4)
				BEGIN
					SET @query2 = @query2 + ',DataId '
				END
				ELSE IF(@logFrameSelected = 3)
				BEGIN
					SET @query2 = @query2 + ',ActivityId '
				END
				ELSE IF(@logFrameSelected = 2)
				BEGIN
					SET @query2 = @query2 + ',IndicatorId '
				END
				ELSE IF(@logFrameSelected = 1)
				BEGIN
					SET @query2 = @query2 + ',ObjectiveId '
				END
				
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId '
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				
				SET @query2 = @query2 + ' , sum(target) AS Target
				, sum(achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
				
		FROM	#temp1 t JOIN locations l ON t.locationparentid = l.locationid
				JOIN locations l2 ON l.locationparentid = l2.locationid
		GROUP BY l2.locationid, l2.locationname '
		IF(@logFrameSelected = 4)
		BEGIN
			SET @query2 = @query2 + ',DataId '
		END
		ELSE IF(@logFrameSelected = 3)
		BEGIN
			SET @query2 = @query2 + ',ActivityId '
		END
		ELSE IF(@logFrameSelected = 2)
		BEGIN
			SET @query2 = @query2 + ',IndicatorId '
		END
		ELSE IF(@logFrameSelected = 1)
		BEGIN
			SET @query2 = @query2 + ',ObjectiveId '
		END
		
		IF (@durationType = 1)
		BEGIN
			SET @query2 = @query2 + ',MonthId '
		END
		ELSE IF (@durationType = 2)
		BEGIN
			SET @query2 = @query2 + ',QId '
		END
		ELSE IF (@durationType = 3)
		BEGIN
			SET @query2 = @query2 + ',YearId '
		END
		
		SET @query2 = @query2 + '
		ORDER BY l2.LocationName
		
	END
	ELSE IF(' + CONVERT(VARCHAR(10), @locationType) + ' = 2)
	BEGIN
	
		SELECT	l.LocationId
				,l.Locationname AS Location '
				IF(@logFrameSelected = 4)
				BEGIN
					SET @query2 = @query2 + ',DataId '
				END
				ELSE IF(@logFrameSelected = 3)
				BEGIN
					SET @query2 = @query2 + ',ActivityId '
				END
				ELSE IF(@logFrameSelected = 2)
				BEGIN
					SET @query2 = @query2 + ',IndicatorId '
				END
				ELSE IF(@logFrameSelected = 1)
				BEGIN
					SET @query2 = @query2 + ',ObjectiveId '
				END
				
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId '
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				
				SET @query2 = @query2 + ' 
				,SUM(Target) AS Target
				,SUM(Achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
		FROM	#temp1 t JOIN Locations l ON t.LocationParentId = l.LocationId
		GROUP BY l.locationid, l.LocationName '
				IF(@logFrameSelected = 4)
				BEGIN
					SET @query2 = @query2 + ',DataId '
				END
				ELSE IF(@logFrameSelected = 3)
				BEGIN
					SET @query2 = @query2 + ',ActivityId '
				END
				ELSE IF(@logFrameSelected = 2)
				BEGIN
					SET @query2 = @query2 + ',IndicatorId '
				END
				ELSE IF(@logFrameSelected = 1)
				BEGIN
					SET @query2 = @query2 + ',ObjectiveId '
				END
				
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId '
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				
				SET @query2 = @query2 + ' 
		ORDER BY l.LocationName
		
	END
	ELSE IF (' + CONVERT(VARCHAR(10), @locationType) + ' = 3)
	BEGIN
	
		SELECT	LocationId
				,Locationname AS Location '
				IF(@logFrameSelected = 4)
				BEGIN
					SET @query2 = @query2 + ',DataId '
				END
				ELSE IF(@logFrameSelected = 3)
				BEGIN
					SET @query2 = @query2 + ',ActivityId '
				END
				ELSE IF(@logFrameSelected = 2)
				BEGIN
					SET @query2 = @query2 + ',IndicatorId '
				END
				ELSE IF(@logFrameSelected = 1)
				BEGIN
					SET @query2 = @query2 + ',ObjectiveId '
				END
				
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId '
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				
				SET @query2 = @query2 + ' 
				,SUM(Target) AS Target
				,SUM(Achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
		FROM	#temp1
		WHERE	LocationId IN (SELECT LocationId FROM #temp)
		GROUP BY LocationId, LocationName '
				IF(@logFrameSelected = 4)
				BEGIN
					SET @query2 = @query2 + ',DataId '
				END
				ELSE IF(@logFrameSelected = 3)
				BEGIN
					SET @query2 = @query2 + ',ActivityId '
				END
				ELSE IF(@logFrameSelected = 2)
				BEGIN
					SET @query2 = @query2 + ',IndicatorId '
				END
				ELSE IF(@logFrameSelected = 1)
				BEGIN
					SET @query2 = @query2 + ',ObjectiveId '
				END
				
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId '
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				
				SET @query2 = @query2 + ' 
		ORDER BY LocationName
	END

	DROP TABLE #locations
	DROP TABLE #organizations
	DROP TABLE #temp
	DROP TABLE #temp1
	DROP TABLE #DataId
	DROP TABLE #logFrameIds
	'

	PRINT @query1 + @query2
	EXEC (@query1 + @query2)
END
GO

----------------------------------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetDataForChartsAndMaps]    Script Date: 10/10/2013 14:30:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Oct 7, 2013
-- Description:	Get Targets, Achieved and WorkDone(percentage)
--				of both of these ON passed parameters.
--				This procedure is written to fetch data FROM charts.
--				This should also work for map (TODO:)
-- =============================================
ALTER PROCEDURE [dbo].[GetDataForChartsAndMaps]
	@locationType INT = NULL,
	@locationIds VARCHAR(100),
	@clusterIds VARCHAR(100) = NULL,
	@organizationIds VARCHAR(100) = NULL,
	@fromYear INT = NULL,
	@fromMonth INT = NULL,
	@toYear INT = NULL,
	@toMonth INT = NULL,
	@logFrameSelected INT = NULL,
	@logFrameActual INT = NULL,
	@logFrameIds VARCHAR(100) = NULL,
	@durationType INT = NULL
AS
BEGIN
	
	SET NOCOUNT ON;
	
	IF(@organizationIds IS NULL)
	BEGIN
		SET @organizationIds = ''
	END
	
	IF(@logFrameIds IS NULL)
	BEGIN
		SET @logFrameIds = ''
	END
	

    DECLARE @query1 VARCHAR(max) = ''
	DECLARE @query2 VARCHAR(max) = ''

	-- Get YearId FROM year number passed as parameter
	DECLARE @fromYearId INT
	SELECT @fromYearId = YearId FROM Years WHERE [Year] = @fromYear
	DECLARE @toYearId INT
	SELECT @toYearId = YearId FROM Years WHERE [Year] = @toYear							

	SET @query1 = '
	
	CREATE TABLE #Result (	LocationId INT,
							Location NVARCHAR(200),
							LogFrameId INT,
							[Target] DECIMAL(18, 2),
							Achieved DECIMAL(18, 2),
							WorkDone INT,
							ClusterId INT,
							ClusterName NVARCHAR(250),
							ObjectiveId  INT,
							ObjectiveName NVARCHAR(4000),
							IndicatorId  INT,
							IndicatorName NVARCHAR(4000),
							ActivityId  INT,
							ActivityName NVARCHAR(4000),
							DataId  INT,
							DataName NVARCHAR(4000)
						)
	
	-- Get comma seperated values
	
	SELECT * INTO #locations FROM dbo.fn_ParseCSVString ( ''' + @locationIds + ''','','')
	SELECT * INTO #organizations FROM dbo.fn_ParseCSVString ( ''' + @organizationIds + ''','','')
	SELECT * INTO #logFrameIds FROM dbO.fn_ParseCSVString( ''' + @logFrameIds + ''','','')
    
    -- Get ActivityDataIds ON the basis of logFrameIs passed FROM parameters
    -- LogFrameType parameter will decide that what joins user need.
    -- 1 = Objective, 2 = Indicator, 3 = Activity, 4 = Data
    CREATE TABLE #DataId (dataId INT)		
    IF( ' + CONVERT(VARCHAR(5), @logFrameActual) + ' = 1)
    BEGIN
		INSERT INTO #DataId
		SELECT	DISTINCT ad.ActivityDataId
		FROM	ClusterObjectives co join ObjectiveIndicators oi ON co.ClusterObjectiveId = oi.ClusterObjectiveId
				JOIN IndicatorActivities ia ON oi.ObjectiveIndicatorId = ia.ObjectiveIndicatorId
				JOIN ActivityData ad ON ia.IndicatorActivityId = ad.IndicatorActivityId
				JOIN #logFrameIds lf ON lf.s = co.ClusterObjectiveId -- s here column name returned FROM fn.ParseCSVString
    END
    ELSE
    IF( ' + CONVERT(VARCHAR(5), @logFrameActual) + ' = 2)
    BEGIN
		INSERT INTO #DataId
		SELECT	DISTINCT ad.ActivityDataId
		FROM	ObjectiveIndicators oi JOIN IndicatorActivities ia ON oi.ObjectiveIndicatorId = ia.ObjectiveIndicatorId
				JOIN ActivityData ad ON ia.IndicatorActivityId = ad.IndicatorActivityId
				JOIN #logFrameIds lf ON lf.s = oi.ObjectiveIndicatorId -- s here column name returned FROM fn.ParseCSVString
    END
    ELSE IF( ' + CONVERT(VARCHAR(5), @logFrameActual) + ' = 3)
    BEGIN
		INSERT INTO #DataId
		SELECT	DISTINCT ad.ActivityDataId
		FROM	IndicatorActivities ia JOIN ActivityData ad ON
				ia.IndicatorActivityId = ad.IndicatorActivityId
				JOIN #logFrameIds lf ON lf.s = ia.IndicatorActivityId -- s here column name returned FROM fn.ParseCSVString
    END
    ELSE IF(' + CONVERT(VARCHAR(5), @logFrameActual) + ' = 4)
    BEGIN
		INSERT INTO #DataId
		SELECT * FROM #logFrameIds
    END    

	-- Recersively get all the child of locations in temp table #locations
	;WITH cte AS
	(
		SELECT LocationId, LocationParentId
		FROM Locations
		WHERE LocationId IN (SELECT * FROM #locations)
		UNION ALL
		SELECT l.locationid, l.locationparentid FROM locations l join cte c ON l.locationparentid = c.locationid
	)
	
	SELECT * INTO #temp FROM cte	
	
	-- #temp1 table.
	-- Apply all filters except locations and put data into this temp table.
	CREATE TABLE #temp1 (LocationId INT,
					 LocationName VARCHAR(50),
					 LocationParentId INT,
					 [Target] DECIMAL(18,2),
					 Achieved DECIMAL(18, 2),
					 DataId INT,
					 ActivityId INT,
					 IndicatorId INT,
					 ObjectiveId INT,
					 YearId INT,
					 MonthId INT,
					 QId INT
					 )

	INSERT	INTO #temp1
	SELECT	l.LocationId, 
			locationname AS Location, 
			l.LocationParentId, 
			rd.Target, 
			rd.Achieved,
			d.ActivityDataId,
			ia.IndicatorActivityId,
			oi.ObjectiveIndicatorId,
			co.ClusterObjectiveId,
			r.YearId,
			r.MonthId '
			
	IF (@durationType = 2)
	BEGIN
		SET @query1 = @query1 + ', Q.QNumber '
	END
	ELSE
	BEGIN
		SET @query1 = @query1 + ', 0 AS QId '
	END
			
	SET @query1 = @query1 + ' FROM	Reports r JOIN ReportLocations rl ON r.ReportId = rl.ReportId
			LEFT JOIN Reportdetails rd ON rl.ReportlocationId = rd.ReportlocationId
			LEFT JOIN ActivityData d ON rd.ActivityDataId = d.ActivityDataId
			JOIN IndicatorActivities ia ON ia.IndicatorActivityId = d.IndicatorActivityId
			JOIN ObjectiveIndicators oi ON oi.ObjectiveIndicatorId = ia.ObjectiveIndicatorId
			JOIN ClusterObjectives co ON co.ClusterObjectiveId = oi.ClusterObjectiveId
			JOIN Locations l ON rl.LocationId = l.LocationId
			JOIN #temp t ON l.Locationid = t.LocationId '
	-- If organizations are selected then add tables in resultset and then apply filters ON it.
	IF (@organizationIds != '''' OR @organizationIds IS NOT NULL)
	BEGIN
		SET @query1 = @query1 + '	JOIN Offices ofc ON r.OfficeId = ofc.OfficeId
									JOIN Organizations org ON ofc.OrganizationId = org.OrganizationId '
	END
	
	-- Duration is Monthly, Quarterly and Yearly.
	-- Monthly and Yearly is alredy in Reports table but we need to add Qurarterly table in resultset
	-- if user pass Quarterly FROM front end
	IF (@durationType = 2)
	BEGIN
		SET @query1 = @query1 + ' JOIN Quarters q ON Q.MonthId = r.MonthId '
	END
	
	SET @query1 = @query1 + '	WHERE	d.ActivityDataId IN (SELECT * FROM #DataId) '
	
	-- Filter data ON organizations, if it is passed by user FROM fron end
	IF (@organizationIds != '' AND @organizationids IS NOT NULL)
	BEGIN
		SET @query1 = @query1 + ' AND org.OrganizationId IN (SELECT * FROM #organizations) '
	END
	
	-- Filter ON Year
	IF (@fromYearId > 0 AND @toYearId > 0)
	BEGIN
		SET @query1 = @query1 + ' AND r.YearId >= ' + CONVERT(VARCHAR(2), @fromYearId) + ' AND r.YearId <= ' + CONVERT(VARCHAR(2), @toYearId)
	END
	ELSE IF (@fromYearId > 0 AND (@toYearId = 0 OR @toYearId IS NULL))
	BEGIN
		SET @query1 = @query1 + ' AND r.YearId >= ' + CONVERT(VARCHAR(2), @fromYearId)
	END
	ELSE IF (@toYearId > 0 AND (@fromYearId = 0 OR @fromYearId IS NULL))
	BEGIN
		SET @query1 = @query1 + ' AND r.YearId <= ' + CONVERT(VARCHAR(2), @toYearId)
	END
	
	-- Filter ON Months
	IF (@fromMonth IS NOT NULL AND @toMonth IS NOT NULL)
	BEGIN
		SET @query1 = @query1 + ' AND r.MonthId >= ' + CONVERT(VARCHAR(2), @fromMonth) + ' AND r.MonthId <= ' + CONVERT(VARCHAR(2), @toMonth)
	END
	ELSE IF (@fromMonth IS NOT NULL AND @toMonth IS NULL)
	BEGIN
		SET @query1 = @query1 + ' AND r.MonthId >= ' + CONVERT(VARCHAR(2), @fromMonth)
	END
	ELSE IF (@toMonth IS NOT NULL AND @fromMonth IS NULL)
	BEGIN
		SET @query1 = @query1 + ' AND r.MonthId <= ' + CONVERT(VARCHAR(2), @toMonth)
	END
	
	SET @query1 = @query1 + ' ORDER BY LocationName	

	UPDATE #temp1 SET Target = 0 WHERE Target IS NULL
	UPDATE #temp1 SET Achieved = 0 WHERE Achieved IS NULL
	
	--SELECT * FROM #temp1'
	SET @query2 = '
	IF(' + CONVERT(VARCHAR(10), @locationType) + ' = 1)
	BEGIN	
		INSERT INTO #Result
		(
			LocationId,
			Location,
			LogFrameId,
			[Target],
			Achieved,
			WorkDone
		)
		SELECT	l2.LocationId
				,l2.Locationname AS Location '
				IF(@logFrameSelected = 4)
				BEGIN
					SET @query2 = @query2 + ',DataId '
				END
				ELSE IF(@logFrameSelected = 3)
				BEGIN
					SET @query2 = @query2 + ',ActivityId '
				END
				ELSE IF(@logFrameSelected = 2)
				BEGIN
					SET @query2 = @query2 + ',IndicatorId '
				END
				ELSE IF(@logFrameSelected = 1)
				BEGIN
					SET @query2 = @query2 + ',ObjectiveId '
				END
				
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId '
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				
				SET @query2 = @query2 + ' , sum(target) AS Target
				, sum(achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
				
		FROM	#temp1 t JOIN locations l ON t.locationparentid = l.locationid
				JOIN locations l2 ON l.locationparentid = l2.locationid
		GROUP BY l2.locationid, l2.locationname '
		IF(@logFrameSelected = 4)
		BEGIN
			SET @query2 = @query2 + ',DataId '
		END
		ELSE IF(@logFrameSelected = 3)
		BEGIN
			SET @query2 = @query2 + ',ActivityId '
		END
		ELSE IF(@logFrameSelected = 2)
		BEGIN
			SET @query2 = @query2 + ',IndicatorId '
		END
		ELSE IF(@logFrameSelected = 1)
		BEGIN
			SET @query2 = @query2 + ',ObjectiveId '
		END
		
		IF (@durationType = 1)
		BEGIN
			SET @query2 = @query2 + ',MonthId '
		END
		ELSE IF (@durationType = 2)
		BEGIN
			SET @query2 = @query2 + ',QId '
		END
		ELSE IF (@durationType = 3)
		BEGIN
			SET @query2 = @query2 + ',YearId '
		END
		
		SET @query2 = @query2 + '
		ORDER BY l2.LocationName
		
	END
	ELSE IF(' + CONVERT(VARCHAR(10), @locationType) + ' = 2)
	BEGIN
		INSERT INTO #Result
		(
			LocationId,
			Location,
			LogFrameId,
			[Target],
			Achieved,
			WorkDone
		)
		SELECT	l.LocationId
				,l.Locationname AS Location '
				IF(@logFrameSelected = 4)
				BEGIN
					SET @query2 = @query2 + ',DataId '
				END
				ELSE IF(@logFrameSelected = 3)
				BEGIN
					SET @query2 = @query2 + ',ActivityId '
				END
				ELSE IF(@logFrameSelected = 2)
				BEGIN
					SET @query2 = @query2 + ',IndicatorId '
				END
				ELSE IF(@logFrameSelected = 1)
				BEGIN
					SET @query2 = @query2 + ',ObjectiveId '
				END
				
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId '
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				
				SET @query2 = @query2 + ' 
				,SUM(Target) AS Target
				,SUM(Achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
		FROM	#temp1 t JOIN Locations l ON t.LocationParentId = l.LocationId
		GROUP BY l.locationid, l.LocationName '
				IF(@logFrameSelected = 4)
				BEGIN
					SET @query2 = @query2 + ',DataId '
				END
				ELSE IF(@logFrameSelected = 3)
				BEGIN
					SET @query2 = @query2 + ',ActivityId '
				END
				ELSE IF(@logFrameSelected = 2)
				BEGIN
					SET @query2 = @query2 + ',IndicatorId '
				END
				ELSE IF(@logFrameSelected = 1)
				BEGIN
					SET @query2 = @query2 + ',ObjectiveId '
				END
				
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId '
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				
				SET @query2 = @query2 + ' 
		ORDER BY l.LocationName
		
	END
	ELSE IF (' + CONVERT(VARCHAR(10), @locationType) + ' = 3)
	BEGIN
		INSERT INTO #Result
		(
			LocationId,
			Location,
			LogFrameId,
			[Target],
			Achieved,
			WorkDone
		)
		SELECT	LocationId
				,Locationname AS Location '
				IF(@logFrameSelected = 4)
				BEGIN
					SET @query2 = @query2 + ',DataId '
				END
				ELSE IF(@logFrameSelected = 3)
				BEGIN
					SET @query2 = @query2 + ',ActivityId '
				END
				ELSE IF(@logFrameSelected = 2)
				BEGIN
					SET @query2 = @query2 + ',IndicatorId '
				END
				ELSE IF(@logFrameSelected = 1)
				BEGIN
					SET @query2 = @query2 + ',ObjectiveId '
				END
				
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId '
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				
				SET @query2 = @query2 + ' 
				,SUM(Target) AS Target
				,SUM(Achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
		FROM	#temp1
		WHERE	LocationId IN (SELECT LocationId FROM #temp)
		GROUP BY LocationId, LocationName '
				IF(@logFrameSelected = 4)
				BEGIN
					SET @query2 = @query2 + ',DataId '
				END
				ELSE IF(@logFrameSelected = 3)
				BEGIN
					SET @query2 = @query2 + ',ActivityId '
				END
				ELSE IF(@logFrameSelected = 2)
				BEGIN
					SET @query2 = @query2 + ',IndicatorId '
				END
				ELSE IF(@logFrameSelected = 1)
				BEGIN
					SET @query2 = @query2 + ',ObjectiveId '
				END
				
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId '
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				
				SET @query2 = @query2 + ' 
		ORDER BY LocationName
	END

	DROP TABLE #locations
	DROP TABLE #organizations
	DROP TABLE #temp
	DROP TABLE #temp1
	DROP TABLE #DataId
	DROP TABLE #logFrameIds	
	
	UPDATE	#Result
	SET		ClusterId = c.ClusterId,
			ClusterName = c.ClusterName,
			ObjectiveId = co.ClusterObjectiveId,
			ObjectiveName = co.ObjectiveName,
			IndicatorId = oi.ObjectiveIndicatorId,
			IndicatorName = oi.IndicatorName,
			ActivityId = ia.IndicatorActivityId,
			ActivityName = ia.ActivityName,
			DataId = ad.ActivityDataId,
			DataName = ad.DataName
			
	FROM	ActivityData ad JOIN IndicatorActivities ia ON ad.IndicatorActivityId = ia.IndicatorActivityId
			JOIN ObjectiveIndicators oi ON oi.ObjectiveIndicatorId = ia.ObjectiveIndicatorId
			JOIN ClusterObjectives co ON co.ClusterObjectiveId = oi.ClusterObjectiveId
			JOIN EmergencyClusters ec ON ec.EmergencyClusterId = co.EmergencyClusterId
			JOIN Clusters c ON c.ClusterId = ec.ClusterId '
			
	IF(@logFrameSelected = 1)
	BEGIN
		SET @query2 = @query2 + ' JOIN #Result r ON r.LogFrameId = co.ClusterObjectiveId '
	END
	ELSE IF(@logFrameSelected = 2)
	BEGIN
		SET @query2 = @query2 + ' JOIN #Result r ON r.LogFrameId = oi.ObjectiveIndicatorId '
	END
	ELSE IF(@logFrameSelected = 3)
	BEGIN
		SET @query2 = @query2 + ' JOIN #Result r ON r.LogFrameId = ia.IndicatorActivityId '
	END
	ELSE IF(@logFrameSelected = 4)
	BEGIN
		SET @query2 = @query2 + ' JOIN #Result r ON r.LogFrameId = ad.ActivityDataId '
	END

	SET @query2 = @query2 + ' 
	SELECT * FROM #Result
	DROP TABLE #Result
	'
		
	PRINT @query1 + @query2
	EXEC (@query1 + @query2)
END
--------------------------------------------------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetDataForChartsAndMaps]    Script Date: 10/12/2013 18:16:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Oct 7, 2013
-- Description:	Get Targets, Achieved and WorkDone(percentage)
--				of both of these ON passed parameters.
--				This procedure is written to fetch data FROM charts.
--				This should also work for map (TODO:)
-- =============================================
ALTER PROCEDURE [dbo].[GetDataForChartsAndMaps]
	@locationType INT = NULL,
	@locationIds VARCHAR(100),
	@clusterIds VARCHAR(100) = NULL,
	@organizationIds VARCHAR(100) = NULL,
	@fromYear INT = NULL,
	@fromMonth INT = NULL,
	@toYear INT = NULL,
	@toMonth INT = NULL,
	@logFrameSelected INT = NULL,
	@logFrameActual INT = NULL,
	@logFrameIds VARCHAR(100) = NULL,
	@durationType INT = NULL
AS
BEGIN
	
	SET NOCOUNT ON;
	
	IF(@organizationIds IS NULL)
	BEGIN
		SET @organizationIds = ''
	END
	
	IF(@logFrameIds IS NULL)
	BEGIN
		SET @logFrameIds = ''
	END
	

    DECLARE @query1 VARCHAR(max) = ''
	DECLARE @query2 VARCHAR(max) = ''

	-- Get YearId FROM year number passed as parameter
	DECLARE @fromYearId INT
	SELECT @fromYearId = YearId FROM Years WHERE [Year] = @fromYear
	DECLARE @toYearId INT
	SELECT @toYearId = YearId FROM Years WHERE [Year] = @toYear							

	SET @query1 = '
	
	CREATE TABLE #Result (	LocationId INT,
							Location NVARCHAR(200),
							LogFrameId INT,
							[Target] DECIMAL(18, 2),
							Achieved DECIMAL(18, 2),
							WorkDone INT,
							ClusterId INT,
							ClusterName NVARCHAR(250),
							ObjectiveId  INT,
							ObjectiveName NVARCHAR(4000),
							IndicatorId  INT,
							IndicatorName NVARCHAR(4000),
							ActivityId  INT,
							ActivityName NVARCHAR(4000),
							DataId  INT,
							DataName NVARCHAR(4000),
							LogFrameType NVARCHAR(20),
							DurationType INT
						)
	
	-- Get comma seperated values
	
	SELECT * INTO #locations FROM dbo.fn_ParseCSVString ( ''' + @locationIds + ''','','')
	SELECT * INTO #organizations FROM dbo.fn_ParseCSVString ( ''' + @organizationIds + ''','','')
	SELECT * INTO #logFrameIds FROM dbO.fn_ParseCSVString( ''' + @logFrameIds + ''','','')
    
    -- Get ActivityDataIds ON the basis of logFrameIs passed FROM parameters
    -- LogFrameType parameter will decide that what joins user need.
    -- 1 = Objective, 2 = Indicator, 3 = Activity, 4 = Data
    CREATE TABLE #DataId (dataId INT)		
    IF( ' + CONVERT(VARCHAR(5), @logFrameActual) + ' = 1)
    BEGIN
		INSERT INTO #DataId
		SELECT	DISTINCT ad.ActivityDataId
		FROM	ClusterObjectives co join ObjectiveIndicators oi ON co.ClusterObjectiveId = oi.ClusterObjectiveId
				JOIN IndicatorActivities ia ON oi.ObjectiveIndicatorId = ia.ObjectiveIndicatorId
				JOIN ActivityData ad ON ia.IndicatorActivityId = ad.IndicatorActivityId
				JOIN #logFrameIds lf ON lf.s = co.ClusterObjectiveId -- s here column name returned FROM fn.ParseCSVString
    END
    ELSE
    IF( ' + CONVERT(VARCHAR(5), @logFrameActual) + ' = 2)
    BEGIN
		INSERT INTO #DataId
		SELECT	DISTINCT ad.ActivityDataId
		FROM	ObjectiveIndicators oi JOIN IndicatorActivities ia ON oi.ObjectiveIndicatorId = ia.ObjectiveIndicatorId
				JOIN ActivityData ad ON ia.IndicatorActivityId = ad.IndicatorActivityId
				JOIN #logFrameIds lf ON lf.s = oi.ObjectiveIndicatorId -- s here column name returned FROM fn.ParseCSVString
    END
    ELSE IF( ' + CONVERT(VARCHAR(5), @logFrameActual) + ' = 3)
    BEGIN
		INSERT INTO #DataId
		SELECT	DISTINCT ad.ActivityDataId
		FROM	IndicatorActivities ia JOIN ActivityData ad ON
				ia.IndicatorActivityId = ad.IndicatorActivityId
				JOIN #logFrameIds lf ON lf.s = ia.IndicatorActivityId -- s here column name returned FROM fn.ParseCSVString
    END
    ELSE IF(' + CONVERT(VARCHAR(5), @logFrameActual) + ' = 4)
    BEGIN
		INSERT INTO #DataId
		SELECT * FROM #logFrameIds
    END    

	-- Recersively get all the child of locations in temp table #locations
	;WITH cte AS
	(
		SELECT LocationId, LocationParentId
		FROM Locations
		WHERE LocationId IN (SELECT * FROM #locations)
		UNION ALL
		SELECT l.locationid, l.locationparentid FROM locations l join cte c ON l.locationparentid = c.locationid
	)
	
	SELECT * INTO #temp FROM cte	
	
	-- #temp1 table.
	-- Apply all filters except locations and put data into this temp table.
	CREATE TABLE #temp1 (LocationId INT,
					 LocationName VARCHAR(50),
					 LocationParentId INT,
					 [Target] DECIMAL(18,2),
					 Achieved DECIMAL(18, 2),
					 DataId INT,
					 ActivityId INT,
					 IndicatorId INT,
					 ObjectiveId INT,
					 YearId INT,
					 MonthId INT,
					 QId INT
					 )

	INSERT	INTO #temp1
	SELECT	l.LocationId, 
			locationname AS Location, 
			l.LocationParentId, 
			rd.Target, 
			rd.Achieved,
			d.ActivityDataId,
			ia.IndicatorActivityId,
			oi.ObjectiveIndicatorId,
			co.ClusterObjectiveId,
			r.YearId,
			r.MonthId
			'
			
	IF (@durationType = 2)
	BEGIN
		SET @query1 = @query1 + ', Q.QNumber  AS QId'
	END
	ELSE
	BEGIN
		SET @query1 = @query1 + ', 0 AS QId '
	END
			
	SET @query1 = @query1 + ' FROM	Reports r JOIN ReportLocations rl ON r.ReportId = rl.ReportId
			LEFT JOIN Reportdetails rd ON rl.ReportlocationId = rd.ReportlocationId
			LEFT JOIN ActivityData d ON rd.ActivityDataId = d.ActivityDataId
			JOIN IndicatorActivities ia ON ia.IndicatorActivityId = d.IndicatorActivityId
			JOIN ObjectiveIndicators oi ON oi.ObjectiveIndicatorId = ia.ObjectiveIndicatorId
			JOIN ClusterObjectives co ON co.ClusterObjectiveId = oi.ClusterObjectiveId
			JOIN Locations l ON rl.LocationId = l.LocationId
			JOIN #temp t ON l.Locationid = t.LocationId '
	-- If organizations are selected then add tables in resultset and then apply filters ON it.
	IF (@organizationIds != '''' OR @organizationIds IS NOT NULL)
	BEGIN
		SET @query1 = @query1 + '	JOIN Offices ofc ON r.OfficeId = ofc.OfficeId
									JOIN Organizations org ON ofc.OrganizationId = org.OrganizationId '
	END
	
	-- Duration is Monthly, Quarterly and Yearly.
	-- Monthly and Yearly is alredy in Reports table but we need to add Qurarterly table in resultset
	-- if user pass Quarterly FROM front end
	IF (@durationType = 2)
	BEGIN
		SET @query1 = @query1 + ' JOIN Quarters q ON Q.MonthId = r.MonthId '
	END
	
	SET @query1 = @query1 + '	WHERE	d.ActivityDataId IN (SELECT * FROM #DataId) '
	
	-- Filter data ON organizations, if it is passed by user FROM fron end
	IF (@organizationIds != '' AND @organizationids IS NOT NULL)
	BEGIN
		SET @query1 = @query1 + ' AND org.OrganizationId IN (SELECT * FROM #organizations) '
	END
	
	-- Filter ON Year
	IF (@fromYearId > 0 AND @toYearId > 0)
	BEGIN
		SET @query1 = @query1 + ' AND r.YearId >= ' + CONVERT(VARCHAR(2), @fromYearId) + ' AND r.YearId <= ' + CONVERT(VARCHAR(2), @toYearId)
	END
	ELSE IF (@fromYearId > 0 AND (@toYearId = 0 OR @toYearId IS NULL))
	BEGIN
		SET @query1 = @query1 + ' AND r.YearId >= ' + CONVERT(VARCHAR(2), @fromYearId)
	END
	ELSE IF (@toYearId > 0 AND (@fromYearId = 0 OR @fromYearId IS NULL))
	BEGIN
		SET @query1 = @query1 + ' AND r.YearId <= ' + CONVERT(VARCHAR(2), @toYearId)
	END
	
	-- Filter ON Months
	IF (@fromMonth IS NOT NULL AND @toMonth IS NOT NULL)
	BEGIN
		SET @query1 = @query1 + ' AND r.MonthId >= ' + CONVERT(VARCHAR(2), @fromMonth) + ' AND r.MonthId <= ' + CONVERT(VARCHAR(2), @toMonth)
	END
	ELSE IF (@fromMonth IS NOT NULL AND @toMonth IS NULL)
	BEGIN
		SET @query1 = @query1 + ' AND r.MonthId >= ' + CONVERT(VARCHAR(2), @fromMonth)
	END
	ELSE IF (@toMonth IS NOT NULL AND @fromMonth IS NULL)
	BEGIN
		SET @query1 = @query1 + ' AND r.MonthId <= ' + CONVERT(VARCHAR(2), @toMonth)
	END
	
	SET @query1 = @query1 + ' ORDER BY LocationName	

	UPDATE #temp1 SET Target = 0 WHERE Target IS NULL
	UPDATE #temp1 SET Achieved = 0 WHERE Achieved IS NULL
	
	--SELECT * FROM #temp1'
	SET @query2 = '
	IF(' + CONVERT(VARCHAR(10), @locationType) + ' = 1)
	BEGIN	
		INSERT INTO #Result
		(
			LocationId,
			Location,
			LogFrameId,
			[Target],
			Achieved,
			WorkDone '			
			IF (@durationType IS NOT NULL)
			BEGIN
				SET @query2 = @query2 + ',DurationType '
			END
				
		SET @query2 = @query2 + '	
		)
		SELECT	l2.LocationId
				,l2.Locationname AS Location '
				IF(@logFrameSelected = 4)
				BEGIN
					SET @query2 = @query2 + ',DataId '
				END
				ELSE IF(@logFrameSelected = 3)
				BEGIN
					SET @query2 = @query2 + ',ActivityId '
				END
				ELSE IF(@logFrameSelected = 2)
				BEGIN
					SET @query2 = @query2 + ',IndicatorId '
				END
				ELSE IF(@logFrameSelected = 1)
				BEGIN
					SET @query2 = @query2 + ',ObjectiveId '
				END	
								
				SET @query2 = @query2 + ' , sum(target) AS Target
				, sum(achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone '
					
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId '
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
		SET @query2 = @query2 + ' 		
		FROM	#temp1 t JOIN locations l ON t.locationparentid = l.locationid
				JOIN locations l2 ON l.locationparentid = l2.locationid
		GROUP BY l2.locationid, l2.locationname '
		IF(@logFrameSelected = 4)
		BEGIN
			SET @query2 = @query2 + ',DataId '
		END
		ELSE IF(@logFrameSelected = 3)
		BEGIN
			SET @query2 = @query2 + ',ActivityId '
		END
		ELSE IF(@logFrameSelected = 2)
		BEGIN
			SET @query2 = @query2 + ',IndicatorId '
		END
		ELSE IF(@logFrameSelected = 1)
		BEGIN
			SET @query2 = @query2 + ',ObjectiveId '
		END
		
		IF (@durationType = 1)
		BEGIN
			SET @query2 = @query2 + ',MonthId '
		END
		ELSE IF (@durationType = 2)
		BEGIN
			SET @query2 = @query2 + ',QId '
		END
		ELSE IF (@durationType = 3)
		BEGIN
			SET @query2 = @query2 + ',YearId '
		END
		
		SET @query2 = @query2 + '
		ORDER BY l2.LocationName
		
	END
	ELSE IF(' + CONVERT(VARCHAR(10), @locationType) + ' = 2)
	BEGIN
		INSERT INTO #Result
		(
			LocationId,
			Location,
			LogFrameId,
			[Target],
			Achieved,
			WorkDone '
			IF (@durationType IS NOT NULL)
			BEGIN
				SET @query2 = @query2 + ',DurationType '
			END
				
		SET @query2 = @query2 + '	
		)
		SELECT	l.LocationId
				,l.Locationname AS Location '
				IF(@logFrameSelected = 4)
				BEGIN
					SET @query2 = @query2 + ',DataId '
				END
				ELSE IF(@logFrameSelected = 3)
				BEGIN
					SET @query2 = @query2 + ',ActivityId '
				END
				ELSE IF(@logFrameSelected = 2)
				BEGIN
					SET @query2 = @query2 + ',IndicatorId '
				END
				ELSE IF(@logFrameSelected = 1)
				BEGIN
					SET @query2 = @query2 + ',ObjectiveId '
				END		
												
				SET @query2 = @query2 + ' 
				,SUM(Target) AS Target
				,SUM(Achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone '
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId '
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				SET @query2 = @query2 + ' 
		FROM	#temp1 t JOIN Locations l ON t.LocationParentId = l.LocationId
		GROUP BY l.locationid, l.LocationName '
				IF(@logFrameSelected = 4)
				BEGIN
					SET @query2 = @query2 + ',DataId '
				END
				ELSE IF(@logFrameSelected = 3)
				BEGIN
					SET @query2 = @query2 + ',ActivityId '
				END
				ELSE IF(@logFrameSelected = 2)
				BEGIN
					SET @query2 = @query2 + ',IndicatorId '
				END
				ELSE IF(@logFrameSelected = 1)
				BEGIN
					SET @query2 = @query2 + ',ObjectiveId '
				END
				
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId '
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				
				SET @query2 = @query2 + ' 
		ORDER BY l.LocationName
		
	END
	ELSE IF (' + CONVERT(VARCHAR(10), @locationType) + ' = 3)
	BEGIN
		INSERT INTO #Result
		(
			LocationId,
			Location,
			LogFrameId,
			[Target],
			Achieved,
			WorkDone '
			IF (@durationType IS NOT NULL)
			BEGIN
				SET @query2 = @query2 + ',DurationType '
			END
				
		SET @query2 = @query2 + '	
		)
		SELECT	LocationId
				,Locationname AS Location '
				IF(@logFrameSelected = 4)
				BEGIN
					SET @query2 = @query2 + ',DataId '
				END
				ELSE IF(@logFrameSelected = 3)
				BEGIN
					SET @query2 = @query2 + ',ActivityId '
				END
				ELSE IF(@logFrameSelected = 2)
				BEGIN
					SET @query2 = @query2 + ',IndicatorId '
				END
				ELSE IF(@logFrameSelected = 1)
				BEGIN
					SET @query2 = @query2 + ',ObjectiveId '
				END		
								
				SET @query2 = @query2 + ' 
				,SUM(Target) AS Target
				,SUM(Achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone '
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId '
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				SET @query2 = @query2 + ' 
		FROM	#temp1
		WHERE	LocationId IN (SELECT LocationId FROM #temp)
		GROUP BY LocationId, LocationName '
				IF(@logFrameSelected = 4)
				BEGIN
					SET @query2 = @query2 + ',DataId '
				END
				ELSE IF(@logFrameSelected = 3)
				BEGIN
					SET @query2 = @query2 + ',ActivityId '
				END
				ELSE IF(@logFrameSelected = 2)
				BEGIN
					SET @query2 = @query2 + ',IndicatorId '
				END
				ELSE IF(@logFrameSelected = 1)
				BEGIN
					SET @query2 = @query2 + ',ObjectiveId '
				END
				
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId '
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				
				SET @query2 = @query2 + ' 
		ORDER BY LocationName
	END

	DROP TABLE #locations
	DROP TABLE #organizations
	DROP TABLE #temp
	DROP TABLE #temp1
	DROP TABLE #DataId
	DROP TABLE #logFrameIds	
	
	UPDATE	#Result
	SET		ClusterId = c.ClusterId,
			ClusterName = c.ClusterName,
			ObjectiveId = co.ClusterObjectiveId,
			ObjectiveName = co.ObjectiveName,
			IndicatorId = oi.ObjectiveIndicatorId,
			IndicatorName = oi.IndicatorName,
			ActivityId = ia.IndicatorActivityId,
			ActivityName = ia.ActivityName,
			DataId = ad.ActivityDataId,
			DataName = ad.DataName
			
	FROM	ActivityData ad JOIN IndicatorActivities ia ON ad.IndicatorActivityId = ia.IndicatorActivityId
			JOIN ObjectiveIndicators oi ON oi.ObjectiveIndicatorId = ia.ObjectiveIndicatorId
			JOIN ClusterObjectives co ON co.ClusterObjectiveId = oi.ClusterObjectiveId
			JOIN EmergencyClusters ec ON ec.EmergencyClusterId = co.EmergencyClusterId
			JOIN Clusters c ON c.ClusterId = ec.ClusterId '
			
	IF(@logFrameSelected = 1)
	BEGIN
		SET @query2 = @query2 + ' JOIN #Result r ON r.LogFrameId = co.ClusterObjectiveId 
		UPDATE #Result SET LogFrameType = ''Objectives''
		'
		
	END
	ELSE IF(@logFrameSelected = 2)
	BEGIN
		SET @query2 = @query2 + ' JOIN #Result r ON r.LogFrameId = oi.ObjectiveIndicatorId 
		UPDATE #Result SET LogFrameType = ''Indicators''
		'
	END
	ELSE IF(@logFrameSelected = 3)
	BEGIN
		SET @query2 = @query2 + ' JOIN #Result r ON r.LogFrameId = ia.IndicatorActivityId 
		UPDATE #Result SET LogFrameType = ''Activity''
		'
	END
	ELSE IF(@logFrameSelected = 4)
	BEGIN
		SET @query2 = @query2 + ' JOIN #Result r ON r.LogFrameId = ad.ActivityDataId 
		UPDATE #Result SET LogFrameType = ''Data''
		'
	END

	SET @query2 = @query2 + ' 
	SELECT * FROM #Result
	DROP TABLE #Result
	'
		
	PRINT @query1 + @query2
	EXEC (@query1 + @query2)
END

---------------------------------------------------------------------------------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetIndicatorsOfMultipleObjectives]    Script Date: 10/17/2013 17:06:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: 20 Sep, 2013
-- Description:	Get Indicators on passed objectives in parameter
-- =============================================
ALTER PROCEDURE [dbo].[GetIndicatorsOfMultipleObjectives]
	@ids VARCHAR(MAX)
AS
BEGIN
	-- interfering with SELECT statements.
	SET NOCOUNT ON;	
	SELECT * INTO #ids FROM dbo.fn_ParseCSVString (@ids, ',')

    -- Insert statements for procedure here
	SELECT	oi.ObjectiveIndicatorId,
			oi.IndicatorName
	FROM	ObjectiveIndicators oi JOIN ClusterObjectives co ON oi.ClusterObjectiveId = co.ClusterObjectiveId
	WHERE	oi.ClusterObjectiveId IN (SELECT * FROM #ids)
	ORDER BY oi.IndicatorName
END
----------------------------------------------------------------------------------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetActivitiesOfMultipleIndicators]    Script Date: 10/17/2013 17:10:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: 20 Sep, 2013
-- Description:	Get activities of indicators passed as parameters
-- =============================================
ALTER PROCEDURE [dbo].[GetActivitiesOfMultipleIndicators]
	@ids VARCHAR(MAX)
AS
BEGIN
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT * INTO #ids FROM dbo.fn_ParseCSVString (@ids, ',')

	SELECT	ia.IndicatorActivityId,
			ia.ActivityName
	FROM	IndicatorActivities ia JOIN ObjectiveIndicators oi ON ia.ObjectiveIndicatorId = oi.ObjectiveIndicatorId
	WHERE	oi.ObjectiveIndicatorId IN (SELECT * FROM #ids)
	ORDER BY ia.ActivityName
END
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetDatItemsOfMultipleActivities]    Script Date: 10/17/2013 17:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: 20 Sep, 2013
-- Description:	Get Data of activities passed as parameters
-- =============================================
ALTER PROCEDURE [dbo].[GetDatItemsOfMultipleActivities]
	@ids VARCHAR(MAX)
AS
BEGIN
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT * INTO #ids FROM dbo.fn_ParseCSVString (@ids, ',')
	
	SELECT	ad.ActivityDataId,
			ad.DataName
			
	FROM	ActivityData ad JOIN IndicatorActivities ia on ad.IndicatorActivityId = ia.IndicatorActivityId
	WHERE	ia.IndicatorActivityId IN (SELECT * FROM #ids)
	ORDER BY ad.DataName

END

---------------------------------------------------------------------------------------------------------------------------------------------------------------------USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetDataForChartsAndMaps]    Script Date: 10/17/2013 15:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Oct 7, 2013
-- Description:	Get Targets, Achieved and WorkDone(percentage)
--				of both of these ON passed parameters.
--				This procedure is written to fetch data FROM charts.
--				This should also work for map (TODO:)
-- =============================================
ALTER PROCEDURE [dbo].[GetDataForChartsAndMaps]
	@locationType INT = NULL,
	@locationIds VARCHAR(MAX),
	@clusterIds VARCHAR(100) = NULL,
	@organizationIds VARCHAR(MAX) = NULL,
	@fromYear INT = NULL,
	@fromMonth INT = NULL,
	@toYear INT = NULL,
	@toMonth INT = NULL,	
	@logFrameActual INT = NULL,
	@logFrameIds VARCHAR(MAX) = NULL,
	@durationType INT = NULL
AS
BEGIN
	
	SET NOCOUNT ON;
	
	IF(@organizationIds IS NULL)
	BEGIN
		SET @organizationIds = ''
	END
	
	IF(@logFrameIds IS NULL)
	BEGIN
		SET @logFrameIds = ''
	END	

    DECLARE @query1 VARCHAR(max) = ''
	DECLARE @query2 VARCHAR(max) = ''
	DECLARE @query3 VARCHAR(max) = ''

	-- Get YearId FROM year number passed as parameter
	DECLARE @fromYearId INT
	SELECT @fromYearId = YearId FROM Years WHERE [Year] = @fromYear
	DECLARE @toYearId INT
	SELECT @toYearId = YearId FROM Years WHERE [Year] = @toYear							

	SET @query1 = '
	
	CREATE TABLE #Result (	LocationId INT,
							Location NVARCHAR(200),
							LogFrameId INT,
							[Target] DECIMAL(18, 2),
							Achieved DECIMAL(18, 2),
							WorkDone INT,
							ClusterId INT,
							ClusterName NVARCHAR(250),
							ObjectiveId  INT,
							ObjectiveName NVARCHAR(4000),
							IndicatorId  INT,
							IndicatorName NVARCHAR(4000),
							ActivityId  INT,
							ActivityName NVARCHAR(4000),
							DataId  INT,
							DataName NVARCHAR(4000),							
							DurationType INT,
							DurationTypeName NVARCHAR(20),
							MonthId INT,
							MonthName NVARCHAR(30),
							QNumber INT,
							QName NVARCHAR(20),
							YearId INT,
							YearName INT
						)
	
	-- Get comma seperated values
	
	SELECT * INTO #locations FROM dbo.fn_ParseCSVString ( ''' + @locationIds + ''','','')
	SELECT * INTO #organizations FROM dbo.fn_ParseCSVString ( ''' + @organizationIds + ''','','')
	SELECT * INTO #logFrameIds FROM dbO.fn_ParseCSVString( ''' + @logFrameIds + ''','','')
    
    -- Get ActivityDataIds ON the basis of logFrameIs passed FROM parameters
    -- LogFrameType parameter will decide that what joins user need.
    -- 1 = Objective, 2 = Indicator, 3 = Activity, 4 = Data
    CREATE TABLE #DataId (dataId INT)		
    IF( ' + CONVERT(VARCHAR(5), @logFrameActual) + ' = 1)
    BEGIN
		INSERT INTO #DataId
		SELECT	ad.ActivityDataId
		FROM	ClusterObjectives co join ObjectiveIndicators oi ON co.ClusterObjectiveId = oi.ClusterObjectiveId
				JOIN IndicatorActivities ia ON oi.ObjectiveIndicatorId = ia.ObjectiveIndicatorId
				JOIN ActivityData ad ON ia.IndicatorActivityId = ad.IndicatorActivityId
				JOIN #logFrameIds lf ON lf.s = co.ClusterObjectiveId -- s here column name returned FROM fn.ParseCSVString
    END
    ELSE
    IF( ' + CONVERT(VARCHAR(5), @logFrameActual) + ' = 2)
    BEGIN
		INSERT INTO #DataId
		SELECT	ad.ActivityDataId
		FROM	ObjectiveIndicators oi JOIN IndicatorActivities ia ON oi.ObjectiveIndicatorId = ia.ObjectiveIndicatorId
				JOIN ActivityData ad ON ia.IndicatorActivityId = ad.IndicatorActivityId
				JOIN #logFrameIds lf ON lf.s = oi.ObjectiveIndicatorId -- s here column name returned FROM fn.ParseCSVString
    END
    ELSE IF( ' + CONVERT(VARCHAR(5), @logFrameActual) + ' = 3)
    BEGIN
		INSERT INTO #DataId
		SELECT	ad.ActivityDataId
		FROM	IndicatorActivities ia JOIN ActivityData ad ON
				ia.IndicatorActivityId = ad.IndicatorActivityId
				JOIN #logFrameIds lf ON lf.s = ia.IndicatorActivityId -- s here column name returned FROM fn.ParseCSVString
    END
    ELSE IF(' + CONVERT(VARCHAR(5), @logFrameActual) + ' = 4)
    BEGIN
		INSERT INTO #DataId
		SELECT * FROM #logFrameIds
    END    
    
    --select * from #DataId

	-- Recersively get all the child of locations in temp table #locations
	;WITH cte AS
	(
		SELECT LocationId, LocationParentId
		FROM Locations
		WHERE LocationId IN (SELECT * FROM #locations)
		UNION ALL
		SELECT l.locationid, l.locationparentid FROM locations l join cte c ON l.locationparentid = c.locationid
	)
	
	SELECT * INTO #temp FROM cte	
	
	-- #temp1 table.
	-- Apply all filters except locations and put data into this temp table.
	CREATE TABLE #temp1 (LocationId INT,
					 LocationName VARCHAR(50),
					 LocationParentId INT,
					 [Target] DECIMAL(18,2),
					 Achieved DECIMAL(18, 2),
					 DataId INT,
					 ActivityId INT,
					 IndicatorId INT,
					 ObjectiveId INT,
					 YearId INT,
					 MonthId INT,
					 QNumber INT
					 )

	INSERT	INTO #temp1
	SELECT	l.LocationId, 
			locationname AS Location, 
			l.LocationParentId, 
			rd.Target, 
			rd.Achieved,
			d.ActivityDataId,
			ia.IndicatorActivityId,
			oi.ObjectiveIndicatorId,
			co.ClusterObjectiveId,
			r.YearId,
			r.MonthId
			'
			
	IF (@durationType = 2)
	BEGIN
		SET @query1 = @query1 + ', Q.QNumber '
	END
	ELSE
	BEGIN
		SET @query1 = @query1 + ', 0 AS QNumber '
	END
			
	SET @query1 = @query1 + ' FROM	Reports r JOIN ReportLocations rl ON r.ReportId = rl.ReportId
			LEFT JOIN Reportdetails rd ON rl.ReportlocationId = rd.ReportlocationId
			LEFT JOIN ActivityData d ON rd.ActivityDataId = d.ActivityDataId
			JOIN IndicatorActivities ia ON ia.IndicatorActivityId = d.IndicatorActivityId
			JOIN ObjectiveIndicators oi ON oi.ObjectiveIndicatorId = ia.ObjectiveIndicatorId
			JOIN ClusterObjectives co ON co.ClusterObjectiveId = oi.ClusterObjectiveId
			JOIN Locations l ON rl.LocationId = l.LocationId
			JOIN #temp t ON l.Locationid = t.LocationId '
	-- If organizations are selected then add tables in resultset and then apply filters ON it.
	IF (@organizationIds != '''' OR @organizationIds IS NOT NULL)
	BEGIN
		SET @query1 = @query1 + '	JOIN Offices ofc ON r.OfficeId = ofc.OfficeId
									JOIN Organizations org ON ofc.OrganizationId = org.OrganizationId '
	END
	
	-- Duration is Monthly, Quarterly and Yearly.
	-- Monthly and Yearly is alredy in Reports table but we need to add Qurarterly table in resultset
	-- if user pass Quarterly FROM front end
	IF (@durationType = 2)
	BEGIN
		SET @query1 = @query1 + ' JOIN Quarters q ON Q.MonthId = r.MonthId '
	END
	
	SET @query1 = @query1 + '	WHERE	d.ActivityDataId IN (SELECT * FROM #DataId) '
	
	-- Filter data ON organizations, if it is passed by user FROM fron end
	IF (@organizationIds != '' AND @organizationids IS NOT NULL)
	BEGIN
		SET @query1 = @query1 + ' AND org.OrganizationId IN (SELECT * FROM #organizations) '
	END
	
	-- Filter ON Year
	IF (@fromYearId > 0 AND @toYearId > 0)
	BEGIN
		SET @query1 = @query1 + ' AND r.YearId >= ' + CONVERT(VARCHAR(2), @fromYearId) + ' AND r.YearId <= ' + CONVERT(VARCHAR(2), @toYearId)
	END
	ELSE IF (@fromYearId > 0 AND (@toYearId = 0 OR @toYearId IS NULL))
	BEGIN
		SET @query1 = @query1 + ' AND r.YearId >= ' + CONVERT(VARCHAR(2), @fromYearId)
	END
	ELSE IF (@toYearId > 0 AND (@fromYearId = 0 OR @fromYearId IS NULL))
	BEGIN
		SET @query1 = @query1 + ' AND r.YearId <= ' + CONVERT(VARCHAR(2), @toYearId)
	END
	
	-- Filter ON Months
	IF (@fromMonth IS NOT NULL AND @toMonth IS NOT NULL)
	BEGIN
		SET @query1 = @query1 + ' AND r.MonthId >= ' + CONVERT(VARCHAR(2), @fromMonth) + ' AND r.MonthId <= ' + CONVERT(VARCHAR(2), @toMonth)
	END
	ELSE IF (@fromMonth IS NOT NULL AND @toMonth IS NULL)
	BEGIN
		SET @query1 = @query1 + ' AND r.MonthId >= ' + CONVERT(VARCHAR(2), @fromMonth)
	END
	ELSE IF (@toMonth IS NOT NULL AND @fromMonth IS NULL)
	BEGIN
		SET @query1 = @query1 + ' AND r.MonthId <= ' + CONVERT(VARCHAR(2), @toMonth)
	END
	
	SET @query1 = @query1 + ' ORDER BY LocationName	

	UPDATE #temp1 SET Target = 0 WHERE Target IS NULL
	UPDATE #temp1 SET Achieved = 0 WHERE Achieved IS NULL
	
	--SELECT * FROM #temp1'
	SET @query2 = '
	IF(' + CONVERT(VARCHAR(10), @locationType) + ' = 1)
	BEGIN	
		INSERT INTO #Result
		(
			LocationId,
			Location,
			LogFrameId,
			[Target],
			Achieved,
			WorkDone '
			IF (@durationType = 1)
			BEGIN
				SET @query2 = @query2 + ',DurationType, YearId '
			END
			ELSE IF (@durationType = 2)
			BEGIN
				SET @query2 = @query2 + ',DurationType, YearId '
			END
			ELSE IF (@durationType = 3)
			BEGIN
				SET @query2 = @query2 + ',DurationType '
			END
				
		SET @query2 = @query2 + '	
		)
		SELECT	l2.LocationId
				,l2.Locationname AS Location 
				, DataId 
				, sum(target) AS Target
				, sum(achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone '
										
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId, YearId '
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QNumber, YearId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
		SET @query2 = @query2 + ' 		
		FROM	#temp1 t JOIN locations l ON t.locationparentid = l.locationid
				JOIN locations l2 ON l.locationparentid = l2.locationid
		GROUP BY l2.locationid, l2.locationname, DataId '
		
		IF (@durationType = 1)
		BEGIN
			SET @query2 = @query2 + ',MonthId, YearId '
		END
		ELSE IF (@durationType = 2)
		BEGIN
			SET @query2 = @query2 + ',Qnumber, YearId '
		END
		ELSE IF (@durationType = 3)
		BEGIN
			SET @query2 = @query2 + ',YearId '
		END
		
		SET @query2 = @query2 + '
		ORDER BY l2.LocationName
		
	END
	ELSE IF(' + CONVERT(VARCHAR(10), @locationType) + ' = 2)
	BEGIN
		INSERT INTO #Result
		(
			LocationId,
			Location,
			LogFrameId,
			[Target],
			Achieved,
			WorkDone '
			IF (@durationType = 1)
			BEGIN
				SET @query2 = @query2 + ',DurationType, YearId '
			END
			ELSE IF (@durationType = 2)
			BEGIN
				SET @query2 = @query2 + ',DurationType, YearId '
			END
			ELSE IF (@durationType = 3)
			BEGIN
				SET @query2 = @query2 + ',DurationType '
			END
				
		SET @query2 = @query2 + '	
		)
		SELECT	l.LocationId
				,l.Locationname AS Location
				,DataId 
				,SUM(Target) AS Target
				,SUM(Achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone '
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId, YearId'
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QNumber, YearId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				SET @query2 = @query2 + ' 
		FROM	#temp1 t JOIN Locations l ON t.LocationParentId = l.LocationId
		GROUP BY l.locationid, l.LocationName, DataId '
				
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId, YearId '
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QNumber, YearId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				
				SET @query2 = @query2 + ' 
		ORDER BY l.LocationName
		
	END
	ELSE IF (' + CONVERT(VARCHAR(10), @locationType) + ' = 3)
	BEGIN
		INSERT INTO #Result
		(
			LocationId,
			Location,
			LogFrameId,
			[Target],
			Achieved,
			WorkDone '
			IF (@durationType = 1)
			BEGIN
				SET @query2 = @query2 + ',DurationType, YearId '
			END
			ELSE IF (@durationType = 2)
			BEGIN
				SET @query2 = @query2 + ',DurationType, YearId '
			END
			ELSE IF (@durationType = 3)
			BEGIN
				SET @query2 = @query2 + ',DurationType '
			END
				
		SET @query2 = @query2 + '	
		)
		SELECT	LocationId
				,Locationname AS Location 
				,DataId 
				,SUM(Target) AS Target
				,SUM(Achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone '
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId, YearId'
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QNumber, YearId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				SET @query2 = @query2 + ' 
		FROM	#temp1
		WHERE	LocationId IN (SELECT LocationId FROM #temp)
		GROUP BY LocationId, LocationName, DataId '				
				
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId, YearId '
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QNumber, YearId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				
				SET @query3 = @query3 + ' 
		ORDER BY LocationName
	END

	DROP TABLE #locations
	DROP TABLE #organizations
	DROP TABLE #temp
	DROP TABLE #temp1
	DROP TABLE #DataId
	DROP TABLE #logFrameIds	
	
	UPDATE	#Result
	SET		ClusterId = c.ClusterId,
			ClusterName = c.ClusterName,
			ObjectiveId = co.ClusterObjectiveId,
			ObjectiveName = co.ObjectiveName,
			IndicatorId = oi.ObjectiveIndicatorId,
			IndicatorName = oi.IndicatorName,
			ActivityId = ia.IndicatorActivityId,
			ActivityName = ia.ActivityName,
			DataId = ad.ActivityDataId,
			DataName = ad.DataName
			
	FROM	ActivityData ad JOIN IndicatorActivities ia ON ad.IndicatorActivityId = ia.IndicatorActivityId
			JOIN ObjectiveIndicators oi ON oi.ObjectiveIndicatorId = ia.ObjectiveIndicatorId
			JOIN ClusterObjectives co ON co.ClusterObjectiveId = oi.ClusterObjectiveId
			JOIN EmergencyClusters ec ON ec.EmergencyClusterId = co.EmergencyClusterId
			JOIN Clusters c ON c.ClusterId = ec.ClusterId 
			JOIN #Result r ON r.LogFrameId = ad.ActivityDataId 			
			'
			
	IF(@durationType = 1)
	BEGIN
		SET @query3 = @query3 + ' UPDATE	#Result
		SET		DurationTypeName = ''Month'',
				MonthId = m.MonthId,
				[MonthName] = m.MonthName,
				YearName = y.Year
		FROM	#Result r JOIN Months m ON r.DurationType = m.MonthId
				JOIN Years y ON r.yearId = y.YearId '
	END
	ELSE IF(@durationType = 2)
	BEGIN
		SET @query3 = @query3 + ' UPDATE	#Result
		SET		DurationTypeName = ''Quarter'',
				QNumber = q.QNumber,
				QName = q.QName,
				YearName = y.Year
		FROM	#Result r JOIN Quarters q ON r.DurationType = q.QNumber 
				JOIN Years y ON r.yearId = y.YearId '
	END
	ELSE IF(@durationType = 3)
	BEGIN
		SET @query3 = @query3 + ' UPDATE	#Result
		SET		DurationTypeName = ''Year'',
				yearId = y.YearId,
				YearName = y.Year
		FROM	#Result r JOIN Years y ON r.DurationType = y.YearId '
	END

	SET @query3 = @query3 + ' 
	SELECT * FROM #Result
	ORDER BY Location, ClusterName, ObjectiveName, IndicatorName, ActivityName, DataName, YearId, DurationType
	DROP TABLE #Result
	'
		
	PRINT @query1 + @query2 + @query3
	EXEC (@query1 + @query2 + @query3)
END


------------------------------------------------------------------------------------------------------------------------------
UPDATE [Months] SET [MonthName] = 'January' WHERE MonthId = 1
UPDATE [Months] SET [MonthName] = 'February' WHERE MonthId = 2
UPDATE [Months] SET [MonthName] = 'March' WHERE MonthId = 3
UPDATE [Months] SET [MonthName] = 'April' WHERE MonthId = 4
UPDATE [Months] SET [MonthName] = 'May' WHERE MonthId = 5
UPDATE [Months] SET [MonthName] = 'June' WHERE MonthId = 6
UPDATE [Months] SET [MonthName] = 'July' WHERE MonthId = 7
UPDATE [Months] SET [MonthName] = 'August' WHERE MonthId = 8
UPDATE [Months] SET [MonthName] = 'September' WHERE MonthId = 9
UPDATE [Months] SET [MonthName] = 'October' WHERE MonthId = 10
UPDATE [Months] SET [MonthName] = 'November' WHERE MonthId = 11
UPDATE [Months] SET [MonthName] = 'Decmeber' WHERE MonthId = 12

------------------------------------------------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[InsertReport]    Script Date: 10/19/2013 15:03:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Feb 8, 2012
-- Description:	Save report
-- =============================================
ALTER PROCEDURE [dbo].[InsertReport]
	@reportName NVARCHAR(200)
   ,@oOfficeId INT   
   ,@yearId INT
   ,@monthId INT
   ,@locationEmergencyId INT
   ,@reportFrequencyTypeId INT   
   ,@createdById UNIQUEIDENTIFIER
   ,@UID int = NULL OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
DECLARE @InsertedRecords TABLE(id int)		
    
    INSERT INTO Reports
		(
           ReportName
           ,OfficeId           
           ,YearId
           ,MonthId
           ,LocationEmergencyId
           ,ReportFrequencyTypeId           
           ,CreatedById
		)
	OUTPUT INSERTED.ReportId INTO @InsertedRecords
     VALUES
		(
           @reportName
		   ,@oOfficeId		   
		   ,@yearId
		   ,@monthId
		   ,@locationEmergencyId
		   ,@reportFrequencyTypeId		   
		   ,@createdById
		)
		
		SELECT @UID = id FROM @InsertedRecords
END

----------------------------------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetTargetAchievedByLocation]    Script Date: 10/19/2013 16:03:14 ******/
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
		ORDER BY l2.LocationName
		
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
		ORDER BY l.LocationName
		
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
		ORDER BY LocationName
	END

	DROP TABLE #locations
	DROP TABLE #temp
	DROP TABLE #temp1
    
END

--------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetTargetAchievedOfLocationByQuarterly]    Script Date: 10/19/2013 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetTargetAchievedOfLocationByQuarterly]
	@locationIds VARCHAR(MAX)
	,@locationType INT	
	,@dataId INT = NULL
	,@yearId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	-- TODO: divide by zeor in percentage
	
	SELECT * INTO #locations FROM dbo.fn_ParseCSVString (@locationIds, ',')
	CREATE TABLE #temp1 (LocationId INT,
						 LocationName NVARCHAR(150),
						 LocationParentId INT,
						 QId INT,
						 QName NVARCHAR(10),
						 [Target] DECIMAL(18,2),
						 Achieved DECIMAL(18, 2))

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
			q.QNumber,
			q.QName,
			rd.Target, 
			rd.Achieved
			
	FROM	Reports r JOIN ReportLocations rl ON r.ReportId = rl.ReportId
			LEFT JOIN Reportdetails rd ON rl.ReportlocationId = rd.ReportlocationId			
			LEFT JOIN ActivityData d on rd.ActivityDataId = d.ActivityDataId
			JOIN Locations l ON rl.LocationId = l.LocationId
			JOIN Months m ON m.MonthId = r.MonthId
			JOIN Quarters q ON Q.MonthId = m.MonthId
			
	WHERE	l.LocationId IN (SELECT LocationId FROM #temp)	
	AND		(d.ActivityDataId = @dataId OR @dataId IS NULL)
	AND		(r.YearId = @yearId OR @yearId IS NULL)
	
	ORDER BY LocationName

	UPDATE #temp1 SET Target = 0 WHERE Target IS NULL
	UPDATE #temp1 SET Achieved = 0 WHERE Achieved IS NULL

	IF(@locationType = 1)
	BEGIN
	
		SELECT	l2.LocationId
				,l2.Locationname AS Location
				, QId
				, QName
				, sum(target) AS Target
				, sum(achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
				
		FROM	#temp1 t JOIN locations l ON t.locationparentid = l.locationid
				JOIN locations l2 ON l.locationparentid = l2.locationid				
		GROUP BY l2.locationid, l2.locationname, QId, QName
		ORDER BY QName, l2.LocationName
		
	END
	ELSE IF(@locationType = 2)
	BEGIN
	
		SELECT	l.LocationId
				,l.Locationname ASLocation
				, QId
				, QName
				, sum(target) AS Target
				, sum(achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
		FROM	#temp1 t JOIN Locations l ON t.LocationParentId = l.LocationId
		GROUP BY l.locationid, l.LocationName, QId, QName
		ORDER BY QName, l.LocationName
		
	END
	ELSE IF (@locationType = 3)
	BEGIN
	
		SELECT	LocationId
				,Locationname AS Location
				, QId
				, QName
				,sum(target) AS Target
				,sum(achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
		FROM	#temp1
		WHERE	LocationId IN (SELECT LocationId FROM #temp)
		GROUP BY LocationId, LocationName, QId, QName
		ORDER BY QName, LocationName
	END

	DROP TABLE #locations
	DROP TABLE #temp
	DROP TABLE #temp1
    
END

------------------------------------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetTargetAchievedOfLocationByMonthly]    Script Date: 10/19/2013 16:04:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetTargetAchievedOfLocationByMonthly]
	@locationIds VARCHAR(MAX)
	,@locationType INT
	,@dataId INT = NULL
	,@yearId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	-- TODO: divide by zeor in percentage
	
	SELECT * INTO #locations FROM dbo.fn_ParseCSVString (@locationIds, ',')
	CREATE TABLE #temp1 (LocationId INT,
						 LocationName NVARCHAR(150),
						 LocationParentId INT,
						 MonthId INT,
						 [MonthName] NVARCHAR(10),
						 [Target] DECIMAL(18,2),
						 Achieved DECIMAL(18, 2))

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
			m.MonthId,
			[MonthName],
			rd.Target, 
			rd.Achieved
			
	FROM	Reports r JOIN ReportLocations rl ON r.ReportId = rl.ReportId
			LEFT JOIN Reportdetails rd ON rl.ReportlocationId = rd.ReportlocationId
			LEFT JOIN ActivityData d on rd.ActivityDataId = d.ActivityDataId
			JOIN Locations l ON rl.LocationId = l.LocationId
			JOIN Months m ON m.MonthId = r.MonthId
			
	WHERE	l.LocationId IN (SELECT LocationId FROM #temp)
	AND		(d.ActivityDataId = @dataId OR @dataId IS NULL)
	AND		(r.YearId = @yearId OR @yearId IS NULL)
	ORDER BY LocationName

	UPDATE #temp1 SET Target = 0 WHERE Target IS NULL
	UPDATE #temp1 SET Achieved = 0 WHERE Achieved IS NULL

	IF(@locationType = 1)
	BEGIN
	
		SELECT	l2.LocationId
				,l2.Locationname AS Location
				,MonthId
				,[MonthName]
				, sum(target) AS Target
				, sum(achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
				
		FROM	#temp1 t JOIN locations l ON t.locationparentid = l.locationid
				JOIN locations l2 ON l.locationparentid = l2.locationid				
		GROUP BY l2.locationid, l2.locationname, MonthId, [MonthName]
		ORDER BY l2.LocationName
		
	END
	ELSE IF(@locationType = 2)
	BEGIN
	
		SELECT	l.LocationId
				,l.Locationname ASLocation
				,MonthId
				,[MonthName]
				, sum(target) AS Target
				, sum(achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
		FROM	#temp1 t JOIN Locations l ON t.LocationParentId = l.LocationId
		GROUP BY l.locationid, l.LocationName, MonthId, [MonthName]
		ORDER BY l.LocationName
		
	END
	ELSE IF (@locationType = 3)
	BEGIN
	
		SELECT	LocationId
				,Locationname AS Location
				,MonthId
				,[MonthName]
				, sum(target) AS Target
				, sum(achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
		FROM	#temp1
		WHERE	LocationId IN (SELECT LocationId FROM #temp)
		GROUP BY LocationId, LocationName, MonthId, [MonthName]
		ORDER BY LocationName
	END

	DROP TABLE #locations
	DROP TABLE #temp
	DROP TABLE #temp1
    
END
-------------------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetTargetAchievedOfLocationByYearly]    Script Date: 10/19/2013 16:05:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetTargetAchievedOfLocationByYearly]
	@locationIds VARCHAR(MAX)
	,@locationType INT
	,@dataId INT = NULL
	,@yearId INT = NULL	
AS
BEGIN
	SET NOCOUNT ON;

	-- TODO: divide by zeor in percentage
	
	SELECT * INTO #locations FROM dbo.fn_ParseCSVString (@locationIds, ',')
	CREATE TABLE #temp1 (LocationId INT,
						 LocationName NVARCHAR(150),
						 LocationParentId INT,
						 YearId INT,
						 YearName NVARCHAR(10),
						 [Target] DECIMAL(18,2),
						 Achieved DECIMAL(18, 2))

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
			y.YearId,
			y.Year,
			rd.Target, 
			rd.Achieved
			
	FROM	Reports r JOIN ReportLocations rl ON r.ReportId = rl.ReportId
			LEFT JOIN Reportdetails rd ON rl.ReportlocationId = rd.ReportlocationId
			LEFT JOIN ActivityData d on rd.ActivityDataId = d.ActivityDataId			
			JOIN Locations l ON rl.LocationId = l.LocationId
			JOIN Years y ON y.YearId = r.YearId
			
	WHERE	l.LocationId IN (SELECT LocationId FROM #temp)	
	AND		(d.ActivityDataId = @dataId OR @dataId IS NULL)
	ORDER BY LocationName

	UPDATE #temp1 SET Target = 0 WHERE Target IS NULL
	UPDATE #temp1 SET Achieved = 0 WHERE Achieved IS NULL

	IF(@locationType = 1)
	BEGIN
	
		SELECT	l2.LocationId
				,l2.Locationname AS Location
				,YearId
				,YearName
				, sum(target) AS Target
				, sum(achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
				
		FROM	#temp1 t JOIN locations l ON t.locationparentid = l.locationid
				JOIN locations l2 ON l.locationparentid = l2.locationid				
		GROUP BY l2.locationid, l2.locationname, YearId ,YearName
		ORDER BY YearName, l2.LocationName
		
	END
	ELSE IF(@locationType = 2)
	BEGIN
	
		SELECT	l.LocationId
				,l.Locationname ASLocation
				,YearId
				,YearName
				, sum(target) AS Target
				, sum(achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
		FROM	#temp1 t JOIN Locations l ON t.LocationParentId = l.LocationId
		GROUP BY l.locationid, l.LocationName, YearId ,YearName
		ORDER BY YearName, l.LocationName
		
	END
	ELSE IF (@locationType = 3)
	BEGIN
	
		SELECT	LocationId
				,Locationname AS Location
				,YearId
				,YearName
				,sum(target) AS Target
				,sum(achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
		FROM	#temp1
		WHERE	LocationId IN (SELECT LocationId FROM #temp)
		GROUP BY LocationId, LocationName, YearId ,YearName
		ORDER BY YearName, LocationName
	END

	DROP TABLE #locations
	DROP TABLE #temp
	DROP TABLE #temp1
    
END

-------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetTargetAndAchievedByMonthAndLocationPivot_t]    Script Date: 10/19/2013 16:06:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[GetTargetAndAchievedByMonthAndLocationPivot_t]
	@locationIds VARCHAR(MAX) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from	
	-- interfering with SELECT statements.
	
	--set @locationIds = '21,30,31'

	SELECT * INTO #locations FROM dbo.fn_ParseCSVString (@locationIds, ',')
	
	SELECT	l.LocationId, l.locationname as Location, 
			SUM(ISNULL(rd.Target, 0)) AS 'T', SUM(ISNULL(rd.Achieved, 0)) AS 'A' 

	FROM	Reports r JOIN ReportLocations rl ON r.ReportId = rl.ReportId
			LEFT JOIN Reportdetails rd ON rl.ReportlocationId = rd.ReportlocationId
			JOIN Locations l ON rl.LocationId = l.LocationId
			WHERE l.LocationId IN (SELECT * FROM #locations) OR @locationIds IS NULL
			GROUP BY l.LocationId, l.LocationName
			ORDER BY l.LocationName
END
----------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetAllTasksDataReport2]    Script Date: 10/19/2013 16:06:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Feb 19, 2013
-- Description:	Get all data for report
-- =============================================
ALTER PROCEDURE [dbo].[GetAllTasksDataReport2]
	@emergencyIds VARCHAR(500) = NULL,
	@officeId INT = NULL,
	@userId UNIQUEIDENTIFIER = NULL,
	@yearId INT = NULL,
	@monthIds VARCHAR(100) = NULL,
	@locationIds VARCHAR(MAX) = NULL,
	@clusterIds VARCHAR(4000) = NULL,	
	@orgIds VARCHAR(MAX) = NULL,	
	@orgTypeIds VARCHAR(1000) = NULL,
	@pageIndex INT = NULL,
	@pageSize INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from	
	-- interfering with SELECT statements.
	
	SELECT * INTO #emergency FROM dbo.fn_ParseCSVString (@emergencyIds, ',')
	SELECT * INTO #months FROM dbo.fn_ParseCSVString (@monthIds, ',')
	SELECT * INTO #locations FROM dbo.fn_ParseCSVString (@locationIds, ',')
	SELECT * INTO #clusters FROM dbo.fn_ParseCSVString (@clusterIds, ',')
	SELECT * INTO #orgs FROM dbo.fn_ParseCSVString (@orgIds, ',')
	SELECT * INTO #orgTypes FROM dbo.fn_ParseCSVString (@orgTypeIds, ',')
	
	SET NOCOUNT ON;
	
	DECLARE @start INT = @pageIndex
	DECLARE @end INT = @pageSize	
	IF(@pageIndex > 1)
	BEGIN
		SET @start = ((@pageSize * @pageIndex) - @pageSize) + 1
		SET @end = @pageSize * @pageIndex
	END
	
	SET @pageSize = @pageIndex

	;WITH cte as
	(
		SELECT	ROW_NUMBER() OVER (ORDER BY rd.ReportDetailId) AS rnumber,
				rd.ActivityDataId AS DataId, 
				e.EmergencyName AS Emergency,
				org.OrganizationName AS Organization,
				org.OrganizationAcronym,  
				o.Officename AS Office, 
				y.Year, 
				m.MonthName AS Month, 
				c.ClusterName AS Cluster, 
				co.ObjectiveName AS Objective, 
				oi.IndicatorName AS Indicator, 
				ia.ActivityName AS Activity, 
				ac.ActivityType,
				ad.DataName AS Data,
				l2.LocationName AS '(AD1)Location',
				l2.LocationPCode AS '(AD1)PCode',
				l.LocationName AS '(Ad2)Location',
				l.LocationPCode AS '(Ad2)PCode',
				cast(cast(rd.Target as DECIMAL(18,2)) as float) As Target,
				cast(cast(rd.Achieved as DECIMAL(18,2)) as float) As Achieved,
				u.UserName,
				um.Email
				,CONVERT(VARCHAR(10),
						DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,CAST(
						  CAST(y.Year AS VARCHAR(4)) +
						  RIGHT('0' + CAST(m.MonthId AS VARCHAR(2)), 2) +
						  RIGHT('0' + CAST(1 AS VARCHAR(2)), 2) 
					   AS DATETIME))+1,0)), 103) AS ReportDate,
				un.Unit
				
		FROM	Reports r JOIN ReportLocations rl ON r.ReportId = rl.ReportId
		LEFT	JOIN Reportdetails rd ON rl.ReportlocationId = rd.ReportlocationId
		JOIN	Locations l ON rl.LocationId = l.LocationId
		JOIN	Years y ON r.YearId = y.YearId
		JOIN	Months m ON r.MonthId = m.MonthId
		JOIN	Offices o ON r.OfficeId= o.OfficeId
		JOIN	ActivityData ad ON ad.ActivityDataId = rd.ActivityDataId
		JOIN	IndicatorActivities ia ON ad.IndicatorActivityId = ia.IndicatorActivityId
		JOIN	ObjectiveIndicators oi ON ia.ObjectiveIndicatorid = oi.ObjectiveIndicatorId
		JOIN	ClusterObjectives co ON oi.ClusterObjectiveId = co.ClusterObjectiveId
		JOIN	EmergencyClusters ec ON ec.EmergencyClusterId = co.EmergencyClusterId
		JOIN	LocationEmergencies le ON ec.LocationEmergencyId = le.LocationEmergencyId
		JOIN	Emergency e ON le.EmergencyId = e.EmergencyId
		JOIN	Clusters c ON ec.ClusterId = c.ClusterId
		JOIN	aspnet_Users u ON u.UserId = r.CreatedById
		JOIN	aspnet_Membership um ON u.UserId = um.UserId
		JOIN	Organizations org ON org.OrganizationId = o.OrganizationId
		JOIN	OrganizationTypes ot ON org.OrganizationTypeId = ot.OrganizationTypeId
		JOIN	Locations l2 on l2.LocationId = l.LocationParentId
		LEFT	JOIN ActivityTypes ac ON ia.ActivityTypeId = ac.ActivityTypeId
		LEFT	JOIN Units un ON ad.UnitId = un.UnitId
		
		WHERE	(e.EmergencyId IN( SELECT * FROM #emergency) OR @emergencyIds IS NULL)
		AND		(o.OfficeId = @officeId OR @officeId IS NULL)
		AND		(u.UserId = @userId OR @userId = '00000000-0000-0000-0000-000000000000')
		AND		(y.YearId = @yearId OR @yearId IS NULL)
		AND		(m.MonthId IN (SELECT * FROM #months) OR @monthIds IS NULL)
		AND		(l.LocationId IN (SELECT * FROM #locations) OR @locationIds IS NULL)
		AND		(c.ClusterId IN (SELECT * FROM #clusters) OR @clusterIds IS NULL)
		AND		(org.OrganizationId IN (SELECT * FROM #orgs) OR @orgIds IS NULL)
		AND		(ot.OrganizationTypeID IN (SELECT * FROM #orgTypes) OR @orgTypeIds IS NULL)
		
		
	)	
	
	  
	SELECT * FROM CTE WHERE	rnumber BETWEEN @start AND @end
	
	DROP TABLE #clusters
	DROP TABLE #emergency
	DROP TABLE #months
	DROP TABLE #locations
	DROP TABLE #orgs
	DROP TABLE #orgTypes
END

------------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetTargetAndAchievedByMonthAndLocationPivot]    Script Date: 10/19/2013 16:07:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[GetTargetAndAchievedByMonthAndLocationPivot]
	@emergencyIds VARCHAR(500) = NULL,
	@officeId INT = NULL,
	@userId UNIQUEIDENTIFIER = NULL,
	@yearId INT = NULL,
	@monthIds VARCHAR(100) = NULL,
	@locationIds VARCHAR(MAX) = NULL,
	@clusterIds VARCHAR(4000) = NULL,	
	@orgIds VARCHAR(4000) = NULL,	
	@orgTypeIds VARCHAR(1000) = NULL
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from	
	-- interfering with SELECT statements.
	
	SELECT * INTO #emergency FROM dbo.fn_ParseCSVString (@emergencyIds, ',')
	SELECT * INTO #months FROM dbo.fn_ParseCSVString (@monthIds, ',')
	SELECT * INTO #locations FROM dbo.fn_ParseCSVString (@locationIds, ',')
	SELECT * INTO #clusters FROM dbo.fn_ParseCSVString (@clusterIds, ',')
	SELECT * INTO #orgs FROM dbo.fn_ParseCSVString (@orgIds, ',')
	SELECT * INTO #orgTypes FROM dbo.fn_ParseCSVString (@orgTypeIds, ',')

select l.LocationId, m.MonthId, m.monthname as Month, l.locationname as Location, 
	SUM(ISNULL(rd.Target, 0)) AS 'T', SUM(ISNULL(rd.Achieved, 0)) AS 'A' 
into #temp
FROM	Reports r JOIN ReportLocations rl ON r.ReportId = rl.ReportId
		LEFT	JOIN Reportdetails rd ON rl.ReportlocationId = rd.ReportlocationId
		JOIN	Locations l ON rl.LocationId = l.LocationId
		JOIN	Years y ON r.YearId = y.YearId
		JOIN	Months m ON r.MonthId = m.MonthId
		JOIN	Offices o ON r.OfficeId= o.OfficeId
		JOIN	ActivityData ad ON ad.ActivityDataId = rd.ActivityDataId
		JOIN	IndicatorActivities ia ON ad.IndicatorActivityId = ia.IndicatorActivityId
		JOIN	ObjectiveIndicators oi ON ia.ObjectiveIndicatorid = oi.ObjectiveIndicatorId
		JOIN	ClusterObjectives co ON oi.ClusterObjectiveId = co.ClusterObjectiveId
		JOIN	EmergencyClusters ec ON ec.EmergencyClusterId = co.EmergencyClusterId
		JOIN	LocationEmergencies le ON ec.LocationEmergencyId = le.LocationEmergencyId
		JOIN	Emergency e ON le.EmergencyId = e.EmergencyId
		JOIN	Clusters c ON ec.ClusterId = c.ClusterId
		JOIN	aspnet_Users u ON u.UserId = r.CreatedById
		JOIN	aspnet_Membership um ON u.UserId = um.UserId
		JOIN	Organizations org ON org.OrganizationId = o.OrganizationId
		JOIN	OrganizationTypes ot ON org.OrganizationTypeId = ot.OrganizationTypeId
		JOIN	Locations l2 on l2.LocationId = l.LocationParentId
		LEFT	JOIN ActivityTypes ac ON ia.ActivityTypeId = ac.ActivityTypeId
		LEFT	JOIN Units un ON ad.UnitId = un.UnitId
		WHERE	(e.EmergencyId IN( SELECT * FROM #emergency) OR @emergencyIds IS NULL)
		AND		(o.OfficeId = @officeId OR @officeId IS NULL)
		AND		(u.UserId = @userId OR @userId = '00000000-0000-0000-0000-000000000000')
		AND		(y.YearId = @yearId OR @yearId IS NULL)
		AND		(m.MonthId IN (SELECT * FROM #months) OR @monthIds IS NULL)
		AND		(l.LocationId IN (SELECT * FROM #locations) OR @locationIds IS NULL)
		AND		(c.ClusterId IN (SELECT * FROM #clusters) OR @clusterIds IS NULL)
		AND		(org.OrganizationId IN (SELECT * FROM #orgs) OR @orgIds IS NULL)
		AND		(ot.OrganizationTypeID IN (SELECT * FROM #orgTypes) OR @orgTypeIds IS NULL)
		GROUP BY m.MonthId, m.MonthName, l.LocationId, l.LocationName
		ORDER BY m.MonthId

DECLARE @cols AS NVARCHAR(MAX)
DECLARE @query AS NVARCHAR(MAX)

SELECT @cols = STUFF((SELECT ',' + QUOTENAME(Location+' '+t.tasks) 
						FROM #temp
						cross apply
						(
						  SELECT 'T' tasks
						  union all
						  SELECT 'A'
						) t
						group by Location, LocationId, tasks
						order by location, tasks desc
				FOR XML PATH(''), TYPE
				).value('.', 'NVARCHAR(MAX)') 
			,1,1,'')

	set @query = ';WITH unpiv AS
				  (
					SELECT MonthId, Month, 
						Location+'' ''+col AS col, 
						value
					  FROM
					  (
						SELECT MonthId, Month,
						  CAST(ISNULL(T,0) AS VARCHAR(50)) T,
						  CAST(ISNULL(A,0) AS VARCHAR(50)) A,
						  LocationId,
						  Location
						  
						FROM #temp
					  ) src
					  unpivot
					  (
						value
						for col in (T, A)
					  ) unpiv
				  ),
				  piv AS
				  (
					SELECT MonthId, Month,
					  
					  '+@cols+'
					FROM unpiv
					pivot
					(
					  max(value)
					  for col in ('+@cols+')
					) piv
				  )
				  SELECT Month,
					'+@cols+'
	  
				  FROM piv
				  Order By MonthId
				  '


execute(@query)

drop table #temp
end

-------------------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetIPData1]    Script Date: 10/19/2013 16:11:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[GetIPData1]
	@locEmergencyId INT,
	@locationIds VARCHAR(1000),
	@officeID INT,
	@yearId INT,
	@monthId INT,
	@locIdsNotIncluded VARCHAR(1000)
AS
BEGIN

	IF(@officeId > 0 AND @yearId > 0 AND @monthId > 0)
	BEGIN

		DECLARE @cols AS NVARCHAR(MAX)
		DECLARE @query AS NVARCHAR(MAX)

		DECLARE @loc TABLE (LocId INT, LocName NVARCHAR(150))
		INSERT INTO @loc(LocId)
		SELECT * FROM dbo.fn_ParseCSVString (@locationIds, ',')
		UNION	
		SELECT rl.LocationId FROM Reports r JOIN ReportLocations rl ON r.ReportId = rl.ReportId
		WHERE	OfficeId = @officeId 
		AND		YearId = @yearId
		AND		MonthId = @monthId

		DELETE FROM @loc WHERE LocId  IN (SELECT * FROM dbo.fn_ParseCSVString (@locIdsNotIncluded, ','))
		
		--select * From @loc
		
		UPDATE @loc SET LocName = l.LocationName
		FROM @loc lt JOIN Locations l ON lt.LocId = l.LocationId

		-- Create temp table to populate all the data and then we will update this table with T,A and other Ids.
		CREATE table #temp (ReportId INT, ClusterName NVARCHAR(100), IndicatorName NVARCHAR(500), ActivityName NVARCHAR(500), 
							DataName NVARCHAR(4000), T DECIMAL(18,2), A DECIMAL(18,2), 
							ActivityDataId INT, IsActive bit, Location VARCHAR(20),
							LocId INT, ReportLocationId INT)

		INSERT	INTO #temp(	ClusterName, IndicatorName, ActivityName, DataName, 
							ActivityDataId, IsActive, Location, LocId)
							
		SELECT	c.ClusterShortName, oi.IndicatorName, ActivityName, DataName + '  (' + ISNULL(Unit, '') + ')', 
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
		WHERE le.LocationEmergencyId= @locEmergencyId

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
		
		UPDATE #temp SET ReportId = (SELECT TOP 1 ReportId FROM #temp2)
		UPDATE #temp SET ReportLocationId = (SELECT TOP 1 ReportId FROM #temp2)	

		UPDATE	#temp SET T = t2.Target, A = t2.Achieved 
		FROM	#temp t JOIN #temp2 t2 ON t.ActivityDataId = t2.ActivityDataId
		JOIN	@loc l ON t.LocId = l.LocId AND t2.LocationId = l.LocId

		UPDATE	#temp SET IsActive = ISNULL(t2.IsActive, 0)
		FROM	#temp t JOIN #temp2 t2 ON t.ActivityDataId = t2.ActivityDataId

		--select * from #temp
		--print @locationIds
		--select * from #temp2
		IF EXISTS( SELECT 1 FROM #temp WHERE ReportId IS NOT NULL OR @locationIds != '')
		BEGIN
		SELECT @cols = STUFF((SELECT ',' + QUOTENAME(CAST(locid AS VARCHAR(5))+'^'+Location+'_'+t.tasks) 
							FROM #temp
							cross apply
							(
							  SELECT 'T' tasks
							  union all
							  SELECT 'A'
							) t
							group by locid,location, tasks
							order by location
					FOR XML PATH(''), TYPE
					).value('.', 'NVARCHAR(MAX)') 
				,1,1,'')

		set @query = ';WITH unpiv AS
					  (
						SELECT IsActive, ReportId, ClusterName, IndicatorName, ActivityDataId, ActivityName, 
							DataName, 
							CAST(locid AS VARCHAR(5))+''^''+Location+''_''+col AS col, 
							value
						  FROM
						  (
							SELECT IsActive, ReportId, ClusterName, IndicatorName, ActivityDataId, ActivityName,
							  DataName,
							  CAST(ISNULL(T, -1) AS VARCHAR(50)) T,
							  CAST(ISNULL(A, -1 ) AS VARCHAR(50)) A,
							  Location,
							  locid
							FROM #temp
						  ) src
						  unpivot
						  (
							value
							for col in (T, A)
						  ) unpiv
					  ),
					  piv AS
					  (
						SELECT IsActive, ReportId, ClusterName, IndicatorName, ActivityDataId, ActivityName,
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
					  SELECT IsActive, ReportId, ClusterName, IndicatorName, ActivityDataId, case when rn = 1 then ActivityName else ActivityName end ActivityName,
						DataName,
						'+@cols+'
		                          
					  FROM piv
					  Order By ClusterName, IndicatorName, ActivityName, DataName
					  '

			execute(@query)
			END
			drop table #temp
			drop table #temp2
		END	
end

-----------------------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetAllTasksDataReport]    Script Date: 10/19/2013 16:12:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Feb 19, 2013
-- Description:	Get all data for report
-- =============================================
ALTER PROCEDURE [dbo].[GetAllTasksDataReport]
	@emergencyIds VARCHAR(500) = NULL,
	@officeId INT = NULL,
	@userId UNIQUEIDENTIFIER = NULL,
	@yearId INT = NULL,
	@monthIds VARCHAR(100) = NULL,
	@locationIds VARCHAR(MAX) = NULL,
	@clusterIds VARCHAR(4000) = NULL,	
	@orgIds VARCHAR(2000) = NULL,	
	@orgTypeIds VARCHAR(1000) = NULL,
	@pageIndex INT = NULL,
	@pageSize INT = NULL,
	@allowPaging INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from	
	-- interfering with SELECT statements.
	
	SELECT * INTO #emergency FROM dbo.fn_ParseCSVString (@emergencyIds, ',')
	SELECT * INTO #months FROM dbo.fn_ParseCSVString (@monthIds, ',')
	SELECT * INTO #locations FROM dbo.fn_ParseCSVString (@locationIds, ',')
	SELECT * INTO #clusters FROM dbo.fn_ParseCSVString (@clusterIds, ',')
	SELECT * INTO #orgs FROM dbo.fn_ParseCSVString (@orgIds, ',')
	SELECT * INTO #orgTypes FROM dbo.fn_ParseCSVString (@orgTypeIds, ',')
	
	SET NOCOUNT ON;
	
	DECLARE @start INT = @pageIndex
	DECLARE @end INT = @pageSize	
	
	SET @pageIndex += 1	
	IF(@pageIndex > 1)
	BEGIN
		SET @start = ((@pageSize * @pageIndex) - @pageSize) + 1
		SET @end = @pageSize * @pageIndex
	END
	
	SET @pageSize = @pageIndex

	;WITH cte as
	(
		SELECT	ROW_NUMBER() OVER (ORDER BY rd.ReportDetailId) AS rnumber,
				rd.ActivityDataId AS DataId, 
				e.EmergencyName AS Emergency,
				org.OrganizationName AS Organization,
				org.OrganizationAcronym,  
				o.Officename AS Office, 
				y.Year, 
				m.MonthName AS Month, 
				c.ClusterName AS Cluster, 
				co.ObjectiveName AS Objective, 
				oi.IndicatorName AS Indicator, 
				ia.ActivityName AS Activity, 
				ac.ActivityType,
				ad.DataName AS Data,
				l2.LocationName AS '(AD1)Location',
				l2.LocationPCode AS '(AD1)PCode',
				l.LocationName AS '(Ad2)Location',
				l.LocationPCode AS '(Ad2)PCode',
				cast(cast(rd.Target as DECIMAL(18,2)) as float) As Target,
				cast(cast(rd.Achieved as DECIMAL(18,2)) as float) As Achieved,
				u.UserName,
				um.Email
				,CONVERT(VARCHAR(10),
						DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,CAST(
						  CAST(y.Year AS VARCHAR(4)) +
						  RIGHT('0' + CAST(m.MonthId AS VARCHAR(2)), 2) +
						  RIGHT('0' + CAST(1 AS VARCHAR(2)), 2) 
					   AS DATETIME))+1,0)), 103) AS ReportDate,
				un.Unit,
				0 AS cnt
				
		FROM	Reports r JOIN ReportLocations rl ON r.ReportId = rl.ReportId
		LEFT	JOIN Reportdetails rd ON rl.ReportlocationId = rd.ReportlocationId
		JOIN	Locations l ON rl.LocationId = l.LocationId
		JOIN	Years y ON r.YearId = y.YearId
		JOIN	Months m ON r.MonthId = m.MonthId
		JOIN	Offices o ON r.OfficeId= o.OfficeId
		JOIN	ActivityData ad ON ad.ActivityDataId = rd.ActivityDataId
		JOIN	IndicatorActivities ia ON ad.IndicatorActivityId = ia.IndicatorActivityId
		JOIN	ObjectiveIndicators oi ON ia.ObjectiveIndicatorid = oi.ObjectiveIndicatorId
		JOIN	ClusterObjectives co ON oi.ClusterObjectiveId = co.ClusterObjectiveId
		JOIN	EmergencyClusters ec ON ec.EmergencyClusterId = co.EmergencyClusterId
		JOIN	LocationEmergencies le ON ec.LocationEmergencyId = le.LocationEmergencyId
		JOIN	Emergency e ON le.EmergencyId = e.EmergencyId
		JOIN	Clusters c ON ec.ClusterId = c.ClusterId
		JOIN	aspnet_Users u ON u.UserId = r.CreatedById
		JOIN	aspnet_Membership um ON u.UserId = um.UserId
		JOIN	Organizations org ON org.OrganizationId = o.OrganizationId
		JOIN	OrganizationTypes ot ON org.OrganizationTypeId = ot.OrganizationTypeId
		JOIN	Locations l2 on l2.LocationId = l.LocationParentId
		LEFT	JOIN ActivityTypes ac ON ia.ActivityTypeId = ac.ActivityTypeId
		LEFT	JOIN Units un ON ad.UnitId = un.UnitId
		
		WHERE	(e.EmergencyId IN( SELECT * FROM #emergency) OR @emergencyIds IS NULL)
		AND		(o.OfficeId = @officeId OR @officeId IS NULL)
		AND		(u.UserId = @userId OR @userId = '00000000-0000-0000-0000-000000000000')
		AND		(y.YearId = @yearId OR @yearId IS NULL)
		AND		(m.MonthId IN (SELECT * FROM #months) OR @monthIds IS NULL)
		AND		(l.LocationId IN (SELECT * FROM #locations) OR @locationIds IS NULL)
		AND		(c.ClusterId IN (SELECT * FROM #clusters) OR @clusterIds IS NULL)
		AND		(org.OrganizationId IN (SELECT * FROM #orgs) OR @orgIds IS NULL)
		AND		(ot.OrganizationTypeID IN (SELECT * FROM #orgTypes) OR @orgTypeIds IS NULL)
	)	

	SELECT	* INTO #tbl FROM CTE
	UPDATE	#tbl SET Cnt = (SELECT COUNT(*) FROM #tbl)
	
	-- If paging is allowed then filter on start index and pagesize
	-- otherwise return all the restulset.
	IF(@allowPaging = 1)
	BEGIN
		SELECT	* FROM #tbl
		WHERE	rnumber BETWEEN @start AND @end
	END
	ELSE
	BEGIN
		SELECT	* FROM #tbl
	END
	
	DROP TABLE #tbl
	DROP TABLE #clusters
	DROP TABLE #emergency
	DROP TABLE #months
	DROP TABLE #locations
	DROP TABLE #orgs
	DROP TABLE #orgTypes
END
---------------------------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetAllReportData]    Script Date: 10/19/2013 16:13:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[GetAllReportData]
	@locEmergencyId INT,
	@locationIds VARCHAR(MAX),
	@officeID INT,
	@yearId INT,
	@monthId INT,
	@locIdsNotIncluded VARCHAR(MAX)
AS
BEGIN

	IF(@officeId > 0 AND @yearId > 0 AND @monthId > 0)
	BEGIN

		DECLARE @cols AS NVARCHAR(MAX)
		DECLARE @query AS NVARCHAR(MAX)

		DECLARE @loc TABLE (LocId INT, LocName NVARCHAR(150))
		INSERT INTO @loc(LocId)
		SELECT * FROM dbo.fn_ParseCSVString (@locationIds, ',')
		UNION	
		SELECT rl.LocationId FROM Reports r JOIN ReportLocations rl ON r.ReportId = rl.ReportId
		WHERE	OfficeId = @officeId 
		AND		YearId = @yearId
		AND		MonthId = @monthId

		DELETE FROM @loc WHERE LocId  IN (SELECT * FROM dbo.fn_ParseCSVString (@locIdsNotIncluded, ','))
		
		--select * From @loc
		
		UPDATE @loc SET LocName = l.LocationName
		FROM @loc lt JOIN Locations l ON lt.LocId = l.LocationId

		-- Create temp table to populate all the data and then we will update this table with T,A and other Ids.
		CREATE table #temp (ReportId INT, ClusterName NVARCHAR(500), IndicatorName NVARCHAR(4000), ActivityName NVARCHAR(4000), 
							DataName NVARCHAR(4000), T DECIMAL(18,2), A DECIMAL(18,2), 
							ActivityDataId INT, IsActive bit, Location VARCHAR(20),
							LocId INT, ReportLocationId INT)

		INSERT	INTO #temp(	ClusterName, IndicatorName, ActivityName, DataName, 
							ActivityDataId, IsActive, Location, LocId)
							
		SELECT	c.ClusterShortName, oi.IndicatorName, ActivityName, DataName + '  (' + ISNULL(Unit, '') + ')', 
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
		WHERE le.LocationEmergencyId= @locEmergencyId

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
		
		UPDATE #temp SET ReportId = (SELECT TOP 1 ReportId FROM #temp2)
		UPDATE #temp SET ReportLocationId = (SELECT TOP 1 ReportId FROM #temp2)	

		UPDATE	#temp SET T = t2.Target, A = t2.Achieved 
		FROM	#temp t JOIN #temp2 t2 ON t.ActivityDataId = t2.ActivityDataId
		JOIN	@loc l ON t.LocId = l.LocId AND t2.LocationId = l.LocId

		UPDATE	#temp SET IsActive = ISNULL(t2.IsActive, 0)
		FROM	#temp t JOIN #temp2 t2 ON t.ActivityDataId = t2.ActivityDataId

		--select * from #temp
		--print @locationIds
		--select * from #temp2
		IF EXISTS( SELECT 1 FROM #temp WHERE ReportId IS NOT NULL OR @locationIds != '')
		BEGIN
		SELECT @cols = STUFF((SELECT ',' + QUOTENAME(CAST(locid AS VARCHAR(5))+'^'+Location+'_'+t.tasks) 
							FROM #temp
							cross apply
							(
							  SELECT 'T' tasks
							  union all
							  SELECT 'A'
							) t
							group by locid,location, tasks
							order by location
					FOR XML PATH(''), TYPE
					).value('.', 'NVARCHAR(MAX)') 
				,1,1,'')

		set @query = ';WITH unpiv AS
					  (
						SELECT IsActive, ReportId, ClusterName, IndicatorName, ActivityDataId, ActivityName, 
							DataName, 
							CAST(locid AS VARCHAR(5))+''^''+Location+''_''+col AS col, 
							value
						  FROM
						  (
							SELECT IsActive, ReportId, ClusterName, IndicatorName, ActivityDataId, ActivityName,
							  DataName,
							  CAST(ISNULL(T, -1) AS VARCHAR(50)) T,
							  CAST(ISNULL(A, -1 ) AS VARCHAR(50)) A,
							  Location,
							  locid
							FROM #temp
						  ) src
						  unpivot
						  (
							value
							for col in (T, A)
						  ) unpiv
					  ),
					  piv AS
					  (
						SELECT IsActive, ReportId, ClusterName, IndicatorName, ActivityDataId, ActivityName,
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
					  SELECT IsActive, ReportId, ClusterName, IndicatorName, ActivityDataId, case when rn = 1 then ActivityName else ActivityName end ActivityName,
						DataName,
						'+@cols+'
		                          
					  FROM piv
					  Order By ClusterName, IndicatorName, ActivityName, DataName
					  '

			execute(@query)
			END
			drop table #temp
			drop table #temp2
		END	
end

-----------------------------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetTargetAchievedByLocationMap]    Script Date: 10/19/2013 16:14:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[GetTargetAchievedByLocationMap]
	@locationIds VARCHAR(MAX),
	@locationType INT,
	@dataId INT = NULL,
	@target INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * INTO #locations FROM dbo.fn_ParseCSVString (@locationIds, ',')
	CREATE TABLE #temp1 (LocationId INT, LocationName NVARCHAR(150), LocationParentId INT, [Target] DECIMAL(18,2), Achieved DECIMAL(18, 2))
	CREATE TABLE #temp2 (LocationId INT, Location NVARCHAR(150), [Target] DECIMAL(18,2), Achieved DECIMAL(18, 2), Gap INT)
	
	TRUNCATE TABLE MapResults

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
	
	IF(@locationType = 2)
	BEGIN
	
		INSERT INTO #temp2
		SELECT	l.LocationId
				,l.Locationname AS Location
				,SUM(Target) AS Target
				,SUM(Achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
		FROM	#temp1 t JOIN Locations l ON t.LocationParentId = l.LocationId
		GROUP BY l.locationid, l.LocationName
		ORDER BY l.LocationName
		
	END
	ELSE IF (@locationType = 3)
	BEGIN
		INSERT INTO #temp2
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
		ORDER BY LocationName
	END
	
	
	IF (@locationType = 2)
	BEGIN
	INSERT INTO MapResults
	SELECT	[rowca].[dbo].ConvertWKT2KML(geom.STAsText()) as geom, 
			[ADM1_NAME] as name, 
			'Target = ' + convert(varchar(10), t.Target) + ' Achieved = ' + convert(varchar(10), t.Achieved) + ' Percentage = ' + CONVERT(VARCHAR(5), t.Gap) as [desc], 
			[rowca].[dbo].GetColourRampVal('00ff00', 'ff0000', t.Gap + 20) AS [colour] 
	FROM	[rowca].[dbo].[Mali] m JOIN [rowca].[dbo].[Locations] l 
			ON m.ADM1_COD = l.LocationPCode 
			JOIN #temp2 t ON l.LocationId = t.LocationId
	END
	ELSE IF (@locationType = 3)
	BEGIN
	INSERT INTO MapResults
	SELECT	[rowca].[dbo].ConvertWKT2KML(geom.STAsText()) as geom, 
			[ADM1_NAME] as name, 
			'Target = ' + convert(varchar(10), t.Target) + ' Achieved = ' + convert(varchar(10), t.Achieved) + ' Percentage = ' + CONVERT(VARCHAR(5), t.Gap) as [desc], 
			[rowca].[dbo].GetColourRampVal('00ff00', 'ff0000', t.Gap + 20) AS [colour] 
	FROM	[rowca].[dbo].[Mali] m JOIN [rowca].[dbo].[Locations] l ON m.ADM2_COD = l.LocationPCode
			JOIN #temp2 t ON l.LocationId = t.LocationId
	END

	DROP TABLE #locations
	DROP TABLE #temp
	DROP TABLE #temp1
	DROP TABLE #temp2
END

--EXEC [GetTargetAchievedByLocationMap] 23, 2

--CREATE TABLE Mapresults (Geom GEOGRAPHY, Name NVARCHAR(100), [Desc] NVARCHAR(1000), Colour NVARCHAR(10))

--select * From mapresults

-------------------------------------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetAllTasksDataReport1]    Script Date: 10/19/2013 16:15:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Feb 19, 2013
-- Description:	Get all data for report
-- =============================================
ALTER PROCEDURE [dbo].[GetAllTasksDataReport1]
	@emergencyIds VARCHAR(500) = NULL,
	@officeId INT = NULL,
	@userId UNIQUEIDENTIFIER = NULL,
	@yearId INT = NULL,
	@monthIds VARCHAR(100) = NULL,
	@locationIds VARCHAR(MAX) = NULL,
	@clusterIds VARCHAR(4000) = NULL,	
	@orgIds VARCHAR(4000) = NULL,	
	@orgTypeIds VARCHAR(1000) = NULL	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from	
	-- interfering with SELECT statements.
	
	SELECT * INTO #emergency FROM dbo.fn_ParseCSVString (@emergencyIds, ',')
	SELECT * INTO #months FROM dbo.fn_ParseCSVString (@monthIds, ',')
	SELECT * INTO #locations FROM dbo.fn_ParseCSVString (@locationIds, ',')
	SELECT * INTO #clusters FROM dbo.fn_ParseCSVString (@clusterIds, ',')
	SELECT * INTO #orgs FROM dbo.fn_ParseCSVString (@orgIds, ',')
	SELECT * INTO #orgTypes FROM dbo.fn_ParseCSVString (@orgTypeIds, ',')
	
	SET NOCOUNT ON;
	
	
	SELECT rd.ActivityDataId AS DataId, 
			e.EmergencyName AS Emergency,
			org.OrganizationName AS Organization,
			org.OrganizationAcronym,  
			o.Officename AS Office, 
			y.Year, 
			m.MonthName AS Month, 
			c.ClusterName AS Cluster, 
			co.ObjectiveName AS Objective, 
			oi.IndicatorName AS Indicator, 
			ia.ActivityName AS Activity, 
			ac.ActivityType,
			ad.DataName AS Data,
			l2.LocationName AS '(AD1)Location',
			l2.LocationPCode AS '(AD1)PCode',
			l.LocationName AS '(Ad2)Location',
			l.LocationPCode AS '(Ad2)PCode',
			cast(cast(rd.Target as DECIMAL(18,2)) as float) As Target,
			cast(cast(rd.Achieved as DECIMAL(18,2)) as float) As Achieved,
			u.UserName,
			um.Email
			,CONVERT(VARCHAR(10),
					DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,CAST(
					  CAST(y.Year AS VARCHAR(4)) +
					  RIGHT('0' + CAST(m.MonthId AS VARCHAR(2)), 2) +
					  RIGHT('0' + CAST(1 AS VARCHAR(2)), 2) 
				   AS DATETIME))+1,0)), 103) AS ReportDate,
			un.Unit,
			0 AS cnt
			
	FROM	Reports r JOIN ReportLocations rl ON r.ReportId = rl.ReportId
	LEFT	JOIN Reportdetails rd ON rl.ReportlocationId = rd.ReportlocationId
	JOIN	Locations l ON rl.LocationId = l.LocationId
	JOIN	Years y ON r.YearId = y.YearId
	JOIN	Months m ON r.MonthId = m.MonthId
	JOIN	Offices o ON r.OfficeId= o.OfficeId
	JOIN	ActivityData ad ON ad.ActivityDataId = rd.ActivityDataId
	JOIN	IndicatorActivities ia ON ad.IndicatorActivityId = ia.IndicatorActivityId
	JOIN	ObjectiveIndicators oi ON ia.ObjectiveIndicatorid = oi.ObjectiveIndicatorId
	JOIN	ClusterObjectives co ON oi.ClusterObjectiveId = co.ClusterObjectiveId
	JOIN	EmergencyClusters ec ON ec.EmergencyClusterId = co.EmergencyClusterId
	JOIN	LocationEmergencies le ON ec.LocationEmergencyId = le.LocationEmergencyId
	JOIN	Emergency e ON le.EmergencyId = e.EmergencyId
	JOIN	Clusters c ON ec.ClusterId = c.ClusterId
	JOIN	aspnet_Users u ON u.UserId = r.CreatedById
	JOIN	aspnet_Membership um ON u.UserId = um.UserId
	JOIN	Organizations org ON org.OrganizationId = o.OrganizationId
	JOIN	OrganizationTypes ot ON org.OrganizationTypeId = ot.OrganizationTypeId
	JOIN	Locations l2 on l2.LocationId = l.LocationParentId
	LEFT	JOIN ActivityTypes ac ON ia.ActivityTypeId = ac.ActivityTypeId
	LEFT	JOIN Units un ON ad.UnitId = un.UnitId
	
	WHERE	(e.EmergencyId IN( SELECT * FROM #emergency) OR @emergencyIds IS NULL)
	AND		(o.OfficeId = @officeId OR @officeId IS NULL)
	AND		(u.UserId = @userId OR @userId = '00000000-0000-0000-0000-000000000000')
	AND		(y.YearId = @yearId OR @yearId IS NULL)
	AND		(m.MonthId IN (SELECT * FROM #months) OR @monthIds IS NULL)
	AND		(l.LocationId IN (SELECT * FROM #locations) OR @locationIds IS NULL)
	AND		(c.ClusterId IN (SELECT * FROM #clusters) OR @clusterIds IS NULL)
	AND		(org.OrganizationId IN (SELECT * FROM #orgs) OR @orgIds IS NULL)
	AND		(ot.OrganizationTypeID IN (SELECT * FROM #orgTypes) OR @orgTypeIds IS NULL)

	DROP TABLE #clusters
	DROP TABLE #emergency
	DROP TABLE #months
	DROP TABLE #locations
	DROP TABLE #orgs
	DROP TABLE #orgTypes
END
-------------------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetDataForChartsAndMaps]    Script Date: 10/19/2013 16:17:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Oct 7, 2013
-- Description:	Get Targets, Achieved and WorkDone(percentage)
--				of both of these ON passed parameters.
--				This procedure is written to fetch data FROM charts.
--				This should also work for map (TODO:)
-- =============================================
ALTER PROCEDURE [dbo].[GetDataForChartsAndMaps]
	@locationType INT = NULL,
	@locationIds VARCHAR(MAX),
	@clusterIds VARCHAR(4000) = NULL,
	@organizationIds VARCHAR(MAX) = NULL,
	@fromYear INT = NULL,
	@fromMonth INT = NULL,
	@toYear INT = NULL,
	@toMonth INT = NULL,	
	@logFrameActual INT = NULL,
	@logFrameIds VARCHAR(MAX) = NULL,
	@durationType INT = NULL
AS
BEGIN
	
	SET NOCOUNT ON;
	
	IF(@organizationIds IS NULL)
	BEGIN
		SET @organizationIds = ''
	END
	
	IF(@logFrameIds IS NULL)
	BEGIN
		SET @logFrameIds = ''
	END	

    DECLARE @query1 VARCHAR(max) = ''
	DECLARE @query2 VARCHAR(max) = ''
	DECLARE @query3 VARCHAR(max) = ''

	-- Get YearId FROM year number passed as parameter
	DECLARE @fromYearId INT
	SELECT @fromYearId = YearId FROM Years WHERE [Year] = @fromYear
	DECLARE @toYearId INT
	SELECT @toYearId = YearId FROM Years WHERE [Year] = @toYear							

	SET @query1 = '
	
	CREATE TABLE #Result (	LocationId INT,
							Location NVARCHAR(150),
							LogFrameId INT,
							[Target] DECIMAL(18, 2),
							Achieved DECIMAL(18, 2),
							WorkDone INT,
							ClusterId INT,
							ClusterName NVARCHAR(250),
							ObjectiveId  INT,
							ObjectiveName NVARCHAR(4000),
							IndicatorId  INT,
							IndicatorName NVARCHAR(4000),
							ActivityId  INT,
							ActivityName NVARCHAR(4000),
							DataId  INT,
							DataName NVARCHAR(4000),							
							DurationType INT,
							DurationTypeName NVARCHAR(20),
							MonthId INT,
							MonthName NVARCHAR(20),
							QNumber INT,
							QName NVARCHAR(20),
							YearId INT,
							YearName INT
						)
	
	-- Get comma seperated values
	
	SELECT * INTO #locations FROM dbo.fn_ParseCSVString ( ''' + @locationIds + ''','','')
	SELECT * INTO #organizations FROM dbo.fn_ParseCSVString ( ''' + @organizationIds + ''','','')
	SELECT * INTO #logFrameIds FROM dbO.fn_ParseCSVString( ''' + @logFrameIds + ''','','')
    
    -- Get ActivityDataIds ON the basis of logFrameIs passed FROM parameters
    -- LogFrameType parameter will decide that what joins user need.
    -- 1 = Objective, 2 = Indicator, 3 = Activity, 4 = Data
    CREATE TABLE #DataId (dataId INT)		
    IF( ' + CONVERT(VARCHAR(5), @logFrameActual) + ' = 1)
    BEGIN
		INSERT INTO #DataId
		SELECT	ad.ActivityDataId
		FROM	ClusterObjectives co join ObjectiveIndicators oi ON co.ClusterObjectiveId = oi.ClusterObjectiveId
				JOIN IndicatorActivities ia ON oi.ObjectiveIndicatorId = ia.ObjectiveIndicatorId
				JOIN ActivityData ad ON ia.IndicatorActivityId = ad.IndicatorActivityId
				JOIN #logFrameIds lf ON lf.s = co.ClusterObjectiveId -- s here column name returned FROM fn.ParseCSVString
    END
    ELSE
    IF( ' + CONVERT(VARCHAR(5), @logFrameActual) + ' = 2)
    BEGIN
		INSERT INTO #DataId
		SELECT	ad.ActivityDataId
		FROM	ObjectiveIndicators oi JOIN IndicatorActivities ia ON oi.ObjectiveIndicatorId = ia.ObjectiveIndicatorId
				JOIN ActivityData ad ON ia.IndicatorActivityId = ad.IndicatorActivityId
				JOIN #logFrameIds lf ON lf.s = oi.ObjectiveIndicatorId -- s here column name returned FROM fn.ParseCSVString
    END
    ELSE IF( ' + CONVERT(VARCHAR(5), @logFrameActual) + ' = 3)
    BEGIN
		INSERT INTO #DataId
		SELECT	ad.ActivityDataId
		FROM	IndicatorActivities ia JOIN ActivityData ad ON
				ia.IndicatorActivityId = ad.IndicatorActivityId
				JOIN #logFrameIds lf ON lf.s = ia.IndicatorActivityId -- s here column name returned FROM fn.ParseCSVString
    END
    ELSE IF(' + CONVERT(VARCHAR(5), @logFrameActual) + ' = 4)
    BEGIN
		INSERT INTO #DataId
		SELECT * FROM #logFrameIds
    END    
    
    --select * from #DataId

	-- Recersively get all the child of locations in temp table #locations
	;WITH cte AS
	(
		SELECT LocationId, LocationParentId
		FROM Locations
		WHERE LocationId IN (SELECT * FROM #locations)
		UNION ALL
		SELECT l.locationid, l.locationparentid FROM locations l join cte c ON l.locationparentid = c.locationid
	)
	
	SELECT * INTO #temp FROM cte	
	
	-- #temp1 table.
	-- Apply all filters except locations and put data into this temp table.
	CREATE TABLE #temp1 (LocationId INT,
					 LocationName NVARCHAR(150),
					 LocationParentId INT,
					 [Target] DECIMAL(18,2),
					 Achieved DECIMAL(18, 2),
					 DataId INT,
					 ActivityId INT,
					 IndicatorId INT,
					 ObjectiveId INT,
					 YearId INT,
					 MonthId INT,
					 QNumber INT
					 )

	INSERT	INTO #temp1
	SELECT	l.LocationId, 
			locationname AS Location, 
			l.LocationParentId, 
			rd.Target, 
			rd.Achieved,
			d.ActivityDataId,
			ia.IndicatorActivityId,
			oi.ObjectiveIndicatorId,
			co.ClusterObjectiveId,
			r.YearId,
			r.MonthId
			'
			
	IF (@durationType = 2)
	BEGIN
		SET @query1 = @query1 + ', Q.QNumber '
	END
	ELSE
	BEGIN
		SET @query1 = @query1 + ', 0 AS QNumber '
	END
			
	SET @query1 = @query1 + ' FROM	Reports r JOIN ReportLocations rl ON r.ReportId = rl.ReportId
			LEFT JOIN Reportdetails rd ON rl.ReportlocationId = rd.ReportlocationId
			LEFT JOIN ActivityData d ON rd.ActivityDataId = d.ActivityDataId
			JOIN IndicatorActivities ia ON ia.IndicatorActivityId = d.IndicatorActivityId
			JOIN ObjectiveIndicators oi ON oi.ObjectiveIndicatorId = ia.ObjectiveIndicatorId
			JOIN ClusterObjectives co ON co.ClusterObjectiveId = oi.ClusterObjectiveId
			JOIN Locations l ON rl.LocationId = l.LocationId
			JOIN #temp t ON l.Locationid = t.LocationId '
	-- If organizations are selected then add tables in resultset and then apply filters ON it.
	IF (@organizationIds != '''' OR @organizationIds IS NOT NULL)
	BEGIN
		SET @query1 = @query1 + '	JOIN Offices ofc ON r.OfficeId = ofc.OfficeId
									JOIN Organizations org ON ofc.OrganizationId = org.OrganizationId '
	END
	
	-- Duration is Monthly, Quarterly and Yearly.
	-- Monthly and Yearly is alredy in Reports table but we need to add Qurarterly table in resultset
	-- if user pass Quarterly FROM front end
	IF (@durationType = 2)
	BEGIN
		SET @query1 = @query1 + ' JOIN Quarters q ON Q.MonthId = r.MonthId '
	END
	
	SET @query1 = @query1 + '	WHERE	d.ActivityDataId IN (SELECT * FROM #DataId) '
	
	-- Filter data ON organizations, if it is passed by user FROM fron end
	IF (@organizationIds != '' AND @organizationids IS NOT NULL)
	BEGIN
		SET @query1 = @query1 + ' AND org.OrganizationId IN (SELECT * FROM #organizations) '
	END
	
	-- Filter ON Year
	IF (@fromYearId > 0 AND @toYearId > 0)
	BEGIN
		SET @query1 = @query1 + ' AND r.YearId >= ' + CONVERT(VARCHAR(2), @fromYearId) + ' AND r.YearId <= ' + CONVERT(VARCHAR(2), @toYearId)
	END
	ELSE IF (@fromYearId > 0 AND (@toYearId = 0 OR @toYearId IS NULL))
	BEGIN
		SET @query1 = @query1 + ' AND r.YearId >= ' + CONVERT(VARCHAR(2), @fromYearId)
	END
	ELSE IF (@toYearId > 0 AND (@fromYearId = 0 OR @fromYearId IS NULL))
	BEGIN
		SET @query1 = @query1 + ' AND r.YearId <= ' + CONVERT(VARCHAR(2), @toYearId)
	END
	
	-- Filter ON Months
	IF (@fromMonth IS NOT NULL AND @toMonth IS NOT NULL)
	BEGIN
		SET @query1 = @query1 + ' AND r.MonthId >= ' + CONVERT(VARCHAR(2), @fromMonth) + ' AND r.MonthId <= ' + CONVERT(VARCHAR(2), @toMonth)
	END
	ELSE IF (@fromMonth IS NOT NULL AND @toMonth IS NULL)
	BEGIN
		SET @query1 = @query1 + ' AND r.MonthId >= ' + CONVERT(VARCHAR(2), @fromMonth)
	END
	ELSE IF (@toMonth IS NOT NULL AND @fromMonth IS NULL)
	BEGIN
		SET @query1 = @query1 + ' AND r.MonthId <= ' + CONVERT(VARCHAR(2), @toMonth)
	END
	
	SET @query1 = @query1 + ' ORDER BY LocationName	

	UPDATE #temp1 SET Target = 0 WHERE Target IS NULL
	UPDATE #temp1 SET Achieved = 0 WHERE Achieved IS NULL
	
	--SELECT * FROM #temp1'
	SET @query2 = '
	IF(' + CONVERT(VARCHAR(10), @locationType) + ' = 1)
	BEGIN	
		INSERT INTO #Result
		(
			LocationId,
			Location,
			LogFrameId,
			[Target],
			Achieved,
			WorkDone '
			IF (@durationType = 1)
			BEGIN
				SET @query2 = @query2 + ',DurationType, YearId '
			END
			ELSE IF (@durationType = 2)
			BEGIN
				SET @query2 = @query2 + ',DurationType, YearId '
			END
			ELSE IF (@durationType = 3)
			BEGIN
				SET @query2 = @query2 + ',DurationType '
			END
				
		SET @query2 = @query2 + '	
		)
		SELECT	l2.LocationId
				,l2.Locationname AS Location 
				, DataId 
				, sum(target) AS Target
				, sum(achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone '
										
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId, YearId '
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QNumber, YearId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
		SET @query2 = @query2 + ' 		
		FROM	#temp1 t JOIN locations l ON t.locationparentid = l.locationid
				JOIN locations l2 ON l.locationparentid = l2.locationid
		GROUP BY l2.locationid, l2.locationname, DataId '
		
		IF (@durationType = 1)
		BEGIN
			SET @query2 = @query2 + ',MonthId, YearId '
		END
		ELSE IF (@durationType = 2)
		BEGIN
			SET @query2 = @query2 + ',Qnumber, YearId '
		END
		ELSE IF (@durationType = 3)
		BEGIN
			SET @query2 = @query2 + ',YearId '
		END
		
		SET @query2 = @query2 + '
		ORDER BY l2.LocationName
		
	END
	ELSE IF(' + CONVERT(VARCHAR(10), @locationType) + ' = 2)
	BEGIN
		INSERT INTO #Result
		(
			LocationId,
			Location,
			LogFrameId,
			[Target],
			Achieved,
			WorkDone '
			IF (@durationType = 1)
			BEGIN
				SET @query2 = @query2 + ',DurationType, YearId '
			END
			ELSE IF (@durationType = 2)
			BEGIN
				SET @query2 = @query2 + ',DurationType, YearId '
			END
			ELSE IF (@durationType = 3)
			BEGIN
				SET @query2 = @query2 + ',DurationType '
			END
				
		SET @query2 = @query2 + '	
		)
		SELECT	l.LocationId
				,l.Locationname AS Location
				,DataId 
				,SUM(Target) AS Target
				,SUM(Achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone '
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId, YearId'
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QNumber, YearId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				SET @query2 = @query2 + ' 
		FROM	#temp1 t JOIN Locations l ON t.LocationParentId = l.LocationId
		GROUP BY l.locationid, l.LocationName, DataId '
				
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId, YearId '
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QNumber, YearId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				
				SET @query2 = @query2 + ' 
		ORDER BY l.LocationName
		
	END
	ELSE IF (' + CONVERT(VARCHAR(10), @locationType) + ' = 3)
	BEGIN
		INSERT INTO #Result
		(
			LocationId,
			Location,
			LogFrameId,
			[Target],
			Achieved,
			WorkDone '
			IF (@durationType = 1)
			BEGIN
				SET @query2 = @query2 + ',DurationType, YearId '
			END
			ELSE IF (@durationType = 2)
			BEGIN
				SET @query2 = @query2 + ',DurationType, YearId '
			END
			ELSE IF (@durationType = 3)
			BEGIN
				SET @query2 = @query2 + ',DurationType '
			END
				
		SET @query2 = @query2 + '	
		)
		SELECT	LocationId
				,Locationname AS Location 
				,DataId 
				,SUM(Target) AS Target
				,SUM(Achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone '
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId, YearId'
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QNumber, YearId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				SET @query2 = @query2 + ' 
		FROM	#temp1
		WHERE	LocationId IN (SELECT LocationId FROM #temp)
		GROUP BY LocationId, LocationName, DataId '				
				
				IF (@durationType = 1)
				BEGIN
					SET @query2 = @query2 + ',MonthId, YearId '
				END
				ELSE IF (@durationType = 2)
				BEGIN
					SET @query2 = @query2 + ',QNumber, YearId '
				END
				ELSE IF (@durationType = 3)
				BEGIN
					SET @query2 = @query2 + ',YearId '
				END
				
				SET @query3 = @query3 + ' 
		ORDER BY LocationName
	END

	DROP TABLE #locations
	DROP TABLE #organizations
	DROP TABLE #temp
	DROP TABLE #temp1
	DROP TABLE #DataId
	DROP TABLE #logFrameIds	
	
	UPDATE	#Result
	SET		ClusterId = c.ClusterId,
			ClusterName = c.ClusterName,
			ObjectiveId = co.ClusterObjectiveId,
			ObjectiveName = co.ObjectiveName,
			IndicatorId = oi.ObjectiveIndicatorId,
			IndicatorName = oi.IndicatorName,
			ActivityId = ia.IndicatorActivityId,
			ActivityName = ia.ActivityName,
			DataId = ad.ActivityDataId,
			DataName = ad.DataName
			
	FROM	ActivityData ad JOIN IndicatorActivities ia ON ad.IndicatorActivityId = ia.IndicatorActivityId
			JOIN ObjectiveIndicators oi ON oi.ObjectiveIndicatorId = ia.ObjectiveIndicatorId
			JOIN ClusterObjectives co ON co.ClusterObjectiveId = oi.ClusterObjectiveId
			JOIN EmergencyClusters ec ON ec.EmergencyClusterId = co.EmergencyClusterId
			JOIN Clusters c ON c.ClusterId = ec.ClusterId 
			JOIN #Result r ON r.LogFrameId = ad.ActivityDataId 			
			'
			
	IF(@durationType = 1)
	BEGIN
		SET @query3 = @query3 + ' UPDATE	#Result
		SET		DurationTypeName = ''Month'',
				MonthId = m.MonthId,
				[MonthName] = m.MonthName,
				YearName = y.Year
		FROM	#Result r JOIN Months m ON r.DurationType = m.MonthId
				JOIN Years y ON r.yearId = y.YearId '
	END
	ELSE IF(@durationType = 2)
	BEGIN
		SET @query3 = @query3 + ' UPDATE	#Result
		SET		DurationTypeName = ''Quarter'',
				QNumber = q.QNumber,
				QName = q.QName,
				YearName = y.Year
		FROM	#Result r JOIN Quarters q ON r.DurationType = q.QNumber 
				JOIN Years y ON r.yearId = y.YearId '
	END
	ELSE IF(@durationType = 3)
	BEGIN
		SET @query3 = @query3 + ' UPDATE	#Result
		SET		DurationTypeName = ''Year'',
				yearId = y.YearId,
				YearName = y.Year
		FROM	#Result r JOIN Years y ON r.DurationType = y.YearId '
	END

	SET @query3 = @query3 + ' 
	SELECT * FROM #Result
	ORDER BY  YearId, DurationType, Location, ClusterName, ObjectiveName, IndicatorName, ActivityName, DataName
	DROP TABLE #Result
	'
		
	PRINT @query1 + @query2 + @query3
	EXEC (@query1 + @query2 + @query3)
END

--------------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetDataForChart]    Script Date: 10/19/2013 16:19:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Oct 3, 2013
-- Description:	Get accumulated Target & Achieved on provided filters
-- =============================================
ALTER PROCEDURE [dbo].[GetDataForChart]
	@locationType INT = NULL,
	@locationIds VARCHAR(100) = NULL,
	@clusterIds VARCHAR(100) = NULL,
	@organizationIds VARCHAR(100) = NULL,
	@dateFrom DATETIME = NULL,
	@dateTo DATETIME = NULL,
	@logFrameType INT = NULL,
	@logFrameIds VARCHAR(100) = NULL	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT * INTO #locations FROM dbo.fn_ParseCSVString (@locationIds, ',')
    SELECT * INTO #logFrameIds FROM dbO.fn_ParseCSVString(@logFrameIds, ',')
    
    CREATE TABLE #DataId (dataId INT)
    IF(@logFrameType = 3)    
    BEGIN
		INSERT INTO #DataId
		SELECT	DISTINCT ad.ActivityDataId
		FROM	IndicatorActivities ia JOIN ActivityData ad ON
				ia.IndicatorActivityId = ad.IndicatorActivityId
				JOIN #logFrameIds lf ON lf.s = ia.IndicatorActivityId -- s here column name returned from fn.ParseCSVString
    END
    ELSE IF(@logFrameType = 4)
    BEGIN
		INSERT INTO #DataId
		SELECT * FROM #logFrameIds
    END
    
	CREATE TABLE #temp1 (LocationId INT,
					 LocationName NVARCHAR(150),
					 LocationParentId INT,
					 [Target] DECIMAL(18,2),
					 Achieved DECIMAL(18, 2),
					 DataId INT,
					 ActivityId INT,
					 IndicatorId INT,
					 ObjectiveId INT
					 )

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
			l.LocationParentId, 
			rd.Target, 
			rd.Achieved,
			d.ActivityDataId,
			ia.IndicatorActivityId,
			oi.ObjectiveIndicatorId,
			co.ClusterObjectiveId
			
	FROM	Reports r JOIN ReportLocations rl ON r.ReportId = rl.ReportId
			LEFT JOIN Reportdetails rd ON rl.ReportlocationId = rd.ReportlocationId
			LEFT JOIN ActivityData d on rd.ActivityDataId = d.ActivityDataId
			JOIN IndicatorActivities ia ON ia.IndicatorActivityId = d.IndicatorActivityId
			JOIN ObjectiveIndicators oi ON oi.ObjectiveIndicatorId = ia.ObjectiveIndicatorId
			JOIN ClusterObjectives co ON co.ClusterObjectiveId = oi.ClusterObjectiveId
			JOIN Locations l ON rl.LocationId = l.LocationId
			JOIN #temp t ON l.Locationid = t.LocationId			
			
	WHERE	d.ActivityDataId IN (SELECT * FROM #DataId)	
	ORDER BY LocationName

	UPDATE #temp1 SET Target = 0 WHERE Target IS NULL
	UPDATE #temp1 SET Achieved = 0 WHERE Achieved IS NULL
	
	--select * from #temp1

	IF(@locationType = 1)
	BEGIN
	
		SELECT	l2.LocationId
				,l2.Locationname AS Location
				,DataId
				, sum(target) AS Target
				, sum(achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
				
		FROM	#temp1 t JOIN locations l ON t.locationparentid = l.locationid
				JOIN locations l2 ON l.locationparentid = l2.locationid
		GROUP BY l2.locationid, l2.locationname, DataId
		ORDER BY l2.LocationName
		
	END
	ELSE IF(@locationType = 2)
	BEGIN
	
		SELECT	l.LocationId
				,l.Locationname AS Location
				,DataId
				,SUM(Target) AS Target
				,SUM(Achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
		FROM	#temp1 t JOIN Locations l ON t.LocationParentId = l.LocationId
		GROUP BY l.locationid, l.LocationName, DataId
		ORDER BY l.LocationName
		
	END
	ELSE IF (@locationType = 3)
	BEGIN
	
		SELECT	LocationId
				,Locationname AS Location
				,DataId
				,SUM(Target) AS Target
				,SUM(Achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
		FROM	#temp1
		WHERE	LocationId IN (SELECT LocationId FROM #temp)
		GROUP BY LocationId, LocationName, DataId
		ORDER BY LocationName
	END

	DROP TABLE #locations
	DROP TABLE #temp
	DROP TABLE #temp1
	DROP TABLE #DataId
END

-----------------------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetObjectivesOfMultipleClusters]    Script Date: 10/19/2013 16:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: 20 Sep, 2013
-- Description:	Get objectives of selected clusters
-- =============================================
ALTER PROCEDURE [dbo].[GetObjectivesOfMultipleClusters]
	@ids VARCHAR(MAX)
AS
BEGIN
	
	SET NOCOUNT ON;
	SELECT * INTO #ids FROM dbo.fn_ParseCSVString (@ids, ',')

    SELECT	co.ClusterObjectiveId,
			co.ObjectiveName
			
	FROM	EmergencyClusters ec JOIN	ClusterObjectives co on ec.EmergencyClusterId = co.EmergencyClusterId
	
	WHERE	ec.EmergencyClusterId IN(SELECT * FROM #ids)
	ORDER BY co.ObjectiveName
END

----------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetAllLocations]    Script Date: 10/19/2013 16:23:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: 15 feb, 2012
-- Description:	Get all locations with hirerachy and location level.
-- =============================================
ALTER PROCEDURE [dbo].[GetAllLocations]
	@locationName NVARCHAR(150) = NULL
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	CREATE TABLE #AllLocations
	(
		LocationId INT,
		LocationName VARCHAR(100),
		LocationParentId INT,
		LocationTypeId INT,
		LocationTypeName VARCHAR(20),
		Latitude FLOAT,
		Longitude FLOAT,
		LocLeve INT,
		ProvinceId INT,
		ProvinceName VARCHAR(100),
		DistrictId INT,
		DistrictName VARCHAR(100),
		TehsilId INT,
		TehsilName VARCHAR(100),
		UCId INT,
		UCName VARCHAR(100),
		LocationFullName VARCHAR(250)
	)
	
	;with CTELoc as
	(
		SELECT	LocationID, 
				LocationName, 
				LocationParentId, 
				ll.LocationTypeId, 
				ll.LocationType,
				1 AS 'LocLevel'
		FROM	Locations l INNER JOIN LocationTypes ll ON l.LocationTypeId = ll.LocationTypeId 
		WHERE	LocationID IN (1, 2, 3, 2526, 2527, 2528, 2529, 2530)
		UNION	ALL
		SELECT	l.locationid, 
				l.locationname, 
				l.LocationParentId, 
				ll.LocationTypeId, 
				ll.LocationType,
				LocLevel + 1 AS 'LocLevel'
		FROM	Locations l INNER JOIN LocationTypes ll ON l.LocationTypeId = ll.LocationTypeId
				INNER JOIN CTELoc c ON l.LocationParentId = c.LocationID
	)
	
	
	INSERT INTO #AllLocations
	(
		LocationId,
		LocationName,
		LocationParentId,
		LocationTypeId,
		LocationTypeName,
		LocLeve
	)
	SELECT	LocationID,
			LocationName,
			LocationParentId,
			LocationTypeId,
			LocationType,
			LocLevel
	FROM CTELoc
	
	-- Update table with UC names
	UPDATE	#AllLocations
	SET		UCId = LocationParentId
	WHERE	LocationTypeId = 6	
	
	UPDATE	#AllLocations
	SET		UCName = l.LocationName
	FROM	#AllLocations a INNER JOIN Locations l ON a.UCId = l.LocationID
		
	-- Update table with tehsil names
	UPDATE	#AllLocations
	SET		TehsilId = l.LocationParentId
	FROM	#AllLocations a INNER JOIN Locations l ON a.UCId = l.LocationID
	where	l.LocationTypeId = 6
	AND		TehsilId IS NULL
	
	UPDATE	#AllLocations
	SET		TehsilId = LocationParentId
	WHERE	LocationTypeId = 4
	
	UPDATE	#AllLocations
	SET		TehsilName = l.LocationName
	FROM	#AllLocations a INNER JOIN Locations l ON a.TehsilId = l.LocationID
	
	-- Update table with district names.
	UPDATE	#AllLocations
	SET		DistrictId = l.LocationParentId
	FROM	#AllLocations a INNER JOIN Locations l ON a.TehsilId = l.LocationID
	where	DistrictId IS NULL
	
	UPDATE	#AllLocations
	SET		DistrictId = LocationParentId
	WHERE	LocationTypeId = 3
	AND		DistrictId IS NULL
	
	UPDATE	#AllLocations
	SET		DistrictName = l.LocationName
	FROM	#AllLocations a INNER JOIN Locations l ON a.DistrictId = l.LocationID
	
	-- Update table with provice names
	UPDATE	#AllLocations
	SET		ProvinceId = l.LocationParentId
	FROM	#AllLocations a INNER JOIN Locations l ON a.DistrictId = l.LocationID
	where	ProvinceId IS NULL
	
	UPDATE	#AllLocations
	SET		ProvinceId = LocationParentId
	WHERE	LocationTypeId = 2
	AND		ProvinceId IS NULL
	
	UPDATE	#AllLocations
	SET		ProvinceName = l.LocationName
	FROM	#AllLocations a INNER JOIN Locations l ON a.ProvinceId = l.LocationID
	
	UPDATE	#AllLocations
	SET		LocationFullName = LocationName + ' ' + DistrictName + ' ' + ProvinceName
	
	SELECT * FROM #AllLocations ORDER BY LocationTypeId, LocationName
	
	DROP TABLE #AllLocations
END

---------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetDataForChart]    Script Date: 10/19/2013 16:24:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Oct 3, 2013
-- Description:	Get accumulated Target & Achieved on provided filters
-- =============================================
ALTER PROCEDURE [dbo].[GetDataForChart]
	@locationType INT = NULL,
	@locationIds VARCHAR(MAX) = NULL,
	@clusterIds VARCHAR(MAX) = NULL,
	@organizationIds VARCHAR(4000) = NULL,
	@dateFrom DATETIME = NULL,
	@dateTo DATETIME = NULL,
	@logFrameType INT = NULL,
	@logFrameIds VARCHAR(MAX) = NULL	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT * INTO #locations FROM dbo.fn_ParseCSVString (@locationIds, ',')
    SELECT * INTO #logFrameIds FROM dbO.fn_ParseCSVString(@logFrameIds, ',')
    
    CREATE TABLE #DataId (dataId INT)
    IF(@logFrameType = 3)    
    BEGIN
		INSERT INTO #DataId
		SELECT	DISTINCT ad.ActivityDataId
		FROM	IndicatorActivities ia JOIN ActivityData ad ON
				ia.IndicatorActivityId = ad.IndicatorActivityId
				JOIN #logFrameIds lf ON lf.s = ia.IndicatorActivityId -- s here column name returned from fn.ParseCSVString
    END
    ELSE IF(@logFrameType = 4)
    BEGIN
		INSERT INTO #DataId
		SELECT * FROM #logFrameIds
    END
    
	CREATE TABLE #temp1 (LocationId INT,
					 LocationName NVARCHAR(150),
					 LocationParentId INT,
					 [Target] DECIMAL(18,2),
					 Achieved DECIMAL(18, 2),
					 DataId INT,
					 ActivityId INT,
					 IndicatorId INT,
					 ObjectiveId INT
					 )

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
			l.LocationParentId, 
			rd.Target, 
			rd.Achieved,
			d.ActivityDataId,
			ia.IndicatorActivityId,
			oi.ObjectiveIndicatorId,
			co.ClusterObjectiveId
			
	FROM	Reports r JOIN ReportLocations rl ON r.ReportId = rl.ReportId
			LEFT JOIN Reportdetails rd ON rl.ReportlocationId = rd.ReportlocationId
			LEFT JOIN ActivityData d on rd.ActivityDataId = d.ActivityDataId
			JOIN IndicatorActivities ia ON ia.IndicatorActivityId = d.IndicatorActivityId
			JOIN ObjectiveIndicators oi ON oi.ObjectiveIndicatorId = ia.ObjectiveIndicatorId
			JOIN ClusterObjectives co ON co.ClusterObjectiveId = oi.ClusterObjectiveId
			JOIN Locations l ON rl.LocationId = l.LocationId
			JOIN #temp t ON l.Locationid = t.LocationId			
			
	WHERE	d.ActivityDataId IN (SELECT * FROM #DataId)	
	ORDER BY LocationName

	UPDATE #temp1 SET Target = 0 WHERE Target IS NULL
	UPDATE #temp1 SET Achieved = 0 WHERE Achieved IS NULL
	
	--select * from #temp1

	IF(@locationType = 1)
	BEGIN
	
		SELECT	l2.LocationId
				,l2.Locationname AS Location
				,DataId
				, sum(target) AS Target
				, sum(achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
				
		FROM	#temp1 t JOIN locations l ON t.locationparentid = l.locationid
				JOIN locations l2 ON l.locationparentid = l2.locationid
		GROUP BY l2.locationid, l2.locationname, DataId
		ORDER BY l2.LocationName
		
	END
	ELSE IF(@locationType = 2)
	BEGIN
	
		SELECT	l.LocationId
				,l.Locationname AS Location
				,DataId
				,SUM(Target) AS Target
				,SUM(Achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
		FROM	#temp1 t JOIN Locations l ON t.LocationParentId = l.LocationId
		GROUP BY l.locationid, l.LocationName, DataId
		ORDER BY l.LocationName
		
	END
	ELSE IF (@locationType = 3)
	BEGIN
	
		SELECT	LocationId
				,Locationname AS Location
				,DataId
				,SUM(Target) AS Target
				,SUM(Achieved) AS Achieved
				, CASE WHEN ((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR((SUM(ISNULL(Achieved,0)) / SUM(NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone
		FROM	#temp1
		WHERE	LocationId IN (SELECT LocationId FROM #temp)
		GROUP BY LocationId, LocationName, DataId
		ORDER BY LocationName
	END

	DROP TABLE #locations
	DROP TABLE #temp
	DROP TABLE #temp1
	DROP TABLE #DataId
END

------------------------------------------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[InsertLocation]    Script Date: 10/19/2013 16:25:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: 16 Feb, 2013
-- Description:	Save Location
-- =============================================
ALTER PROCEDURE [dbo].[InsertLocation]
	@locationType INT,
	@parentId INT,
	@locationName NVARCHAR(150),
	@lat FLOAT,
	@lng FLOAT,
	@isAccurate INT,
	@userId uniqueidentifier,
	@UID int = NULL OUTPUT
AS
BEGIN
	DECLARE @InsertedRecords TABLE(id int)
	
	SET NOCOUNT ON;
    
	INSERT INTO	Locations
	(
		LocationName,
		LocationTypeId,
		LocationParentId,
		Latitude,
		Longitude,
		IsAccurateLatLng,
		CreatedById
	)
	OUTPUT INSERTED.LocationId INTO @InsertedRecords
	VALUES
	(
		@locationName,
		@locationType,
		@parentId,
		@lat,
		@lng,
		@isaccurate,
		@userId
	)
	
	SELECT @UID = id FROM @InsertedRecords
	
END

----------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetTargetAndAchievedByMonthAndLocation]    Script Date: 10/19/2013 16:28:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	ALTER PROCEDURE [dbo].[GetTargetAndAchievedByMonthAndLocation]
		@locationIds VARCHAR(MAX) = NULL
	AS
	BEGIN
		SET @locationIds = '34,42,51'
	
		SELECT * INTO #locations FROM dbo.fn_ParseCSVString (@locationIds, ',')
	
		SELECT m.MonthName, l.LocationName
		, SUM(rd.Target) Target , SUM(rd.Achieved) Achieved
		
		
		FROM Reports r JOIN Months m ON r.MonthId = m.MonthId
		JOIN Years y ON r.YearId = y.YearId
		JOIN ReportLocations rl ON r.ReportId = rl.ReportId
		JOIN ReportDetails rd ON r.ReportId = rd.ReportId 
		AND rl.ReportLocationId = rd.ReportLocationId
		JOIN Locations l ON rl.LocationId = l.LocationId
		WHERE l.LocationId IN (SELECT * FROM #locations) OR @locationIds IS NULL
		GROUP BY m.MonthName, m.MonthId, l.LocationName, l.LocationId
		ORDER BY m.MonthId, l.LocationName
		
		DROP TABLE #locations
	
	END
---------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetIPData]    Script Date: 10/19/2013 16:29:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[GetIPData]
	@locEmergencyId INT,
	@locationIds VARCHAR(MAX),
	@officeID INT,
	@yearId INT,
	@monthId INT,
	@locIdsNotIncluded VARCHAR(MAX),
	@userId UNIQUEIDENTIFIER
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
							DataName NVARCHAR(4000), T DECIMAL(18,2), A DECIMAL(18,2), 
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

		UPDATE	#temp SET T = t2.Target, A = t2.Achieved 
		FROM	#temp t JOIN #temp2 t2 ON t.ActivityDataId = t2.ActivityDataId
		JOIN	@loc l ON t.LocId = l.LocId AND t2.LocationId = l.LocId

		UPDATE	#temp SET IsActive = ISNULL(t2.IsActive, 0)
		FROM	#temp t JOIN #temp2 t2 ON t.ActivityDataId = t2.ActivityDataId

		
		-- Variables for dynamic query to build pivot table.		
		DECLARE @cols AS NVARCHAR(MAX)
		DECLARE @query AS NVARCHAR(MAX)
		
		IF EXISTS( SELECT 1 FROM #temp WHERE ReportId IS NOT NULL OR @locationIds != '')
		BEGIN
		SELECT @cols = STUFF((SELECT ',' + QUOTENAME(CAST(locid AS VARCHAR(5))+'^'+Location+'_'+t.tasks) 
							FROM #temp
							cross apply
							(
							  SELECT 'T' tasks
							  union all
							  SELECT 'A'
							) t
							group by locid,location, tasks
							order by location
					FOR XML PATH(''), TYPE
					).value('.', 'NVARCHAR(MAX)') 
				,1,1,'')

		set @query = ';WITH unpiv AS
					  (
						SELECT IsActive, ReportId, ClusterName, IndicatorName, ActivityDataId, ActivityName, 
							DataName, 
							CAST(locid AS VARCHAR(5))+''^''+Location+''_''+col AS col, 
							value
						  FROM
						  (
							SELECT IsActive, ReportId, ClusterName, IndicatorName, ActivityDataId, ActivityName,
							  DataName,
							  CAST(ISNULL(T, -1) AS VARCHAR(50)) T,
							  CAST(ISNULL(A, -1 ) AS VARCHAR(50)) A,
							  Location,
							  locid
							FROM #temp
						  ) src
						  unpivot
						  (
							value
							for col in (T, A)
						  ) unpiv
					  ),
					  piv AS
					  (
						SELECT IsActive, ReportId, ClusterName, IndicatorName, ActivityDataId, ActivityName,
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
					  SELECT IsActive, ReportId, ClusterName, IndicatorName, ActivityDataId, case when rn = 1 then ActivityName else ActivityName end ActivityName,
						DataName,
						'+@cols+'
		                          
					  FROM piv
					  Order By ClusterName, IndicatorName, ActivityName, DataName
					  '

			execute(@query)
			END
			drop table #temp
			drop table #temp2
		END	
end
----------------------------------------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetIPData1]    Script Date: 10/19/2013 16:30:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[GetIPData1]
	@locEmergencyId INT,
	@locationIds VARCHAR(MAX),
	@officeID INT,
	@yearId INT,
	@monthId INT,
	@locIdsNotIncluded VARCHAR(MAX)
AS
BEGIN

	IF(@officeId > 0 AND @yearId > 0 AND @monthId > 0)
	BEGIN

		DECLARE @cols AS NVARCHAR(MAX)
		DECLARE @query AS NVARCHAR(MAX)

		DECLARE @loc TABLE (LocId INT, LocName NVARCHAR(150))
		INSERT INTO @loc(LocId)
		SELECT * FROM dbo.fn_ParseCSVString (@locationIds, ',')
		UNION	
		SELECT rl.LocationId FROM Reports r JOIN ReportLocations rl ON r.ReportId = rl.ReportId
		WHERE	OfficeId = @officeId 
		AND		YearId = @yearId
		AND		MonthId = @monthId

		DELETE FROM @loc WHERE LocId  IN (SELECT * FROM dbo.fn_ParseCSVString (@locIdsNotIncluded, ','))
		
		--select * From @loc
		
		UPDATE @loc SET LocName = l.LocationName
		FROM @loc lt JOIN Locations l ON lt.LocId = l.LocationId

		-- Create temp table to populate all the data and then we will update this table with T,A and other Ids.
		CREATE table #temp (ReportId INT, ClusterName NVARCHAR(100), IndicatorName NVARCHAR(500), ActivityName NVARCHAR(500), 
							DataName NVARCHAR(4000), T DECIMAL(18,2), A DECIMAL(18,2), 
							ActivityDataId INT, IsActive bit, Location VARCHAR(20),
							LocId INT, ReportLocationId INT)

		INSERT	INTO #temp(	ClusterName, IndicatorName, ActivityName, DataName, 
							ActivityDataId, IsActive, Location, LocId)
							
		SELECT	c.ClusterShortName, oi.IndicatorName, ActivityName, DataName + '  (' + ISNULL(Unit, '') + ')', 
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
		WHERE le.LocationEmergencyId= @locEmergencyId

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
		
		UPDATE #temp SET ReportId = (SELECT TOP 1 ReportId FROM #temp2)
		UPDATE #temp SET ReportLocationId = (SELECT TOP 1 ReportId FROM #temp2)	

		UPDATE	#temp SET T = t2.Target, A = t2.Achieved 
		FROM	#temp t JOIN #temp2 t2 ON t.ActivityDataId = t2.ActivityDataId
		JOIN	@loc l ON t.LocId = l.LocId AND t2.LocationId = l.LocId

		UPDATE	#temp SET IsActive = ISNULL(t2.IsActive, 0)
		FROM	#temp t JOIN #temp2 t2 ON t.ActivityDataId = t2.ActivityDataId

		--select * from #temp
		--print @locationIds
		--select * from #temp2
		IF EXISTS( SELECT 1 FROM #temp WHERE ReportId IS NOT NULL OR @locationIds != '')
		BEGIN
		SELECT @cols = STUFF((SELECT ',' + QUOTENAME(CAST(locid AS VARCHAR(5))+'^'+Location+'_'+t.tasks) 
							FROM #temp
							cross apply
							(
							  SELECT 'T' tasks
							  union all
							  SELECT 'A'
							) t
							group by locid,location, tasks
							order by location
					FOR XML PATH(''), TYPE
					).value('.', 'NVARCHAR(MAX)') 
				,1,1,'')

		set @query = ';WITH unpiv AS
					  (
						SELECT IsActive, ReportId, ClusterName, IndicatorName, ActivityDataId, ActivityName, 
							DataName, 
							CAST(locid AS VARCHAR(5))+''^''+Location+''_''+col AS col, 
							value
						  FROM
						  (
							SELECT IsActive, ReportId, ClusterName, IndicatorName, ActivityDataId, ActivityName,
							  DataName,
							  CAST(ISNULL(T, -1) AS VARCHAR(50)) T,
							  CAST(ISNULL(A, -1 ) AS VARCHAR(50)) A,
							  Location,
							  locid
							FROM #temp
						  ) src
						  unpivot
						  (
							value
							for col in (T, A)
						  ) unpiv
					  ),
					  piv AS
					  (
						SELECT IsActive, ReportId, ClusterName, IndicatorName, ActivityDataId, ActivityName,
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
					  SELECT IsActive, ReportId, ClusterName, IndicatorName, ActivityDataId, case when rn = 1 then ActivityName else ActivityName end ActivityName,
						DataName,
						'+@cols+'
		                          
					  FROM piv
					  Order By ClusterName, IndicatorName, ActivityName, DataName
					  '

			execute(@query)
			END
			drop table #temp
			drop table #temp2
		END	
end

--------------------------------------------------------------------------------------------------------------

USE [ROWCA]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_ParseCSVString]    Script Date: 10/19/2013 16:31:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER function [dbo].[fn_ParseCSVString]
(
@CSVString 	varchar(MAX) ,
@Delimiter	varchar(10)
)
returns @tbl table (s varchar(MAX))
as
/*
select * from dbo.fn_ParseCSVString ('qwe,c,rew,c,wer', ',c,')
*/
begin
declare @i int ,
	@j int
	select 	@i = 1
	while @i <= len(@CSVString)
	begin
		select	@j = charindex(@Delimiter, @CSVString, @i)
		if @j = 0
		begin
			select	@j = len(@CSVString) + 1
		end
		insert	@tbl select substring(@CSVString, @i, @j - @i)
		select	@i = @j + len(@Delimiter)
	end
	return
end


--------------------------------------------------------------------------------------------