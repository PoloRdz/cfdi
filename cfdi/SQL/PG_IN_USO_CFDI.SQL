USE [CFD19_GestionCFDi_V9999_R0]
GO
/****** Object:  StoredProcedure [dbo].[PG_SK_CERT_INFO_EMISOR]    Script Date: 6/1/2020 10:29:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[PG_IN_USO_CFDI] 
	@PP_USO_CFDI		VARCHAR(3),
	@PP_DESCRIPCRION	VARCHAR(100),
	@PP_EXPLICACION 	VARCHAR(200),
	@PP_FISICA			BIT,
	@PP_MORAL			BIT
AS

	BEGIN
		INSERT INTO USO_CFDI(
			USO_CFDI,
			DESCRIPCION,
			EXPLICACION,
			FISICA,
			MORAL,
			L_USO_CFDI
		) VALUES (
			@PP_USO_CFDI,
			@PP_DESCRIPCRION,
			@PP_EXPLICACION,
			@PP_FISICA,
			@PP_MORAL,
			1
		)
	END

	-- ===============================

