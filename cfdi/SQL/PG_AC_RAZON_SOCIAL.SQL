USE [CFD19_GestionCFDi_V9999_R0]
GO
/****** Object:  StoredProcedure [dbo].[PG_SK_CERT_INFO_EMISOR]    Script Date: 6/1/2020 10:29:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[PG_AC_RAZON_SOCIAL] 
	@PP_K_RAZON_SOCIAL		INT
AS
	BEGIN

		UPDATE RAZON_SOCIAL
		SET 
			L_BORRADO = 1
		WHERE K_RAZON_SOCIAL = @PP_K_RAZON_SOCIAL

	END
	

	-- ===============================

