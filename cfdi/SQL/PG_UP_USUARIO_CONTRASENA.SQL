USE [CFD19_GestionCFDi_V9999_R0]
GO
/****** Object:  StoredProcedure [dbo].[PG_SK_CERT_INFO_EMISOR]    Script Date: 6/1/2020 10:29:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[PG_UP_USUARIO_CONTRASENA] 
	@PP_ID_USUARIO		INT,
	@PP_PASSWORD		VARCHAR(256)

AS
	BEGIN

		UPDATE USUARIOS
		SET 
			CONTRASENA = @PP_PASSWORD
		WHERE ID_USUARIO = @PP_ID_USUARIO

	END
	

	-- ===============================

