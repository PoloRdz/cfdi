USE [CFD19_GestionCFDi_V9999_R0]
GO
/****** Object:  StoredProcedure [dbo].[PG_SK_CERT_INFO_EMISOR]    Script Date: 6/1/2020 10:29:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[PG_IN_USUARIO_ROL]
	@PP_ID_USUARIO		INT,
	@PP_ID_ROL			INT
AS
	BEGIN
		INSERT INTO USUARIOS_ROLES(
			ID_ROL,
			ID_USUARIO
		) VALUES (
			@PP_ID_ROL,
			@PP_ID_USUARIO
		)
	END;

	-- ===============================

