USE [CFD19_GestionCFDi_V9999_R0]
GO
/****** Object:  StoredProcedure [dbo].[PG_SK_CERT_INFO_EMISOR]    Script Date: 6/1/2020 10:29:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[PG_DL_SERIE] 
	@PP_ID_SERIE		INT
AS
	BEGIN

		UPDATE SERIES
		SET 
			L_SERIE = 0
		WHERE ID_SERIE = @PP_ID_SERIE

	END
	

	-- ===============================

