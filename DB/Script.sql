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