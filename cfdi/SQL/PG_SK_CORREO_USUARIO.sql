USE [CFD19_GestionCFDi_V9999_R0]
GO
/****** Object:  StoredProcedure [dbo].[PG_IN_USUARIO_V2]    Script Date: 7/1/2020 9:40:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[PG_SK_CORREO_USUARIO] 
	@PP_CORREO	  VARCHAR(100)
AS

	BEGIN
		SELECT U.CORREO FROM USUARIOS U
		WHERE U.CORREO = @PP_CORREO
	END

	-- ===============================

