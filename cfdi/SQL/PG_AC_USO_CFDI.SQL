USE [CFD19_GestionCFDi_V9999_R0]
GO
/****** Object:  StoredProcedure [dbo].[PG_SK_CERT_INFO_EMISOR]    Script Date: 6/1/2020 10:29:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[PG_AC_USO_CFDI] 
	@PP_USO_CFDI		VARCHAR(3)
AS

	BEGIN
		UPDATE USO_CFDI SET
			L_USO_CFDI = 1
		WHERE USO_CFDI = @PP_USO_CFDI
	END

	-- ===============================

