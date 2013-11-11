-- Add DummyOrganization to db
USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetAllTasksDataReport]    Script Date: 11/07/2013 14:41:47 ******/
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
				l3.LocationName AS 'Country',
				l2.LocationName AS '(AD1)Location',
				l2.LocationPCode AS '(AD1)PCode',
				l.LocationName AS '(Ad2)Location',
				l.LocationPCode AS '(Ad2)PCode',
				cast(cast(rd.Target as DECIMAL(18,2)) as float) As Target,
				cast(cast(rd.Achieved as DECIMAL(18,2)) as float) As Achieved,
				CASE WHEN (((ISNULL(Achieved,0)) / (NULLIF(ISNULL(Target,0), 0))) * 100) IS NULL
					THEN 0 ELSE FLOOR(((ISNULL(Achieved,0)) / (NULLIF(ISNULL(Target,0), 0))) * 100)
					END AS WorkDone,
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
		JOIN	Locations l3 on l3.LocationId = l2.LocationParentId
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


USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[InsertASPNetUserCustomWithMultipleLocations]    Script Date: 11/11/2013 17:23:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Oct 26, 2013
-- Description:	Register User with multiple locations
-- =============================================
ALTER PROCEDURE [dbo].[InsertASPNetUserCustomWithMultipleLocations]
	@userId uniqueidentifier,
	@orgId INT,
	@countryId VARCHAR(100),
	@phone NVARCHAR(50),
	@UID int = NULL OUTPUT
AS
BEGIN
	
	SET NOCOUNT ON;
	
	IF(@orgId IS NULL)
	BEGIN
		SELECT @orgId = Organizationid FROM Organizations WHERE OrganizationName LIKE 'DummyOrganization'
	END
	
    SELECT * INTO #locations FROM dbo.fn_ParseCSVString (@countryId, ',')
	INSERT	INTO aspnet_Users_Custom
	(
		UserId,
		OrganizationId,
		PhoneNumber,
		CountryId
	)
	SELECT @userId,
			@orgId,		
			@phone,
			s
	FROM	#locations
	
	DROP TABLE #locations
		
	SELECT @UID = 0
END

USE [ROWCA]
GO
/****** Object:  StoredProcedure [dbo].[GetAllOfficesByPrincipal]    Script Date: 11/11/2013 17:36:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Feb 19, 2013
-- Description:	Get Offices On Organization
-- =============================================
ALTER PROCEDURE [dbo].[GetAllOfficesByPrincipal]
	@userId UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT	DISTINCT
			f.OfficeId,
			f.OfficeName,
			l.LocationId,
			l.LocationName,
			o.OrganizationId,
			o.OrganizationName,
			o.OrganizationAcronym,
			l.LocationParentId
			
	FROM	Offices f JOIN Organizations o ON f.OrganizationId = o.OrganizationId
	JOIN	Locations l ON f.LocationId = l.LocationId	
	JOIN	aspnet_Users_Custom c ON l.LocationId = c.CountryId
	JOIN	aspnet_Users u ON c.UserId = u.UserId
	JOIN	aspnet_UsersInRoles r ON u.UserId = r.UserId
	WHERE	u.UserId = @userId
	ORDER BY f.OfficeName
END