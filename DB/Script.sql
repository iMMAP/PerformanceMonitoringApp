
/****** Object:  StoredProcedure [dbo].[GetEmergencyClusters]    Script Date: 12/27/2013 7:33:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Muhammad Kashif Nadeem
-- Create date: Feb 5, 2013
-- Description:	Get Emergency Clusters
-- =============================================
ALTER PROCEDURE [dbo].[GetEmergencyClusters]
	@emgId INT,
	@lngId TINYINT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT	ec.EmergencyClusterId,
			c.ClusterName,
			ec.ClusterId
			
	FROM	EmergencyClusters ec
			JOIN Clusters c ON ec.ClusterId = c.ClusterId
			JOIN Emergency e ON e.EmergencyId = ec.EmergencyId
	WHERE	e.EmergencyId = @emgId
	AND		e.SiteLanguageId = @lngId
	AND		c.SiteLanguageId = @lngId
	Order by c.ClusterName
END

------------------------------------------------------------------
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
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE GetActivities
	@emgId INT,
	@clusterId INT,
	@objId INT,
	@prId INT,
	@lngId TINYINT
AS
BEGIN
	
	SET NOCOUNT ON;
	CREATE TABLE #temp (ClusterId INT, ObjectiveId INT, hpId INT, PriorityActivityId INT, ActivityName_En NVARCHAR(2000), ActivityName_Fr NVARCHAR(2000))

	INSERT INTO #temp
	SELECT	c.ClusterId, 		
			o.ObjectiveId, 		
			hp.HumanitarianPriorityId,
			pa.PriorityActivityId,
			pa.ActivityName,
			NULL
		
	FROM	Emergency e JOIN EmergencyClusters ec ON e.EmergencyId = ec.EmergencyId
	JOIN	ClusterObjectives co ON co.EmergencyClusterId = ec.EmergencyClusterId
	JOIN	Clusters c ON c.ClusterId = ec.ClusterId
	JOIN	Objectives o ON o.ObjectiveId = co.ObjectiveId
	JOIN	ObjectivePriorities op ON op.ClusterObjectiveId = co.ClusterObjectiveId
	JOIN	HumanitarianPriorities hp ON hp.HumanitarianPriorityId = op.HumanitarianPriorityId
	JOIN	PriorityActivities pa ON pa.ObjectivePriorityId = op.ObjectivePriorityId

	WHERE	e.EmergencyId = @emgId
	AND		c.ClusterId = @clusterId
	AND		o.ObjectiveId = @objId
	AND		hp.HumanitarianPriorityId = @prId

	AND		e.SiteLanguageId = @lngId
	AND		c.SiteLanguageId = @lngId
	AND		o.SiteLanguageId = @lngId
	AND		hp.SiteLanguageId = @lngId
	AND		pa.SiteLanguageId = @lngId


	UPDATE	#temp
	SET		ActivityName_Fr = pa.ActivityName
	FROM	#temp t JOIN PriorityActivities pa ON t.PriorityActivityId = pa.PriorityActivityId
	WHERE	pa.SiteLanguageId = 2

	SELECT	* FROM	#temp

	DROP TABLE #temp    
	
END
GO
