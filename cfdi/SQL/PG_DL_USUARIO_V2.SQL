USE [CFD19_GestionCFDi_V9999_R0]
GO
/****** Object:  StoredProcedure [dbo].[PG_SK_CERT_INFO_EMISOR]    Script Date: 6/1/2020 10:29:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[PG_DL_USUARIO_V2] 
	@PP_ID_USUARIO		INT
AS
	BEGIN

		UPDATE USUARIOS
		SET 
			L_USUARIO = 0
		WHERE ID_USUARIO = @PP_ID_USUARIO

	END
	

	-- ===============================

