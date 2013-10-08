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