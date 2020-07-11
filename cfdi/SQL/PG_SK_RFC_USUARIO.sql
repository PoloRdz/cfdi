USE [CFD19_GestionCFDi_V9999_R0]
GO
/****** Object:  StoredProcedure [dbo].[PG_IN_USUARIO_V2]    Script Date: 7/1/2020 9:40:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[PG_SK_RFC_USUARIO] 
	@PP_RFC	  VARCHAR(13)
AS

	BEGIN
		SELECT RFC
		FROM INFORMACION_FISCAL_USUARIOS
		WHERE RFC = @PP_RFC
	END

	-- ===============================

