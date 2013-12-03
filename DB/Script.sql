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
