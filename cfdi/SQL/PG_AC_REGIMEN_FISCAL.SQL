USE [CFD19_GestionCFDi_V9999_R0]
GO
/****** Object:  StoredProcedure [dbo].[PG_SK_CERT_INFO_EMISOR]    Script Date: 6/1/2020 10:29:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[PG_AC_REGIMEN_FISCAL] 
	@PP_ID_REGIMEN_FISCAL	INT
AS

	BEGIN
		UPDATE REGIMENES_FISCALES SET
			L_REGIMEN_FISCAL = 1
		WHERE ID_REGIMEN_FISCAL = @PP_ID_REGIMEN_FISCAL
	END

	-- ===============================

