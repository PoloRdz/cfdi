USE [CFD19_GestionCFDi_V9999_R0]
GO
/****** Object:  StoredProcedure [dbo].[PG_SK_CERT_INFO_EMISOR]    Script Date: 6/1/2020 10:29:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[PG_SK_USUARIOS_COUNT] 
	@PP_TOTAL_REG		INT OUTPUT
AS
	BEGIN 
		SELECT 
			@PP_TOTAL_REG = COUNT(*)
		FROM USUARIOS U
	END

	-- ===============================

